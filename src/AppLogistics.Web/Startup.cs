using AppLogistics.Components.ExcelReports;
using AppLogistics.Components.Extensions;
using AppLogistics.Components.Mail;
using AppLogistics.Components.Mvc;
using AppLogistics.Components.Security;
using AppLogistics.Controllers;
using AppLogistics.Data.Core;
using AppLogistics.Data.Logging;
using AppLogistics.Data.Migrations;
using AppLogistics.Objects;
using AppLogistics.Resources;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NonFactors.Mvc.Grid;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppLogistics.Web
{
    public class Startup
    {
        private IConfiguration Config { get; }

        // Updated to IWebHostEnvironment (IHostingEnvironment obsolete in .NET 6+)
        public Startup(IWebHostEnvironment env)
        {
            var config = new Dictionary<string, string>
            {
                {"Application:Path", env.ContentRootPath},
                {"Application:Env", env.EnvironmentName}
            };

            Config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables("ASPNETCORE_")
                .AddEnvironmentVariables("APPLOGISTICS_")
                .AddInMemoryCollection(config)
                .AddJsonFile("configuration.json")
                .AddJsonFile($"configuration.{env.EnvironmentName.ToLower()}.json", optional: true)
                .Build();

            RegisterViewResources();
        }

        public void Configure(IApplicationBuilder app)
        {
            RegisterMiddleware(app);
            RegisterMvc(app);
            UpdateDatabase(app);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterMvc(services);
            RegisterLogging(services);
            RegisterServices(services);
            RegisterLowercaseUrls(services);
            RegisterSecureResponse(services);
        }

        public void RegisterViewResources()
        {
            if (Config["Resources:Path"] is string path)
            {
                string directory = Path.Combine(Config["Application:Path"], path);
                if (Directory.Exists(directory))
                {
                    foreach (string resource in Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories))
                    {
                        string type = Path.GetFileNameWithoutExtension(resource);
                        string language = Path.GetExtension(type).TrimStart('.');
                        type = Path.GetFileNameWithoutExtension(type);
                        Resource.Set(type).Override(language, File.ReadAllText(resource));
                    }
                }
            }

            foreach (Type view in typeof(BaseView).Assembly.GetTypes())
            {
                Type type = view;
                while (typeof(BaseView).IsAssignableFrom(type.BaseType))
                {
                    Resource.Set(view.Name).Inherit(Resource.Set(type.BaseType.Name));
                    type = type.BaseType;
                }
            }
        }

        public void RegisterMvc(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<LanguageFilter>();
                options.Filters.Add<AuthorizationFilter>();
                // Insert custom trimming binder at a stable index
                options.ModelBinderProviders.Insert(4, new TrimmingModelBinderProvider());
            })
            .AddRazorOptions(o => o.ViewLocationExpanders.Add(new ViewLocationExpander()))
            .AddViewOptions(o =>
            {
                o.ClientModelValidatorProviders.Add(new DateValidatorProvider());
                o.ClientModelValidatorProviders.Add(new NumberValidatorProvider());
            })
            .AddMvcOptions(o => o.ModelMetadataDetailsProviders.Add(new DisplayMetadataProvider()))
            .AddSessionStateTempDataProvider(); // Use session-backed TempData (custom session cookie name retained)

            services.AddAuthentication("Cookies").AddCookie(authentication =>
            {
                // Keep custom cookie naming per user preference
                authentication.Cookie.Name = Config["Cookies:Auth:Name"];
                authentication.Events = new AuthenticationEvents();
            });

            services.AddMvcGrid(filters =>
            {
                filters.BooleanFalseOptionText = () => Resource.ForString("No");
                filters.BooleanTrueOptionText = () => Resource.ForString("Yes");
            });
        }

        public void RegisterLogging(IServiceCollection services)
        {
            // Original logging left commented; keep placeholder
        }

        public void RegisterServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddSingleton(Config);
            services.AddTransient<DatabaseConfiguration>();
            services.AddTransient<DbContext, Context>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<Context>(options => options.UseSqlServer(Config["Data:Connection"]));

            services.AddTransient<IAuditLogger>(provider =>
                new AuditLogger(provider.GetService<DbContext>(),
                provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User?.Id()));

            services.AddSingleton<IHasher, Hasher>();
            services.AddSingleton<IMailClient, SmtpMailClient>();
            services.AddSingleton<IMessagebuilder, MessageBuilder>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAdapterProvider>();
            services.AddSingleton<IAuthorization>(provider => new Authorization(typeof(BaseController).Assembly, provider));

            Language[] supported = Config.GetSection("Languages:Supported").Get<Language[]>();
            services.AddSingleton<ILanguages>(new Languages(Config["Languages:Default"], supported));

            string map = File.ReadAllText(Path.Combine(Config["Application:Path"], Config["SiteMap:Path"]));
            services.AddSingleton<ISiteMap>(provider => new SiteMap(map, provider.GetService<IAuthorization>()));

            services.AddTransientImplementations<IService>();
            services.AddTransientImplementations<IValidator>();
            services.AddSingleton<IExcelReportCreator, ExcelReportCreator>();
        }

        public void RegisterLowercaseUrls(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

        public void RegisterSecureResponse(IServiceCollection services)
        {
            // Keep custom cookie names
            services.Configure<SessionOptions>(session => session.Cookie.Name = Config["Cookies:Session:Name"]);
            services.Configure<AntiforgeryOptions>(antiforgery =>
            {
                antiforgery.Cookie.Name = Config["Cookies:Antiforgery:Name"];
                antiforgery.FormFieldName = "_Token_";
            });
        }

        public void RegisterMiddleware(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (response) =>
                {
                    response.Context.Response.Headers["Cache-Control"] = "max-age=8640000";
                }
            });
            app.UseSession();
        }

        public void RegisterMvc(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("MultiArea", "{language}/{area:exists}/{controller}/{action=Index}/{id:int?}");
                endpoints.MapControllerRoute("DefaultArea", "{area:exists}/{controller}/{action=Index}/{id:int?}");
                endpoints.MapControllerRoute("Multi", "{language}/{controller}/{action=Index}/{id:int?}");
                endpoints.MapControllerRoute("Default", "{controller}/{action=Index}/{id:int?}");
                endpoints.MapControllerRoute("Home", "{controller=Home}/{action=Index}");
            });
        }

        public void UpdateDatabase(IApplicationBuilder app)
        {
            using (var configuration = app.ApplicationServices.GetService<DatabaseConfiguration>())
            {
                configuration?.UpdateDatabase();
            }
        }
    }
}

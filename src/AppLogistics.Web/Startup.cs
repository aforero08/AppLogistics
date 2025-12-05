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
using AutoMapper;
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

        public Startup(IWebHostEnvironment env)
        {
            var inMemory = new Dictionary<string, string>
            {
                { "Application:Path", env.ContentRootPath },
                { "Application:Env", env.EnvironmentName }
            };

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"configuration.{env.EnvironmentName.ToLower()}.json", optional: true, reloadOnChange: true);

            // Load user secrets only in Development
            if (env.EnvironmentName.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                builder.AddUserSecrets<Startup>(optional: true, reloadOnChange: true);
            }

            // Let environment variables override secrets if present
            builder
                .AddEnvironmentVariables("ASPNETCORE_")
                .AddEnvironmentVariables("APPLOGISTICS_")
                .AddInMemoryCollection(inMemory);

            Config = builder.Build();

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
                options.ModelBinderProviders.Insert(4, new TrimmingModelBinderProvider());
            })
            .AddRazorOptions(o => o.ViewLocationExpanders.Add(new ViewLocationExpander()))
            .AddViewOptions(o =>
            {
                o.ClientModelValidatorProviders.Add(new DateValidatorProvider());
                o.ClientModelValidatorProviders.Add(new NumberValidatorProvider());
            })
            .AddMvcOptions(o => o.ModelMetadataDetailsProviders.Add(new DisplayMetadataProvider()))
            .AddSessionStateTempDataProvider();

            services.AddAuthentication("Cookies").AddCookie(authentication =>
            {
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
        }

        public void RegisterServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddSingleton(Config);

            var conn = Config["Data:Connection"];
            if (string.IsNullOrEmpty(conn))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }

            services.AddDbContext<DbContext, Context>(options =>
            {
                options.UseSqlServer(conn);
                options.UseLazyLoadingProxies();
                options.EnableDetailedErrors();
                //options.EnableSensitiveDataLogging();
                //options.LogTo(Console.WriteLine);
            });

            services.AddScoped<DatabaseConfiguration>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuditLogger>(provider =>
                new AuditLogger(provider.GetRequiredService<Context>(),
                provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User?.Id()));

            services.AddSingleton<IHasher, Hasher>();
            services.AddSingleton<IMailClient, SmtpMailClient>();
            services.AddSingleton<IMessagebuilder, MessageBuilder>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAdapterProvider>();
            services.AddSingleton<IAuthorization>(provider => new Authorization(typeof(BaseController).Assembly, provider));
            services.AddSingleton(new MapperConfiguration(mapper => mapper.AddMaps(typeof(BaseView).Assembly)).CreateMapper());

            Language[] supported = Config.GetSection("Languages:Supported").Get<Language[]>();
            services.AddSingleton<ILanguages>(new Languages(Config["Languages:Default"], supported));

            string map = File.ReadAllText(Path.Combine(Config["Application:Path"], Config["SiteMap:Path"]));
            services.AddSingleton<ISiteMap>(provider => new SiteMap(map, provider.GetRequiredService<IAuthorization>()));

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
                OnPrepareResponse = response =>
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

        // Safer migration pattern
        public void UpdateDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Context>();

            // Diagnostic logging before migrate
            if (!db.Database.CanConnect())
            {
                // Replace with your logger
                Console.WriteLine("Cannot connect with: " + db.Database.GetDbConnection().ConnectionString);
                return;
            }

            db.Database.Migrate();

            // Resolve seeding abstraction if needed
            var config = scope.ServiceProvider.GetRequiredService<DatabaseConfiguration>();
            config.SeedData();
        }
    }
}

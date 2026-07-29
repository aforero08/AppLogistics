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
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NonFactors.Mvc.Grid;

namespace AppLogistics.Web;

public class Startup
{
    private IConfiguration Config { get; }
    private IHostEnvironment Environment { get; }

    public Startup(IWebHostEnvironment env)
    {
        Environment = env;
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
        UpdateDatabase(app);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        RegisterMvc(services);
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
            ModelMessagesProvider.Set(options.ModelBindingMessageProvider);
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

    public void RegisterServices(IServiceCollection services)
    {
        services.AddSession();
        services.AddSingleton(Config);

        var dbConnectionString = Config["Data:Connection"];
        if (string.IsNullOrEmpty(dbConnectionString))
            throw new InvalidOperationException("Database connection string is not configured.");

        services.AddDbContext<DbContext, Context>(options =>
        {
            options.UseSqlServer(dbConnectionString);
            options.UseLazyLoadingProxies();
            if (Environment.IsDevelopment())
                options.EnableDetailedErrors();
            //options.EnableSensitiveDataLogging();
            //options.LogTo(Console.WriteLine);
        });

        services.AddScoped<DatabaseConfiguration>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAuditLogger>(provider =>
            new AuditLogger(
                new Context(provider.GetRequiredService<DbContextOptions<Context>>()),
                provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User?.Id()));

        services.AddSingleton<IHasher, Hasher>();
        services.AddSingleton<IMailClient, SmtpMailClient>();
        services.AddSingleton<IMessagebuilder, MessageBuilder>();
        services.AddHttpContextAccessor();
        services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAdapterProvider>();
        services.AddSingleton<IAuthorization>(provider => new Authorization(
            typeof(BaseController).Assembly,
            provider.GetRequiredService<IServiceScopeFactory>()));
        services.AddAutoMapper(mapper => mapper.AddMaps(typeof(BaseView).Assembly));

        Language[] supported = Config.GetSection("Languages:Supported").Get<Language[]>();
        services.AddSingleton<ILanguages>(new Languages(Config["Languages:Default"], supported));

        string map = File.ReadAllText(Path.Combine(Config["Application:Path"], Config["SiteMap:Path"]));
        services.AddScoped<ISiteMap>(provider => new SiteMap(map, provider.GetRequiredService<IAuthorization>()));

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
        if (Environment.IsDevelopment())
            app.UseMiddleware<DeveloperExceptionPageMiddleware>();
        else
            app.UseMiddleware<ErrorResponseMiddleware>();

        app.UseMiddleware<SecureHeadersMiddleware>();

        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = response => response.Context.Response.Headers.CacheControl = "max-age=8640000"
        });
    
        app.UseSession();
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
        using var scope = app.ApplicationServices.CreateScope();
        scope.ServiceProvider.GetRequiredService<DatabaseConfiguration>().UpdateDatabase();
    }
}

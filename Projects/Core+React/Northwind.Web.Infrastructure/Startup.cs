using System;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using Northwind.Infrastructure.Data;
using Northwind.Web.Infrastructure.Authorization;

namespace Northwind.Web.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        protected void ConfigureRequiredWebInfrastructureServices(IServiceCollection services)
        {
            
            Northwind.Infrastructure.DependencyInjection.InfrastructureDependency.RegisterAppCore(services);

            services.AddScoped<IDatabaseContextFactory>(options =>
            {
                return new MsSqlContextFactory(Configuration.GetConnectionString("ESSConnectionString"));
            });
            services.AddTransient<IDatabaseSingletonContextFactory>(options =>
            {
                return new MsSqlSingletonContextFactory(Configuration.GetConnectionString("ESSConnectionString"));
            });
            services.AddAuthorization(config =>
            {
                config.AddPolicy("CustomPolicy", builder =>
                {
                    builder.AddAuthenticationSchemes("Northwind.App").RequireAuthenticatedUser().AddRequirements(new IAuthorizationRequirement[] { new PolicyRequirement("", "") });
                });
            });
            services.AddScoped<IDatabaseContext>(options =>
            {
                return new ESSDbContext(Configuration.GetConnectionString("ESSConnectionString"));

            });

            services.AddTransient<IDatabaseSingletonContext>(options =>
            {
                return new ESSSingletonDbContext(Configuration.GetConnectionString("ESSConnectionString"));

            });
            services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PolicyAuthorizationHandler>();
            services.AddScoped<IActiveDirectoryContext>(options =>
            {
                var ldapServer = Configuration.GetSection("LDAPConnection")["LDAPServer"];
                var ldapUsername = Configuration.GetSection("LDAPConnection")["LDAPUsername"];
                var ldapPassword = Configuration.GetSection("LDAPConnection")["LDAPPassword"];
                var ldapDomain = Configuration.GetSection("LDAPConnection")["LDAPDomainController"];
                return new ActiveDirectoryContext(ldapServer, ldapDomain, ldapUsername, ldapPassword);
            });

            services.AddScoped<Core.Interfaces.IEmailSender>(options =>
            {
                var fromMailAddress = Configuration.GetSection("EmailService")["fromMailAddress"];
                var fromMailAddressTitle = Configuration.GetSection("EmailService")["fromMailAddressTitle"];
                var toMailAddressTitle = Configuration.GetSection("EmailService")["toMailAddressTitle"];
                var smtpServer = Configuration.GetSection("EmailService")["smtpServer"];
                var smtpPortNumber = Configuration.GetSection("EmailService")["smtpPortNumber"];
                var username = Configuration.GetSection("EmailService")["username"];
                var password = Configuration.GetSection("EmailService")["password"];
                var mode = Configuration.GetSection("EmailService")["mode"];
                var toEmailAddress = Configuration.GetSection("EmailService")["devEmailAddress"];
                return new EmailService(fromMailAddress, fromMailAddressTitle, toMailAddressTitle, smtpServer, smtpPortNumber, username, password,mode,toEmailAddress);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
           
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            //var protectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"C:\iissharedkeys\"));
            //var dataProtector = protectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "Northwind.Application","v2");
            //var ticketFormat = new TicketDataFormat(dataProtector);

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"c:\iissharedkeys\")).SetApplicationName("Northwind.App");
            services.AddAuthentication("Northwind.App")
            .AddCookie("Northwind.App", options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Cookie.Name = "Northwind.App";
               // options.TicketDataFormat = ticketFormat;
                options.CookieManager = new ChunkingCookieManager();
            });

            services.AddAntiforgery(o => {
                o.Cookie.Name = "X-CSRF-TOKEN";
            });

            services.AddMemoryCache();
        }
    }
}

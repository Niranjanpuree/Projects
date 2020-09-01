using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using Hangfire;
using Northwind.Core.Interfaces.Sync;
using Northwind.Core.Services.SyncServices;
using Northwind.Web.Helpers;
using Hangfire.Dashboard;
using Hangfire.Annotations;
using System.Net;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Services.ContractRefactor;
using Northwind.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Northwind.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Northwind.Web.Infrastructure.Helpers;
using System.Collections.Generic;
using NLog.Web;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.AuditLog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Northwind.Infrastructure.DependencyInjection;
using Northwind.Infrastructure.Data.FarClauseRepo;
using Northwind.Core.Interfaces.DocumentMgmt;
using Northwind.Core.Services.DocumentMgmt;
using Microsoft.AspNetCore.Antiforgery;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.CostPoint.Interfaces;
using NorthWind.Costpoint.Data;
using Northwind.CostPoint.Services;
using Northwind.Costpoint.Data;
using Northwind.Infrastructure.Data.RecentActivity;
using Northwind.Core.Interfaces.CrossSiteInterface;
using Northwind.Core.Services.CrossSiteServices;
using Northwind.Infrastructure.Data.Contract.ContractRefactor;

namespace Northwind.Web
{
    public class Startup : Northwind.Web.Infrastructure.Startup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {

        }


        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureRequiredWebInfrastructureServices(services);
            //Register Default Repositories And Services 
            InfrastructureDependency.RegisterWebRequiredServices(services);
            

            services.AddScoped<IHangfireScheduler, HangfireScheduler>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddScoped(x => x.GetService<IUrlHelperFactory>()
                .GetUrlHelper(x.GetService<IActionContextAccessor>().ActionContext));
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("ESSConnectionString")));


            ConfigurationServicesForPFS(services);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper(); 
            services.AddSingleton(mapper);
            
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<AntiforgeryOptions>(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN";
            });

            services.AddAntiforgery(o =>
            {
                o.Cookie.Name = "X-CSRF-TOKEN";
                o.HeaderName = "X-CSRF-TOKEN";
            });
        }

        private void ConfigurationServicesForPFS(IServiceCollection services)
        {
            /// Database connection for Cost Point
            services.AddScoped<IPFSDBContextFactory>(options =>
            {
                return new PFSDBContextFactory(Configuration.GetConnectionString("PFSConnectionString"));
            });

            services.AddScoped<IPFSDBContext>((opt) =>
            {
                return new PFSDBContext(Configuration.GetConnectionString("PFSConnectionString"));
            });
            /// End Database connection for Cost Point
            /// 

            services.AddScoped<IProjectServiceCP, ProjectServiceCP>();
            services.AddScoped<IProjectRepositoryCP, ProjectRepositoryCP>();

            services.AddScoped<IProjectModServiceCP, ProjectModServiceCP>();
            services.AddScoped<IProjectModRepositoryCP, ProjectModRepositoryCP>();

            services.AddScoped<IWbsRepositoryCP, WbsRepositoryCP>();
            services.AddScoped<IWbsServiceCP, WbsServiceCP>();

            services.AddScoped<ILaborRepositoryCP, LaborRepositoryCP>();
            services.AddScoped<ILaborServiceCP, LaborPayrollServiceCP>();

            services.AddScoped<IRecentActivityRepository, RecentActivityRepository>();
            services.AddScoped<IRecentActivityService, RecentActivityService>();

            services.AddScoped<IContractServiceCrossSite, ContractServiceCrossSite>();
            services.AddScoped<IContractsRepository, ContractsRepository>();

            services.AddScoped<IPOServiceCP, POServiceCP>();
            services.AddScoped<IPORepositoryCP, PORepositoryCP>();

            services.AddScoped<ICostRepositoryCP, CostRepositoryCP>();
            services.AddScoped<ICostServiceCP, CostServiceCP>();

            services.AddScoped<IVendorPaymentServiceCP, VendorPaymentServiceCP>();
            services.AddScoped<IVendorPaymentRepositoryCP, VendorPaymentRepositoryCP>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHangfireScheduler hangfire, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
           
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseHangfireServer();
            app.UseAuthentication();

            env.ConfigureNLog("NLog.config");            

            var arrOrigins = Configuration.GetSection("CORSUrls");
            var arrCors = arrOrigins.Get<List<string>>();

            app.UseCors(options =>
            {
                options.AllowCredentials();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.WithOrigins(arrCors.ToArray());
            });

            app.UseStatusCodePages(async context=>{
                var response =  context.HttpContext.Response;
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized && context.HttpContext.Request.ContentType != "application/json")
                {
                     response.Redirect("/login");
                }
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireAuth() }, AppPath = "/home" });
            hangfire.Register();

            app.Use(next => context =>
            {
                var tokens = antiforgery.GetTokens(context);
                if(tokens.CookieToken != null)
                {
                    context.Response.Cookies.Append("X-CSRF-TOKEN", tokens.CookieToken);
                }                
                context.Response.Cookies.Append(tokens.HeaderName, tokens.RequestToken);
                return next(context);
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
                routes.MapRoute(
                 name: "areas",
                 template: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{id1?}/{id2?}"
               );
            });
        }

    }

    class HangfireAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.Identity.IsAuthenticated;
        }


    }
}

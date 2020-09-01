using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Interfaces.CrossSiteInterface;
using Northwind.Core.Services;
using Northwind.Core.Services.CrossSiteServices;
using Northwind.Costpoint.Data;
using Northwind.CostPoint.Interfaces;
using Northwind.CostPoint.Services;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Data.Admin;
using Northwind.Infrastructure.Data.Contract.ContractRefactor;
using Northwind.Infrastructure.Data.RecentActivity;
using Northwind.PFS.Web.Helpers;
using Northwind.Web.Infrastructure.Helpers;
using NorthWind.Costpoint.Data;
using System.Reflection;

namespace Northwind.PFS.Web
{
    public class Startup: Northwind.Web.Infrastructure.Startup
    {
        public Startup(IConfiguration configuration): base(configuration)
        {
            
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureRequiredWebInfrastructureServices(services);
            services.AddScoped<IWbsDictionaryRepository, WbsDictionaryRepository>();
            services.AddScoped<IWbsDictionaryService, WbsDictionaryService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGroupUserService, GroupUserService>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


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

            /// Dependency Injections for Cost Point
            
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

            

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //var assembly2 = typeof(ESSMenuTagHelper).GetTypeInfo().Assembly;
            //.AddApplicationPart(assembly2)
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Project}/{action=Index}/{id?}/{id1?}/{id2?}");
                routes.MapRoute(
                 name: "areas",
                 template: "{area:exists}/{controller=Project}/{action=Index}/{id?}/{id1?}/{id2?}"
               );
            });
        }
    }
}

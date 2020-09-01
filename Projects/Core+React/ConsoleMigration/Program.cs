using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Service;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Data.Admin;
using Northwind.Infrastructure.DependencyInjection;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using Northwind.Infrastructure.Data.CustomerContactType;

namespace ConsoleMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Data Migration");
            IServiceCollection services = ConfigureServices();
            InfrastructureDependency.RegisterAppCoreMigration(services);
            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("Synching user from active directory");
            serviceProvider.GetService<SyncUser>().SyncUserFromActiveDirectory();
            serviceProvider.GetService<UserRole>().AssignRoleToUser();
            bool isImported = false;
           
            //var appSettingPath = Console.ReadLine();
            var appSettingPath = string.Empty;
            Console.Write("Username: ");
            var userName  = Console.ReadLine();

          
            
            if (!string.IsNullOrWhiteSpace(userName))
            {
                var isUsernameValid = serviceProvider.GetService<Migration>().IsUsernameValid(userName);
                if (isUsernameValid)
                {
                    //to sync user from active directory from web controller
                    //var baseURL = GetAppSetttingValueByKey("BaseURL");
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseURL + "Scheduler");
                    //request.Method = "GET";
                    ////specify other request properties
                    //var response = (HttpWebResponse)request.GetResponse();

                   
                    isImported = serviceProvider.GetService<Migration>().MigrateData(userName);
                    if(isImported)
                        Console.WriteLine("Data import executed successfully");
                }
                else
                    Console.WriteLine("Invalid username");
            }
            
            serviceProvider.Dispose();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        //register missing services
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            var importService = InfrastructureDependency.RegisterImportServices(services);
            var config = LoadConfiguration();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICustomerContactService, CustomerContactService>();
            services.AddScoped<ICustomerContactRepository, CustomerContactRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerContactTypeService, CustomerContactTypeService>();
            services.AddScoped<ICustomerContactTypeRepository, CustomerContactTypeRepository>();
            services.AddScoped<IDatabaseContext>(options =>
            {
                return new ESSDbContext(config.GetConnectionString("ESSConnectionString"));

            });
            services.AddSingleton<IDatabaseSingletonContext>(options =>
            {
                return new ESSSingletonDbContext(config.GetConnectionString("ESSConnectionString"));

            });
            services.AddScoped<IActiveDirectoryContext>(options =>
            {
                var ldapServer = config.GetSection("LDAPConnection")["LDAPServer"];
                var ldapUsername = config.GetSection("LDAPConnection")["LDAPUsername"];
                var ldapPassword = config.GetSection("LDAPConnection")["LDAPPassword"];
                var ldapDomain = config.GetSection("LDAPConnection")["LDAPDomainController"];
                return new ActiveDirectoryContext(ldapServer, ldapDomain, ldapUsername, ldapPassword);
            });
            services.AddSingleton(config);
            services.AddTransient<Migration>();
            services.AddTransient<SyncUser>();
            services.AddTransient<UserRole>();
            services.AddMemoryCache();
            return services;
        }

        //get configuration from web appsetting
        private static IConfiguration LoadConfiguration()
        {
            var appSettingPath = GetAppSetttingValueByKey("AppSettingPath");
            var fileName = GetAppSetttingValueByKey("FileName");
            if (!string.IsNullOrWhiteSpace(appSettingPath))
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(appSettingPath)
                .AddJsonFile(fileName, true, true);
                return builder.Build();
            }
            return null;
        }

        //Get value by key from app setting
        private static string GetAppSetttingValueByKey(string key)
        {
            var value = GetConfiguration().GetSection(key).Value;
            return value;
        }

        //get configuration from console appsetting
        private static IConfiguration GetConfiguration()
        {
            //app setting path for debugging
            //var appSettingPath = "D:\\Xylontech Projects\\src\\ConsoleMigration";

            //path for publish one
            string path = AppDomain.CurrentDomain.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("consoleAppsettings.json", true, true);
            return builder.Build();
        }

       
    }
}

using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Web;
using AutoCare.Product.Web.Infrastructure.ExceptionHandling;
using AutoCare.Product.Web.Infrastructure.IdentityAuthentication;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using Owin;
using Unity.WebApi;

[assembly: OwinStartup(typeof(Startup))]
namespace AutoCare.Product.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app, UnityConfig.Container);
            app.UseOAuthAuthorizationServer(UnityConfig.Container.Resolve<AutoCareOAuthOptions>());
            //app.UseJwtBearerAuthentication(UnityConfig.Container.Resolve<AutoCareJwtOptions>());
            //app.UseOAuthBearerAuthentication(UnityConfig.Container.Resolve<AutoCareJwtOptions>());
            ConfigureWebApi(app, UnityConfig.Container);
        }

        private void ConfigureWebApi(IAppBuilder app, IUnityContainer container)
        {
            

            app.Map("/api", inner =>
            {
                var apiRouteConfig = new HttpConfiguration();

                apiRouteConfig.MapHttpAttributeRoutes();

                //To return Json Data
                apiRouteConfig.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

                //To return Json Data in camelcase
                apiRouteConfig.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();

                apiRouteConfig.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

                apiRouteConfig.DependencyResolver = new UnityDependencyResolver(container);

                apiRouteConfig.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger(new ApplicationLoggerNLog()));

                apiRouteConfig.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

                apiRouteConfig.SuppressDefaultHostAuthentication();
                apiRouteConfig.Filters.Add(new HostAuthenticationFilter(DefaultAuthenticationTypes.ApplicationCookie));


                apiRouteConfig.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                    );

                inner.UseWebApi(apiRouteConfig);

            });
            //app.UseWebApi(apiRouteConfig);

        }
    }
}

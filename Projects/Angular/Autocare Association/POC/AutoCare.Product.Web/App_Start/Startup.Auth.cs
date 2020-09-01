using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoCare.Product.Web.Infrastructure.IdentityAuthentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Practices.Unity;
using Owin;

namespace AutoCare.Product.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app, IUnityContainer container)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            CookieAuthenticationProvider provider = new CookieAuthenticationProvider
            {
                OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, AutoCareUser>(
                    validateInterval: TimeSpan.FromMinutes(1),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
            };

            var originalHandler = provider.OnApplyRedirect;

            //Our logic to dynamically modify the path (maybe needs some fine tuning)
            provider.OnApplyRedirect = context =>
            {
                if (context.Request.Uri.ToString().Contains("api"))
                {
                    return;
                }

                //Get the current language  
                RouteValueDictionary routeValues = new RouteValueDictionary();

                //Reuse the RetrunUrl
                Uri uri = new Uri(context.RedirectUri);
                string returnUrl = HttpUtility.ParseQueryString(uri.Query)[context.Options.ReturnUrlParameter];
                routeValues.Add(context.Options.ReturnUrlParameter, returnUrl);

                //Overwrite the redirection uri
                context.RedirectUri = url.Action("login", "account", routeValues);
                originalHandler.Invoke(context);
            };

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                //AuthenticationType = "JWT",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //AuthenticationMode = AuthenticationMode.Active,
                LoginPath = new PathString( "/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, AutoCareUser>(
                    validateInterval: TimeSpan.FromMinutes(1),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                    OnApplyRedirect = ApplyRedirect
                },
                TicketDataFormat = container.Resolve<AutoCareJwtFormat>(),
                CookieName = DefaultAuthenticationTypes.ApplicationCookie
            });

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {
            if (context.Request.Uri.ToString().Contains("api") || 
                context.Request.Uri.ToString().Contains("gettoken"))
            {
                return;
            }

            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            //Get the current language  
            RouteValueDictionary routeValues = new RouteValueDictionary();

            //Reuse the RetrunUrl
            Uri uri = new Uri(context.RedirectUri);
            string returnUrl = HttpUtility.ParseQueryString(uri.Query)[context.Options.ReturnUrlParameter];
            routeValues.Add(context.Options.ReturnUrlParameter, returnUrl);

            //Overwrite the redirection uri
            context.Response.Redirect(url.Action("login", "account", routeValues));
        }
    }
}
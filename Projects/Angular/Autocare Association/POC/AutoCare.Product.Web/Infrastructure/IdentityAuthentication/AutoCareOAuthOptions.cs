using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class AutoCareOAuthOptions : OAuthAuthorizationServerOptions
    {
        public AutoCareOAuthOptions(IPersonifyHelper personifyHelper)
        {
            TokenEndpointPath = new PathString("/autocareoauth/token/");
            AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(59);
            AccessTokenFormat = new AutoCareJwtFormat(this);
            Provider = new AutoCareOAuthAuthorizationProvider(personifyHelper);
//#if DEBUG
            AllowInsecureHttp = true;
//#endif
        }
    }
}
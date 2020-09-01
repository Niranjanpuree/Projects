using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class AutoCareJwtOptions : JwtBearerAuthenticationOptions
    {
        private string token = null;
        public AutoCareJwtOptions(IPersonifyHelper personifyHelper, IUnityContainer container)
        {
            //AccessTokenFormat = container.Resolve<AutoCareJwtFormat>();

            var issuer = "http://vcdbpoc-staging.azurewebsites.net";
            var audience = "all";
            var key = Convert.FromBase64String("KJiuweUH2234Kkjk234234fgdfc56566");
            AuthenticationMode = AuthenticationMode.Active;
            AuthenticationType = "JWT";
            AllowedAudiences = new[] { audience };
            IssuerSecurityTokenProviders = new[]
            {
                new SymmetricKeyIssuerSecurityTokenProvider(issuer, key)
            };
            Provider = new AutoCareOAuthAuthenticationProvider();
        }
    }
}
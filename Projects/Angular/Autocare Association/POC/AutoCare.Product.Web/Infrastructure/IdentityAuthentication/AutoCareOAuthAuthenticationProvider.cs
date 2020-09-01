using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class AutoCareOAuthAuthenticationProvider : IOAuthBearerAuthenticationProvider
    {
        private string token;

        public Task RequestToken(OAuthRequestTokenContext context)
        {
            var tokenCookie = context.Request.Cookies.LastOrDefault();
            token = tokenCookie.Equals(default(KeyValuePair<string, string>)) ? null : tokenCookie.Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            //var plainToken = tokenHandler.ReadToken(token);
            //if (!string.IsNullOrEmpty(token))
            //    context.Request.Headers.Add("Authorization", new[] { string.Format("Bearer {0}", token) });
            context.Token = token;
            return Task.FromResult<object>(null);
        }

        public Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "all",
                    IssuerSigningToken = new BinarySecretSecurityToken(Convert.FromBase64String("KJiuweUH2234KJBJbkjk234234fgdfc56566")),
                    ValidIssuer = "http://vcdbpoc-staging.azurewebsites.net"
                };
                try
                {
                    SecurityToken securityToken;
                    var claimsPrinciple = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                    context.Validated(new ClaimsIdentity(claimsPrinciple.Claims, OAuthDefaults.AuthenticationType));
                    return Task.FromResult(0);
                }
                catch (Exception ex)
                {
                    context.Rejected();
                    return Task.FromResult(0);
                }
                
                //var notPadded = token.Split('.')[1];
                //var padded = notPadded.PadRight(notPadded.Length + (4 - notPadded.Length % 4) % 4, '=');
                //var urlUnescaped = padded.Replace('-', '+').Replace('_', '/');
                //var claimsPart = Convert.FromBase64String(urlUnescaped);

                //var obj = JObject.Parse(Encoding.UTF8.GetString(claimsPart, 0, claimsPart.Length));

                //// simple, not handling specific types, arrays, etc.
                //foreach (var prop in obj.Properties().AsJEnumerable())
                //{
                //    if (!context.Ticket.Identity.HasClaim(prop.Name, prop.Value.Value<string>()))
                //    {
                //        context.Ticket.Identity.AddClaim(new Claim(prop.Name, prop.Value.Value<string>()));
                //    }
                //}
            }

            context.Rejected();
            return Task.FromResult(0);
        }

        public Task ApplyChallenge(OAuthChallengeContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}
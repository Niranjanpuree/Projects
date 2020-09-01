using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class AutoCareJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly AutoCareOAuthOptions _autoCareOAuthOptions;

        public AutoCareJwtFormat(AutoCareOAuthOptions autoCareOAuthOptions)
        {
            _autoCareOAuthOptions = autoCareOAuthOptions;
        }

        public string SignatureAlgorithm => "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

        public string DigestAlgorithm => "http://www.w3.org/2001/04/xmlenc#sha256";

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            // Token Creation
            var issuer = "http://vcdbpoc-staging.azurewebsites.net";
            var audience = "all";
            var key = Convert.FromBase64String("KJiuweUH2234KJBJbkjk234234fgdfc56566");
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_autoCareOAuthOptions.AccessTokenExpireTimeSpan.TotalMinutes);

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(issuer, audience, data.Identity,
                now, expires, new SigningCredentials(
                    new InMemorySymmetricSecurityKey(key),
                    SignatureAlgorithm,
                    DigestAlgorithm)));


            
            //var tokenString = tokenHandler.ReadToken(token);
            //var validationParameters = new TokenValidationParameters()
            //{
            //    ValidAudience = "all",
            //    IssuerSigningToken = new BinarySecretSecurityToken(Convert.FromBase64String(securityKey)),
            //    ValidIssuer = "http://vcdbpoc-staging.azurewebsites.net"
            //};
            //SecurityToken securityToken;
            //var claimsPrinciple = tokenHandler.ValidateToken(token, validationParameters,out securityToken);

            return token;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            //byte[] data = Convert.FromBase64String(protectedText);
            //string decodedString = Encoding.UTF8.GetString(data);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidAudience = "all",
                IssuerSigningToken = new BinarySecretSecurityToken(Convert.FromBase64String("KJiuweUH2234KJBJbkjk234234fgdfc56566")),
                ValidIssuer = "http://vcdbpoc-staging.azurewebsites.net",
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };
            try
            {
                SecurityToken securityToken;
                var claimsPrinciple = tokenHandler.ValidateToken(protectedText, validationParameters, out securityToken);
                var validFrom = securityToken.ValidFrom;
                var validTo = securityToken.ValidTo;
                var now = DateTime.UtcNow;

                
                var ticket = new AuthenticationTicket(new CustomClaimsIdentity(claimsPrinciple.Claims, DefaultAuthenticationTypes.ApplicationCookie), 
                    new AuthenticationProperties());

                var expTime = EpochTime.DateTime(Convert.ToInt64(claimsPrinciple.Claims.First(x => x.Type == "exp").Value));

                //var ticket = new AuthenticationTicket(new ClaimsIdentity(claimsPrinciple.Claims, DefaultAuthenticationTypes.ApplicationCookie),
                //    new AuthenticationProperties());
                //ticket = new AuthenticationTicket(new ClaimsIdentity(claimsPrinciple.Claims, OAuthDefaults.AuthenticationType),
                //    new AuthenticationProperties());

                return ticket;
            }
            catch (SecurityTokenExpiredException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            
            //var props = new AuthenticationProperties(new Dictionary<string, string>
            //    {
            //        {
            //             "audience", "all",
            //        }
            //    });

            
            return null;
        }
    }
}
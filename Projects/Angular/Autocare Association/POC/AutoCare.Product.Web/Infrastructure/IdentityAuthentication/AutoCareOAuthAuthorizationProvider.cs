using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class AutoCareOAuthAuthorizationProvider : OAuthAuthorizationServerProvider
    {
        private readonly IPersonifyHelper _personifyHelper;

        public AutoCareOAuthAuthorizationProvider(IPersonifyHelper personifyHelper)
        {
            _personifyHelper = personifyHelper;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var identity = new CustomClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            var customerId = context.OwinContext.Get<string>("ac:customer_id");
            identity.AddClaim(new Claim(CustomClaimTypes.CustomerId, context.OwinContext.Get<string>("ac:customer_id")));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.OwinContext.Get<string>("ac:username")));
            identity.AddClaim(new Claim(ClaimTypes.Email, context.OwinContext.Get<string>("ac:email")));

            var roles = context.OwinContext.Get<string>("ac:role");
            if (String.IsNullOrWhiteSpace(roles))
            {
                var rolesList = await _personifyHelper.GetRolesAsync(customerId);
                roles = string.Join(",", rolesList);
            }
            
            identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", roles)));
            context.Validated(identity);
            return;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                var username = context.Parameters["username"];
                var password = context.Parameters["password"];

                if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
                {
                    context.SetError("Invalid credentials");
                    context.Rejected();
                    return;
                }

                if (username == password)//Only for testing purpose
                {
                    context.OwinContext.Set("ac:username", username);
                    context.OwinContext.Set("ac:customer_id", username);
                    context.OwinContext.Set("ac:email", username);
                    if (username.Equals("user@autocare.com"))
                    {
                        context.OwinContext.Set("ac:role", CustomRoles.User);
                        context.OwinContext.Set("ac:role_id", 3);
                    }
                    else if (username.Equals("researcher@autocare.com"))
                    {
                        context.OwinContext.Set("ac:role", CustomRoles.Researcher);
                        context.OwinContext.Set("ac:role_id", 2);
                    }
                    else if (username.Equals("admin@autocare.com"))
                    {
                        context.OwinContext.Set("ac:role", CustomRoles.Admin);
                        context.OwinContext.Set("ac:role_id", 1);
                    }
                    else
                    {
                        context.OwinContext.Set("ac:role", CustomRoles.User);
                        context.OwinContext.Set("ac:role_id", 3);
                    }
                    context.Validated();
                    return;
                }

                var autoCareUser = await _personifyHelper.GetUserAsync(username, password);

                if (autoCareUser == null)
                {
                    context.SetError("Invalid credentials");
                    context.Rejected();
                    return;
                }

                context.OwinContext.Set("ac:username", autoCareUser.UserName);
                context.OwinContext.Set("ac:customer_id", autoCareUser.Id);
                context.OwinContext.Set("ac:email", autoCareUser.Email);
                context.OwinContext.Set("ac:customer_token", autoCareUser.CustomerToken);
                context.Validated();
            }
            catch
            {
                context.SetError("Server error");
                context.Rejected();
            }
        }
    }
}
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class AutoCareUser: IUser
    {
        public AutoCareUser(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string CustomerToken { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AutoCareUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            //var userIdentity = await manager.CreateIdentityAsync(this, "JWT");
            // Add custom user claims here
            return userIdentity;
        }
    }
}
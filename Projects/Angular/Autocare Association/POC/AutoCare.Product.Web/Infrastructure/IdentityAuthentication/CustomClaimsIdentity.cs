using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class CustomClaimsIdentity : ClaimsIdentity
    {
        public CustomClaimsIdentity()
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Security.Claims.ClaimsIdentity class
        //     with an empty claims collection and the specified authentication type.
        //
        // Parameters:
        //   authenticationType:
        //     The type of authentication used.
        public CustomClaimsIdentity(string authenticationType) : base(authenticationType)
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Security.Claims.ClaimsIdentity class
        //     with the specified claims and authentication type.
        //
        // Parameters:
        //   claims:
        //     The claims with which to populate the claims identity.
        //
        //   authenticationType:
        //     The type of authentication used.
        public CustomClaimsIdentity(IEnumerable<Claim> claims, string authenticationType): base(claims, authenticationType)
        {
            
        }

        public string CustomerId => this.FindFirstValue(CustomClaimTypes.CustomerId);
        public string UserName => this.FindFirstValue(ClaimTypes.Name);
        public string Email => this.FindFirstValue(ClaimTypes.Email);

        public List<string> Roles
        {
            get
            {
                var roles = this.FindFirstValue(ClaimTypes.Role);
                if (!String.IsNullOrWhiteSpace(roles))
                {
                    return roles.Split(',').ToList();
                }
                return new List<string>();
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Authorization
{
    public class PolicyProvider: IAuthorizationPolicyProvider
    {
        
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);

        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if(policyName.StartsWith("CustomPolicy"))
            {
                var policy = new AuthorizationPolicyBuilder();
                var splitPolicyName = policyName.Split(":::");
                
                policy.AddRequirements(new PolicyRequirement(splitPolicyName[1],splitPolicyName[2]));
                return Task.FromResult(policy.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
           

        }

    }
}

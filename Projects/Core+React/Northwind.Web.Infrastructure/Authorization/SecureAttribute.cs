using Microsoft.AspNetCore.Authorization;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Infrastructure.Authorization
{
    public class SecureAttribute: AuthorizeAttribute
    {
        const string POLICY_PREFIX = "CustomPolicy";
        private readonly string _resourceNamespace;
        private readonly string _actionNamespace;
        
        public SecureAttribute(string resourceNamespace, string actionNamespace) : base(POLICY_PREFIX + ":::"+ resourceNamespace+":::"+ actionNamespace)
        {
            _resourceNamespace = resourceNamespace;
            _actionNamespace = actionNamespace;            
            
        }

        public SecureAttribute(ResourceType resourceNamespace, ResourceActionPermission actionNamespace) : base(POLICY_PREFIX + ":::" + resourceNamespace + ":::" + actionNamespace)
        {
            _resourceNamespace = resourceNamespace.ToString();
            _actionNamespace = actionNamespace.ToString();

        }

        public string ActionNamespace
        {
            get
            {
                var splitPolicy = Policy.Split(":::");
                return splitPolicy[1]; 
            }
            set
            {
                Policy = $"{POLICY_PREFIX}:::{_resourceNamespace}:::{value.ToString()}";
            }
        }

        public string ResourceNamespace
        {
            get
            {
                var splitPolicy = Policy.Split(":::");
                return splitPolicy[2];
            }
            set
            {
                Policy = $"{POLICY_PREFIX}:::{value.ToString()}:::{_actionNamespace}";
            }
        }
    }
}

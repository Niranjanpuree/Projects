using Microsoft.AspNetCore.Authorization;

namespace Northwind.Web.Infrastructure.Authorization
{
    public class PolicyRequirement: IAuthorizationRequirement
    {
        public string ResourceNamespace { get; private set; }
        public string ActionNamespace { get; private set; }
        public PolicyRequirement(string resourceNamespace, string actionNamespace)
        {
            ResourceNamespace = resourceNamespace;
            ActionNamespace = actionNamespace;

        }

    }
}

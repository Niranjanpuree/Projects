

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Authorization
{
    public class PolicyAuthorizationHandler:AuthorizationHandler<PolicyRequirement>
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IGroupPermissionService _grouPermission;
        public PolicyAuthorizationHandler(IPolicyRepository policyRepository, IGroupPermissionService groupPermission)
        {
            _policyRepository = policyRepository;
            _grouPermission = groupPermission;

        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        { 
            context.Succeed(requirement);
            var ctx = context.Resource as AuthorizationFilterContext;
            //Here we need to call the methods to work with our policies
            var authRequest = new AuthorizationRequest();
            var claims = context.User.Claims;
            foreach(var claim in claims)
            {
                authRequest.SubjectAttributes.Add(claim.Type, claim.Value);
            }
            authRequest.ResourceAttributes.Add("resource", requirement.ResourceNamespace);
            authRequest.ActionAttributes.Add("action", requirement.ActionNamespace);
            PolicyDecisionPoint _pdp = new PolicyDecisionPoint(_policyRepository, _grouPermission);
            var authResponse = _pdp.Evaluate(authRequest);
            if (authResponse.Decision == AuthorizationDecision.Permit)
            {
                context.Succeed(requirement);
            }
            else
            {
                if (ctx.HttpContext.Request.ContentType == "application/json")
                {
                    ctx.Result = new UnauthorizedResult();
                }
                else
                {
                    context.Fail();
                }
                
            }
            
            return Task.CompletedTask;
        }
        
    }
}

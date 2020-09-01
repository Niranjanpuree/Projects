using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Northwind.Core.Interfaces;
using System.Collections;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Core.Services
{
    public class PolicyDecisionPoint
    {

        private readonly IPolicyRepository _policyRepository;
        private readonly IGroupPermissionService _grouPermission;

        public PolicyDecisionPoint(IPolicyRepository policyRepository, IGroupPermissionService groupPermission)
        {
            _policyRepository = policyRepository;
            _grouPermission = groupPermission;
        }

        public IEnumerable<Policy> GetPolicies(Guid UserGuid)
        {
            IEnumerable<Policy> policies = _policyRepository.GetPolicies(UserGuid);
            return policies;
        }

        public AuthorizationResponse Evaluate(AuthorizationRequest request)
        {
            var response = new AuthorizationResponse();
            List<Rule> rulesToEvaluate = new List<Rule>();
            Guid userGuid = Guid.Empty;
            if (request.AllAttributes.ContainsKey(System.Security.Claims.ClaimTypes.Name))
            {
                Guid.TryParse(request.AllAttributes[System.Security.Claims.ClaimTypes.Name], out userGuid);
            }
                
            if(userGuid == Guid.Empty)
            {
                response.Decision = AuthorizationDecision.Deny;
                return response;
            }

            var resourceType = (ResourceType)Enum.Parse(typeof(ResourceType), request.ResourceAttributes["resource"]);
            var resourceAction = (ResourceActionPermission)Enum.Parse(typeof(ResourceActionPermission), request.ActionAttributes["action"]);

            var result = _grouPermission.IsUserPermitted(userGuid, resourceType, resourceAction);

            /*
            var policies = GetPolicies(userGuid);

            //Match Policy with Target
            foreach (var p in policies)
            {
                var rules = p.Rules.Where(c => c.Resource.Contains(request.ResourceAttributes["resource"]) || c.Resource.Contains("*")).ToList();
                foreach (var rule in rules)
                {
                    var ruleContainsResource = rule.Resource.Contains(request.ResourceAttributes["resource"]);
                    var ruleContainsResourceWildCard = rule.Resource.Contains("*");

                    var ruleContainsAction = rule.Action.Contains(request.ActionAttributes["action"]);
                    var ruleContainsActionWildCard = rule.Action.Contains("*");

                    if ((ruleContainsResource || ruleContainsResourceWildCard) && (ruleContainsAction || ruleContainsActionWildCard))
                    {
                        rulesToEvaluate.Add(rule);
                    }
                }
                 answer = EvaluateRules(rulesToEvaluate,request);
            }
            */

            if (result)
            {
                response.Decision = AuthorizationDecision.Permit;                
            }
            else
            {
                response.Decision = AuthorizationDecision.Deny;
            }
            //return response;
            //Always allow for now
            //response.Decision = AuthorizationDecision.Permit;
            return response;

        }

        private bool EvaluateRules(IEnumerable<Rule> rules, AuthorizationRequest request)
        {
            var allowedRules = new List<Rule>();
            
            foreach (var rule in rules)
            {
                var condition = rule.Condition;

                //If condition doesn't apply
                if(condition == null)
                {
                    return true;
                }

                foreach (var item in condition)
                {
                    var op = item.Key;
                    var operand = item.Value;
                    if (EvaluateOperation(op, operand, request))
                    {
                        if(rule.Effect=="deny")
                        {
                            break;
                        }
                        else
                        {
                            allowedRules.Add(rule);
                        }
                    }
                }
            }

            if (allowedRules.Count > 0) return true;

            return false;
        }

        private bool EvaluateOperation(string op, object operand, AuthorizationRequest request)
        {         
            var normalizedOp = op.ToLower();
            var opDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(operand.ToString());
            var leftOperand = opDictionary.Keys.FirstOrDefault<string>();
            var rightOperand = opDictionary.GetValueOrDefault(leftOperand);

            switch (normalizedOp)
            {
                case "stringequals":
                    var valueOfLeftOperand = request.AllAttributes.GetValueOrDefault(leftOperand);
                    if(rightOperand.GetType()==typeof(Newtonsoft.Json.Linq.JArray))
                    {
                        JArray arr = JArray.Parse(rightOperand.ToString());
                        if (arr.Contains(valueOfLeftOperand))
                            return true;
                    } 
                    else
                    {                   
                       if(string.Equals(valueOfLeftOperand,rightOperand.ToString(),StringComparison.OrdinalIgnoreCase))
                       {
                            return true; 
                       }
                    }                                       
                   
                    break;
                case "bool":
                    valueOfLeftOperand = request.AllAttributes.GetValueOrDefault(leftOperand);
                    var valueOfRightOperant = rightOperand.ToString();
                    if (valueOfLeftOperand != null && valueOfLeftOperand == valueOfRightOperant)
                        return true;
                    break;
            }

            return false;
        }







        private bool HasMatchingKeys(Dictionary<string, object> dictionary, List<string> targetKeys)
        {
            foreach (var requestKey in dictionary.Keys)
            {
                if (targetKeys.Contains(requestKey)) return true;
            }
            return false;
        }

        private List<string> GetTarget(string targetKey, Rule r)
        {
            var returnString = new List<string>();

            //if(r.Resource.Keys.Count!=0 && r.Resource.ContainsKey(targetKey))
            //{
            //    object o = r.Resource[targetKey];
            //    returnString = ((IEnumerable)o).Cast<object>().Select(x => x.ToString()).ToList();

            //}
            return returnString;

        }



    }
}

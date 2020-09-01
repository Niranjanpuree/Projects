using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Northwind.Core.Entities
{
    /// <summary>
    /// TODO: Refactor this to possibly include only one Dictionary
    /// </summary>
    public class AuthorizationRequest
    {
        public AuthorizationRequest()
        {
            SubjectAttributes = new Dictionary<string, string>();
            ResourceAttributes = new Dictionary<string, string>();
            ActionAttributes = new Dictionary<string, string>();
            EnvironmentAttributes = new Dictionary<string, string>();

        }
        public Dictionary<string, string> AllAttributes {
            get
            {
                return SubjectAttributes.Union(ResourceAttributes)
                    .Union(ActionAttributes)
                    .Union(EnvironmentAttributes)
                    .ToDictionary(x=>x.Key,x=>x.Value);
                        
            }
        }        
        public Dictionary<string, string> SubjectAttributes { get; set; }
        public Dictionary<string, string> ResourceAttributes { get; set; }
        public Dictionary<string, string> ActionAttributes{get; set;}
        public Dictionary<string, string> EnvironmentAttributes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    /// <summary>
    /// This class represents a authorization policy. This class will be serialized to JSON and saved 
    /// </summary>
    public class Policy
    {
        //Id of the policy
        public Guid Id { get; set; }

        //Name of the policy
        public string Name { get; set; }

        //Descirpiton of the policy to be shown in ui
        public string Description { get; set; }

        //Set of rules that are applicable for this particular policy.
        public List<Rule> Rules { get; set; }

        //Whether this policy is active or inactive 
        public EntityStatus Status {get; set;}
       
    }

    public class PolicyEntity
    {
        public Guid PolicyGuid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PolicyJson { get; set; }        

        public Policy Policy
        {
            get { return Newtonsoft.Json.JsonConvert.DeserializeObject<Policy>(PolicyJson); }
            set { PolicyJson = Newtonsoft.Json.JsonConvert.SerializeObject(value); }
        }

    }
}


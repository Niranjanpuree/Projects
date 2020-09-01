using System;
using System.Collections.Generic;

namespace Northwind.Core.Entities
{
    public class Rule
    {
        private Dictionary<string, object> condition = new Dictionary<string, object>();
        public Guid RuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Resource {get; set; }
        public List<string> Action { get; set; }
        public string Effect { get; set; }
        public Dictionary<string, object> Condition
        {
            get { return condition; }
            set { condition = value; }
        }


    }
}
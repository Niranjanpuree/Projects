using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Specifications
{
    public class BaseSearchSpec
    {
        public List<string> AttributesToReturn { get; set; } 
        public List<ICriteria> Criteria = new List<ICriteria>();
        public int Take { get; set; }
        public int Skip { get; set; }
        public SortCriteria SortCriteria { get; set; } 

        public void AddCriteria(ICriteria criteria)
        {
            Criteria.Add(criteria);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Core.Entities
{
    public class Company : BaseModel
    {
        public Guid CompanyGuid { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public Guid President { get; set; }
        public string PresidentName { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
    }
}

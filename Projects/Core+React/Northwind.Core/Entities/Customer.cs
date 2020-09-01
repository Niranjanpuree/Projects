using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Core.Entities
{
    public class Customer : BaseModel
    {
        public Guid CustomerGuid { get; set; }
        public string CustomerName { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public Guid? StateId { get; set; }
        public Guid CountryId { get; set; }
        public Guid CustomerTypeGuid { get; set; }
        public string CustomerDescription { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PrimaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        public string Abbreviations { get; set; }
        public string Tags { get; set; }
        public string Url { get; set; }
        public string Department { get; set; }
        public string CustomerCode { get; set; }
        public string Agency { get; set; }
        public string CustomerTypeName { get; set; }
        public string StatesName { get; set; }
        public string CountryName { get; set; }
    }
}

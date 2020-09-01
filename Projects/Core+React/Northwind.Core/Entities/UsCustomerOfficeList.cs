using System;

namespace Northwind.Core.Entities
{
    public class UsCustomerOfficeList
    {
        public Guid Id { get; set; }
        public string DepartmentdD { get; set; }
        public string DepartmentName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ContractingOfficeCode { get; set; }
        public string ContractingOfficeName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
    }
}

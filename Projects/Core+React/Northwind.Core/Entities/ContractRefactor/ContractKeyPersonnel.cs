using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.ContractRefactor
{
   
    public class ContractKeyPersonnel: IUser
    {
        public static string _companyPresident = "company-president";
        public static string _projectManager = "project-manager";
        public static string _regionalManager = "regional-manager";
        public static string _accountRepresentative = "account-representative";
        public static string _contractRepresentative = "contract-representative";
        public static string _projectControls = "project-controls";

        public static string _laborCategoryRates = "labor-category-rates";
        public static string _employeeBillingRates = "employee-billing-rates";
        public static string _wbs = "work-breakdown-structure";

        public string UserRole { get; set; }
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public string DisplayName { get; set; }
        public string UserStatus { get; set; }
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string JobStatus { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Extension { get; set; }

    }
}

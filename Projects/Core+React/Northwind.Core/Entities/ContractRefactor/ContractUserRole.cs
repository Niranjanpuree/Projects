using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.ContractRefactor
{
    public class ContractUserRole
    {
        public static string _companyPresident = "company-president";
        public static string _projectManager = "project-manager";
        public static string _regionalManager = "regional-manager";
        public static string _deputyregionalManager = "deputy-regional-manager";
        public static string _bdregionalManager = "BD-regional-manager";
        public static string _hsregionalManager = "HS-regional-manager";
        public static string _accountRepresentative = "account-representative";
        public static string _contractRepresentative = "contract-representative";
        public static string _projectControls = "project-controls";
        public static string _subContractAdministrator = "subcontract-administrator";
        public static string _purchasingRepresentative = "purchasing-representative";
        public static string _humanResourceRepresentative = "human-resource-representative";
        public static string _qualityRepresentative = "quality-representative";
        public static string _safetyOfficer = "safety-officer";
        public static string _operationManager = "operation-manager";

        public Guid ContractUserRoleGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string UserRole { get; set; }
        public User User { get; set; }

    }
}

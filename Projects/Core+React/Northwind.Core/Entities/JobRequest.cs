using System;
using System.Collections.Generic;
using System.ComponentModel;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Core.Entities
{
    public class JobRequest : BaseModel
    {
        public static string _laborCategoryRates = "labor-category-rates";
        public static string _employeeBillingRates = "employee-billing-rates";
        public static string _wbs = "workbreak-structure";
        public Guid JobRequestGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public int Status { get; set; }
        [DisplayName("Are there any intercompany work orders?")]
        public bool IsIntercompanyWorkOrder { get; set; }
        public string Companies { get; set; }
        public string Notes { get; set; }
        public JobRequestStatus JobRequestStatus { get; set; }
        public Guid? ParentContractGuid { get; set; }
        public BasicContractInfoModel BasicContractInfo { get; set; }
        public KeyPersonnelModel KeyPersonnel { get; set; }
        public CustomerInformationModel CustomerInformation { get; set; }
        public FinancialInformationModel FinancialInformation { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle { get; set; }
        public string TaskOrderNumber { get; set; }
        public string ContractNumber { get; set; }
        public Contracts Contracts { get; set; }
        public List<ContractKeyPersonnel> KeyPersonnelList { get; set; }
        public User RegionalManager { get; set; }
        public User ProjectManager { get; set; }
        public User AccountRepresentative { get; set; }
        public User CompanyPresident { get; set; }
        public User ProjectControls { get; set; }
        public User ContractRepresentative { get; set; }
        public string InitiatedBy { get; set; }
        public ContractQuestionaire ContractQuestionaire { get; set; }
        public List<string> CompanySelected { get; set; }
        public ICollection<KeyValuePairModel<Guid, string>> companyList { get; set; }
        public IDictionary<bool, string> radioIsIntercompanyWorkOrder { get; set; }
        public AutoCompleteReturnModel AutoCompleteReturnModel { get; set; }
    }
}

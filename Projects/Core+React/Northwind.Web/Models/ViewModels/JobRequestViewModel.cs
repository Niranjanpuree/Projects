using Northwind.Web.Helpers;
using Northwind.Web.Models.ViewModels.Contract;
using Northwind.Web.Models.ViewModels.FarClause;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using static Northwind.Web.Models.ViewModels.EnumGlobal;

namespace Northwind.Web.Models.ViewModels
{
    public class JobRequestViewModel : BaseViewModel
    {
        //public Guid JobRequestGuid { get; set; }
        //public Guid ContractGuid { get; set; }
        //public int Status { get; set; }
        //[DisplayName("Are there any intercompany work orders?")]
        //public bool IsIntercompanyWorkOrder { get; set; }
        //public string Companies { get; set; }

        public Guid JobRequestGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public int Status { get; set; }
        [DisplayName("Are there any intercompany work orders?")]
        public bool IsIntercompanyWorkOrder { get; set; }
        public string Companies { get; set; }
        [DisplayName("Additional Notes")]
        public string Notes { get; set; }

        //public JobRequest JobRequest { get; set; }
        public JobRequestStatus JobRequestStatus { get; set; }
        public string _JobRequestStatus
        {
            get
            {
                return ReviewStageHelper.ReviewStageByStatus(Status);
            }
        }
        public Guid? Parent_ContractGuid { get; set; }
        public BasicContractInfoViewModel BasicContractInfo { get; set; }
        public KeyPersonnelViewModel KeyPersonnel { get; set; }
        public CustomerInformationViewModel CustomerInformation { get; set; }
        public FinancialInformationViewModel FinancialInformation { get; set; }
        public FarContractViewModel farContractViewModel { get; set; }

        public ContractWBSViewModel ContractWBS { get; set; }
        public EmployeeBillingRatesViewModel EmployeeBillingRates { get; set; }
        public LaborCategoryRatesViewModel LaborCategoryRates { get; set; }
        public ContractQuestionaireViewModel ContractQuestionaire { get; set; }

        public List<string> CompanySelected { get; set; }
        public ICollection<KeyValuePairModel<Guid, string>> companyList { get; set; }
        public IDictionary<bool, string> radioIsIntercompanyWorkOrder { get; set; }
        public AutoCompleteReturnViewModel AutoCompleteReturnModel { get; set; }

        public string Displayname { get; set; }
        public string BaseUrl { get; set; }
        public bool IsEditable { get; set; } = true;
        public bool IsNew { get; set; }
    }
}

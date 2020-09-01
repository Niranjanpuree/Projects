using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractQuestionaireViewModel : BaseViewModel
    {
        public Guid ContractQuestionaireGuid { get; set; }
        public Guid ContractGuid { get; set; }


        public bool IsReportExecCompensation { get; set; }

        public string IsReportExecCompensationStatus { get { return IsReportExecCompensation == true ? "Yes" : "No"; } }

        public DateTime? ReportLastReportDate { get; set; }

        public DateTime? ReportNextReportDate { get; set; }

        public bool IsGSAschedulesale { get; set; }
        public string IsGSAschedulesaleStatus { get { return IsGSAschedulesale == true ? "Yes" : "No"; } }

        public DateTime? GSALastReportDate { get; set; }

        public DateTime? GSANextReportDate { get; set; }

        public bool IsSBsubcontract { get; set; }

        public string IsSBsubcontractStatus { get { return IsSBsubcontract == true ? "Yes" : "No"; } }

        public DateTime? SBLastReportDate { get; set; }

        public DateTime? SBNextReportDate { get; set; }

        public bool IsGQAC { get; set; }

        public string IsGQACStatus { get { return IsGQAC == true ? "Yes" : "No"; } }

        public DateTime? GQACLastReportDate { get; set; }

        public DateTime? GQACNextReportDate { get; set; }

        public bool IsCPARS { get; set; }
        public string IsCPARSStatus { get { return IsCPARS == true ? "Yes" : "No"; } }

        public DateTime? CPARSLastReportDate { get; set; }

        public DateTime? CPARSNextReportDate { get; set; }

        public bool IsWarranties { get; set; }
        public string IsWarrantiesStatus { get { return IsWarranties == true ? "Yes" : "No"; } }

        public bool IsStandardIndustryProvision { get; set; }

        public string IsStandardIndustryProvisionStatus { get { return IsStandardIndustryProvision == true ? "Yes" : "No"; } }

        public string WarrantyProvisionDescription { get; set; }
        public string Displayname { get; set; }

        public IDictionary<bool, string> DictionaryBoolString { get; set; }
    }

    public class QuestionaireViewModel {
        public string DisplayName { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}

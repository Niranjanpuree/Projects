using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities
{
    public class ContractQuestionaire : BaseModel
    {
        public Guid ContractQuestionaireGuid { get; set; }

        public Guid ContractGuid { get; set; }

        public bool IsReportExecCompensation { get; set; }

        public DateTime? ReportLastReportDate { get; set; }

        public DateTime? ReportNextReportDate { get; set; }

        public bool IsGSAschedulesale { get; set; }

        public DateTime? GSALastReportDate { get; set; }

        public DateTime? GSANextReportDate { get; set; }

        public bool IsSBsubcontract { get; set; }

        public DateTime? SBLastReportDate { get; set; }

        public DateTime? SBNextReportDate { get; set; }

        public bool IsGQAC { get; set; }

        public DateTime? GQACLastReportDate { get; set; }

        public DateTime? GQACNextReportDate { get; set; }

        public bool IsCPARS { get; set; }

        public DateTime? CPARSLastReportDate { get; set; }

        public DateTime? CPARSNextReportDate { get; set; }

        public bool IsWarranties { get; set; }

        public bool IsStandardIndustryProvision { get; set; }

        public string WarrantyProvisionDescription { get; set; }

    }

    public class Questionaires 
    {
        
        public Guid QuestionGuid { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string Textanswer { get; set; }
        public string YesNoAnswer { get; set; }
        public List<MultiSelectOption> MultiSelectAnswer { get; set; }
        public string Checkedoption { get; set; }
        public DateTime? ReportLastReportDate { get; set; }
        public DateTime? ReportNextReportDate { get; set; }
        public string CheckBoxAnswer { get; set; }
        public string Id { get; set; }
        public string ControlClass { get; set; }
        public string ChildId { get; set; }
        public string ChildYesNoAnswer { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime Updatedon { get; set; }

    }

    public class MultiSelectOption
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public Guid Id { get; set; }
    }

   
}

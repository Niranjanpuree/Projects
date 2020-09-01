using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
   public class QuestionaireUserAnswer
    {
        public Guid QuestionaireUserAnswerGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public Guid QuestionaireMasterGuid { get; set; }
        public Guid ManagerUserGuid { get; set; }
        public Guid ContractCloseApprovalGuid { get; set; }
        public string Questions { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string RepresentativeType { get; set; }
        public string Answer { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}

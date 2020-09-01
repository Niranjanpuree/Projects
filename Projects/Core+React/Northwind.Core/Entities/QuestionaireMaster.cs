using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class QuestionaireMaster
    {
        public Guid QuestionaireMasterGuid { get; set; }
        public Guid ParentMasterGuid { get; set; }
        public int OrderNumber { get; set; }
        public Guid QuestionaireManagerTypeGuid { get; set; }
        public string Questions { get; set; }
        public string ResourceType { get; set; }
        public string QuestionType { get; set; }
        public string Textanswer { get; set; }
        public string MultiSelectoption { get; set; }
        public string Answer { get; set; }
        public DateTime? DateOfLastReport { get; set; }
        public DateTime? DateOfNextReport { get; set; }
        public string CheckBoxAnswer { get; set; }
        public string Id { get; set; }
        public string Class { get; set; }
        public string ChildId { get; set; } 
        public string ChildYesNoAnswer { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}

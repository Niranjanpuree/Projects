using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Questionaire
{

    public class ContractQuestionnaireViewModel
    {
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string Textanswer { get; set; }
        public string YesNoAnswer { get; set; }
        public List<string> MultiSelectAnswer { get; set; }
    }

   
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Questionaire
{

    public class ContractCloseOutViewModel
    {
        public List<QuestionaryAnswerViewModel> ProjectManagerQuestions { get; set; }
        public List<QuestionaryAnswerViewModel> ContractRepresentativeQuestions { get; set; }
        public List<QuestionaryAnswerViewModel> AccountingRepresentativeQuestions { get; set; }
        public List<QuestionaryAnswerViewModel> SubQuestions { get; set; }

        public List<FileUploadModel> ProjetcManagerFiles { get; set; }
        public List<FileUploadModel> ContractRepresentativeFiles { get; set; }
        public List<FileUploadModel> AccountingRepresentativeFiles { get; set; }

        public Guid ContractGuid { get; set; }
        public Guid ParentContractGuid { get; set; }
        public string RepresentativeType { get; set; }
    }

    public class QuestionaryAnswerViewModel
    {
        public Guid QuestionaireMasterGuid { get; set; }
        public Guid QuestionaireManagerTypeGuid { get; set; }
        public Guid ParentMasterGuid { get; set; }
        public string Questions { get; set; }
        public string Notes { get; set; }
        public string RepresentativeType { get; set; }
        public int OrderNumber { get; set; }
        public string Answer { get; set; }
    }
    
    public class FileUploadModel
    {
        public Guid UploadedFileGuid { get; set; }
        public string UploadFileName { get; set; }
        public string Description { get; set; }
    }
}

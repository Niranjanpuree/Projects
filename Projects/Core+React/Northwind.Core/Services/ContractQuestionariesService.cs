using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Core.Services
{
   public class ContractQuestionariesService : IContractQuestionariesService
    {
        private readonly IContractQuestionariesRepository _contractQuestionariesRepository;
        private readonly IUserService _userService;
        public ContractQuestionariesService(IContractQuestionariesRepository contractQuestionariesRepository, IUserService userService)
        {
            _contractQuestionariesRepository = contractQuestionariesRepository;
            _userService = userService;
        }
        public int AddContractQuestionaires(ContractQuestionaire ContractQuestionaire)
        {
            return _contractQuestionariesRepository.AddContractQuestionaires(ContractQuestionaire);
        }
        public bool AddQuestionaires(IList<Questionaires> Questionaire,Guid contractGuid, DateTime CreatedOn, Guid CreatedBy, DateTime UpdatedOn, Guid UpdatedBy)
        {
            
            foreach (var resource in Questionaire)
            {
                List<string> list = new List<string>();
                if (resource.QuestionType == "multi")
                {
                    if (resource.MultiSelectAnswer != null)
                    {
                        foreach (var data in resource.MultiSelectAnswer)
                        {
                            if (data.IsSelected)
                            {
                                list.Add(data.Name);
                            }
                        }
                    }
                    resource.CheckBoxAnswer = string.Join("|", list.ToArray());
                }
            }
            List<Questionaires> FilteredQuestionaire = new List<Questionaires>();

            foreach (Questionaires quest in Questionaire)
            {
                Questionaires filteredquestions = new Questionaires();
                if (quest.YesNoAnswer != null || !string.IsNullOrEmpty(quest.YesNoAnswer) || !string.IsNullOrEmpty(quest.CheckBoxAnswer) || quest.Textanswer !=null)
                {
                    FilteredQuestionaire.Add(quest);
                }
            }

            return _contractQuestionariesRepository.AddQuestionaires(FilteredQuestionaire, contractGuid, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy);
        }
        public ContractQuestionaire GetContractQuestionariesById(Guid id)
        {
            return _contractQuestionariesRepository.GetContractQuestionariesById(id);
        }

        public IEnumerable<Questionaires> GetContractQuestionaries(string resourceType,string Action,Guid ContractGuid)
        {
            var  questionniareMaster = _contractQuestionariesRepository.GetContractQuestionaries(resourceType,Action, ContractGuid);
            List<Questionaires> listQuestionaire = new List<Questionaires>();
            foreach (var data in questionniareMaster)
            {
                Questionaires questionaire = new Questionaires();
                questionaire.Question = data.Questions;
                questionaire.QuestionType = data.QuestionType;
                questionaire.Textanswer = data.Textanswer;
                questionaire.Id = data.Id;
                List<string> multiSelectOptions = data.MultiSelectoption?.Split('|').ToList();
                List<string> checkboxAnswer = data.CheckBoxAnswer?.Split('|').ToList();
                List<MultiSelectOption> option = new List<MultiSelectOption>();
                
                if (multiSelectOptions != null)
                {
                    foreach (var resource in multiSelectOptions)
                    {
                        MultiSelectOption opt = new MultiSelectOption();
                        opt.Name = resource;
                        if (checkboxAnswer != null)
                        {
                            if (checkboxAnswer.Contains(resource))
                            {
                                opt.IsSelected = true;
                            }
                            else
                            {
                                opt.IsSelected = false;
                            }
                            opt.Id = Guid.NewGuid();
                        }
                        else {
                            opt.IsSelected = false;
                        }

                        option.Add(opt);
                    }
                }
                questionaire.MultiSelectAnswer = option;
                questionaire.YesNoAnswer = data.Answer;
                questionaire.QuestionGuid = data.QuestionaireMasterGuid;
                questionaire.ReportLastReportDate = data.DateOfLastReport;
                questionaire.ReportNextReportDate = data.DateOfNextReport;
                questionaire.ControlClass = data.Class;
                questionaire.ChildId = data.ChildId;
                questionaire.ChildYesNoAnswer = data.ChildYesNoAnswer;
                questionaire.CheckBoxAnswer = data.CheckBoxAnswer;
                questionaire.Updatedon = data.UpdatedOn;
                questionaire.UpdatedBy = _userService.GetUserByUserGuid(data.UpdatedBy)?.DisplayName; 
               // questionaire.Checkedoption = data.CheckBoxAnswer;
                listQuestionaire.Add(questionaire);
            }
            return listQuestionaire;
        }

        public int UpdateContractQuestionairesById(ContractQuestionaire ContractQuestionaire)
        {
            return _contractQuestionariesRepository.UpdateContractQuestionairesById(ContractQuestionaire);
        }
    }
}

using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
   public class QuestionaireMasterService : IQuestionaireMasterService
    {
        private readonly IQuestionaireMasterRepository _questionaireMasterRepository;
        public QuestionaireMasterService(IQuestionaireMasterRepository questionaireMasterRepository)
        {
            _questionaireMasterRepository = questionaireMasterRepository;
        }

        public IEnumerable<QuestionaireMaster> GetAll(string resourceType)
        {
            return _questionaireMasterRepository.GetAll(resourceType);
        }

        public QuestionaireMaster GetQuestionaireByQuestion(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return null;
            return _questionaireMasterRepository.GetQuestionaireByQuestion(question);
        }
        //public IEnumerable<QuestionaireMaster> GetQuestionsByManagerType(Guid managerType, string resourceType)
        //{
        //    return _iQuestionaireMasterRepository.GetQuestionsByManagerType(managerType, resourceType);
        //}
    }
}

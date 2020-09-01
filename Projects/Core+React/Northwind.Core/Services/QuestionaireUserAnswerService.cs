using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
   public class QuestionaireUserAnswerService : IQuestionaireUserAnswerService
    {
        private readonly IQuestionaireUserAnswerRepository _questionaireUserAnswerRepository;
        public QuestionaireUserAnswerService(IQuestionaireUserAnswerRepository questionaireUserAnswerRepository)
        {
            _questionaireUserAnswerRepository = questionaireUserAnswerRepository;
        }

        public void Add(QuestionaireUserAnswer answer)
        {
            _questionaireUserAnswerRepository.Add(answer);
        }

        public IEnumerable<QuestionaireUserAnswer> GetByContractAndUserGuid(Guid contractGuid, Guid userGuid)
        {
            if ((contractGuid == null || contractGuid == Guid.Empty) && (userGuid == null || userGuid == Guid.Empty))
            {
                return null;
            }
            return _questionaireUserAnswerRepository.GetByContractAndUserGuid(contractGuid,userGuid);
        }

        public IEnumerable<QuestionaireUserAnswer> GetByContractGuid(Guid contractGuid)
        {
            return _questionaireUserAnswerRepository.GetByContractGuid(contractGuid);
        }

        public QuestionaireUserAnswer GetDetailById(Guid contractGuid, string resourceType)
        {
            return _questionaireUserAnswerRepository.GetDetailById(contractGuid, resourceType);
        }

        public IEnumerable<Guid> GetDistinctUserGuid(Guid contractGuid)
        {
            return _questionaireUserAnswerRepository.GetDistinctUserGuid(contractGuid);
        }

        public bool CheckQAByContractGuid(Guid contractGuid)
        {
            if (contractGuid == null)
                return false;
            return _questionaireUserAnswerRepository.CheckQAByContractGuid(contractGuid);
        }
    }
}

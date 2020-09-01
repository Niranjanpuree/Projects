using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface IQuestionaireUserAnswerRepository
    {
        IEnumerable<QuestionaireUserAnswer> GetByContractAndUserGuid(Guid contractGuid, Guid userGuid);
        IEnumerable<Guid> GetDistinctUserGuid(Guid contractGuid);
        IEnumerable<QuestionaireUserAnswer> GetByContractGuid(Guid contractGuid);
        void Add(QuestionaireUserAnswer answer);
        QuestionaireUserAnswer GetDetailById(Guid contractGuid,string resourceType);
        bool CheckQAByContractGuid(Guid contractGuid);
    }
}

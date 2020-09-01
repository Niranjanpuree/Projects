using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IQuestionaireUserAnswerService
    {
        IEnumerable<QuestionaireUserAnswer> GetByContractAndUserGuid(Guid contractGuid, Guid userGuid);
        IEnumerable<QuestionaireUserAnswer> GetByContractGuid(Guid contractGuid);
        IEnumerable<Guid> GetDistinctUserGuid(Guid contractGuid);
        void Add(QuestionaireUserAnswer answer);
        QuestionaireUserAnswer GetDetailById(Guid contractGuid, string resourceType);
        bool CheckQAByContractGuid(Guid contractGuid);
    }
}

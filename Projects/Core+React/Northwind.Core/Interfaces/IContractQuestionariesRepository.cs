using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface IContractQuestionariesRepository
    {
        int AddContractQuestionaires(ContractQuestionaire ContractQuestionaire);
        bool AddQuestionaires(IList<Questionaires> Questionaire,Guid contractGuid, DateTime CreatedOn, Guid CreatedBy, DateTime UpdatedOn, Guid UpdatedBy);
        ContractQuestionaire GetContractQuestionariesById(Guid id);
        IEnumerable<QuestionaireMaster> GetContractQuestionaries(string ResourceType,string Action,Guid ContractGuid );
        int UpdateContractQuestionairesById(ContractQuestionaire ContractQuestionaire);
    }
}

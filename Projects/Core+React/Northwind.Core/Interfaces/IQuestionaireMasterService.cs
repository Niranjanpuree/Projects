using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IQuestionaireMasterService
    {
        IEnumerable<QuestionaireMaster> GetAll(string resourceType);
        QuestionaireMaster GetQuestionaireByQuestion(string question);
        //IEnumerable<QuestionaireMaster> GetQuestionsByManagerType(Guid managerType, string resourceType);
    }
}

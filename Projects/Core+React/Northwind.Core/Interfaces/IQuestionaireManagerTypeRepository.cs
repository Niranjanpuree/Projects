using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IQuestionaireManagerTypeRepository
    {
        Guid GetGuidByNormailzedName(string managerTypeNormalize);
        string GetNameByGuid(Guid id);
    }
}

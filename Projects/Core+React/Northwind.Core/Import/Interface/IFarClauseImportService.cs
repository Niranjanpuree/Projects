using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IFarClauseImportService
    {
        void ImportFarClauseData(object configuration, Guid userGuid, string errorPath,bool isDelete);
        void ImportQuestionaireData(object configuration, Guid userGuid, string errorPath, bool isDelete);
    }
}

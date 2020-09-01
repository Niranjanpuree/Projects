using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IImportService
    {
        void ImportData(string filePath, Guid userGuid, string errorLogPath);
        void ImportAttachment(string filePath, Guid userGuid, string errorLogPath);
    }
}

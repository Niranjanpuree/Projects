using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IAttachmentImportService
    {
        void ImportAttachment(FileConfiguration configuration, Guid userGuid, string errorLogPath,bool isDelete);
    }
}

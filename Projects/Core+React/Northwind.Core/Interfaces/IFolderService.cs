using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IFolderService
    {
        void CreateFolderTemplate(string contractGuid,string contractNumber, string resourceType, Guid resourceId,Guid userGuid);
    }
}

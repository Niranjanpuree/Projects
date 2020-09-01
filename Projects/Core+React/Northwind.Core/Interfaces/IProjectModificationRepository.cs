using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface IProjectModificationRepository
    {
        IEnumerable<ProjectModificationModel> GetAll(Guid projectGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(Guid projectGuid);
        int Add(ProjectModificationModel projectModificationModel);
        int Edit(ProjectModificationModel projectModificationModel);
        int Delete(Guid[] ids);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);
        ProjectModificationModel GetDetailById(Guid id);
        bool IsExistModificationNumber(Guid projectGuid, string modificationNumber);
    }
}
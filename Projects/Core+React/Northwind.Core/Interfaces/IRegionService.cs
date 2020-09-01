using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface IRegionService
    {
        IEnumerable<Region> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string sortDirection);
        int GetCount(string searchValue);
        int CheckDuplicates(Region region);
        int Add(Region region);
        int AddUpdateManager(RegionUserRoleMapping regionManager);
        //int UpdateManager(RegionUserRoleMapping regionManager);
        int Edit(Region region);
        int Delete(Guid[] ids);
        int DeleteById(Guid id);
        Region GetById(Guid id);
        Region GetDetailsById(Guid id);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);

        Region GetRegionByCode(string regionCode);

        Region GetRegionByCodeOrName(string regionCode, string regionName);

        int CheckDuplicateRegionByName(string regionName, Guid regionGuid);

        IEnumerable<Region> GetRegionList();
    }
}

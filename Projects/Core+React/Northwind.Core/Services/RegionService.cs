using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public IEnumerable<Region> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string sortDirection)
        {
            IEnumerable<Region> getall = _regionRepository.GetAll(searchValue, pageSize, skip,  take, sortField, sortDirection);
            return getall;
        }
        public int GetCount(string searchValue)
        {
            int totalRecord = _regionRepository.GetCount(searchValue);
            return totalRecord;
        }
        public int CheckDuplicates(Region Region)
        {
            int count = _regionRepository.CheckDuplicates(Region);
            return count;
        }
        public int Add(Region Region)
        {
            return _regionRepository.Add(Region);
        }
        public int AddUpdateManager(RegionUserRoleMapping RegionManager)
        {
            return _regionRepository.AddUpdateManager(RegionManager);
        }
        //public int UpdateManager(RegionUserRoleMapping RegionManager)
        //{
        //    return _regionRepository.AddUpdateManager(RegionManager);
        //}
        public int Edit(Region Region)
        {
            return _regionRepository.Edit(Region);
        }
        public Region GetById(Guid id)
        {
            return _regionRepository.GetById(id);
        }
        public Region GetDetailsById(Guid id)
        {
            return _regionRepository.GetDetailsById(id);
        }
        public int Delete(Guid[] ids)
        {
            return _regionRepository.Delete(ids);
        }
        public int Disable(Guid[] ids)
        {
            return _regionRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return _regionRepository.Enable(ids);
        }

        public Region GetRegionByCode(string regionCode)
        {
            return _regionRepository.GetRegionByCode(regionCode);
        }

        public Region GetRegionByCodeOrName(string regionCode, string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionCode) || string.IsNullOrWhiteSpace(regionName))
                return null;
            return _regionRepository.GetRegionByCodeOrName(regionCode.Trim(), regionName.Trim());
        }

        public int CheckDuplicateRegionByName(string regionName, Guid regionGuid)
        {
            return _regionRepository.CheckDuplicateRegionByName(regionName, regionGuid);
        }

        public IEnumerable<Region> GetRegionList()
        {
            return _regionRepository.GetRegionList();
        }

        public int DeleteById(Guid id)
        {
            return _regionRepository.DeleteById(id);
        }
    }
}

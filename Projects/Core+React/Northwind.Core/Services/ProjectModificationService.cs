
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Services
{
    public class ProjectModificationService : IProjectModificationService
    {
        private readonly IProjectModificationRepository _iProjectModificationRepository;
        public ProjectModificationService(IProjectModificationRepository projectModificationRepository)
        {
            _iProjectModificationRepository = projectModificationRepository;
        }
        public IEnumerable<ProjectModificationModel> GetAll(Guid projectGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<ProjectModificationModel> getAll = _iProjectModificationRepository.GetAll(projectGuid, searchValue, pageSize, skip, sortField, sortDirection);
            return getAll;
        }
        public int TotalRecord(Guid projectGuid)
        {
            int totalRecord = _iProjectModificationRepository.TotalRecord(projectGuid);
            return totalRecord;
        }
        public int Add(ProjectModificationModel projectModificationModel)
        {
            return _iProjectModificationRepository.Add(projectModificationModel);
        }
        public int Edit(ProjectModificationModel projectModificationModel)
        {
            return _iProjectModificationRepository.Edit(projectModificationModel);
        }
        public int Delete(Guid[] ids)
        {
            return _iProjectModificationRepository.Delete(ids);
        }
        public int Disable(Guid[] ids)
        {
            return _iProjectModificationRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return _iProjectModificationRepository.Enable(ids);
        }

        public ProjectModificationModel GetDetailById(Guid id)
        {
            return _iProjectModificationRepository.GetDetailById(id);
        }
        public bool IsExistModificationNumber(Guid projectGuid, string modificationNumber)
        {
            return _iProjectModificationRepository.IsExistModificationNumber(projectGuid, modificationNumber);
        }
    }
}

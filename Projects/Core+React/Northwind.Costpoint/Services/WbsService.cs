using Northwind.Core.Models;
using Northwind.Costpoint.Entities;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Services
{
    public class WbsServiceCP : IWbsServiceCP
    {
        IWbsRepositoryCP _wbsRepository;
        public WbsServiceCP(IWbsRepositoryCP wbsRepository)
        {
            _wbsRepository = wbsRepository;
        }

        public IEnumerable<WbsCP> GetWbs(string projectNumber, string searchValue,int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue)
        {
            var searchSpec = new SearchSpecCP
            {
                AdvancedSearchCriteria = postValue,
                Direction = dir,
                OrderBy = orderBy,
                ProjectNumber = projectNumber,
                SearchText = searchValue,
                Skip = skip,
                Take = take
            };
            return _wbsRepository.GetWbs(searchSpec);
        }

        public int GetCount(string projectNumber, string searchValue, List<AdvancedSearchRequest> postValue)
        {
            var searchSpec = new SearchSpecCP
            {
                AdvancedSearchCriteria = postValue,
                ProjectNumber = projectNumber,
                SearchText = searchValue
            };
            return _wbsRepository.GetCount(searchSpec);
        }

        public WbsCP GetById(Guid Id)
        {
            return _wbsRepository.GetById(Id);
        }

    }
}

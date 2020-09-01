using Northwind.Core.Entities;
using Northwind.Core.Models;
using Northwind.Costpoint.Entities;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Services
{
    public class CostServiceCP : ICostServiceCP
    {

        ICostRepositoryCP _costRepository;
        public CostServiceCP(ICostRepositoryCP costRepository)
        {
            _costRepository = costRepository;
        }

        public CostCP GetById(long Id)
        {
            return _costRepository.GetById(Id);
        }

        public IEnumerable<CostCP> GetCosts(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                postValue.Add(new AdvancedSearchRequest
                {
                    Attribute = new AdvancedSearchAttribute
                    {
                        AttributeName = "TransactionDate",
                        AttributeType = (int)ResourceAttributeType.DateTime
                    },
                    IsEntity = false,
                    Operator = new AdvancedSearchOperator
                    {
                        OperatorName = (int)OperatorName.DateTimeBetween,
                        OperatorType = (int)ResourceAttributeType.DateTime
                    },
                    Value = startDate,
                    Value2 = endDate
                });
            }
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
            return _costRepository.GetCosts(searchSpec);
        }

        public IEnumerable<dynamic> GetCostForPieChart(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                postValue.Add(new AdvancedSearchRequest
                {
                    Attribute = new AdvancedSearchAttribute
                    {
                        AttributeName = "TransactionDate",
                        AttributeType = (int)ResourceAttributeType.DateTime
                    },
                    IsEntity = false,
                    Operator = new AdvancedSearchOperator
                    {
                        OperatorName = (int)OperatorName.DateTimeBetween,
                        OperatorType = (int)ResourceAttributeType.DateTime
                    },
                    Value = startDate,
                    Value2 = endDate
                });
            }
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
            return _costRepository.GetCostForPieChart(searchSpec);
        }

        public IEnumerable<dynamic> GetCostForBarChart(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                postValue.Add(new AdvancedSearchRequest
                {
                    Attribute = new AdvancedSearchAttribute
                    {
                        AttributeName = "TransactionDate",
                        AttributeType = (int)ResourceAttributeType.DateTime
                    },
                    IsEntity = false,
                    Operator = new AdvancedSearchOperator
                    {
                        OperatorName = (int)OperatorName.DateTimeBetween,
                        OperatorType = (int)ResourceAttributeType.DateTime
                    },
                    Value = startDate,
                    Value2 = endDate
                });
            }
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
            return _costRepository.GetCostForBarChart(searchSpec);
        }

        public int GetCount(string projectNumber, string searchValue, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                postValue.Add(new AdvancedSearchRequest
                {
                    Attribute = new AdvancedSearchAttribute
                    {
                        AttributeName = "TransactionDate",
                        AttributeType = (int)ResourceAttributeType.DateTime
                    },
                    IsEntity = false,
                    Operator = new AdvancedSearchOperator
                    {
                        OperatorName = (int)OperatorName.DateTimeBetween,
                        OperatorType = (int)ResourceAttributeType.DateTime
                    },
                    Value = startDate,
                    Value2 = endDate
                });
            }
            var searchSpec = new SearchSpecCP
            {
                AdvancedSearchCriteria = postValue,
                ProjectNumber = projectNumber,
                SearchText = searchValue
            };
            return _costRepository.GetCount(searchSpec);
        }
    }
}

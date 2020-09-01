
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Services
{
    public class RevenueRecognitionService : IRevenueRecognitionService
    {
        private readonly IRevenueRecognitionRepository _iRevenueRecognitionRepository;
        public RevenueRecognitionService(IRevenueRecognitionRepository RevenueRecognitionRepository)
        {
            _iRevenueRecognitionRepository = RevenueRecognitionRepository;
        }
        public IEnumerable<RevenueRecognition> GetAll(Guid ContractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<RevenueRecognition> getall = _iRevenueRecognitionRepository.GetAll(ContractGuid, searchValue, pageSize, skip, sortField, sortDirection);
            return getall;
        }
        public int TotalRecord(Guid ContractGuid)
        {
            int totalRecord = _iRevenueRecognitionRepository.TotalRecord(ContractGuid);
            return totalRecord;
        }
        public int UpdateRevenueRecognition(RevenueRecognition RevenueRecognition)
        {
            return _iRevenueRecognitionRepository.UpdateRevenueRecognition(RevenueRecognition);
        }
        public int UpdateIsNotify(Guid id)
        {
            return _iRevenueRecognitionRepository.UpdateIsNotify(id);
        }
        public int DeleteRevenueRecognition(Guid Ids)
        {
            return _iRevenueRecognitionRepository.DeleteRevenueRecognition(Ids);
        }
        public int DisableRevenueRecognition(Guid[] ids)
        {
            return _iRevenueRecognitionRepository.DisableRevenueRecognition(ids);
        }
        public int EnableRevenueRecognition(Guid[] ids)
        {
            return _iRevenueRecognitionRepository.EnableRevenueRecognition(ids);
        }
        public RevenueRecognition GetDetailsById(Guid id)
        {
            return _iRevenueRecognitionRepository.GetDetailsById(id);
        }
     
        public List<RevenueContractExtension> GetContractExtension(Guid id)
        {
            return _iRevenueRecognitionRepository.GetContractExtension(id);
        }
        public List<RevenuePerformanceObligation> GetPerformanceObligation(Guid id)
        {
            return _iRevenueRecognitionRepository.GetPerformanceObligation(id);
        }
        public int DisableRevenueExtensionPeriod(Guid[] ids)
        {
            return _iRevenueRecognitionRepository.DisableRevenueExtensionPeriod(ids);
        }
        public int DisableRevenueObligation(Guid[] ids)
        {
            return _iRevenueRecognitionRepository.DisableRevenueObligation(ids);
        }

        public bool AddContractExtension(List<RevenueContractExtension> contractExtensions)
        {
            return _iRevenueRecognitionRepository.AddContractExtension(contractExtensions);
        }

        public bool AddPerformanceObligation(List<RevenuePerformanceObligation> performanceObligations)
        {
            return _iRevenueRecognitionRepository.AddPerformanceObligation(performanceObligations);
        }

        public int UpdateContractExtension(List<RevenueContractExtension> contractExtensions)
        {
            return _iRevenueRecognitionRepository.UpdateContractExtension(contractExtensions);
        }

        public int UpdatePerformanceObligation(List<RevenuePerformanceObligation> performanceObligations)
        {
            return _iRevenueRecognitionRepository.UpdatePerformanceObligation(performanceObligations);
        }

        public Guid GetAccountRepresentive(Guid revenueGuid)
        {
            return _iRevenueRecognitionRepository.GetAccountRepresentive(revenueGuid);
        }

        public RevenueTriggeredDetailModel GetAwardAmountDetail(Guid? contractGuid)
        {
            return _iRevenueRecognitionRepository.GetAwardAmountDetail(contractGuid);
        }

        public bool AddRevenueWithResourceGuid(RevenueRecognition revenueRecognition)
        {
            return _iRevenueRecognitionRepository.AddRevenueWithResourceGuid(revenueRecognition);
        }

        RevenueRecognition IRevenueRecognitionService.GetInfoForDetailPage(Guid revenueRecognizationGuid)
        {
            return _iRevenueRecognitionRepository.GetInfoForDetailPage(revenueRecognizationGuid);
        }

        public List<RevenueRecognition> DetailList(Guid id, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            return _iRevenueRecognitionRepository.DetailList(id,searchValue,pageSize,skip,take,sortField,dir);
        }

        public int DetailListCount(Guid id, string searchValue)
        {
            return _iRevenueRecognitionRepository.DetailListCount(id, searchValue);
        }

        public int CountRevenueByContractGuid(Guid id)
        {
            return _iRevenueRecognitionRepository.CountRevenueByContractGuid(id);
        }
    }
}

using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using static Northwind.Core.Entities.RevenueRecognition;

namespace Northwind.Core.Interfaces
{
    public interface IRevenueRecognitionService
    {
        IEnumerable<RevenueRecognition> GetAll(Guid ContractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);

        int TotalRecord(Guid ContractGuid);

        int UpdateRevenueRecognition(RevenueRecognition RevenueRecognition);

        bool AddContractExtension(List<RevenueContractExtension> contractExtensions);

        bool AddPerformanceObligation(List<RevenuePerformanceObligation> performanceObligations);

        int UpdateContractExtension(List<RevenueContractExtension> contractExtensions);

        int UpdateIsNotify(Guid id);

        int UpdatePerformanceObligation(List<RevenuePerformanceObligation> performanceObligations);

        int DeleteRevenueRecognition(Guid ids);

        int DisableRevenueRecognition(Guid[] ids);

        int EnableRevenueRecognition(Guid[] ids);

        RevenueRecognition GetDetailsById(Guid id);

        int CountRevenueByContractGuid(Guid id);

        List<RevenueContractExtension> GetContractExtension(Guid id);

        List<RevenuePerformanceObligation> GetPerformanceObligation(Guid id);

        int DisableRevenueExtensionPeriod(Guid[] id);

        int DisableRevenueObligation(Guid[] id);

        Guid GetAccountRepresentive(Guid revenueGuid);

        List<RevenueRecognition> DetailList(Guid id, string searchValue, int pageSize, int skip, int take, string sortField, string dir);

        RevenueRecognition GetInfoForDetailPage(Guid revenueRecognizationGuid);

        RevenueTriggeredDetailModel GetAwardAmountDetail(Guid? contractGuid);

        bool AddRevenueWithResourceGuid(RevenueRecognition revenueRecognition);

        int DetailListCount(Guid id, string searchValue);

    }
}
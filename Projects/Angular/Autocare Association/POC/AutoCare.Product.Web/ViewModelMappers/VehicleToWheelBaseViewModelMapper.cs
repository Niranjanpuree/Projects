using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleToWheelBaseViewModelMapper : IVehicleToWheelBaseViewModelMapper
    {
        public List<VehicleToWheelBaseViewModel> Map(VehicleToWheelBaseSearchResult result)
        {
            Guid guid;
            return result.Documents.Where(item => !Guid.TryParse(item.VehicleToWheelBaseId, out guid)).Select(item => new VehicleToWheelBaseViewModel
            {
                Id = Convert.ToInt32(item.VehicleToWheelBaseId),
                WheelBase = new WheelBaseViewModel
                {
                    Id = item.WheelBaseId ?? 0,
                    Base = item.WheelBaseName,
                    WheelBaseMetric = item.WheelBaseMetric,
                    ChangeRequestId = item.WheelBaseChangeRequestId
                },
                ChangeRequestId = item.VehicleToWheelBaseChangeRequestId,
                Vehicle = new VehicleViewModel
                {
                    BaseVehicleId = item.BaseVehicleId ?? 0,
                    Id = Convert.ToInt32(item.VehicleId),
                    MakeId = item.MakeId ?? 0,
                    MakeName = item.MakeName,
                    ModelId = item.ModelId ?? 0,
                    ModelName = item.ModelName,
                    RegionId = item.RegionId ?? 0,
                    RegionName = item.RegionName,
                    SourceName = item.Source,
                    SubModelId = item.SubModelId ?? 0,
                    SubModelName = item.SubModelName,
                    YearId = item.YearId ?? 0,
                },
                WheelBaseId = item.WheelBaseId ?? 0,
                VehicleId = item.VehicleId ?? 0,
                WheelBaseName = item.WheelBaseName,
            }).ToList();
        }
    }
}
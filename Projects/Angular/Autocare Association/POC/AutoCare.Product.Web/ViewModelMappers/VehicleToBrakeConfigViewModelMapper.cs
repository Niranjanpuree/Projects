using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleToBrakeConfigViewModelMapper : IVehicleToBrakeConfigViewModelMapper
    {
        public List<VehicleToBrakeConfigViewModel> Map(VehicleToBrakeConfigSearchResult result)
        {
            Guid guid;
            return result.Documents.Where(item => !Guid.TryParse(item.VehicleToBrakeConfigId, out guid)).Select(item => new VehicleToBrakeConfigViewModel
            {
                Id = Convert.ToInt32(item.VehicleToBrakeConfigId),
                BrakeConfig = new BrakeConfigViewModel
                {
                    Id = item.BrakeConfigId ?? 0,
                    FrontBrakeTypeId = item.FrontBrakeTypeId ?? 0,
                    BrakeSystemId = item.BrakeSystemId ?? 0,
                    BrakeABSId = item.BrakeABSId ?? 0,
                    RearBrakeTypeId = item.RearBrakeTypeId ?? 0,
                    FrontBrakeTypeName = item.FrontBrakeTypeName,
                    RearBrakeTypeName = item.RearBrakeTypeName,
                    BrakeSystemName = item.BrakeSystemName,
                    BrakeABSName = item.BrakeABSName,
                    ChangeRequestId = item.BrakeConfigChangeRequestId,
                },
                ChangeRequestId = item.VehicleToBrakeConfigChangeRequestId,
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
                BrakeConfigId = item.BrakeConfigId ?? 0,
                VehicleId = item.VehicleId ?? 0,
                FrontBrakeTypeId = item.FrontBrakeTypeId ?? 0,
                FrontBrakeTypeName = item.FrontBrakeTypeName,
                RearBrakeTypeId = item.RearBrakeTypeId ?? 0,
                RearBrakeTypeName = item.RearBrakeTypeName,
                BrakeSystemId = item.BrakeSystemId ?? 0,
                BrakeSystemName = item.BrakeSystemName,
                BrakeABSId = item.BrakeABSId ?? 0,
                BrakeABSName = item.BrakeABSName,
            }).ToList();
        }
    }
}
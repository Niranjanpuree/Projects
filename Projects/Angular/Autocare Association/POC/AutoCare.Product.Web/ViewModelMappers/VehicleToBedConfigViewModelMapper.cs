using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleToBedConfigViewModelMapper : IVehicleToBedConfigViewModelMapper
    {
        public List<VehicleToBedConfigViewModel> Map(VehicleToBedConfigSearchResult result)
        {
            Guid guid;
            return result.Documents.Where(item => !Guid.TryParse(item.VehicleToBedConfigId, out guid)).Select(item => new VehicleToBedConfigViewModel
            {
                Id = Convert.ToInt32(item.VehicleToBedConfigId),
                BedConfig = new BedConfigViewModel
                {
                    Id = item.BedConfigId ?? 0,
                    BedLengthId = item.BedLengthId ?? 0,
                    Length = item.BedLength,
                    BedTypeId = item.BedTypeId ?? 0,
                    BedTypeName = item.BedTypeName,
                    ChangeRequestId = item.BedConfigChangeRequestId,
                },
                ChangeRequestId = item.VehicleToBedConfigChangeRequestId,
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
                BedConfigId = item.BedConfigId ?? 0,
                VehicleId = item.VehicleId ?? 0,
                BedLengthId = item.BedLengthId ?? 0,
                Length = item.BedLength,
                BedTypeId = item.BedTypeId ?? 0,
                BedTypeName = item.BedTypeName
            }).ToList();
        }
    }
}
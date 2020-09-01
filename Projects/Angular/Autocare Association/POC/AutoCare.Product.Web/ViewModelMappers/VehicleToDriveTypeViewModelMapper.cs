using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleToDriveTypeViewModelMapper : IVehicleToDriveTypeViewModelMapper
    {
        public List<VehicleToDriveTypeViewModel> Map(VehicleToDriveTypeSearchResult result)
        {
            Guid guid;
            return result.Documents.Where(item => !Guid.TryParse(item.VehicleToDriveTypeId, out guid)).Select(item => new VehicleToDriveTypeViewModel
            {
                Id = Convert.ToInt32(item.VehicleToDriveTypeId),
                DriveType = new DriveTypeViewModel
                {
                    Id = item.DriveTypeId ?? 0,
                    Name = item.DriveTypeName,
                    ChangeRequestId = item.DriveTypeChangeRequestId,
                },
                ChangeRequestId = item.VehicleToDriveTypeChangeRequestId,
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
                DriveTypeId = item.DriveTypeId ?? 0,
                VehicleId = item.VehicleId ?? 0,
                Name = item.DriveTypeName,
            }).ToList();
        }

    }
}
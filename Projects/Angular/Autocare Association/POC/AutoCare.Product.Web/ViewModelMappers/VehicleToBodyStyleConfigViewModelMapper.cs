using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleToBodyStyleConfigViewModelMapper: IVehicleToBodyStyleConfigViewModelMapper
    {
        public List<VehicleToBodyStyleConfigViewModel> Map(VehicleToBodyStyleConfigSearchResult result)
        {
            Guid guid;
            return result.Documents.Where(item => !Guid.TryParse(item.VehicleToBodyStyleConfigId, out guid)).Select(item => new VehicleToBodyStyleConfigViewModel
            {
                Id = Convert.ToInt32(item.VehicleToBodyStyleConfigId),
                BodyStyleConfig = new BodyStyleConfigViewModel
                {
                    Id = item.BodyStyleConfigId ?? 0,
                    BodyNumDoorsId = item.BodyNumDoorsId ?? 0,
                    NumDoors = item.BodyNumDoors,
                    BodyTypeId = item.BodyTypeId ?? 0,
                    BodyTypeName = item.BodyTypeName,
                    ChangeRequestId = item.BodyStyleConfigChangeRequestId
                },
                ChangeRequestId = item.VehicleToBodyStyleConfigChangeRequestId,
                Vehicle = new VehicleViewModel
                {
                    BaseVehicleId = item.BaseVehicleId ?? 0,
                    Id = item.VehicleId ?? 0,
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
                BodyStyleConfigId = item.BodyStyleConfigId ?? 0,
                VehicleId = item.VehicleId ?? 0,
                BodyNumberDoorsId = item.BodyNumDoorsId ?? 0,
                BodyNumDoors = item.BodyNumDoors,
                BodyTypeId = item.BodyTypeId ?? 0,
                BodyTypeName = item.BodyTypeName
            }).ToList();
        }
    }
}
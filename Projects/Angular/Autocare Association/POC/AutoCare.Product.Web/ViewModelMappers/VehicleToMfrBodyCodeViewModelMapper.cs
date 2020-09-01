using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleToMfrBodyCodeViewModelMapper : IVehicleToMfrBodyCodeViewModelMapper
    {
        public List<VehicleToMfrBodyCodeViewModel> Map(VehicleToMfrBodyCodeSearchResult result)
        {
            Guid guid;
            return result.Documents.Where(item => !Guid.TryParse(item.VehicleToMfrBodyCodeId, out guid)).Select(item => new VehicleToMfrBodyCodeViewModel
            {
                Id = Convert.ToInt32(item.VehicleToMfrBodyCodeId),
                MfrBodyCode = new MfrBodyCodeViewModel
                {
                    Id = item.MfrBodyCodeId ?? 0,
                    Name = item.MfrBodyCodeName,
                    ChangeRequestId = item.MfrBodyCodeChangeRequestId,
                },
                ChangeRequestId = item.VehicleToMfrBodyCodeChangeRequestId,
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
                MfrBodyCodeId = item.MfrBodyCodeId ?? 0,
                VehicleId = item.VehicleId ?? 0,
                Name = item.MfrBodyCodeName,
            }).ToList();
        }

    }
}
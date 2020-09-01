using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;
using System.Collections.Generic;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public interface IVehicleToBrakeConfigViewModelMapper //: IViewModelMapper<VehicleToBrakeConfigSearchResult, VehicleToBrakeConfigSearchViewModel>
    {
        List<VehicleToBrakeConfigViewModel> Map(VehicleToBrakeConfigSearchResult result);
    }
}

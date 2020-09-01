using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;
using System.Collections.Generic;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public interface IVehicleToBedConfigViewModelMapper //: IViewModelMapper<VehicleToBrakeConfigSearchResult, VehicleToBrakeConfigSearchViewModel>
    {
        List<VehicleToBedConfigViewModel> Map(VehicleToBedConfigSearchResult result);
    }
}

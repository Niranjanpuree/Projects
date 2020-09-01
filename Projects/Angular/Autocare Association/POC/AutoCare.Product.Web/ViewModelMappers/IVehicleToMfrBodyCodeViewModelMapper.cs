using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.ViewModels;
using System.Collections.Generic;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public interface IVehicleToMfrBodyCodeViewModelMapper
    {
        List<VehicleToMfrBodyCodeViewModel> Map(VehicleToMfrBodyCodeSearchResult result);
    }
}

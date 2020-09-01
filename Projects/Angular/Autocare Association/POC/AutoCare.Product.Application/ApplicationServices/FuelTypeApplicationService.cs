using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class FuelTypeApplicationService : VcdbApplicationService<FuelType>, IFuelTypeApplicationService
    {
        public FuelTypeApplicationService(IFuelTypeBusinessService fuelTypeBusinessService)
            :base(fuelTypeBusinessService)
        {
        }
    }
}

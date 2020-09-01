using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class FuelSystemControlTypeApplicationService : VcdbApplicationService<FuelSystemControlType>, IFuelSystemControlTypeApplicationService
    {
        public FuelSystemControlTypeApplicationService(IFuelSystemControlTypeBusinessService fuelSystemControlTypeBusinessService)
            :base(fuelSystemControlTypeBusinessService)
        {
        }
    }
}

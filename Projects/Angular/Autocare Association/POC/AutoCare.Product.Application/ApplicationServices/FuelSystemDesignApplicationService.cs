using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class FuelSystemDesignApplicationService : VcdbApplicationService<FuelSystemDesign>, IFuelSystemDesignApplicationService
    {
        public FuelSystemDesignApplicationService(IFuelSystemDesignBusinessService fuelSystemDesignBusinessService)
            :base(fuelSystemDesignBusinessService)
        {
        }
    }
}

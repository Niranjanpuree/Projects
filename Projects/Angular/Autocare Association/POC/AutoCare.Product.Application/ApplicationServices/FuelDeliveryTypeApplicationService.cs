using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class FuelDeliveryTypeApplicationService : VcdbApplicationService<FuelDeliveryType>, IFuelDeliveryTypeApplicationService
    {
        public FuelDeliveryTypeApplicationService(IFuelDeliveryTypeBusinessService fuelDeliveryTypeBusinessService)
            :base(fuelDeliveryTypeBusinessService)
        {
        }
    }
}

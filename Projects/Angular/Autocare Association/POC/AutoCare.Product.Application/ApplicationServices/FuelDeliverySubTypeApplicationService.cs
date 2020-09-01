using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class FuelDeliverySubTypeApplicationService : VcdbApplicationService<FuelDeliverySubType>, IFuelDeliverySubTypeApplicationService
    {
        public FuelDeliverySubTypeApplicationService(IFuelDeliverySubTypeBusinessService fuelDeliverySubTypeBusinessService)
            :base(fuelDeliverySubTypeBusinessService)
        {
        }
    }
}

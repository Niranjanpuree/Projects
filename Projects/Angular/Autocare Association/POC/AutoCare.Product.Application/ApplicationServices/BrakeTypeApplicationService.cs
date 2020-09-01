using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BrakeTypeApplicationService : VcdbApplicationService<BrakeType>, IBrakeTypeApplicationService
    {
        public BrakeTypeApplicationService(IBrakeTypeBusinessService brakeTypeBusinessService)
            :base(brakeTypeBusinessService)
        {
        }
    }
}

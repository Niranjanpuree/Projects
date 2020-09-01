using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BrakeSystemApplicationService : VcdbApplicationService<BrakeSystem>, IBrakeSystemApplicationService
    {
        public BrakeSystemApplicationService(IBrakeSystemBusinessService brakeSystemBusinessService)
            :base(brakeSystemBusinessService)
        {
        }
    }
}

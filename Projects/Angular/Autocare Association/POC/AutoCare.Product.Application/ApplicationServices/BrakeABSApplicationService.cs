using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BrakeABSApplicationService : VcdbApplicationService<BrakeABS>, IBrakeABSApplicationService
    {
        public BrakeABSApplicationService(IBrakeABSBusinessService brakeABSBusinessService)
            :base(brakeABSBusinessService)
        {
        }
    }
}

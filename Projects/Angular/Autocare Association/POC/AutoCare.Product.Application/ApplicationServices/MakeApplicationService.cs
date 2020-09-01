using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class MakeApplicationService : VcdbApplicationService<Make>, IMakeApplicationService
    {
        public MakeApplicationService(IMakeBusinessService makeBusinessService)
            :base(makeBusinessService)
        {
        }
    }
}

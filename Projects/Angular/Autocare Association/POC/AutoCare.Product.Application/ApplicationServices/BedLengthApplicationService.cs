using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BedLengthApplicationService : VcdbApplicationService<BedLength>, IBedLengthApplicationService
    {
        public BedLengthApplicationService(BedLengthBusinessService bedLengthBusinessService)
            : base(bedLengthBusinessService)
        {
        }
    }
}

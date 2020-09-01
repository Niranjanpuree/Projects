using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BedTypeApplicationService : VcdbApplicationService<BedType>, IBedTypeApplicationService
    {
        public BedTypeApplicationService(IBedTypeBusinessService bedTypeBusinessService)
            : base(bedTypeBusinessService)
        {
        }
    }
}

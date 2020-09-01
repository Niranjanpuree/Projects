using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BodyTypeApplicationService : VcdbApplicationService<BodyType>, IBodyTypeApplicationService
    {
        public BodyTypeApplicationService(IBodyTypeBusinessService bodyTypeBusinessService)
            :base(bodyTypeBusinessService)
        {
        }
    }
}

using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class SubModelApplicationService : VcdbApplicationService<SubModel>, ISubModelApplicationService
    {
        public SubModelApplicationService(ISubModelBusinessService submodelBusinessService)
            : base(submodelBusinessService)
        {

        }
    }
}

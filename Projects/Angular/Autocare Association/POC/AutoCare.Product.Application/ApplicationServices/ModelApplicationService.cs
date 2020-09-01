using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class ModelApplicationService : VcdbApplicationService<Model>, IModelApplicationService
    {
        public ModelApplicationService(IModelBusinessService modelBusinessService)
            :base(modelBusinessService)
        {
        }
    }
}

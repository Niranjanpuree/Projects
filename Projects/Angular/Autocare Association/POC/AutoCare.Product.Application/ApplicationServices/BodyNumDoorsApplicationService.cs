using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BodyNumDoorsApplicationService : VcdbApplicationService<BodyNumDoors>, IBodyNumDoorsApplicationService
    {
        public BodyNumDoorsApplicationService(IBodyNumDoorsBusinessService bodyNumDoorsBusinessService)
            : base(bodyNumDoorsBusinessService)
        {
        }
    }
}

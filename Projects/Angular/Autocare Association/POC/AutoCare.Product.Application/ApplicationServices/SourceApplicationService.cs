using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class SourceApplicationService : VcdbApplicationService<Source>, ISourceApplicationService
    {
        public SourceApplicationService(IVcdbBusinessService<Source> sourceBusinessService)
            :base(sourceBusinessService)
        {

        }
    }
}

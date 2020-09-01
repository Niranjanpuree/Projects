using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class YearApplicationService : VcdbApplicationService<Year>, IYearApplicationService
    {
        public YearApplicationService(IYearBusinessService yearBusinessService)
            :base(yearBusinessService)
        {
        }
    }
}

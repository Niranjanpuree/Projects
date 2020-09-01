using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IVcdbApplicationService<T>: IApplicationService<T>
        where T:class
    {
        Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings);
    }
}

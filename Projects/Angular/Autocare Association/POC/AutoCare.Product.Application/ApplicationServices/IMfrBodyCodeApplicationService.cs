using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IMfrBodyCodeApplicationService : IVcdbApplicationService<MfrBodyCode>
    {
        new Task<MfrBodyCodeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}

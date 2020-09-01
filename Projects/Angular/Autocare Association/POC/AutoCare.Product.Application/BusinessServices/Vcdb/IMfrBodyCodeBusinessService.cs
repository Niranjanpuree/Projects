using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IMfrBodyCodeBusinessService : IVcdbBusinessService<MfrBodyCode>
    {
        new Task<MfrBodyCodeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}
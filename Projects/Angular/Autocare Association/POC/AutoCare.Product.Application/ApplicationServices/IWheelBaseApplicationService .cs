using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IWheelBaseApplicationService : IVcdbApplicationService<WheelBase>
    {
        new Task<WheelBaseChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}

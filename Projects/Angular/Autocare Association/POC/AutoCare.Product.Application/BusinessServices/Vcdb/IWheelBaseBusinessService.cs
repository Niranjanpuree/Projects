using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IWheelBaseBusinessService: IVcdbBusinessService<WheelBase>
    {
        new Task<WheelBaseChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}

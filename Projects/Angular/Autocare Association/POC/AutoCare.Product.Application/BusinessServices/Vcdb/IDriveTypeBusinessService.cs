using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IDriveTypeBusinessService : IVcdbBusinessService<DriveType>
    {
        new Task<DriveTypeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}
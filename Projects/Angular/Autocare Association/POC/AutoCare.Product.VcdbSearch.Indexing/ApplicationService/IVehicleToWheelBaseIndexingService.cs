using AutoCare.Product.VcdbSearchIndex.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IVehicleToWheelBaseIndexingService
    {
        Task UploadDocumentsAsync(List<VehicleToWheelBaseDocument> vehicleToWheelBaseDocuments);
        Task UploadDocumentAsync(VehicleToWheelBaseDocument vehicleToWheelBaseDocuments);
        Task UpdateVehicleToWheelBaseChangeRequestIdAsync(string vehicleToWheelBaseId, long vehicleToWheelBaseChangeRequestId);
        Task DeleteDocumentByVehicleToWheelBaseIdAsync(string vehicleToWheelBaseId);
        Task UpdateWheelBaseChangeRequestIdAsync(string vehicleToWheelBaseId, long wheelBaseChangeRequestId);
    }
}

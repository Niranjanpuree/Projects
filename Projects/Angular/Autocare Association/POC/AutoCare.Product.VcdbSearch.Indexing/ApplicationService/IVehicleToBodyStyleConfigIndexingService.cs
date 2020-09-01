using AutoCare.Product.VcdbSearchIndex.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IVehicleToBodyStyleConfigIndexingService
    {
        Task UploadDocumentsAsync(List<VehicleToBodyStyleConfigDocument> vehicleToBodyStyleConfigDocuments);
        Task UploadDocumentAsync(VehicleToBodyStyleConfigDocument vehicleToBodyStyleConfigDocuments);
        Task UpdateVehicleToBodyStyleConfigChangeRequestIdAsync(int vehicleToBodyStyleConfigId, long vehicleToBodyStyleConfigChangeRequestId);
        Task UpdateBodyStyleConfigChangeRequestIdAsync(string vehicleToBodyStyleConfigId,long bodyStyleConfigChangeRequestId);
        Task DeleteDocumentByVehicleToBodyStyleConfigIdAsync(string vehicleToBodyStyleConfigId);
    }
}

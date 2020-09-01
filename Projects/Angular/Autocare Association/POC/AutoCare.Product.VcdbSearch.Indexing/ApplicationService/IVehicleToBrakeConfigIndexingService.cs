using AutoCare.Product.VcdbSearchIndex.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IVehicleToBrakeConfigIndexingService
    {
        Task UploadDocumentsAsync(List<VehicleToBrakeConfigDocument> vehicleToBrakeConfigDocuments);
        Task UploadDocumentAsync(VehicleToBrakeConfigDocument vehicleToBrakeConfigDocuments);
        Task UpdateVehicleToBrakeConfigChangeRequestIdAsync(int vehicleToBrakeConfigId, long vehicleToBrakeConfigChangeRequestId);
        Task UpdateBrakeConfigChangeRequestIdAsync(string vehicleToBrakeConfigId,long brakeConfigChangeRequestId);
        Task DeleteDocumentByVehicleToBrakeConfigIdAsync(string vehicleToBrakeConfigId);
    }
}

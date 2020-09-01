using AutoCare.Product.VcdbSearchIndex.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IVehicleToDriveTypeIndexingService
    {
        Task UploadDocumentsAsync(List<VehicleToDriveTypeDocument> vehicleToDriveTypeDocuments);
        Task UploadDocumentAsync(VehicleToDriveTypeDocument vehicleToDriveTypeDocument);
        Task UpdateVehicleToDriveTypeChangeRequestIdAsync(int vehicleToDriveTypeId, long vehicleToDriveTypeChangeRequestId);
        Task UpdateDriveTypeChangeRequestIdAsync(string vehicleToDriveTypeId, long driveTypeChangeRequestId);
        Task DeleteDocumentByVehicleToDriveTypeIdAsync(string vehicleToDriveTypeId);
    }
}

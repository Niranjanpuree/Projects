using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IVehicleIndexingService
    {
        Task UploadDocumentsAsync(List<VehicleDocument> vehicleDocuments);
        Task UploadDocumentAsync(VehicleDocument vehicleDocument);
        Task UploadDocumentsAsync(string vehicles);
        Task UploadDocumentAsync(string vehicle);
        Task UploadDocumentForVehicleId(int vehicleId);
        Task UpdateIndexForChangeRequest(ChangeRequestStaging changeRequestStaging);
        Task UpdateVehicleChangeRequestIdAsync(int vehicleId, long vehicleChangeRequestId);
        Task UpdateBaseVehicleChangeRequestIdAsync(string vehicleId, long baseVehicleChangeRequestId);
        Task DeleteDocumentByVehicleIdAsync(string vehicleId);
    }
}

using AutoCare.Product.VcdbSearchIndex.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IVehicleToMfrBodyCodeIndexingService
    {
        Task UploadDocumentsAsync(List<VehicleToMfrBodyCodeDocument> vehicleToMfrBodyCodeDocuments);
        Task UploadDocumentAsync(VehicleToMfrBodyCodeDocument vehicleToMfrBodyCodeDocuments);
        Task UpdateVehicleToMfrBodyCodeChangeRequestIdAsync(int vehicleToMfrBodyCodeId, long vehicleToMfrBodyCodeChangeRequestId);
        Task UpdateMfrBodyCodeChangeRequestIdAsync(string vehicleToMfrBodyCodeId, long mfrBodyCodeChangeRequestId);
        Task DeleteDocumentByVehicleToMfrBodyCodeIdAsync(string vehicleToMfrBodyCodeId);
    }
}

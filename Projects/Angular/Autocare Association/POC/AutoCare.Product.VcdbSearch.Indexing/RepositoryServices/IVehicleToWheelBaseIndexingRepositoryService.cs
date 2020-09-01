using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IVehicleToWheelBaseIndexingRepositoryService
    {
        Task UploadDocumentsAsync(List<VehicleToWheelBaseDocument> documents);
        Task UploadDocumentAsync(VehicleToWheelBaseDocument vehicleToBrakeConfigDocument);
        Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToWheelBaseDocument> documents);
        Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToWheelBaseDocument vehicleToWheelBaseDocument);
        Task<DocumentIndexResult> DeleteDocumentByVehicleToWheelBaseIdAsync(string vehicleToWheelBaseId);
    }
}

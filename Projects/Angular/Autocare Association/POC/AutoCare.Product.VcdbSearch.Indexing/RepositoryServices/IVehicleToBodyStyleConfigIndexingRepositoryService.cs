using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IVehicleToBodyStyleConfigIndexingRepositoryService
    {
        Task UploadDocumentsAsync(List<VehicleToBodyStyleConfigDocument> documents);
        Task UploadDocumentAsync(VehicleToBodyStyleConfigDocument vehicleToBodyStyleConfigDocument);
        Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToBodyStyleConfigDocument> documents);
        Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToBodyStyleConfigDocument vehicleToBodyStyleConfigDocument);
        Task<DocumentIndexResult> DeleteDocumentByVehicleToBodyStyleConfigIdAsync(string vehicleToBodyStyleConfigId);
    }
}

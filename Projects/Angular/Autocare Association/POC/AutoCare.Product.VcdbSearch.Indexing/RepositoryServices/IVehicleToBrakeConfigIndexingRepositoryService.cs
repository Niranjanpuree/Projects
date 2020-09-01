using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IVehicleToBrakeConfigIndexingRepositoryService
    {
        Task UploadDocumentsAsync(List<VehicleToBrakeConfigDocument> documents);
        Task UploadDocumentAsync(VehicleToBrakeConfigDocument vehicleToBrakeConfigDocument);
        Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToBrakeConfigDocument> documents);
        Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToBrakeConfigDocument vehicleToBrakeConfigDocument);
        Task<DocumentIndexResult> DeleteDocumentByVehicleToBrakeConfigIdAsync(string vehicleToBrakeConfigId);
    }
}

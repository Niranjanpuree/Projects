using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IVehicleToBedConfigIndexingRepositoryService
    {
        Task UploadDocumentsAsync(List<VehicleToBedConfigDocument> documents);
        Task UploadDocumentAsync(VehicleToBedConfigDocument vehicleToBedConfigDocument);
        Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToBedConfigDocument> documents);
        Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToBedConfigDocument vehicleToBedConfigDocument);
        Task<DocumentIndexResult> DeleteDocumentByVehicleToBedConfigIdAsync(string vehicleToBedConfigId);
    }
}

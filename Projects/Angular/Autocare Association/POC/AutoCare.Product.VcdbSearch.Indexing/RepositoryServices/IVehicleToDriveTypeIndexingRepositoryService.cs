using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IVehicleToDriveTypeIndexingRepositoryService
    {
        Task UploadDocumentsAsync(List<VehicleToDriveTypeDocument> documents);
        Task UploadDocumentAsync(VehicleToDriveTypeDocument vehicleToDriveTypeDocument);
        Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToDriveTypeDocument> documents);
        Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToDriveTypeDocument vehicleToDriveTypeDocument);
        Task<DocumentIndexResult> DeleteDocumentByVehicleToDriveTypeIdAsync(string vehicleToDriveTypeId);
    }
}

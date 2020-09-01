using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IVehicleToMfrBodyCodeIndexingRepositoryService
    {
        Task UploadDocumentsAsync(List<VehicleToMfrBodyCodeDocument> documents);
        Task UploadDocumentAsync(VehicleToMfrBodyCodeDocument vehicleToMfrBodyCodeDocument);
        Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToMfrBodyCodeDocument> documents);
        Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToMfrBodyCodeDocument vehicleToMfrBodyCodeDocument);
        Task<DocumentIndexResult> DeleteDocumentByVehicleToMfrBodyCodeIdAsync(string vehicleToMfrBodyCodeId);
    }
}

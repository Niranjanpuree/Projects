using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IVehicleIndexingRepositoryService
    {
        //Task<VehicleDocument> GetVechicleDocumentByVehicleIdAsync(int vehicleId);
        //Task<List<VehicleDocument>> GetVehicleDocumentsByMakeIdAsync(int makeId);
        //Task<List<VehicleDocument>> GetVehicleDocumentsByModelIdAsync(int modelId);
        //Task<List<VehicleDocument>> GetVehicleDocumentsByYearAsync(int year);
        //Task<List<VehicleDocument>> GetVehicleDocumentsByBaseVehicleIdAsync(int baseVehicleId);
        //Task<List<VehicleDocument>> GetVehicleDocumentsByRegionIdAsync(int regionId);
        //Task<List<VehicleDocument>> GetVehicleDocumentsBySubModelIdAsync(int subModelId);
        Task UploadDocumentsAsync(List<VehicleDocument> vechileIndexDocuments);
        Task UploadDocumentAsync(VehicleDocument vehicleDocument);
        Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleDocument> vehicleDocuments);
        Task<DocumentIndexResult> UpdateDocumentAsync(VehicleDocument vehicleDocument);
        Task<DocumentIndexResult> DeleteDocumentByVehicleIdAsync(string vehicleId);
    }
}

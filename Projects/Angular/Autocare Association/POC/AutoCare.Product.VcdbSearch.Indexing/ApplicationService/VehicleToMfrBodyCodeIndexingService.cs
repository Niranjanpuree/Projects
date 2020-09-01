using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using System.Collections.Generic;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class VehicleToMfrBodyCodeIndexingService:IVehicleToMfrBodyCodeIndexingService
    {
        private readonly IVehicleToMfrBodyCodeIndexingRepositoryService _vehicleToMfrBodyCodeIndexingRepositoryService;

        public VehicleToMfrBodyCodeIndexingService(IVehicleToMfrBodyCodeIndexingRepositoryService vehicleToMfrBodyCodeIndexingRepositoryService)
        {
            _vehicleToMfrBodyCodeIndexingRepositoryService = vehicleToMfrBodyCodeIndexingRepositoryService;
        }

        public async Task UploadDocumentAsync(VehicleToMfrBodyCodeDocument vehicleToMfrBodyCodeDocument)
        {
            await _vehicleToMfrBodyCodeIndexingRepositoryService.UpdateDocumentAsync(vehicleToMfrBodyCodeDocument);
        }

        public async Task UploadDocumentsAsync(List<VehicleToMfrBodyCodeDocument> vehicleToMfrBodyCodeDocuments)
        {
            await _vehicleToMfrBodyCodeIndexingRepositoryService.UpdateDocumentsAsync(vehicleToMfrBodyCodeDocuments);
        }

        public async Task UpdateMfrBodyCodeChangeRequestIdAsync(string vehicleToMfrBodyCodeId, long mfrBodyCodeChangeRequestId)
        {
            VehicleToMfrBodyCodeDocument document = new VehicleToMfrBodyCodeDocument
            {
                VehicleToMfrBodyCodeId = vehicleToMfrBodyCodeId.ToString(),
                MfrBodyCodeChangeRequestId = mfrBodyCodeChangeRequestId
            };

            await _vehicleToMfrBodyCodeIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task UpdateVehicleToMfrBodyCodeChangeRequestIdAsync(int vehicleToMfrBodyCodeId, long vehicleToMfrBodyCodeChangeRequestId)
        {
            VehicleToMfrBodyCodeDocument document = new VehicleToMfrBodyCodeDocument
            {
                VehicleToMfrBodyCodeId = vehicleToMfrBodyCodeId.ToString(),
                VehicleToMfrBodyCodeChangeRequestId = vehicleToMfrBodyCodeChangeRequestId,
            };

            await _vehicleToMfrBodyCodeIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentByVehicleToMfrBodyCodeIdAsync(string vehicleToMfrBodyCodeId)
        {
            await _vehicleToMfrBodyCodeIndexingRepositoryService.DeleteDocumentByVehicleToMfrBodyCodeIdAsync(vehicleToMfrBodyCodeId);
        }
    }
}

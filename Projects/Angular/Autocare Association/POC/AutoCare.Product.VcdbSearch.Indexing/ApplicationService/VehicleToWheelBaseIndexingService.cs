using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using System.Collections.Generic;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class VehicleToWheelBaseIndexingService : IVehicleToWheelBaseIndexingService
    {
        private readonly IVehicleToWheelBaseIndexingRepositoryService _vehicleToWheelBaseIndexingRepositoryService;

        public VehicleToWheelBaseIndexingService(IVehicleToWheelBaseIndexingRepositoryService vehicleToWheelBaseIndexingRepositoryService)
        {
            _vehicleToWheelBaseIndexingRepositoryService = vehicleToWheelBaseIndexingRepositoryService;
        }

        public async Task UploadDocumentAsync(VehicleToWheelBaseDocument vehicleToWheelBaseDocument)
        {
            await _vehicleToWheelBaseIndexingRepositoryService.UpdateDocumentAsync(vehicleToWheelBaseDocument);
        }

        public async Task UploadDocumentsAsync(List<VehicleToWheelBaseDocument> vehicleToWheelBaseDocuments)
        {
            await _vehicleToWheelBaseIndexingRepositoryService.UpdateDocumentsAsync(vehicleToWheelBaseDocuments);
        }

        public async Task UpdateWheelBaseChangeRequestIdAsync(string vehicleToWheelBaseId, long wheelBaseChangeRequestId)
        {
            VehicleToWheelBaseDocument document = new VehicleToWheelBaseDocument
            {
                VehicleToWheelBaseId = vehicleToWheelBaseId.ToString(),
                WheelBaseChangeRequestId = wheelBaseChangeRequestId
            };

            await _vehicleToWheelBaseIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task UpdateVehicleToWheelBaseChangeRequestIdAsync(string vehicleToWheelBaseId, long vehicleToWheelBaseChangeRequestId)
        {
            VehicleToWheelBaseDocument document = new VehicleToWheelBaseDocument
            {
                VehicleToWheelBaseId = vehicleToWheelBaseId.ToString(),
                VehicleToWheelBaseChangeRequestId = vehicleToWheelBaseChangeRequestId,
            };

            await _vehicleToWheelBaseIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentByVehicleToWheelBaseIdAsync(string vehicleToWheelBaseId)
        {
            await _vehicleToWheelBaseIndexingRepositoryService.DeleteDocumentByVehicleToWheelBaseIdAsync(vehicleToWheelBaseId);
        }
    }
}

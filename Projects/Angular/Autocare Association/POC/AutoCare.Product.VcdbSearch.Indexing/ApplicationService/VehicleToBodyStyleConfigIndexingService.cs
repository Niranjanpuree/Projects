using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using System.Collections.Generic;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class VehicleToBodyStyleConfigIndexingService : IVehicleToBodyStyleConfigIndexingService
    {
        private readonly IVehicleToBodyStyleConfigIndexingRepositoryService _vehicleToBodyStyleConfigIndexingRepositoryService;

        public VehicleToBodyStyleConfigIndexingService(IVehicleToBodyStyleConfigIndexingRepositoryService vehicleToBodyStyleConfigIndexingRepositoryService)
        {
            _vehicleToBodyStyleConfigIndexingRepositoryService = vehicleToBodyStyleConfigIndexingRepositoryService;
        }

        public async Task UploadDocumentAsync(VehicleToBodyStyleConfigDocument vehicleToBodyStyleConfigDocument)
        {
            await _vehicleToBodyStyleConfigIndexingRepositoryService.UpdateDocumentAsync(vehicleToBodyStyleConfigDocument);
        }

        public async Task UploadDocumentsAsync(List<VehicleToBodyStyleConfigDocument> vehicleToBodyStyleConfigDocuments)
        {
            await _vehicleToBodyStyleConfigIndexingRepositoryService.UpdateDocumentsAsync(vehicleToBodyStyleConfigDocuments);
        }

        public async Task UpdateBodyStyleConfigChangeRequestIdAsync(string vehicleToBodyStyleConfigId, long bodyStyleConfigChangeRequestId)
        {
            VehicleToBodyStyleConfigDocument document = new VehicleToBodyStyleConfigDocument
            {
                VehicleToBodyStyleConfigId = vehicleToBodyStyleConfigId,
                BodyStyleConfigChangeRequestId = bodyStyleConfigChangeRequestId
            };

            await _vehicleToBodyStyleConfigIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task UpdateVehicleToBodyStyleConfigChangeRequestIdAsync(int vehicleToBodyStyleConfigId, long vehicleToBodyStyleConfigChangeRequestId)
        {
            VehicleToBodyStyleConfigDocument document = new VehicleToBodyStyleConfigDocument
            {
                VehicleToBodyStyleConfigId = vehicleToBodyStyleConfigId.ToString(),
                VehicleToBodyStyleConfigChangeRequestId = vehicleToBodyStyleConfigChangeRequestId,
            };

            await _vehicleToBodyStyleConfigIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentByVehicleToBodyStyleConfigIdAsync(string vehicleToBodyStyleConfigId)
        {
            await _vehicleToBodyStyleConfigIndexingRepositoryService.DeleteDocumentByVehicleToBodyStyleConfigIdAsync(vehicleToBodyStyleConfigId);
        }
    }
}

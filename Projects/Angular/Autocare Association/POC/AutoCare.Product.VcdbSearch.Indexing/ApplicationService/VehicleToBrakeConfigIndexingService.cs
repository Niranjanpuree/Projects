using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using System.Collections.Generic;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class VehicleToBrakeConfigIndexingService : IVehicleToBrakeConfigIndexingService
    {
        private readonly IVehicleToBrakeConfigIndexingRepositoryService _vehicleToBrakeConfigIndexingRepositoryService;

        public VehicleToBrakeConfigIndexingService(IVehicleToBrakeConfigIndexingRepositoryService vehicleToBrakeConfigIndexingRepositoryService)
        {
            _vehicleToBrakeConfigIndexingRepositoryService = vehicleToBrakeConfigIndexingRepositoryService;
        }

        public async Task UploadDocumentAsync(VehicleToBrakeConfigDocument vehicleToBrakeConfigDocument)
        {
            await _vehicleToBrakeConfigIndexingRepositoryService.UpdateDocumentAsync(vehicleToBrakeConfigDocument);
        }

        public async Task UploadDocumentsAsync(List<VehicleToBrakeConfigDocument> vehicleToBrakeConfigDocuments)
        {
            await _vehicleToBrakeConfigIndexingRepositoryService.UpdateDocumentsAsync(vehicleToBrakeConfigDocuments);
        }

        public async Task UpdateBrakeConfigChangeRequestIdAsync(string vehicleToBrakeConfigId, long brakeConfigChangeRequestId)
        {
            VehicleToBrakeConfigDocument document = new VehicleToBrakeConfigDocument
            {
                VehicleToBrakeConfigId = vehicleToBrakeConfigId,
                BrakeConfigChangeRequestId = brakeConfigChangeRequestId
            };

            await _vehicleToBrakeConfigIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task UpdateVehicleToBrakeConfigChangeRequestIdAsync(int vehicleToBrakeConfigId, long vehicleToBrakeConfigChangeRequestId)
        {
            VehicleToBrakeConfigDocument document = new VehicleToBrakeConfigDocument
            {
                VehicleToBrakeConfigId = vehicleToBrakeConfigId.ToString(),
                VehicleToBrakeConfigChangeRequestId = vehicleToBrakeConfigChangeRequestId,
            };

            await _vehicleToBrakeConfigIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentByVehicleToBrakeConfigIdAsync(string vehicleToBrakeConfigId)
        {
            await _vehicleToBrakeConfigIndexingRepositoryService.DeleteDocumentByVehicleToBrakeConfigIdAsync(vehicleToBrakeConfigId);
        }
    }
}

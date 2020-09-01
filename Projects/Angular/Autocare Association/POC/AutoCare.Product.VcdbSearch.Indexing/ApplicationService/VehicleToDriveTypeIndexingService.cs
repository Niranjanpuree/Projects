using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using System.Collections.Generic;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class VehicleToDriveTypeIndexingService : IVehicleToDriveTypeIndexingService
    {
        private readonly IVehicleToDriveTypeIndexingRepositoryService _vehicleToDriveTypeIndexingRepositoryService;

        public VehicleToDriveTypeIndexingService(IVehicleToDriveTypeIndexingRepositoryService vehicleToDriveTypeIndexingRepositoryService)
        {
            _vehicleToDriveTypeIndexingRepositoryService = vehicleToDriveTypeIndexingRepositoryService;
        }

        public async Task UploadDocumentAsync(VehicleToDriveTypeDocument vehicleToDriveTypeDocument)
        {
            await _vehicleToDriveTypeIndexingRepositoryService.UpdateDocumentAsync(vehicleToDriveTypeDocument);
        }

        public async Task UploadDocumentsAsync(List<VehicleToDriveTypeDocument> vehicleToDriveTypeDocuments)
        {
            await _vehicleToDriveTypeIndexingRepositoryService.UpdateDocumentsAsync(vehicleToDriveTypeDocuments);
        }

        public async Task UpdateDriveTypeChangeRequestIdAsync(string vehicleToDriveTypeId, long driveTypeChangeRequestId)
        {
            VehicleToDriveTypeDocument document = new VehicleToDriveTypeDocument
            {
                VehicleToDriveTypeId = vehicleToDriveTypeId,
                DriveTypeChangeRequestId = driveTypeChangeRequestId
            };

            await _vehicleToDriveTypeIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task UpdateVehicleToDriveTypeChangeRequestIdAsync(int vehicleToDriveTypeId, long vehicleToDriveTypeChangeRequestId)
        {
            VehicleToDriveTypeDocument document = new VehicleToDriveTypeDocument
            {
                VehicleToDriveTypeId = vehicleToDriveTypeId.ToString(),
                VehicleToDriveTypeChangeRequestId = vehicleToDriveTypeChangeRequestId,
            };

            await _vehicleToDriveTypeIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentByVehicleToDriveTypeIdAsync(string vehicleToDriveTypeId)
        {
            await _vehicleToDriveTypeIndexingRepositoryService.DeleteDocumentByVehicleToDriveTypeIdAsync(vehicleToDriveTypeId);
        }
    }
}

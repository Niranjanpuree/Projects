using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using System.Collections.Generic;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class VehicleToBedConfigIndexingService : IVehicleToBedConfigIndexingService
    {
        private readonly IVehicleToBedConfigIndexingRepositoryService _vehicleToBedConfigIndexingRepositoryService;

        public VehicleToBedConfigIndexingService(IVehicleToBedConfigIndexingRepositoryService vehicleToBedConfigIndexingRepositoryService)
        {
            _vehicleToBedConfigIndexingRepositoryService = vehicleToBedConfigIndexingRepositoryService;
        }

        public async Task UploadDocumentAsync(VehicleToBedConfigDocument vehicleToBedConfigDocument)
        {
            await _vehicleToBedConfigIndexingRepositoryService.UpdateDocumentAsync(vehicleToBedConfigDocument);
        }

        public async Task UploadDocumentsAsync(List<VehicleToBedConfigDocument> vehicleToBedConfigDocuments)
        {
            await _vehicleToBedConfigIndexingRepositoryService.UpdateDocumentsAsync(vehicleToBedConfigDocuments);
        }

        public async Task UpdateBedConfigChangeRequestIdAsync(string vehicleToBedConfigId, long bedConfigChangeRequestId)
        {
            VehicleToBedConfigDocument document = new VehicleToBedConfigDocument
            {
                VehicleToBedConfigId = vehicleToBedConfigId,
                BedConfigChangeRequestId = bedConfigChangeRequestId
            };

            await _vehicleToBedConfigIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task UpdateVehicleToBedConfigChangeRequestIdAsync(int vehicleToBedConfigId, long vehicleToBedConfigChangeRequestId)
        {
            VehicleToBedConfigDocument document = new VehicleToBedConfigDocument
            {
                VehicleToBedConfigId = vehicleToBedConfigId.ToString(),
                VehicleToBedConfigChangeRequestId = vehicleToBedConfigChangeRequestId,
            };

            await _vehicleToBedConfigIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentByVehicleToBedConfigIdAsync(string vehicleToBedConfigId)
        {
            await _vehicleToBedConfigIndexingRepositoryService.DeleteDocumentByVehicleToBedConfigIdAsync(vehicleToBedConfigId);
        }
    }
}

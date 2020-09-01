using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class VehicleIndexingService : IVehicleIndexingService
    {
        private readonly IVehicleIndexingRepositoryService _vehicleIndexingRepositoryService;

        public VehicleIndexingService(IVehicleIndexingRepositoryService vehicleIndexingRepositoryService)
        {
            _vehicleIndexingRepositoryService = vehicleIndexingRepositoryService;
        }
        public async Task UploadDocumentsAsync(List<VehicleDocument> vehicleDocuments)
        {
            await _vehicleIndexingRepositoryService.UpdateDocumentsAsync(vehicleDocuments);
        }

        public async Task UploadDocumentAsync(VehicleDocument vehicleDocument)
        {
            await _vehicleIndexingRepositoryService.UpdateDocumentAsync(vehicleDocument);
        }

        public Task UploadDocumentsAsync(string vehicles)
        {
            throw new NotImplementedException();
        }

        public Task UploadDocumentAsync(string vehicle)
        {
            throw new NotImplementedException();
        }

        public Task UploadDocumentForVehicleId(int vehicleId)
        {
            //TODO: Build vehicle indexing object
            //TODO: Serialize to JSon
            //TODO: If new vhicle, add to Azure Search
            //TODO: If exising vehicle, get vehicle from Azure Search and update

            throw new NotImplementedException();
        }

        public async Task UpdateIndexForChangeRequest(ChangeRequestStaging changeRequestStaging)
        {
            if (changeRequestStaging == null)
            {
                return;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a document in "vehicles" azure index for vehicle change request ids.
        /// </summary>
        /// <param name="vehicleId">Vehicle id is the key field in "vehicles" azure index. This field is required</param>
        /// <param name="vehicleChangeRequestId"></param>
        /// <returns></returns>
        public async Task UpdateVehicleChangeRequestIdAsync(int vehicleId, long vehicleChangeRequestId)
        {
            VehicleDocument document = new VehicleDocument
            {
                VehicleId = vehicleId.ToString(),
                VehicleChangeRequestId = vehicleChangeRequestId
            };

            await _vehicleIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        /// <summary>
        /// Updates a document in "vehicles" azure index for base vehicle change request ids.
        /// </summary>
        /// <param name="vehicleId">Vehicle id is the key field in "vehicles" azure index. This field is required</param>
        /// <param name="baseVehicleChangeRequestId"></param>
        /// <returns></returns>
        public async Task UpdateBaseVehicleChangeRequestIdAsync(string vehicleId, long baseVehicleChangeRequestId)
        {
            VehicleDocument document = new VehicleDocument
            {
                VehicleId = vehicleId,
                BaseVehicleChangeRequestId = baseVehicleChangeRequestId
            };

            await _vehicleIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task DeleteDocumentByVehicleIdAsync(string vehicleId)
        {
            await _vehicleIndexingRepositoryService.DeleteDocumentByVehicleIdAsync(vehicleId);
        }

    }
}

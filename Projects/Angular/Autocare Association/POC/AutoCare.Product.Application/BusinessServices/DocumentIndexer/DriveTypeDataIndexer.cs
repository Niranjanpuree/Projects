using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearchIndex.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Search.Model;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class DriveTypeDataIndexer : IDriveTypeDataIndexer
    {
        private readonly IVcdbUnitOfWork _repositories;
        private readonly IVehicleToDriveTypeSearchService _vehicletoDriveTypeSearchService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService;
        private readonly IVehicleToDriveTypeDataIndexer _vehicleToDriveTypeDataIndexer;

        public DriveTypeDataIndexer(IVcdbUnitOfWork repositories,
            IVehicleToDriveTypeSearchService vehicletoDriveTypeSearchService,
        IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService,
        IVehicleToDriveTypeDataIndexer vehicleToDriveTypeDataIndexer
            )
        {
            _repositories = repositories;
            _vehicletoDriveTypeSearchService = vehicletoDriveTypeSearchService;
            _vehicleToDriveTypeIndexingService = vehicleToDriveTypeIndexingService;
            _vehicleToDriveTypeDataIndexer = vehicleToDriveTypeDataIndexer;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var driveTypeRepositoryService =
                            _repositories.GetRepositoryService<DriveType>() as
                                IVcdbSqlServerEfRepositoryService<DriveType>;

            if (driveTypeRepositoryService == null)
            {
                return;
            }

            var addedDriveTypes =
                                await
                                    driveTypeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000
                                                                                );

            if (addedDriveTypes == null || !addedDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Drive Type Index cannot be updated before the transactional table is updated");
            }

            var addedDriveType = addedDriveTypes.First();
            var vehicleToDriveTypeSearchResult =
                                    await
                                        _vehicletoDriveTypeSearchService.SearchAsync(null,
                                            $"driveTypeId eq {addedDriveType.Id}");
            var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;

            if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
            {
                throw new InvalidOperationException(
                    "Drive Type already exisit in VehicleToDriveTypeIndex. So, this change request cannot be an add request");
            }

            //MfrBodyCode is new and therefore not yet available in "vehicletoMfrBodyCode" azure search index
            VehicleToDriveTypeDocument newDriveTypeConfigDocument = new VehicleToDriveTypeDocument
            {
                VehicleToDriveTypeId = Guid.NewGuid().ToString(),
                DriveTypeId = addedDriveType.Id,
                DriveTypeName = addedDriveType.Name
            };

            await this._vehicleToDriveTypeIndexingService.UploadDocumentAsync(newDriveTypeConfigDocument);
        }
        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var driveTypeRepositoryService =
                            _repositories.GetRepositoryService<DriveType>() as
                                IVcdbSqlServerEfRepositoryService<DriveType>;

            if (driveTypeRepositoryService == null)
            {
                return;
            }

            var updatedDriveTypes =
                                await
                                    driveTypeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000
                                        
                                        );

            if (updatedDriveTypes == null || !updatedDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Drive Type Index cannot be updated before the transactional table is updated");
            }

            var updatedDriveType = updatedDriveTypes.First();

            await UpdateVehicleToDriveTypeDocuments(updatedDriveType);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: This should be handled by VehicleToMfrBodyCode.DeleteChangeRequestIndexerAsync()
            var driveTypeRepositoryService =
                            _repositories.GetRepositoryService<DriveType>() as
                                IVcdbSqlServerEfRepositoryService<DriveType>;

            if (driveTypeRepositoryService == null)
            {
                return;
            }

            var deletedDriveTypes =
                                await
                                    driveTypeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (deletedDriveTypes == null || !deletedDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Drive Type Index cannot be updated before the transactional table is updated");
            }

            var deletedDriveType = deletedDriveTypes.First();

            if (deletedDriveType.VehicleToDriveTypes == null || deletedDriveType.VehicleToDriveTypes.Count == 0)
            {
                var vehicleToDriveTypeSearchResult =
                await
                    _vehicletoDriveTypeSearchService.SearchAsync(null,
                        $"driveTypeId eq {deletedDriveType.Id}");

                var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;
                if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
                {
                    foreach (var existingVehicleToDriveTypeDocument in existingVehicleToDriveTypeDocuments)
                    {
                        //existingVehicleToDriveTypeDocument.VehicleToDriveTypeId must be a GUID string
                        await this._vehicleToDriveTypeIndexingService.DeleteDocumentByVehicleToDriveTypeIdAsync(existingVehicleToDriveTypeDocument.VehicleToDriveTypeId);
                    }
                }
            }
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var driveTypeRepositoryService =
                            _repositories.GetRepositoryService<DriveType>() as
                                IVcdbSqlServerEfRepositoryService<DriveType>;

            if (driveTypeRepositoryService == null)
            {
                return;
            }

            var updatedDriveTypes =
                                await
                                    driveTypeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedDriveTypes == null || !updatedDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Drive Type Index cannot be updated before the transactional table is updated");
            }

            var updatedDriveType = updatedDriveTypes.First();

            var vehicleToDriveTypeSearchResult =
                                    await
                                        _vehicletoDriveTypeSearchService.SearchAsync(null,
                                            $"driveTypeId eq {updatedDriveType.Id}");

            var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;

            if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
            {
                foreach (var existingVehicleToDriveTypeDocument in existingVehicleToDriveTypeDocuments)
                {
                    existingVehicleToDriveTypeDocument.DriveTypeChangeRequestId = -1;
                }

                await
                    this._vehicleToDriveTypeIndexingService.UploadDocumentsAsync(
                        existingVehicleToDriveTypeDocuments.ToList());
            }
        }

        private async Task UpdateVehicleToDriveTypeDocuments(DriveType updatedDriveType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToDriveTypeSearchResult =
                                    await
                                        _vehicletoDriveTypeSearchService.SearchAsync(null,
                                            $"driveTypeId eq {updatedDriveType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;

                if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
                {
                    foreach (var existingVehicleToDriveTypeDocument in existingVehicleToDriveTypeDocuments)
                    {
                        existingVehicleToDriveTypeDocument.DriveTypeChangeRequestId = -1;
                        existingVehicleToDriveTypeDocument.DriveTypeId = updatedDriveType.Id;
                        existingVehicleToDriveTypeDocument.DriveTypeName = updatedDriveType.Name;
                    }

                    await
                        this._vehicleToDriveTypeIndexingService.UploadDocumentsAsync(existingVehicleToDriveTypeDocuments.ToList());

                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);
        }
    }
}

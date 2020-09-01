using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Search.Model;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class BrakeConfigDataIndexer : IBrakeConfigDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;

        public BrakeConfigDataIndexer(IVcdbUnitOfWork repositories,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService)
        {
            _vcdbUnitOfWork = repositories;
            _vehicletoBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var brakeConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BrakeConfig>() as
                                IVcdbSqlServerEfRepositoryService<BrakeConfig>;

            if (brakeConfigRepositoryService == null)
            {
                return;
            }

            var addedBrakeConfigs =
                                await
                                    brakeConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "BrakeABS",
                                        "BrakeSystem",
                                        "FrontBrakeType",
                                        "RearBrakeType");

            if (addedBrakeConfigs == null || !addedBrakeConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Brake Config Index cannot be updated before the transactional table is updated");
            }

            var addedBrakeConfig = addedBrakeConfigs.First();
            var vehicleToBrakeConfigSearchResult =
                                    await
                                        _vehicletoBrakeConfigSearchService.SearchAsync(null,
                                            $"brakeConfigId eq {addedBrakeConfig.Id}");
            var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

            if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
            {
                throw new InvalidOperationException(
                    "BrakeConfig already exisit in VehicleToBrakeConfigIndex. So, this change request cannot be an add request");
            }

            //brake config is new and therefore not yet available in "vehicletobrakeconfigs" azure search index
            VehicleToBrakeConfigDocument newVehicleToBrakeConfigDocument = new VehicleToBrakeConfigDocument
            {
                VehicleToBrakeConfigId = Guid.NewGuid().ToString(),
                BrakeConfigId = addedBrakeConfig.Id,
                FrontBrakeTypeId = addedBrakeConfig.FrontBrakeTypeId,
                FrontBrakeTypeName = addedBrakeConfig.FrontBrakeType.Name,
                RearBrakeTypeId = addedBrakeConfig.RearBrakeTypeId,
                RearBrakeTypeName = addedBrakeConfig.RearBrakeType.Name,
                BrakeABSId = addedBrakeConfig.BrakeABSId,
                BrakeABSName = addedBrakeConfig.BrakeABS.Name,
                BrakeSystemId = addedBrakeConfig.BrakeSystemId,
                BrakeSystemName = addedBrakeConfig.BrakeSystem.Name,
            };

            await this._vehicleToBrakeConfigIndexingService.UploadDocumentAsync(newVehicleToBrakeConfigDocument);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var brakeConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BrakeConfig>() as
                                IVcdbSqlServerEfRepositoryService<BrakeConfig>;

            if (brakeConfigRepositoryService == null)
            {
                return;
            }

            var updatedBrakeConfigs =
                                await
                                    brakeConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "BrakeABS",
                                        "BrakeSystem",
                                        "FrontBrakeType",
                                        "RearBrakeType");

            if (updatedBrakeConfigs == null || !updatedBrakeConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Brake Config Index cannot be updated before the transactional table is updated");
            }

            var updatedBrakeConfig = updatedBrakeConfigs.First();

            await UpdateVehicleToBrakeConfigDocuments(updatedBrakeConfig);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: This following code will delete vehicletobrakeconfig document form vehicletobrakeconfigs index with brake config only information (no associated vehicles)
            var brakeConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BrakeConfig>() as
                                IVcdbSqlServerEfRepositoryService<BrakeConfig>;

            if (brakeConfigRepositoryService == null)
            {
                return;
            }

            var deletedBrakeConfigs =
                                await
                                    brakeConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (deletedBrakeConfigs == null || !deletedBrakeConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Brake Config Index cannot be updated before the transactional table is updated");
            }

            var deletedBrakeConfig = deletedBrakeConfigs.First();

            if (deletedBrakeConfig.VehicleToBrakeConfigs == null || deletedBrakeConfig.VehicleToBrakeConfigs.Count == 0)
            {
                var vehicleToBrakeConfigSearchResult =
                await
                    _vehicletoBrakeConfigSearchService.SearchAsync(null,
                        $"brakeConfigId eq {deletedBrakeConfig.Id}");

                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;
                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        //existingVehicleToBrakeConfigDocument.VehicleToBrakeConfigId must be a GUID string
                        await this._vehicleToBrakeConfigIndexingService.DeleteDocumentByVehicleToBrakeConfigIdAsync(existingVehicleToBrakeConfigDocument.VehicleToBrakeConfigId);
                    }
                }
            }
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var brakeConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BrakeConfig>() as
                                IVcdbSqlServerEfRepositoryService<BrakeConfig>;

            if (brakeConfigRepositoryService == null)
            {
                return;
            }

            var updatedBrakeConfigs =
                                await
                                    brakeConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedBrakeConfigs == null || !updatedBrakeConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Brake Config Index cannot be updated before the transactional table is updated");
            }

            var updatedBrakeConfig = updatedBrakeConfigs.First();

            var vehicleToBrakeConfigSearchResult =
                                    await
                                        _vehicletoBrakeConfigSearchService.SearchAsync(null,
                                            $"brakeConfigId eq {updatedBrakeConfig.Id}");

            var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

            if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
            {
                foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                {
                    existingVehicleToBrakeConfigDocument.BrakeConfigChangeRequestId = -1;
                }

                await
                    this._vehicleToBrakeConfigIndexingService.UploadDocumentsAsync(
                        existingVehicleToBrakeConfigDocuments.ToList());
            }
        }

        private async Task UpdateVehicleToBrakeConfigDocuments(BrakeConfig updatedBrakeConfig)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                                    await
                                        _vehicletoBrakeConfigSearchService.SearchAsync(null,
                                            $"brakeConfigId eq {updatedBrakeConfig.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.BrakeConfigChangeRequestId = -1;
                        existingVehicleToBrakeConfigDocument.FrontBrakeTypeId = updatedBrakeConfig.FrontBrakeTypeId;
                        existingVehicleToBrakeConfigDocument.FrontBrakeTypeName = updatedBrakeConfig.FrontBrakeType.Name;
                        existingVehicleToBrakeConfigDocument.RearBrakeTypeId = updatedBrakeConfig.RearBrakeTypeId;
                        existingVehicleToBrakeConfigDocument.RearBrakeTypeName = updatedBrakeConfig.RearBrakeType.Name;
                        existingVehicleToBrakeConfigDocument.BrakeABSId = updatedBrakeConfig.BrakeABSId;
                        existingVehicleToBrakeConfigDocument.BrakeABSName = updatedBrakeConfig.BrakeABS.Name;
                        existingVehicleToBrakeConfigDocument.BrakeSystemId = updatedBrakeConfig.BrakeSystemId;
                        existingVehicleToBrakeConfigDocument.BrakeSystemName = updatedBrakeConfig.BrakeSystem.Name;
                    }

                    await
                        this._vehicleToBrakeConfigIndexingService.UploadDocumentsAsync(existingVehicleToBrakeConfigDocuments.ToList());

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

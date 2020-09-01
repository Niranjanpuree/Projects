using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Search.Model;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearchIndex.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class BedConfigDataIndexer : IBedConfigDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBedConfigSearchService _vehicletoBedConfigSearchService;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService;

        public BedConfigDataIndexer(IVcdbUnitOfWork repositories,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService)
        {
            _vcdbUnitOfWork = repositories;
            _vehicletoBedConfigSearchService = vehicletoBedConfigSearchService;
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var bedConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BedConfig>() as
                                IVcdbSqlServerEfRepositoryService<BedConfig>;

            if (bedConfigRepositoryService == null)
            {
                return;
            }

            var addedBedConfigs =
                                await
                                    bedConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "BedLength",
                                        "BedType");

            if (addedBedConfigs == null || !addedBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Bed Config Index cannot be updated before the transactional table is updated");
            }

            var addedBedConfig = addedBedConfigs.First();
            var vehicleToBedConfigSearchResult =
                                    await
                                        _vehicletoBedConfigSearchService.SearchAsync(null,
                                            $"bedConfigId eq {addedBedConfig.Id}");
            var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;

            if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
            {
                throw new InvalidOperationException(
                    "BedConfig already exisit in VehicleToBedConfigIndex. So, this change request cannot be an add request");
            }

            //bed config is new and therefore not yet available in "vehicletobedconfigs" azure search index
            VehicleToBedConfigDocument newVehicleToBedConfigDocument = new VehicleToBedConfigDocument
            {
                VehicleToBedConfigId = Guid.NewGuid().ToString(),
                BedConfigId = addedBedConfig.Id,
                BedLengthId = addedBedConfig.BedLengthId,
                BedLength = addedBedConfig.BedLength.Length,
                BedLengthMetric = addedBedConfig.BedLength.BedLengthMetric,
                BedTypeId = addedBedConfig.BedTypeId,
                BedTypeName = addedBedConfig.BedType.Name
            };

            await this._vehicleToBedConfigIndexingService.UploadDocumentAsync(newVehicleToBedConfigDocument);
        }
        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var bedConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BedConfig>() as
                                IVcdbSqlServerEfRepositoryService<BedConfig>;

            if (bedConfigRepositoryService == null)
            {
                return;
            }

            var updatedBedConfigs =
                                await
                                    bedConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "BedLength",
                                        "BedType");

            if (updatedBedConfigs == null || !updatedBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Bed Config Index cannot be updated before the transactional table is updated");
            }

            var updatedBedConfig = updatedBedConfigs.First();

            await UpdateVehicleToBedConfigDocuments(updatedBedConfig);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: This following code will delete vehicletobedconfig document form vehicletobedconfigs index with bed config only information (no associated vehicles)
            var bedConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BedConfig>() as
                                IVcdbSqlServerEfRepositoryService<BedConfig>;

            if (bedConfigRepositoryService == null)
            {
                return;
            }

            var deletedBedConfigs =
                                await
                                    bedConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (deletedBedConfigs == null || !deletedBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Bed Config Index cannot be updated before the transactional table is updated");
            }

            var deletedBedConfig = deletedBedConfigs.First();

            if (deletedBedConfig.VehicleToBedConfigs == null || deletedBedConfig.VehicleToBedConfigs.Count == 0)
            {
                var vehicleToBedConfigSearchResult =
                await
                    _vehicletoBedConfigSearchService.SearchAsync(null,
                        $"bedConfigId eq {deletedBedConfig.Id}");

                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                    {
                        //existingVehicleToBedConfigDocument.VehicleToBedConfigId must be a GUID string
                        await this._vehicleToBedConfigIndexingService.DeleteDocumentByVehicleToBedConfigIdAsync(existingVehicleToBedConfigDocument.VehicleToBedConfigId);
                    }
                }
            }
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var bedConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BedConfig>() as
                                IVcdbSqlServerEfRepositoryService<BedConfig>;

            if (bedConfigRepositoryService == null)
            {
                return;
            }

            var updatedBedConfigs =
                                await
                                    bedConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedBedConfigs == null || !updatedBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Bed Config Index cannot be updated before the transactional table is updated");
            }

            var updatedBedConfig = updatedBedConfigs.First();

            var vehicleToBedConfigSearchResult =
                                    await
                                        _vehicletoBedConfigSearchService.SearchAsync(null,
                                            $"bedConfigId eq {updatedBedConfig.Id}");

            var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;

            if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
            {
                foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                {
                    existingVehicleToBedConfigDocument.BedConfigChangeRequestId = -1;
                }

                await
                    this._vehicleToBedConfigIndexingService.UploadDocumentsAsync(
                        existingVehicleToBedConfigDocuments.ToList());
            }
        }

        private async Task UpdateVehicleToBedConfigDocuments(BedConfig updatedBedConfig)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                                    await
                                        _vehicletoBedConfigSearchService.SearchAsync(null,
                                            $"bedConfigId eq {updatedBedConfig.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;

                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBedConfigDocument.BedConfigChangeRequestId = -1;
                        existingVehicleToBedConfigDocument.BedLengthId = updatedBedConfig.BedLengthId;
                        existingVehicleToBedConfigDocument.BedLength = updatedBedConfig.BedLength.Length;
                        existingVehicleToBedConfigDocument.BedLengthMetric = updatedBedConfig.BedLength.BedLengthMetric;
                        existingVehicleToBedConfigDocument.BedTypeId = updatedBedConfig.BedTypeId;
                        existingVehicleToBedConfigDocument.BedTypeName = updatedBedConfig.BedType.Name;
                    }

                    await
                            this._vehicleToBedConfigIndexingService.UploadDocumentsAsync(existingVehicleToBedConfigDocuments.ToList());

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

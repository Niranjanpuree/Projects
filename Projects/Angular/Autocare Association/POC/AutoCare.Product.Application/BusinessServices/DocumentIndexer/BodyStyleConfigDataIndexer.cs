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
    public class BodyStyleConfigDataIndexer : IBodyStyleConfigDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBodyStyleConfigSearchService _vehicleToBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService;

        public BodyStyleConfigDataIndexer(IVcdbUnitOfWork repositories,
            IVehicleToBodyStyleConfigSearchService vehicleToBodyStyleConfigSearchService,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyStyleConfigIndexingService)
        {
            _vcdbUnitOfWork = repositories;
            _vehicleToBodyStyleConfigSearchService = vehicleToBodyStyleConfigSearchService;
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyStyleConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var bodyStyleConfigRepositoryService =
                           _vcdbUnitOfWork.GetRepositoryService<BodyStyleConfig>() as
                               IVcdbSqlServerEfRepositoryService<BodyStyleConfig>;

            if (bodyStyleConfigRepositoryService == null)
            {
                return;
            }

            var addedBodyStyleConfigs =
                                await
                                    bodyStyleConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "BodyNumDoors",
                                        "BodyType");

            if (addedBodyStyleConfigs == null || !addedBodyStyleConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Body Style Config Index cannot be updated before the transactional table is updated");
            }

            var addedBodyStyleConfig = addedBodyStyleConfigs.First();
            var vehicleToBodyStyleConfigSearchResult =
                                    await
                                        _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                                            $"bodyStyleConfigId eq {addedBodyStyleConfig.Id}");
            var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;

            if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
            {
                throw new InvalidOperationException(
                    "BodyStyleConfig already exisit in VehicleToBodyStyleConfigIndex. So, this change request cannot be an add request");
            }

            //body style config is new and therefore not yet available in "vehicletobodystyleconfigs" azure search index
            VehicleToBodyStyleConfigDocument newVehicleToBodyStyleConfigDocument = new VehicleToBodyStyleConfigDocument
            {
                VehicleToBodyStyleConfigId = Guid.NewGuid().ToString(),
                BodyStyleConfigId = addedBodyStyleConfig.Id,
                BodyNumDoorsId = addedBodyStyleConfig.BodyNumDoorsId,
                BodyNumDoors = addedBodyStyleConfig.BodyNumDoors.NumDoors,
                BodyTypeId = addedBodyStyleConfig.BodyTypeId,
                BodyTypeName = addedBodyStyleConfig.BodyType.Name
            };

            await this._vehicleToBodyStyleConfigIndexingService.UploadDocumentAsync(newVehicleToBodyStyleConfigDocument);
        }
        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var bodyStyleConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BodyStyleConfig>() as
                                IVcdbSqlServerEfRepositoryService<BodyStyleConfig>;

            if (bodyStyleConfigRepositoryService == null)
            {
                return;
            }

            var updatedBodyStyleConfigs =
                                await
                                    bodyStyleConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "BodyNumDoors",
                                        "BodyType");

            if (updatedBodyStyleConfigs == null || !updatedBodyStyleConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Body Style Config Index cannot be updated before the transactional table is updated");
            }

            var updatedBodyStyleConfig = updatedBodyStyleConfigs.First();

            await UpdateVehicleToBodyStyleConfigDocuments(updatedBodyStyleConfig);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: This following code will delete vehicletobodystyleconfig document form vehicletobodystyleconfigs index with body style config only information (no associated vehicles)
            var bodyStyleConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BodyStyleConfig>() as
                                IVcdbSqlServerEfRepositoryService<BodyStyleConfig>;

            if (bodyStyleConfigRepositoryService == null)
            {
                return;
            }

            var deletedBodyStyleConfigs =
                                await
                                    bodyStyleConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (deletedBodyStyleConfigs == null || !deletedBodyStyleConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Body Style Config Index cannot be updated before the transactional table is updated");
            }

            var deletedBodyStyleConfig = deletedBodyStyleConfigs.First();

            if (deletedBodyStyleConfig.VehicleToBodyStyleConfigs == null || deletedBodyStyleConfig.VehicleToBodyStyleConfigs.Count == 0)
            {
                var vehicleToBodyStyleConfigSearchResult =
                await
                    _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                        $"bodyStyleConfigId eq {deletedBodyStyleConfig.Id}");

                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;
                if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                    {
                        //existingVehicleToBodyStyleConfigDocument.VehicleToBodyStyleConfigId must be a GUID string
                        await this._vehicleToBodyStyleConfigIndexingService.DeleteDocumentByVehicleToBodyStyleConfigIdAsync(existingVehicleToBodyStyleConfigDocument.VehicleToBodyStyleConfigId);
                    }
                }
            }
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var bodyStyleConfigRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BodyStyleConfig>() as
                                IVcdbSqlServerEfRepositoryService<BodyStyleConfig>;

            if (bodyStyleConfigRepositoryService == null)
            {
                return;
            }

            var updatedBodyStyleConfigs =
                                await
                                    bodyStyleConfigRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedBodyStyleConfigs == null || !updatedBodyStyleConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Body Style Config Index cannot be updated before the transactional table is updated");
            }

            var updatedBodyStyleConfig = updatedBodyStyleConfigs.First();

            var vehicleToBodyConfigSearchResult =
                                    await
                                        _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                                            $"bodyStyleConfigId eq {updatedBodyStyleConfig.Id}");

            var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyConfigSearchResult.Documents;

            if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
            {
                foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                {
                    existingVehicleToBodyStyleConfigDocument.BodyStyleConfigChangeRequestId = -1;
                }

                await
                    this._vehicleToBodyStyleConfigIndexingService.UploadDocumentsAsync(
                        existingVehicleToBodyStyleConfigDocuments.ToList());
            }
        }

        private async Task UpdateVehicleToBodyStyleConfigDocuments(BodyStyleConfig updatedBodyStyleConfig)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBodyStyleConfigSearchResult =
                                    await
                                        _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                                            $"bodyStyleConfigId eq {updatedBodyStyleConfig.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;

                if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.BodyStyleConfigChangeRequestId = -1;
                        existingVehicleToBodyStyleConfigDocument.BodyNumDoorsId = updatedBodyStyleConfig.BodyNumDoorsId;
                        existingVehicleToBodyStyleConfigDocument.BodyNumDoors = updatedBodyStyleConfig.BodyNumDoors.NumDoors;
                        existingVehicleToBodyStyleConfigDocument.BodyTypeId = updatedBodyStyleConfig.BodyTypeId;
                        existingVehicleToBodyStyleConfigDocument.BodyTypeName = updatedBodyStyleConfig.BodyType.Name;
                    }

                    await
                                this._vehicleToBodyStyleConfigIndexingService.UploadDocumentsAsync(existingVehicleToBodyStyleConfigDocuments.ToList());

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

using System;
using System.Collections.Generic;
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
    public class VehicleToBodyStyleConfigDataIndexer : IVehicleToBodyStyleConfigDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBodyStyleConfigSearchService _vehicleToBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyConfigIndexingService;

        public VehicleToBodyStyleConfigDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            VehicleToBodyStyleConfigSearchService vehicletoBodyConfigSearchService,
            VehicleToBodyStyleConfigIndexingService vehicleToBodyConfigIndexingService)
            
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleToBodyStyleConfigSearchService = vehicletoBodyConfigSearchService;
            _vehicleToBodyConfigIndexingService = vehicleToBodyConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBodyConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBodyStyleConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBodyStyleConfig>;

            if (vehicletoBodyConfigRepositoryService == null)
            {
                return;
            }

            var addedVehicleToBodyConfigs =
                        await
                            vehicletoBodyConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BodyStyleConfig.BodyNumDoors",
                                "BodyStyleConfig.BodyType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (addedVehicleToBodyConfigs == null || !addedVehicleToBodyConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Body Config Index cannot be updated before the transactional table is updated");
            }

            //delete document with vehicleToBodyStyleConfigId of Guid and bedConfigId of addedVehicleToBedConfigs.BedConfigId
            var vehicleToBodyConfigSearchResult =
                                    await
                                        _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                                            $"bodyStyleConfigId eq {addedVehicleToBodyConfigs.First().BodyStyleConfigId}");

            var existingVehicleToBodyConfigDocuments = vehicleToBodyConfigSearchResult.Documents;
            if (existingVehicleToBodyConfigDocuments != null && existingVehicleToBodyConfigDocuments.Any())
            {
                Guid guid;
                foreach (var existingVehicleToBodyConfigDocument in existingVehicleToBodyConfigDocuments)
                {
                    if (Guid.TryParse(existingVehicleToBodyConfigDocument.VehicleToBodyStyleConfigId, out guid))
                    {
                        await
                            this._vehicleToBodyConfigIndexingService.DeleteDocumentByVehicleToBodyStyleConfigIdAsync(
                                existingVehicleToBodyConfigDocument.VehicleToBodyStyleConfigId);
                    }
                }
            }

            await InsertOrUpdateVehicleToBodyConfigDocuments(addedVehicleToBodyConfigs);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBodyConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBodyStyleConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBodyStyleConfig>;

            if (vehicletoBodyConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBodyConfigs =
                        await
                            vehicletoBodyConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BodyStyleConfig.BodyNumDoors",
                                "BodyStyleConfig.BodyType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (updatedVehicleToBodyConfigs == null || !updatedVehicleToBodyConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Body Config Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToBodyConfigDocuments(updatedVehicleToBodyConfigs);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: this method is called when processing a REPLACE bed config as MODIFY of vehicletobedconfigs under that bed config
            var vehicletoBodyConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBodyStyleConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBodyStyleConfig>;

            if (vehicletoBodyConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBodyConfigs =
                        await
                            vehicletoBodyConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BodyStyleConfig.BodyNumDoors",
                                "BodyStyleConfig.BodyType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");


            if (updatedVehicleToBodyConfigs == null || !updatedVehicleToBodyConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Body Config Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToBodyConfigDocuments(updatedVehicleToBodyConfigs, true);

            await ClearBodyConfigChangeRequestId(changeRequestId);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBodyConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBodyStyleConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBodyStyleConfig>;

            if (vehicletoBodyConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBodyConfigs =
                await vehicletoBodyConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicleToBodyConfigs == null || !updatedVehicleToBodyConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Body Config Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToBodyConfig in updatedVehicleToBodyConfigs)
            {
                await
                    this._vehicleToBodyConfigIndexingService.DeleteDocumentByVehicleToBodyStyleConfigIdAsync(
                        updatedVehicleToBodyConfig.Id.ToString());
            }

            //Required when processing bedconfig REPLACE CR
            await ClearBodyConfigChangeRequestId(changeRequestId);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBodyConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBodyStyleConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBodyStyleConfig>;

            if (vehicletoBodyConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBodyConfigs =
                        await
                            vehicletoBodyConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);


            if (updatedVehicleToBodyConfigs == null || !updatedVehicleToBodyConfigs.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Body Config Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToBodyConfig in updatedVehicleToBodyConfigs)
            {
                var vehicletoBodyConfigDocument = new VehicleToBodyStyleConfigDocument
                {
                    VehicleToBodyStyleConfigId = updatedVehicleToBodyConfig.Id.ToString(),
                    VehicleToBodyStyleConfigChangeRequestId = -1,
                };

                await
                    this._vehicleToBodyConfigIndexingService.UploadDocumentAsync(vehicletoBodyConfigDocument);
            }

            //Required when processing bedconfig REPLACE CR
            await ClearBodyConfigChangeRequestId(changeRequestId);
        }

        private async Task InsertOrUpdateVehicleToBodyConfigDocuments(List<VehicleToBodyStyleConfig> updatedVehicleToBodyConfigs, bool isReplace = false)
        {
            if (updatedVehicleToBodyConfigs == null)
            {
                return;
            }

            foreach (var updatedVehicleToBodyConfig in updatedVehicleToBodyConfigs)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            {
                var vehicletoBodyConfigDocument = new VehicleToBodyStyleConfigDocument
                {
                    VehicleToBodyStyleConfigId = updatedVehicleToBodyConfig.Id.ToString(),
                    BodyStyleConfigChangeRequestId = isReplace ? -1 : (long?)null,
                    VehicleToBodyStyleConfigChangeRequestId = -1,
                    BaseVehicleId = updatedVehicleToBodyConfig.Vehicle.BaseVehicleId,
                    BodyNumDoorsId = updatedVehicleToBodyConfig.BodyStyleConfig.BodyNumDoorsId,
                    BodyTypeName = updatedVehicleToBodyConfig.BodyStyleConfig.BodyType.Name,
                    BodyTypeId = updatedVehicleToBodyConfig.BodyStyleConfig.BodyTypeId,
                    BodyNumDoors = updatedVehicleToBodyConfig.BodyStyleConfig.BodyNumDoors.NumDoors,
                    BodyStyleConfigId = updatedVehicleToBodyConfig.BodyStyleConfig.Id,
                    MakeId = updatedVehicleToBodyConfig.Vehicle.BaseVehicle.MakeId,
                    MakeName = updatedVehicleToBodyConfig.Vehicle.BaseVehicle.Make.Name,
                    ModelId = updatedVehicleToBodyConfig.Vehicle.BaseVehicle.ModelId,
                    ModelName = updatedVehicleToBodyConfig.Vehicle.BaseVehicle.Model.Name,
                    RegionId = updatedVehicleToBodyConfig.Vehicle.RegionId,
                    RegionName = updatedVehicleToBodyConfig.Vehicle.Region.Name,
                    //Source = updatedVehicleToBodyConfig.Source,
                    SubModelId = updatedVehicleToBodyConfig.Vehicle.SubModelId,
                    SubModelName = updatedVehicleToBodyConfig.Vehicle.SubModel.Name,
                    VehicleId = updatedVehicleToBodyConfig.VehicleId,
                    VehicleTypeGroupId =
                        updatedVehicleToBodyConfig.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName =
                        updatedVehicleToBodyConfig.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = updatedVehicleToBodyConfig.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = updatedVehicleToBodyConfig.Vehicle.BaseVehicle.Model.VehicleType.Name,
                    YearId = updatedVehicleToBodyConfig.Vehicle.BaseVehicle.YearId,
                };

                await
                    this._vehicleToBodyConfigIndexingService.UploadDocumentAsync(vehicletoBodyConfigDocument);
            }
        }

        private async Task ClearBodyConfigChangeRequestId(long changeRequestId)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                    await
                        _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                            $"bodyStyleConfigChangeRequestId eq {changeRequestId}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleDocument.BodyStyleConfigChangeRequestId = -1;
                    }

                    await
                        this._vehicleToBodyConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBedConfigDocuments.ToList());
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

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
    public class VehicleToBedConfigDataIndexer : IVehicleToBedConfigDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBedConfigSearchService _vehicleToBedConfigSearchService;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService;

        public VehicleToBedConfigDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleToBedConfigSearchService = vehicletoBedConfigSearchService;
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBedConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBedConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBedConfig>;

            if (vehicletoBedConfigRepositoryService == null)
            {
                return;
            }

            var addedVehicleToBedConfigs =
                        await
                            vehicletoBedConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BedConfig.BedLength",
                                "BedConfig.BedType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (addedVehicleToBedConfigs == null || !addedVehicleToBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Bed Config Index cannot be updated before the transactional table is updated");
            }
            
            //delete document with vehicleToBedConfigId of Guid and bedConfigId of addedVehicleToBedConfigs.BedConfigId
            var vehicleToBedConfigSearchResult =
                                    await
                                        _vehicleToBedConfigSearchService.SearchAsync(null,
                                            $"bedConfigId eq {addedVehicleToBedConfigs.First().BedConfigId}");

            var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
            if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
            {
                Guid guid;
                foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                {
                    if (Guid.TryParse(existingVehicleToBedConfigDocument.VehicleToBedConfigId, out guid))
                    {
                        await
                            this._vehicleToBedConfigIndexingService.DeleteDocumentByVehicleToBedConfigIdAsync(
                                existingVehicleToBedConfigDocument.VehicleToBedConfigId);
                    }
                }
            }

            await InsertOrUpdateVehicleToBedConfigDocuments(addedVehicleToBedConfigs);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBedConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBedConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBedConfig>;

            if (vehicletoBedConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBedConfigs =
                        await
                            vehicletoBedConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BedConfig.BedLength",
                                "BedConfig.BedType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (updatedVehicleToBedConfigs == null || !updatedVehicleToBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Bed Config Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToBedConfigDocuments(updatedVehicleToBedConfigs);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: this method is called when processing a REPLACE bed config as MODIFY of vehicletobedconfigs under that bed config
            var vehicletoBedConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBedConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBedConfig>;

            if (vehicletoBedConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBedConfigs =
                        await
                            vehicletoBedConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BedConfig.BedLength",
                                "BedConfig.BedType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");


            if (updatedVehicleToBedConfigs == null || !updatedVehicleToBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Bed Config Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToBedConfigDocuments(updatedVehicleToBedConfigs, true);

            await ClearBedConfigChangeRequestId(changeRequestId);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBedConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBedConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBedConfig>;

            if (vehicletoBedConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBedConfigs =
                await vehicletoBedConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicleToBedConfigs == null || !updatedVehicleToBedConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Bed Config Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToBedConfig in updatedVehicleToBedConfigs)
            {
                await
                    this._vehicleToBedConfigIndexingService.DeleteDocumentByVehicleToBedConfigIdAsync(
                        updatedVehicleToBedConfig.Id.ToString());
            }

            //Required when processing bedconfig REPLACE CR
            await ClearBedConfigChangeRequestId(changeRequestId);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBedConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBedConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBedConfig>;

            if (vehicletoBedConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBedConfigs =
                        await
                            vehicletoBedConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);


            if (updatedVehicleToBedConfigs == null || !updatedVehicleToBedConfigs.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Bed Config Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToBedConfig in updatedVehicleToBedConfigs)
            {
                var vehicletoBedConfigDocument = new VehicleToBedConfigDocument
                {
                    VehicleToBedConfigId = updatedVehicleToBedConfig.Id.ToString(),
                    VehicleToBedConfigChangeRequestId = -1,
                };

                await
                    this._vehicleToBedConfigIndexingService.UploadDocumentAsync(vehicletoBedConfigDocument);
            }

            //Required when processing bedconfig REPLACE CR
            await ClearBedConfigChangeRequestId(changeRequestId);
        }

        private async Task InsertOrUpdateVehicleToBedConfigDocuments(List<VehicleToBedConfig> updatedVehicleToBedConfigs, bool isReplace = false)
        {
            if (updatedVehicleToBedConfigs == null)
            {
                return;
            }

            foreach (var updatedVehicleToBedConfig in updatedVehicleToBedConfigs)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            {
                var vehicletoBedConfigDocument = new VehicleToBedConfigDocument
                {
                    VehicleToBedConfigId = updatedVehicleToBedConfig.Id.ToString(),
                    BedConfigChangeRequestId = isReplace ? -1 : (long?)null,
                    VehicleToBedConfigChangeRequestId = -1,
                    BaseVehicleId = updatedVehicleToBedConfig.Vehicle.BaseVehicleId,
                    BedTypeId = updatedVehicleToBedConfig.BedConfig.BedTypeId,
                    BedTypeName = updatedVehicleToBedConfig.BedConfig.BedType.Name,
                    BedLengthId = updatedVehicleToBedConfig.BedConfig.BedTypeId,
                    BedLength = updatedVehicleToBedConfig.BedConfig.BedLength.Length,
                    BedLengthMetric = updatedVehicleToBedConfig.BedConfig.BedLength.BedLengthMetric,
                    BedConfigId = updatedVehicleToBedConfig.BedConfigId,
                    MakeId = updatedVehicleToBedConfig.Vehicle.BaseVehicle.MakeId,
                    MakeName = updatedVehicleToBedConfig.Vehicle.BaseVehicle.Make.Name,
                    ModelId = updatedVehicleToBedConfig.Vehicle.BaseVehicle.ModelId,
                    ModelName = updatedVehicleToBedConfig.Vehicle.BaseVehicle.Model.Name,
                    RegionId = updatedVehicleToBedConfig.Vehicle.RegionId,
                    RegionName = updatedVehicleToBedConfig.Vehicle.Region.Name,
                    Source = updatedVehicleToBedConfig.Vehicle.SourceName,
                    SubModelId = updatedVehicleToBedConfig.Vehicle.SubModelId,
                    SubModelName = updatedVehicleToBedConfig.Vehicle.SubModel.Name,
                    VehicleId = updatedVehicleToBedConfig.VehicleId,
                    VehicleTypeGroupId =
                        updatedVehicleToBedConfig.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName =
                        updatedVehicleToBedConfig.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = updatedVehicleToBedConfig.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = updatedVehicleToBedConfig.Vehicle.BaseVehicle.Model.VehicleType.Name,
                    YearId = updatedVehicleToBedConfig.Vehicle.BaseVehicle.YearId,
                };

                await
                    this._vehicleToBedConfigIndexingService.UploadDocumentAsync(vehicletoBedConfigDocument);
            }
        }

        private async Task ClearBedConfigChangeRequestId(long changeRequestId)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                    await
                        _vehicleToBedConfigSearchService.SearchAsync(null,
                            $"bedConfigChangeRequestId eq {changeRequestId}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleDocument.BedConfigChangeRequestId = -1;
                    }

                    await
                        this._vehicleToBedConfigIndexingService.UploadDocumentsAsync(
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

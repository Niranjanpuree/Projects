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
    public class VehicleToBrakeConfigDataIndexer : IVehicleToBrakeConfigDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBrakeConfigSearchService _vehicleToBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;

        public VehicleToBrakeConfigDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleToBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBrakeConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBrakeConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBrakeConfig>;

            if (vehicletoBrakeConfigRepositoryService == null)
            {
                return;
            }

            var addedVehicleToBrakeConfigs =
                        await
                            vehicletoBrakeConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BrakeConfig.BrakeABS",
                                "BrakeConfig.BrakeSystem",
                                "BrakeConfig.FrontBrakeType",
                                "BrakeConfig.RearBrakeType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (addedVehicleToBrakeConfigs == null || !addedVehicleToBrakeConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Brake Config Index cannot be updated before the transactional table is updated");
            }

            //delete document with vehicleToBrakeConfigId of Guid and brakeConfigId of addedVehicleToBrakeConfigs.BrakeConfigId
            var vehicleToBrakeConfigSearchResult =
                                    await
                                        _vehicleToBrakeConfigSearchService.SearchAsync(null,
                                            $"brakeConfigId eq {addedVehicleToBrakeConfigs.First().BrakeConfigId}");

            var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;
            if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
            {
                Guid guid;
                foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                {
                    if (Guid.TryParse(existingVehicleToBrakeConfigDocument.VehicleToBrakeConfigId, out guid))
                    {
                        await
                            this._vehicleToBrakeConfigIndexingService.DeleteDocumentByVehicleToBrakeConfigIdAsync(
                                existingVehicleToBrakeConfigDocument.VehicleToBrakeConfigId);
                    }
                }
            }

            await InsertOrUpdateVehicleToBrakeConfigDocuments(addedVehicleToBrakeConfigs);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBrakeConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBrakeConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBrakeConfig>;

            if (vehicletoBrakeConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBrakeConfigs =
                        await
                            vehicletoBrakeConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BrakeConfig.BrakeABS",
                                "BrakeConfig.BrakeSystem",
                                "BrakeConfig.FrontBrakeType",
                                "BrakeConfig.RearBrakeType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (updatedVehicleToBrakeConfigs == null || !updatedVehicleToBrakeConfigs.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Brake Config Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToBrakeConfigDocuments(updatedVehicleToBrakeConfigs);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: this method is called when processing a REPLACE brake config as MODIFY of vehicletobrakeconfigs under that brake config
            var vehicletoBrakeConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBrakeConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBrakeConfig>;

            if (vehicletoBrakeConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBrakeConfigs =
                        await
                            vehicletoBrakeConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "BrakeConfig.BrakeABS",
                                "BrakeConfig.BrakeSystem",
                                "BrakeConfig.FrontBrakeType",
                                "BrakeConfig.RearBrakeType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");


            if (updatedVehicleToBrakeConfigs == null || !updatedVehicleToBrakeConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Brake Config Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToBrakeConfigDocuments(updatedVehicleToBrakeConfigs, true);

            await ClearBrakeConfigChangeRequestId(changeRequestId);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBrakeConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBrakeConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBrakeConfig>;

            if (vehicletoBrakeConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBrakeConfigs =
                await vehicletoBrakeConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicleToBrakeConfigs == null || !updatedVehicleToBrakeConfigs.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Brake Config Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToBrakeConfig in updatedVehicleToBrakeConfigs)
            {
                await
                    this._vehicleToBrakeConfigIndexingService.DeleteDocumentByVehicleToBrakeConfigIdAsync(
                        updatedVehicleToBrakeConfig.Id.ToString());
            }

            //Required when processing brakeconfig REPLACE CR
            await ClearBrakeConfigChangeRequestId(changeRequestId);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoBrakeConfigRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToBrakeConfig>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToBrakeConfig>;

            if (vehicletoBrakeConfigRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToBrakeConfigs =
                        await
                            vehicletoBrakeConfigRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);


            if (updatedVehicleToBrakeConfigs == null || !updatedVehicleToBrakeConfigs.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Brake Config Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToBrakeConfig in updatedVehicleToBrakeConfigs)
            {
                var vehicletoBrakeConfigDocument = new VehicleToBrakeConfigDocument
                {
                    VehicleToBrakeConfigId = updatedVehicleToBrakeConfig.Id.ToString(),
                    VehicleToBrakeConfigChangeRequestId = -1,
                };

                await
                    this._vehicleToBrakeConfigIndexingService.UploadDocumentAsync(vehicletoBrakeConfigDocument);
            }

            //Required when processing brakeconfig REPLACE CR
            await ClearBrakeConfigChangeRequestId(changeRequestId);
        }

        private async Task InsertOrUpdateVehicleToBrakeConfigDocuments(List<VehicleToBrakeConfig> updatedVehicleToBrakeConfigs, bool isReplace = false)
        {
            if (updatedVehicleToBrakeConfigs == null)
            {
                return;
            }

            foreach (var updatedVehicleToBrakeConfig in updatedVehicleToBrakeConfigs)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            {
                var vehicletoBrakeConfigDocument = new VehicleToBrakeConfigDocument
                {
                    VehicleToBrakeConfigId = updatedVehicleToBrakeConfig.Id.ToString(),
                    BrakeConfigChangeRequestId = isReplace ? -1 : (long?)null,
                    VehicleToBrakeConfigChangeRequestId = -1,
                    BaseVehicleId = updatedVehicleToBrakeConfig.Vehicle.BaseVehicleId,
                    BrakeABSId = updatedVehicleToBrakeConfig.BrakeConfig.BrakeABSId,
                    BrakeABSName = updatedVehicleToBrakeConfig.BrakeConfig.BrakeABS.Name,
                    BrakeConfigId = updatedVehicleToBrakeConfig.BrakeConfigId,
                    BrakeSystemId = updatedVehicleToBrakeConfig.BrakeConfig.BrakeSystemId,
                    BrakeSystemName = updatedVehicleToBrakeConfig.BrakeConfig.BrakeSystem.Name,
                    FrontBrakeTypeId = updatedVehicleToBrakeConfig.BrakeConfig.FrontBrakeTypeId,
                    FrontBrakeTypeName = updatedVehicleToBrakeConfig.BrakeConfig.FrontBrakeType.Name,
                    RearBrakeTypeId = updatedVehicleToBrakeConfig.BrakeConfig.RearBrakeTypeId,
                    RearBrakeTypeName = updatedVehicleToBrakeConfig.BrakeConfig.RearBrakeType.Name,
                    MakeId = updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.MakeId,
                    MakeName = updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.Make.Name,
                    ModelId = updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.ModelId,
                    ModelName = updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.Model.Name,
                    RegionId = updatedVehicleToBrakeConfig.Vehicle.RegionId,
                    RegionName = updatedVehicleToBrakeConfig.Vehicle.Region.Name,
                    Source = updatedVehicleToBrakeConfig.Vehicle.SourceName,
                    SubModelId = updatedVehicleToBrakeConfig.Vehicle.SubModelId,
                    SubModelName = updatedVehicleToBrakeConfig.Vehicle.SubModel.Name,
                    VehicleId = updatedVehicleToBrakeConfig.VehicleId,
                    VehicleTypeGroupId =
                        updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName =
                        updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.Model.VehicleType.Name,
                    YearId = updatedVehicleToBrakeConfig.Vehicle.BaseVehicle.YearId,
                };

                await
                    this._vehicleToBrakeConfigIndexingService.UploadDocumentAsync(vehicletoBrakeConfigDocument);
            }
        }

        private async Task ClearBrakeConfigChangeRequestId(long changeRequestId)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                    await
                        _vehicleToBrakeConfigSearchService.SearchAsync(null,
                            $"brakeConfigChangeRequestId eq {changeRequestId}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;
                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleDocument.BrakeConfigChangeRequestId = -1;
                    }

                    await
                        this._vehicleToBrakeConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBrakeConfigDocuments.ToList());
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

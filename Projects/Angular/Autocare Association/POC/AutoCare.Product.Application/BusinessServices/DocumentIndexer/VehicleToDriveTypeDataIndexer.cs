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
    public class VehicleToDriveTypeDataIndexer : IVehicleToDriveTypeDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToDriveTypeSearchService _vehicleToDriveTypeSearchService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService;

        public VehicleToDriveTypeDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToDriveTypeSearchService vehicleToDriveTypeSearchService,
            IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleToDriveTypeSearchService = vehicleToDriveTypeSearchService;
            _vehicleToDriveTypeIndexingService = vehicleToDriveTypeIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoDriveTypeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToDriveType>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToDriveType>;

            if (vehicletoDriveTypeRepositoryService == null)
            {
                return;
            }

            var addedVehicleToDriveTypes =
                        await
                            vehicletoDriveTypeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "DriveType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (addedVehicleToDriveTypes == null || !addedVehicleToDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Drive Type Index cannot be updated before the transactional table is updated");
            }

            //delete document with vehicleToMfrBodyCodeConfigId of Guid and MfrBodyCodeId of addedVehicleToMfrBodyCodes.MfrBodyCodeId
            var vehicleToDriveTypeSearchResult =
                                    await
                                        _vehicleToDriveTypeSearchService.SearchAsync(null,
                                            $"driveTypeId eq {addedVehicleToDriveTypes.First().DriveTypeId}");

            var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;
            if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
            {
                Guid guid;
                foreach (var existingVehicleToDriveTypeDocument in existingVehicleToDriveTypeDocuments)
                {
                    if (Guid.TryParse(existingVehicleToDriveTypeDocument.VehicleToDriveTypeId, out guid))
                    {
                        await
                            this._vehicleToDriveTypeIndexingService.DeleteDocumentByVehicleToDriveTypeIdAsync(
                                existingVehicleToDriveTypeDocument.VehicleToDriveTypeId);
                    }
                }
            }

            await InsertOrUpdateVehicleToDriveTypeDocuments(addedVehicleToDriveTypes);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoDriveTypeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToDriveType>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToDriveType>;

            if (vehicletoDriveTypeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToDriveTypes =
                        await
                            vehicletoDriveTypeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "DriveType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (updatedVehicleToDriveTypes == null || !updatedVehicleToDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Drive Type Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToDriveTypeDocuments(updatedVehicleToDriveTypes);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: this method is called when processing a REPLACE MfrBodyCode as MODIFY of vehicletoMfrBodyCodeconfigs under that MfrBodyCode
            var vehicletoDriveTypeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToDriveType>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToDriveType>;

            if (vehicletoDriveTypeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToDriveTypes =
                        await
                            vehicletoDriveTypeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "DriveType",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");


            if (updatedVehicleToDriveTypes == null || !updatedVehicleToDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Drive Type Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToDriveTypeDocuments(updatedVehicleToDriveTypes, true);

            await ClearDriveTypeChangeRequestId(changeRequestId);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoDriveTypeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToDriveType>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToDriveType>;

            if (vehicletoDriveTypeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToDriveTypes =
                await vehicletoDriveTypeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicleToDriveTypes == null || !updatedVehicleToDriveTypes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Drive Type Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToDriveType in updatedVehicleToDriveTypes)
            {
                await
                    this._vehicleToDriveTypeIndexingService.DeleteDocumentByVehicleToDriveTypeIdAsync(
                        updatedVehicleToDriveType.Id.ToString());
            }

            //Required when processing MfrBodyCode REPLACE CR
            await ClearDriveTypeChangeRequestId(changeRequestId);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoDriveTypeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToDriveType>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToDriveType>;

            if (vehicletoDriveTypeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToDriveTypes =
                        await
                            vehicletoDriveTypeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);


            if (updatedVehicleToDriveTypes == null || !updatedVehicleToDriveTypes.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Drive Type cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToDriveType in updatedVehicleToDriveTypes)
            {
                var vehicletoDriveTypeDocument = new VehicleToDriveTypeDocument
                {
                    VehicleToDriveTypeId = updatedVehicleToDriveType.Id.ToString(),
                    VehicleToDriveTypeChangeRequestId = -1,
                };

                await
                    this._vehicleToDriveTypeIndexingService.UploadDocumentAsync(vehicletoDriveTypeDocument);
            }

            //Required when processing MfrBodyCode REPLACE CR
            await ClearDriveTypeChangeRequestId(changeRequestId);
        }

        private async Task InsertOrUpdateVehicleToDriveTypeDocuments(List<VehicleToDriveType> updatedVehicleToDriveTypes, bool isReplace = false)
        {
            if (updatedVehicleToDriveTypes == null)
            {
                return;
            }

            foreach (var updatedVehicleToDriveType in updatedVehicleToDriveTypes)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            {
                var vehicletoDriveTypeDocument = new VehicleToDriveTypeDocument
                {
                    VehicleToDriveTypeId = updatedVehicleToDriveType.Id.ToString(),
                    DriveTypeChangeRequestId = isReplace ? -1 : (long?)null,
                    VehicleToDriveTypeChangeRequestId = -1,
                    BaseVehicleId = updatedVehicleToDriveType.Vehicle.BaseVehicleId,
                    DriveTypeId = updatedVehicleToDriveType.DriveType.Id,
                    DriveTypeName = updatedVehicleToDriveType.DriveType.Name,
                    MakeId = updatedVehicleToDriveType.Vehicle.BaseVehicle.MakeId,
                    MakeName = updatedVehicleToDriveType.Vehicle.BaseVehicle.Make.Name,
                    ModelId = updatedVehicleToDriveType.Vehicle.BaseVehicle.ModelId,
                    ModelName = updatedVehicleToDriveType.Vehicle.BaseVehicle.Model.Name,
                    RegionId = updatedVehicleToDriveType.Vehicle.RegionId,
                    RegionName = updatedVehicleToDriveType.Vehicle.Region.Name,
                    Source = updatedVehicleToDriveType.Vehicle.SourceName,
                    SubModelId = updatedVehicleToDriveType.Vehicle.SubModelId,
                    SubModelName = updatedVehicleToDriveType.Vehicle.SubModel.Name,
                    VehicleId = updatedVehicleToDriveType.VehicleId,
                    VehicleTypeGroupId =
                        updatedVehicleToDriveType.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName =
                        updatedVehicleToDriveType.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = updatedVehicleToDriveType.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = updatedVehicleToDriveType.Vehicle.BaseVehicle.Model.VehicleType.Name,
                    YearId = updatedVehicleToDriveType.Vehicle.BaseVehicle.YearId,
                };

                await
                    this._vehicleToDriveTypeIndexingService.UploadDocumentAsync(vehicletoDriveTypeDocument);
            }
        }

        private async Task ClearDriveTypeChangeRequestId(long changeRequestId)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToDriveTypeSearchResult =
                    await
                        _vehicleToDriveTypeSearchService.SearchAsync(null,
                            $"driveTypeChangeRequestId eq {changeRequestId}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;
                if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleToDriveTypeDocuments)
                    {
                        existingVehicleDocument.DriveTypeChangeRequestId = -1;
                    }

                    await
                        this._vehicleToDriveTypeIndexingService.UploadDocumentsAsync(
                            existingVehicleToDriveTypeDocuments.ToList());
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

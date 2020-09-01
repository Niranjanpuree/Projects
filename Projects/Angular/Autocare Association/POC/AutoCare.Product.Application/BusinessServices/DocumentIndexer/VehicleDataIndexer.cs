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
    public class VehicleDataIndexer : IVehicleDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleSearchService _vehicleSearchService;
        private readonly IVehicleIndexingService _vehicleIndexingService;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;
        private readonly IVehicleToBedConfigSearchService _vehicletoBedConfigSearchService;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService;
        private readonly IVehicleToBodyStyleConfigSearchService _vehicletoBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService;
        private readonly IVehicleToWheelBaseSearchService _vehicletoWheelBaseConfigSearchService;
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseConfigIndexingService;
        private readonly IVehicleToDriveTypeSearchService _vehicletoDriveTypeConfigSearchService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeConfigIndexingService;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicletoMfrBodyCodeConfigSearchService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeConfigIndexingService;

        public VehicleDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleSearchService vehicleSearchService,
            IVehicleIndexingService vehicleIndexingService,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService,
            IVehicleToBodyStyleConfigSearchService vehicletoBodyStyleConfigSearchService,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyStyleConfigIndexingService,
            IVehicleToWheelBaseSearchService vehicletoWheelBaseSearchService,
            IVehicleToWheelBaseIndexingService vehicleToWheelBaseIndexingService,
            IVehicleToDriveTypeSearchService vehicletoDriveTypeSearchService,
            IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService,
            IVehicleToMfrBodyCodeSearchService vehicletoMfrBodyCodeSearchService,
            IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleSearchService = vehicleSearchService;
            _vehicleIndexingService = vehicleIndexingService;
            _vehicletoBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
            _vehicletoBedConfigSearchService = vehicletoBedConfigSearchService;
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
            _vehicletoBodyStyleConfigSearchService = vehicletoBodyStyleConfigSearchService;
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyStyleConfigIndexingService;
            _vehicletoWheelBaseConfigSearchService = vehicletoWheelBaseSearchService;
            _vehicleToWheelBaseConfigIndexingService = vehicleToWheelBaseIndexingService;
            _vehicletoDriveTypeConfigSearchService = vehicletoDriveTypeSearchService;
            _vehicleToDriveTypeConfigIndexingService = vehicleToDriveTypeIndexingService;
            _vehicletoMfrBodyCodeConfigSearchService = vehicletoMfrBodyCodeSearchService;
            _vehicleToMfrBodyCodeConfigIndexingService = vehicleToMfrBodyCodeIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicleRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;

            if (vehicleRepositoryService == null)
            {
                return;
            }

            var addedVehicles =
                    await vehicleRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                        "SubModel",
                        "Region",
                        "BaseVehicle.Make",
                        "BaseVehicle.Model",
                        "BaseVehicle.Model.VehicleType",
                        "BaseVehicle.Model.VehicleType.VehicleTypeGroup");

            if (addedVehicles == null || !addedVehicles.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle Index cannot be updated before the transactional table is updated");
            }

            //delete document with vehicleId of Guid and baseVeicleId of addedVehicles.BaseVehicleId
            var vehicleSearchResult =
                await
                    _vehicleSearchService.SearchAsync(null, $"baseVehicleId eq {addedVehicles.First().BaseVehicleId}");

            var existingVehicleDocuments = vehicleSearchResult.Documents;
            if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
            {
                Guid guid;
                foreach (var existingVehicleDocument in existingVehicleDocuments)
                {
                    if (Guid.TryParse(existingVehicleDocument.VehicleId, out guid))
                    {
                        await
                            this._vehicleIndexingService.DeleteDocumentByVehicleIdAsync(
                                existingVehicleDocument.VehicleId);
                    }
                }
            }

            await InsertOrUpdateVehicleDocuments(addedVehicles);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicleRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;

            if (vehicleRepositoryService == null)
            {
                return;
            }

            var updatedVehicles =
                    await vehicleRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                        "SubModel",
                        "Region",
                        "BaseVehicle.Make",
                        "BaseVehicle.Model",
                        "BaseVehicle.Model.VehicleType",
                        "BaseVehicle.Model.VehicleType.VehicleTypeGroup");

            if (updatedVehicles == null || !updatedVehicles.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleDocuments(updatedVehicles);

            await UpdateVehicleToBrakeConfigDocuments(updatedVehicles);
            await UpdateVehicleToBedConfigDocuments(updatedVehicles);
            await UpdateVehicleToBodyStyleConfigDocuments(updatedVehicles);
            await UpdateVehicleToWheelBaseConfigDocuments(updatedVehicles);
            await UpdateVehicleToDriveTypeConfigDocuments(updatedVehicles);
            await UpdateVehicleToMfrBodyCodeConfigDocuments(updatedVehicles);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: this method is called when processing a REPLACE base vehicle as MODIFY of vehicles under that base vehicle
            var vehicleRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;

            if (vehicleRepositoryService == null)
            {
                return;
            }

            var updatedVehicles =
                    await vehicleRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                        "SubModel",
                        "Region",
                        "BaseVehicle.Make",
                        "BaseVehicle.Model",
                        "BaseVehicle.Model.VehicleType",
                        "BaseVehicle.Model.VehicleType.VehicleTypeGroup");

            if (updatedVehicles == null || !updatedVehicles.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleDocuments(updatedVehicles, true);

            await UpdateVehicleToBrakeConfigDocuments(updatedVehicles);

            await ClearBaseVehicleChangeRequestId(changeRequestId);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicleRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;

            if (vehicleRepositoryService == null)
            {
                return;
            }

            var deletedVehicles =
                await vehicleRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (deletedVehicles == null || !deletedVehicles.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle Index cannot be updated before the transactional table is updated");
            }

            foreach (var deletedVehicle in deletedVehicles)
            {
                await
                    this._vehicleIndexingService.DeleteDocumentByVehicleIdAsync(deletedVehicle.Id.ToString());
            }

            //Required when processing basevehicle REPLACE CR
            await ClearBaseVehicleChangeRequestId(changeRequestId);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicleRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;

            if (vehicleRepositoryService == null)
            {
                return;
            }

            var updatedVehicles =
                    await vehicleRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicles == null || !updatedVehicles.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicle in updatedVehicles)
            {
                var vehicleDocument = new VehicleDocument
                {
                    VehicleId = updatedVehicle.Id.ToString(),
                    VehicleChangeRequestId = -1,
                };

                await this._vehicleIndexingService.UploadDocumentAsync(vehicleDocument);
            }

            //Required when processing basevehicle REPLACE CR
            await ClearBaseVehicleChangeRequestId(changeRequestId);
        }

        private async Task InsertOrUpdateVehicleDocuments(List<Vehicle> updatedVehicles, bool isReplace = false)
        {
            if (updatedVehicles == null)
            {
                return;
            }

            foreach (var updatedVehicle in updatedVehicles)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            {
                var vehicleDocument = new VehicleDocument
                {
                    VehicleId = updatedVehicle.Id.ToString(),
                    //Question: Raja: I am not sure why do we use 2 different value for BaseVehicleChangeRequestId
                    //Pushkar: -1 is kept to forcefully clear previous value since setting a null will cause document update process to retain previous value.
                    BaseVehicleChangeRequestId = isReplace ? -1 : (long?)null,
                    VehicleChangeRequestId = -1,
                    BaseVehicleId = updatedVehicle.BaseVehicleId,
                    MakeId = updatedVehicle.BaseVehicle.MakeId,
                    MakeName = updatedVehicle.BaseVehicle.Make.Name,
                    ModelId = updatedVehicle.BaseVehicle.ModelId,
                    ModelName = updatedVehicle.BaseVehicle.Model.Name,
                    RegionId = updatedVehicle.RegionId,
                    RegionName = updatedVehicle.Region.Name,
                    Source = updatedVehicle.SourceName,
                    SubModelId = updatedVehicle.SubModelId,
                    SubModelName = updatedVehicle.SubModel.Name,
                    VehicleTypeGroupId = updatedVehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = updatedVehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = updatedVehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = updatedVehicle.BaseVehicle.Model.VehicleType.Name,
                    YearId = updatedVehicle.BaseVehicle.YearId
                };

                await this._vehicleIndexingService.UploadDocumentAsync(vehicleDocument);
            }
        }

        private async Task UpdateVehicleToBrakeConfigDocuments(List<Vehicle> updatedVehicles)
        {
            if (updatedVehicles == null)
            {
                return;
            }

            foreach (var updatedVehicle in updatedVehicles)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            //Pushkar: azure search: length of the request URL may exceed the limit of 8 KB limit may exceed if $filter=vehicleId eq '1' or vehicleId eq '2' or vehicleId eq '3' is used to avoid this foreach loop
            {
                var vehicleToBrakeConfigSearchResult =
                    await
                        _vehicletoBrakeConfigSearchService.SearchAsync(null, $"vehicleId eq {updatedVehicle.Id}");
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.SubModelId = updatedVehicle.SubModelId;
                        existingVehicleToBrakeConfigDocument.SubModelName = updatedVehicle.SubModel.Name;
                        existingVehicleToBrakeConfigDocument.RegionId = updatedVehicle.RegionId;
                        existingVehicleToBrakeConfigDocument.RegionName = updatedVehicle.Region.Name;
                    }

                    await
                        this._vehicleToBrakeConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBrakeConfigDocuments.ToList());
                }
            }
        }

        private async Task UpdateVehicleToBedConfigDocuments(List<Vehicle> updatedVehicles)
        {
            if (updatedVehicles == null)
            {
                return;
            }

            foreach (var updatedVehicle in updatedVehicles)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            //Pushkar: azure search: length of the request URL may exceed the limit of 8 KB limit may exceed if $filter=vehicleId eq '1' or vehicleId eq '2' or vehicleId eq '3' is used to avoid this foreach loop
            {
                var vehicleToBedConfigSearchResult =
                    await
                        _vehicletoBedConfigSearchService.SearchAsync(null, $"vehicleId eq {updatedVehicle.Id}");
                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;

                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBedConfigDocument.SubModelId = updatedVehicle.SubModelId;
                        existingVehicleToBedConfigDocument.SubModelName = updatedVehicle.SubModel.Name;
                        existingVehicleToBedConfigDocument.RegionId = updatedVehicle.RegionId;
                        existingVehicleToBedConfigDocument.RegionName = updatedVehicle.Region.Name;
                    }

                    await
                        this._vehicleToBedConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBedConfigDocuments.ToList());
                }
            }
        }

        private async Task UpdateVehicleToBodyStyleConfigDocuments(List<Vehicle> updatedVehicles)
        {
            if (updatedVehicles == null)
            {
                return;
            }

            foreach (var updatedVehicle in updatedVehicles)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            //Pushkar: azure search: length of the request URL may exceed the limit of 8 KB limit may exceed if $filter=vehicleId eq '1' or vehicleId eq '2' or vehicleId eq '3' is used to avoid this foreach loop
            {
                var vehicleToBodyStyleConfigSearchResult =
                    await
                        _vehicletoBodyStyleConfigSearchService.SearchAsync(null, $"vehicleId eq {updatedVehicle.Id}");
                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;

                if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.SubModelId = updatedVehicle.SubModelId;
                        existingVehicleToBodyStyleConfigDocument.SubModelName = updatedVehicle.SubModel.Name;
                        existingVehicleToBodyStyleConfigDocument.RegionId = updatedVehicle.RegionId;
                        existingVehicleToBodyStyleConfigDocument.RegionName = updatedVehicle.Region.Name;
                    }

                    await
                        this._vehicleToBodyStyleConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBodyStyleConfigDocuments.ToList());
                }
            }
        }

        private async Task UpdateVehicleToWheelBaseConfigDocuments(List<Vehicle> updatedVehicles)
        {
            if (updatedVehicles == null)
            {
                return;
            }

            foreach (var updatedVehicle in updatedVehicles)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            //Pushkar: azure search: length of the request URL may exceed the limit of 8 KB limit may exceed if $filter=vehicleId eq '1' or vehicleId eq '2' or vehicleId eq '3' is used to avoid this foreach loop
            {
                var vehicleToWheelBaseConfigSearchResult =
                    await
                        _vehicletoWheelBaseConfigSearchService.SearchAsync(null, $"vehicleId eq {updatedVehicle.Id}");
                var existingVehicleToWheelBaseConfigDocuments = vehicleToWheelBaseConfigSearchResult.Documents;

                if (existingVehicleToWheelBaseConfigDocuments != null && existingVehicleToWheelBaseConfigDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseConfigDocument in existingVehicleToWheelBaseConfigDocuments)
                    {
                        existingVehicleToWheelBaseConfigDocument.SubModelId = updatedVehicle.SubModelId;
                        existingVehicleToWheelBaseConfigDocument.SubModelName = updatedVehicle.SubModel.Name;
                        existingVehicleToWheelBaseConfigDocument.RegionId = updatedVehicle.RegionId;
                        existingVehicleToWheelBaseConfigDocument.RegionName = updatedVehicle.Region.Name;
                    }

                    await
                        this._vehicleToWheelBaseConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToWheelBaseConfigDocuments.ToList());
                }
            }
        }

        private async Task UpdateVehicleToDriveTypeConfigDocuments(List<Vehicle> updatedVehicles)
        {
            if (updatedVehicles == null)
            {
                return;
            }

            foreach (var updatedVehicle in updatedVehicles)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            //Pushkar: azure search: length of the request URL may exceed the limit of 8 KB limit may exceed if $filter=vehicleId eq '1' or vehicleId eq '2' or vehicleId eq '3' is used to avoid this foreach loop
            {
                var vehicleToDriveTypeConfigSearchResult =
                    await
                        _vehicletoDriveTypeConfigSearchService.SearchAsync(null, $"vehicleId eq {updatedVehicle.Id}");
                var existingVehicleToDriveTypeConfigDocuments = vehicleToDriveTypeConfigSearchResult.Documents;

                if (existingVehicleToDriveTypeConfigDocuments != null && existingVehicleToDriveTypeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToDriveTypeConfigDocument in existingVehicleToDriveTypeConfigDocuments)
                    {
                        existingVehicleToDriveTypeConfigDocument.SubModelId = updatedVehicle.SubModelId;
                        existingVehicleToDriveTypeConfigDocument.SubModelName = updatedVehicle.SubModel.Name;
                        existingVehicleToDriveTypeConfigDocument.RegionId = updatedVehicle.RegionId;
                        existingVehicleToDriveTypeConfigDocument.RegionName = updatedVehicle.Region.Name;
                    }

                    await
                        this._vehicleToDriveTypeConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToDriveTypeConfigDocuments.ToList());
                }
            }
        }

        private async Task UpdateVehicleToMfrBodyCodeConfigDocuments(List<Vehicle> updatedVehicles)
        {
            if (updatedVehicles == null)
            {
                return;
            }

            foreach (var updatedVehicle in updatedVehicles)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            //Pushkar: azure search: length of the request URL may exceed the limit of 8 KB limit may exceed if $filter=vehicleId eq '1' or vehicleId eq '2' or vehicleId eq '3' is used to avoid this foreach loop
            {
                var vehicleToMfrBodyCodeConfigSearchResult =
                    await
                        _vehicletoMfrBodyCodeConfigSearchService.SearchAsync(null, $"vehicleId eq {updatedVehicle.Id}");
                var existingVehicleToMfrBodyCodeConfigDocuments = vehicleToMfrBodyCodeConfigSearchResult.Documents;

                if (existingVehicleToMfrBodyCodeConfigDocuments != null && existingVehicleToMfrBodyCodeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToMfrBodyCodeConfigDocument in existingVehicleToMfrBodyCodeConfigDocuments)
                    {
                        existingVehicleToMfrBodyCodeConfigDocument.SubModelId = updatedVehicle.SubModelId;
                        existingVehicleToMfrBodyCodeConfigDocument.SubModelName = updatedVehicle.SubModel.Name;
                        existingVehicleToMfrBodyCodeConfigDocument.RegionId = updatedVehicle.RegionId;
                        existingVehicleToMfrBodyCodeConfigDocument.RegionName = updatedVehicle.Region.Name;
                    }

                    await
                        this._vehicleToMfrBodyCodeConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToMfrBodyCodeConfigDocuments.ToList());
                }
            }
        }

        private async Task ClearBaseVehicleChangeRequestId(long changeRequestId)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleSearchResult =
                    await
                        _vehicleSearchService.SearchAsync(null,
                            $"baseVehicleChangeRequestId eq {changeRequestId}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleDocuments = vehicleSearchResult.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        existingVehicleDocument.BaseVehicleChangeRequestId = -1;
                    }

                    await
                        this._vehicleIndexingService.UploadDocumentsAsync(
                            existingVehicleDocuments.ToList());
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

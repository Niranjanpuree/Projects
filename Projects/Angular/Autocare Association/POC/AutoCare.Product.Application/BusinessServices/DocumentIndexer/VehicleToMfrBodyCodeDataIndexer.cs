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
    public class VehicleToMfrBodyCodeDataIndexer : IVehicleToMfrBodyCodeDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicleToMfrBodyCodeSearchService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService;

        public VehicleToMfrBodyCodeDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToMfrBodyCodeSearchService vehicleToMfrBodyCodeSearchService,
            IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleToMfrBodyCodeSearchService = vehicleToMfrBodyCodeSearchService;
            _vehicleToMfrBodyCodeIndexingService = vehicleToMfrBodyCodeIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoMfrBodyCodeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToMfrBodyCode>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToMfrBodyCode>;

            if (vehicletoMfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var addedVehicleToMfrBodyCodes =
                        await
                            vehicletoMfrBodyCodeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "MfrBodyCode",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (addedVehicleToMfrBodyCodes == null || !addedVehicleToMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            //delete document with vehicleToMfrBodyCodeConfigId of Guid and MfrBodyCodeId of addedVehicleToMfrBodyCodes.MfrBodyCodeId
            var vehicleToMfrBodyCodeSearchResult =
                                    await
                                        _vehicleToMfrBodyCodeSearchService.SearchAsync(null,
                                            $"mfrBodyCodeId eq {addedVehicleToMfrBodyCodes.First().MfrBodyCodeId}");

            var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;
            if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
            {
                Guid guid;
                foreach (var existingVehicleToMfrBodyCodeDocument in existingVehicleToMfrBodyCodeDocuments)
                {
                    if (Guid.TryParse(existingVehicleToMfrBodyCodeDocument.VehicleToMfrBodyCodeId, out guid))
                    {
                        await
                            this._vehicleToMfrBodyCodeIndexingService.DeleteDocumentByVehicleToMfrBodyCodeIdAsync(
                                existingVehicleToMfrBodyCodeDocument.VehicleToMfrBodyCodeId);
                    }
                }
            }

            await InsertOrUpdateVehicleToMfrBodyCodeDocuments(addedVehicleToMfrBodyCodes);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoMfrBodyCodeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToMfrBodyCode>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToMfrBodyCode>;

            if (vehicletoMfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToMfrBodyCodes =
                        await
                            vehicletoMfrBodyCodeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "MfrBodyCode",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (updatedVehicleToMfrBodyCodes == null || !updatedVehicleToMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToMfrBodyCodeDocuments(updatedVehicleToMfrBodyCodes);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: this method is called when processing a REPLACE MfrBodyCode as MODIFY of vehicletoMfrBodyCodeconfigs under that MfrBodyCode
            var vehicletoMfrBodyCodeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToMfrBodyCode>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToMfrBodyCode>;

            if (vehicletoMfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToMfrBodyCodes =
                        await
                            vehicletoMfrBodyCodeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "MfrBodyCode",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");


            if (updatedVehicleToMfrBodyCodes == null || !updatedVehicleToMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToMfrBodyCodeDocuments(updatedVehicleToMfrBodyCodes, true);

            await ClearMfrBodyCodeChangeRequestId(changeRequestId);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoMfrBodyCodeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToMfrBodyCode>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToMfrBodyCode>;

            if (vehicletoMfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToMfrBodyCodes =
                await vehicletoMfrBodyCodeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicleToMfrBodyCodes == null || !updatedVehicleToMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToMfrBodyCode in updatedVehicleToMfrBodyCodes)
            {
                await
                    this._vehicleToMfrBodyCodeIndexingService.DeleteDocumentByVehicleToMfrBodyCodeIdAsync(
                        updatedVehicleToMfrBodyCode.Id.ToString());
            }

            //Required when processing MfrBodyCode REPLACE CR
            await ClearMfrBodyCodeChangeRequestId(changeRequestId);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoMfrBodyCodeRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToMfrBodyCode>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToMfrBodyCode>;

            if (vehicletoMfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToMfrBodyCodes =
                        await
                            vehicletoMfrBodyCodeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);


            if (updatedVehicleToMfrBodyCodes == null || !updatedVehicleToMfrBodyCodes.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Mfr Body Code cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToMfrBodyCode in updatedVehicleToMfrBodyCodes)
            {
                var vehicletoMfrBodyCodeDocument = new VehicleToMfrBodyCodeDocument
                {
                    VehicleToMfrBodyCodeId = updatedVehicleToMfrBodyCode.Id.ToString(),
                    VehicleToMfrBodyCodeChangeRequestId = -1,
                };

                await
                    this._vehicleToMfrBodyCodeIndexingService.UploadDocumentAsync(vehicletoMfrBodyCodeDocument);
            }

            //Required when processing MfrBodyCode REPLACE CR
            await ClearMfrBodyCodeChangeRequestId(changeRequestId);
        }

        private async Task InsertOrUpdateVehicleToMfrBodyCodeDocuments(List<VehicleToMfrBodyCode> updatedVehicleToMfrBodyCodes, bool isReplace = false)
        {
            if (updatedVehicleToMfrBodyCodes == null)
            {
                return;
            }

            foreach (var updatedVehicleToMfrBodyCode in updatedVehicleToMfrBodyCodes)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            {
                var vehicletoMfrBodyCodeDocument = new VehicleToMfrBodyCodeDocument
                {
                    VehicleToMfrBodyCodeId = updatedVehicleToMfrBodyCode.Id.ToString(),
                    MfrBodyCodeChangeRequestId = isReplace ? -1 : (long?)null,
                    VehicleToMfrBodyCodeChangeRequestId = -1,
                    BaseVehicleId = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicleId,
                    MfrBodyCodeId = updatedVehicleToMfrBodyCode.MfrBodyCode.Id,
                    MfrBodyCodeName = updatedVehicleToMfrBodyCode.MfrBodyCode.Name,
                    MakeId = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.MakeId,
                    MakeName = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.Make.Name,
                    ModelId = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.ModelId,
                    ModelName = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.Model.Name,
                    RegionId = updatedVehicleToMfrBodyCode.Vehicle.RegionId,
                    RegionName = updatedVehicleToMfrBodyCode.Vehicle.Region.Name,
                    Source = updatedVehicleToMfrBodyCode.Vehicle.SourceName,
                    SubModelId = updatedVehicleToMfrBodyCode.Vehicle.SubModelId,
                    SubModelName = updatedVehicleToMfrBodyCode.Vehicle.SubModel.Name,
                    VehicleId = updatedVehicleToMfrBodyCode.VehicleId,
                    VehicleTypeGroupId =
                        updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName =
                        updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.Model.VehicleType.Name,
                    YearId = updatedVehicleToMfrBodyCode.Vehicle.BaseVehicle.YearId,
                };

                await
                    this._vehicleToMfrBodyCodeIndexingService.UploadDocumentAsync(vehicletoMfrBodyCodeDocument);
            }
        }

        private async Task ClearMfrBodyCodeChangeRequestId(long changeRequestId)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToMfrBodyCodeSearchResult =
                    await
                        _vehicleToMfrBodyCodeSearchService.SearchAsync(null,
                            $"mfrBodyCodeChangeRequestId eq {changeRequestId}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;
                if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleToMfrBodyCodeDocuments)
                    {
                        existingVehicleDocument.MfrBodyCodeChangeRequestId = -1;
                    }

                    await
                        this._vehicleToMfrBodyCodeIndexingService.UploadDocumentsAsync(
                            existingVehicleToMfrBodyCodeDocuments.ToList());
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

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
    public class VehicleToWheelBaseDataIndexer : IVehicleToWheelBaseDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToWheelBaseSearchService _vehicleToWheelBaseSearchService;
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService;

        public VehicleToWheelBaseDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToWheelBaseSearchService   vehicleToWheelBaseSearchService,
            IVehicleToWheelBaseIndexingService  vehicleToWheelBaseIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleToWheelBaseSearchService = vehicleToWheelBaseSearchService;
            _vehicleToWheelBaseIndexingService = vehicleToWheelBaseIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoWheelBaseRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToWheelBase>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToWheelBase>;

            if (vehicletoWheelBaseRepositoryService == null)
            {
                return;
            }

            var addedVehicleToWheelBases =
                        await
                            vehicletoWheelBaseRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "WheelBase",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (addedVehicleToWheelBases == null || !addedVehicleToWheelBases.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Wheel Base Index cannot be updated before the transactional table is updated");
            }

            ////delete document with vehicleToWheelBaseId of Guid and WheelBaseId of addedVehicleToWheelBases.WheelBaseId
            //var vehicleToWheelBaseSearchResult =
            //                        await
            //                            _vehicleToWheelBaseSearchService.SearchAsync(null,
            //                                $"wheelBaseId eq {addedVehicleToWheelBases.First().WheelBaseId}");

            //var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
            //if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
            //{
            //    Guid guid;
            //    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
            //    {
            //        if (Guid.TryParse(existingVehicleToWheelBaseDocument.VehicleToWheelBaseId, out guid))
            //        {
            //            await
            //                this._vehicleToWheelBaseIndexingService.DeleteDocumentByVehicleToWheelBaseIdAsync(
            //                    existingVehicleToWheelBaseDocument.VehicleToWheelBaseId);
            //        }
            //    }
            //}

            await InsertOrUpdateVehicleToWheelBaseDocuments(addedVehicleToWheelBases);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoWheelBaseRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToWheelBase>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToWheelBase>;

            if (vehicletoWheelBaseRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToWheelBases =
                        await
                            vehicletoWheelBaseRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "WheelBase",
                                 "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");

            if (updatedVehicleToWheelBases == null || !updatedVehicleToWheelBases.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Wheel Base Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToWheelBaseDocuments(updatedVehicleToWheelBases);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: this method is called when processing a REPLACE brake config as MODIFY of vehicletoWheelBases under that brake config
            var vehicletoWheelBaseRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToWheelBase>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToWheelBase>;

            if (vehicletoWheelBaseRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToWheelBases =
                        await
                            vehicletoWheelBaseRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId, 100000,
                                "WheelBase",
                                "Vehicle.BaseVehicle.Make",
                                "Vehicle.BaseVehicle.Model",
                                "Vehicle.BaseVehicle.Model.VehicleType",
                                "Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup",
                                "Vehicle.SubModel",
                                "Vehicle.Region");


            if (updatedVehicleToWheelBases == null || !updatedVehicleToWheelBases.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Wheel Base Index cannot be updated before the transactional table is updated");
            }

            await InsertOrUpdateVehicleToWheelBaseDocuments(updatedVehicleToWheelBases, true);

            await ClearWheelBaseChangeRequestId(changeRequestId);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoWheelBaseRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToWheelBase>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToWheelBase>;

            if (vehicletoWheelBaseRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToWheelBases =
                await vehicletoWheelBaseRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicleToWheelBases == null || !updatedVehicleToWheelBases.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle To Wheel Base Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToWheelBase in updatedVehicleToWheelBases)
            {
                await
                    this._vehicleToWheelBaseIndexingService.DeleteDocumentByVehicleToWheelBaseIdAsync(
                        updatedVehicleToWheelBase.Id.ToString());
            }

            //Required when processing WheelBase REPLACE CR
            await ClearWheelBaseChangeRequestId(changeRequestId);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var vehicletoWheelBaseRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<VehicleToWheelBase>() as
                    IVcdbSqlServerEfRepositoryService<VehicleToWheelBase>;

            if (vehicletoWheelBaseRepositoryService == null)
            {
                return;
            }

            var updatedVehicleToWheelBases =
                        await
                            vehicletoWheelBaseRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId);


            if (updatedVehicleToWheelBases == null || !updatedVehicleToWheelBases.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Vehicle To Wheel Base Index cannot be updated before the transactional table is updated");
            }

            foreach (var updatedVehicleToWheelBase in updatedVehicleToWheelBases)
            {
                var vehicletoWheelBaseDocument = new VehicleToWheelBaseDocument
                {
                    VehicleToWheelBaseId = updatedVehicleToWheelBase.Id.ToString(),
                    VehicleToWheelBaseChangeRequestId = -1,
                };

                await
                    this._vehicleToWheelBaseIndexingService.UploadDocumentAsync(vehicletoWheelBaseDocument);
            }

            //Required when processing WheelBase REPLACE CR
            await ClearWheelBaseChangeRequestId(changeRequestId);
        }

        private async Task InsertOrUpdateVehicleToWheelBaseDocuments(List<VehicleToWheelBase> updatedVehicleToWheelBases, bool isReplace = false)
        {
            if (updatedVehicleToWheelBases == null)
            {
                return;
            }

            foreach (var updatedVehicleToWheelBase in updatedVehicleToWheelBases)
            //NOTE: updatedVehicles will contain more than 1 item when processing base vehicle replace
            {
                var vehicletoWheelBaseDocument = new VehicleToWheelBaseDocument
                {
                    VehicleToWheelBaseId = updatedVehicleToWheelBase.Id.ToString(),
                    WheelBaseChangeRequestId = isReplace ? -1 : (long?)null,
                    VehicleToWheelBaseChangeRequestId = -1,
                    BaseVehicleId = updatedVehicleToWheelBase.Vehicle.BaseVehicleId,
                    WheelBaseName = updatedVehicleToWheelBase.WheelBase.Base,
                    WheelBaseMetric = updatedVehicleToWheelBase.WheelBase.WheelBaseMetric,
                    WheelBaseId = updatedVehicleToWheelBase.WheelBaseId,
                    MakeId = updatedVehicleToWheelBase.Vehicle.BaseVehicle.MakeId,
                    MakeName = updatedVehicleToWheelBase.Vehicle.BaseVehicle.Make.Name,
                    ModelId = updatedVehicleToWheelBase.Vehicle.BaseVehicle.ModelId,
                    ModelName = updatedVehicleToWheelBase.Vehicle.BaseVehicle.Model.Name,
                    RegionId = updatedVehicleToWheelBase.Vehicle.RegionId,
                    RegionName = updatedVehicleToWheelBase.Vehicle.Region.Name,
                    Source = updatedVehicleToWheelBase.Vehicle.SourceName,
                    SubModelId = updatedVehicleToWheelBase.Vehicle.SubModelId,
                    SubModelName = updatedVehicleToWheelBase.Vehicle.SubModel.Name,
                    VehicleId = updatedVehicleToWheelBase.VehicleId,
                    VehicleTypeGroupId =
                        updatedVehicleToWheelBase.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName =
                        updatedVehicleToWheelBase.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = updatedVehicleToWheelBase.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = updatedVehicleToWheelBase.Vehicle.BaseVehicle.Model.VehicleType.Name,
                    YearId = updatedVehicleToWheelBase.Vehicle.BaseVehicle.YearId,
                };

                await
                    this._vehicleToWheelBaseIndexingService.UploadDocumentAsync(vehicletoWheelBaseDocument);
            }
        }

        private async Task ClearWheelBaseChangeRequestId(long changeRequestId)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToWheelBaseSearchResult =
                    await
                        _vehicleToWheelBaseSearchService.SearchAsync(null,
                            $"wheelBaseChangeRequestId eq {changeRequestId}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleToWheelBaseDocuments)
                    {
                        existingVehicleDocument.WheelBaseChangeRequestId = -1;
                    }

                    await
                        this._vehicleToWheelBaseIndexingService.UploadDocumentsAsync(
                            existingVehicleToWheelBaseDocuments.ToList());
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

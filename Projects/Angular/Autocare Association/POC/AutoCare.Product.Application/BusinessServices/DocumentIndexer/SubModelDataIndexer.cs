using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Search.Model;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class SubModelDataIndexer : ISubModelDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleSearchService _vehicleSearchService;
        private readonly IVehicleIndexingService _vehicleIndexingService;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;
        private readonly IVehicleToBedConfigSearchService        _vehicleToBedConfigSearchService;
        private readonly IVehicleToBedConfigIndexingService     _vehicleToBedConfigIndexingService;
        private readonly IVehicleToBodyStyleConfigSearchService   _vehicleToBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService;
        private readonly IVehicleToWheelBaseSearchService _vehicletoWheelBaseSearchService;
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService;
        private readonly IVehicleToDriveTypeSearchService _vehicletoDriveTypeSearchService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicletoMfrBodyCodeSearchService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService;

        public SubModelDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleSearchService vehicleSearchService,
            IVehicleIndexingService vehicleIndexingService,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService,
            IVehicleToBedConfigSearchService          vehicleToBedConfigSearchService,
            IVehicleToBedConfigIndexingService        vehicleToBedConfigIndexingService,
            IVehicleToBodyStyleConfigSearchService    vehicleToBodyStyleConfigSearchService,
            IVehicleToBodyStyleConfigIndexingService  vehicleToBodyStyleConfigIndexingService,
             IVehicleToWheelBaseSearchService vehicletoWheelBaseSearchService,
            IVehicleToWheelBaseIndexingService vehicleToWheelBaseIndexingService,
             IVehicleToMfrBodyCodeSearchService vehicletoMfrBodyCodeSearchService,
            IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService,
             IVehicleToDriveTypeSearchService vehicletoDriveTypeSearchService,
            IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleSearchService = vehicleSearchService;
            _vehicleIndexingService = vehicleIndexingService;
            _vehicletoBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
            _vehicleToBedConfigSearchService = vehicleToBedConfigSearchService;
           _vehicleToBedConfigIndexingService=vehicleToBedConfigIndexingService;
           _vehicleToBodyStyleConfigSearchService= vehicleToBodyStyleConfigSearchService;
           _vehicleToBodyStyleConfigIndexingService= vehicleToBodyStyleConfigIndexingService;
            _vehicletoWheelBaseSearchService = vehicletoWheelBaseSearchService;
            _vehicleToWheelBaseIndexingService = vehicleToWheelBaseIndexingService;
            _vehicletoDriveTypeSearchService = vehicletoDriveTypeSearchService;
            _vehicleToDriveTypeIndexingService = vehicleToDriveTypeIndexingService;
            _vehicletoMfrBodyCodeSearchService = vehicletoMfrBodyCodeSearchService;
            _vehicleToMfrBodyCodeIndexingService = vehicleToMfrBodyCodeIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var subModelRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<SubModel>() as
                                IVcdbSqlServerEfRepositoryService<SubModel>;

            if (subModelRepositoryService == null)
            {
                return;
            }

            var updatedSubModels =
                                await
                                    subModelRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000);

            if (updatedSubModels == null || !updatedSubModels.Any())
            {
                throw new InvalidOperationException(
                    "Sub Model Index cannot be updated before the transactional table is updated");
            }

            var updatedSubModel = updatedSubModels.First();

            await UpdateVehicleDocuments(updatedSubModel);

            await UpdateVehicleToBrakeConfigDocuments(updatedSubModel);
            await UpdateVehicleToBedConfigDocuments(updatedSubModel);

            await UpdateVehicleToBodyStyleConfigDocuments(updatedSubModel);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        private async Task UpdateVehicleDocuments(SubModel updatedSubModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleSearchResult =
                    await
                        _vehicleSearchService.SearchAsync(null,
                            $"subModelId eq {updatedSubModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleDocuments = vehicleSearchResult.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        existingVehicleDocument.SubModelName = updatedSubModel.Name;
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

        private async Task UpdateVehicleToBrakeConfigDocuments(SubModel updatedSubModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                    await
                        _vehicletoBrakeConfigSearchService.SearchAsync(null,
                            $"subModelId eq {updatedSubModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBrakeConfigDocuments != null &&
                    existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.SubModelName = updatedSubModel.Name;
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
        private async Task UpdateVehicleToBedConfigDocuments(SubModel updatedSubModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                    await
                        _vehicleToBedConfigSearchService.SearchAsync(null,
                            $"subModelId eq {updatedSubModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;

                if (existingVehicleToBedConfigDocuments != null &&
                    existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBedConfigDocument in
                            existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBedConfigDocument.SubModelName = updatedSubModel.Name;
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
        private async Task UpdateVehicleToBodyStyleConfigDocuments(SubModel updatedSubModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBodyStyleConfigSearchResult =
                    await
                        _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                            $"subModelId eq {updatedSubModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;

                if (existingVehicleToBodyStyleConfigDocuments != null &&
                    existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBodyStyleConfigDocument in
                            existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.SubModelName = updatedSubModel.Name;
                    }

                    await
                        this._vehicleToBodyStyleConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBodyStyleConfigDocuments.ToList());
                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);
        }


        private async Task UpdateVehicleToWheelBaseDocuments(SubModel updatedSubModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToWheelBaseSearchResult =
                await
                    _vehicletoWheelBaseSearchService.SearchAsync(null,
                        $"subModelId eq {updatedSubModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
                    {
                        existingVehicleToWheelBaseDocument.VehicleTypeName = updatedSubModel.Name;
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

        private async Task UpdateVehicleToDriveTypeDocuments(SubModel updatedSubModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToDriveTypeSearchResult =
                await
                    _vehicletoDriveTypeSearchService.SearchAsync(null,
                        $"subModelId eq {updatedSubModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;
                if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
                {
                    foreach (var existingVehicleToDriveTypeDocument in existingVehicleToDriveTypeDocuments)
                    {
                        existingVehicleToDriveTypeDocument.VehicleTypeName = updatedSubModel.Name;
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

        private async Task UpdateVehicleToMfrBodyCodeDocuments(SubModel updatedSubModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToMfrBodyCodeSearchResult =
                await
                    _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                        $"subModelId eq {updatedSubModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;
                if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingVehicleToMfrBodyCodeDocument in existingVehicleToMfrBodyCodeDocuments)
                    {
                        existingVehicleToMfrBodyCodeDocument.VehicleTypeName = updatedSubModel.Name;
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

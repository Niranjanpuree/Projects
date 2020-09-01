using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Search.Model;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class VehicleTypeDataIndexer : IVehicleTypeDataIndexer
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
        private readonly IVehicleToWheelBaseSearchService _vehicletoWheelBaseSearchService;
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService;
        private readonly IVehicleToDriveTypeSearchService _vehicletoDriveTypeSearchService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicletoMfrBodyCodeSearchService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService;

        public VehicleTypeDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
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
            _vehicletoBedConfigSearchService = vehicletoBedConfigSearchService;
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
            _vehicletoBodyStyleConfigSearchService = vehicletoBodyStyleConfigSearchService;
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyStyleConfigIndexingService;
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
            var vehicleTypeRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<VehicleType>() as
                                IVcdbSqlServerEfRepositoryService<VehicleType>;

            if (vehicleTypeRepositoryService == null)
            {
                return;
            }

            var updatedVehicleTypes =
                                await
                                    vehicleTypeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedVehicleTypes == null || !updatedVehicleTypes.Any())
            {
                throw new InvalidOperationException(
                    "Vehicle Type Index cannot be updated before the transactional table is updated");
            }

            var updatedVehicleType = updatedVehicleTypes.First();

            await UpdateVehicleDocuments(updatedVehicleType);

            await UpdateVehicleToBrakeConfigDocuments(updatedVehicleType);

            await UpdateVehicleToBedConfigDocuments(updatedVehicleType);

            await UpdateVehicleToBodyStyleConfigDocuments(updatedVehicleType);
            await UpdateVehicleToWheelBaseDocuments(updatedVehicleType);
            await UpdateVehicleToMfrBodyCodeDocuments(updatedVehicleType);
            await UpdateVehicleToDriveTypeDocuments(updatedVehicleType);
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

        private async Task UpdateVehicleDocuments(VehicleType updatedVehicleType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleSearchResult =
                await
                    _vehicleSearchService.SearchAsync(null,
                        $"vehicleTypeId eq {updatedVehicleType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleDocuments = vehicleSearchResult?.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        existingVehicleDocument.VehicleTypeName = updatedVehicleType.Name;
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

        private async Task UpdateVehicleToBrakeConfigDocuments(VehicleType updatedVehicleType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                await
                    _vehicletoBrakeConfigSearchService.SearchAsync(null,
                        $"vehicleTypeId eq {updatedVehicleType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;
                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.VehicleTypeName = updatedVehicleType.Name;
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

        private async Task UpdateVehicleToBedConfigDocuments(VehicleType updatedVehicleType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                await
                    _vehicletoBedConfigSearchService.SearchAsync(null,
                        $"vehicleTypeId eq {updatedVehicleType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBedConfigDocument.VehicleTypeName = updatedVehicleType.Name;
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

        private async Task UpdateVehicleToBodyStyleConfigDocuments(VehicleType updatedVehicleType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBodyStyleConfigSearchResult =
                await
                    _vehicletoBodyStyleConfigSearchService.SearchAsync(null,
                        $"vehicleTypeId eq {updatedVehicleType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;
                if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.VehicleTypeName = updatedVehicleType.Name;
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


        private async Task UpdateVehicleToWheelBaseDocuments(VehicleType updatedVehicleType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToWheelBaseSearchResult =
                await
                    _vehicletoWheelBaseSearchService.SearchAsync(null,
                        $"vehicleTypeId eq {updatedVehicleType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
                    {
                        existingVehicleToWheelBaseDocument.VehicleTypeName = updatedVehicleType.Name;
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

        private async Task UpdateVehicleToDriveTypeDocuments(VehicleType updatedVehicleType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToDriveTypeSearchResult =
                await
                    _vehicletoDriveTypeSearchService.SearchAsync(null,
                        $"vehicleTypeId eq {updatedVehicleType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;
                if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
                {
                    foreach (var existingVehicleToDriveTypeDocument in existingVehicleToDriveTypeDocuments)
                    {
                        existingVehicleToDriveTypeDocument.VehicleTypeName = updatedVehicleType.Name;
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

        private async Task UpdateVehicleToMfrBodyCodeDocuments(VehicleType updatedVehicleType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToMfrBodyCodeSearchResult =
                await
                    _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                        $"vehicleTypeId eq {updatedVehicleType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;
                if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingVehicleToMfrBodyCodeDocument in existingVehicleToMfrBodyCodeDocuments)
                    {
                        existingVehicleToMfrBodyCodeDocument.VehicleTypeName = updatedVehicleType.Name;
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

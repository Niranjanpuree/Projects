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
    public class MakeDataIndexer : IMakeDataIndexer
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
        private readonly IVehicleToDriveTypeSearchService _vehicletoDriveTypeSearchService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicletoMfrBodyCodeSearchService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService;
        private readonly IVehicleToWheelBaseSearchService _vehicletoWheelBaseSearchService;
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService;


        public MakeDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleSearchService vehicleSearchService,
            IVehicleIndexingService vehicleIndexingService,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService,
            IVehicleToBodyStyleConfigSearchService vehicletoBodyStyleConfigSearchService,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyStyleConfigIndexingService,
            IVehicleToDriveTypeSearchService vehicletoDriveTypeSearchService,
            IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService,
            IVehicleToMfrBodyCodeSearchService vehicletoMfrBodyCodeSearchService,
            IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService,
            IVehicleToWheelBaseSearchService vehicletoWheelBaseSearchService,
            IVehicleToWheelBaseIndexingService vehicleToWheelBaseIndexingService)
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
            _vehicletoDriveTypeSearchService = vehicletoDriveTypeSearchService;
            _vehicleToDriveTypeIndexingService = vehicleToDriveTypeIndexingService;
            _vehicletoMfrBodyCodeSearchService = vehicletoMfrBodyCodeSearchService;
            _vehicleToMfrBodyCodeIndexingService = vehicleToMfrBodyCodeIndexingService;
            _vehicletoWheelBaseSearchService = vehicletoWheelBaseSearchService;
            _vehicleToWheelBaseIndexingService = vehicleToWheelBaseIndexingService;
        }
        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var makeRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<Make>() as
                                IVcdbSqlServerEfRepositoryService<Make>;

            if (makeRepositoryService == null)
            {
                return;
            }

            var updatedMakes =
                                await
                                    makeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedMakes == null || !updatedMakes.Any())
            {
                throw new InvalidOperationException(
                    "Make Index cannot be updated before the transactional table is updated");
            }

            var updatedMake = updatedMakes.First();

            await UpdateVehicleDocuments(updatedMake);

            await UpdateVehicleToBrakeConfigDocuments(updatedMake);

            await UpdateVehicleToBedConfigDocuments(updatedMake);

            await UpdateVehicleToBodyStyleConfigDocuments(updatedMake);

            await UpdateVehicleToDriveTypeDocuments(updatedMake);

            await UpdateVehicleToMfrBodyCodeDocuments(updatedMake);

            await UpdateVehicleToWheelBaseDocuments(updatedMake);
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

        private async Task UpdateVehicleDocuments(Make updatedMake)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleSearchResult =
                await
                    _vehicleSearchService.SearchAsync(null,
                        $"makeId eq {updatedMake.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleDocuments = vehicleSearchResult?.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        existingVehicleDocument.MakeName = updatedMake.Name;
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

        private async Task UpdateVehicleToBrakeConfigDocuments(Make updatedMake)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                await
                    _vehicletoBrakeConfigSearchService.SearchAsync(null,
                        $"makeId eq {updatedMake.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;
                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.MakeName = updatedMake.Name;
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

        private async Task UpdateVehicleToBedConfigDocuments(Make updatedMake)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                await
                    _vehicletoBedConfigSearchService.SearchAsync(null,
                        $"makeId eq {updatedMake.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBedConfigDocument.MakeName = updatedMake.Name;
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

        private async Task UpdateVehicleToBodyStyleConfigDocuments(Make updatedMake)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBodyStlyeConfigSearchResult =
                await
                    _vehicletoBodyStyleConfigSearchService.SearchAsync(null,
                        $"makeId eq {updatedMake.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStlyeConfigSearchResult.Documents;
                if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.MakeName = updatedMake.Name;
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

        //Start
        private async Task UpdateVehicleToDriveTypeDocuments(Make updatedMake)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToDriveTypeSearchResult =
                await
                    _vehicletoDriveTypeSearchService.SearchAsync(null,
                        $"makeId eq {updatedMake.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;
                if (existingVehicleToDriveTypeDocuments != null && existingVehicleToDriveTypeDocuments.Any())
                {
                    foreach (var existingVehicleToDriveTypeDocument in existingVehicleToDriveTypeDocuments)
                    {
                        existingVehicleToDriveTypeDocument.MakeName = updatedMake.Name;
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

        private async Task UpdateVehicleToMfrBodyCodeDocuments(Make updatedMake)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToMfrBodyCodeSearchResult =
                await
                    _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                        $"makeId eq {updatedMake.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;
                if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingVehicleToMfrBodyCodeDocument in existingVehicleToMfrBodyCodeDocuments)
                    {
                        existingVehicleToMfrBodyCodeDocument.MakeName = updatedMake.Name;
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

        private async Task UpdateVehicleToWheelBaseDocuments(Make updatedMake)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToWheelBaseSearchResult =
                await
                    _vehicletoWheelBaseSearchService.SearchAsync(null,
                        $"makeId eq {updatedMake.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
                    {
                        existingVehicleToWheelBaseDocument.MakeName = updatedMake.Name;
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
        //End
    }
}

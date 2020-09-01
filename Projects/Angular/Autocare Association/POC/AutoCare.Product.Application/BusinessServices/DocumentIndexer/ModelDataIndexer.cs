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
    public class ModelDataIndexer : IModelDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleSearchService _vehicleSearchService;
        private readonly IVehicleIndexingService _vehicleIndexingService;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;
        private readonly IVehicleToBodyStyleConfigSearchService _vehicletoBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService;
        private readonly IVehicleToBedConfigSearchService _vehicletoBedConfigSearchService;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService;
        private readonly IVehicleToDriveTypeSearchService _vehicletoDriveTypeSearchService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicletoMfrBodyCodeSearchService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService;
        private readonly IVehicleToWheelBaseSearchService _vehicletoWheelBaseSearchService;
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService;

        public ModelDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleSearchService vehicleSearchService,
            IVehicleIndexingService vehicleIndexingService,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyStyleConfigIndexingService,
            IVehicleToBodyStyleConfigSearchService vehicletoBodyStyleConfigSearchService,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService,
            IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService,
            IVehicleToDriveTypeSearchService vehicletoDriveTypeSearchService,
            IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService,
            IVehicleToMfrBodyCodeSearchService vehicletoMfrBodyCodeSearchService,
            IVehicleToWheelBaseIndexingService vehicleToWheelBaseIndexingService,
            IVehicleToWheelBaseSearchService vehicletoWheelBaseSearchService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleSearchService = vehicleSearchService;
            _vehicleIndexingService = vehicleIndexingService;
            _vehicletoBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyStyleConfigIndexingService;
            _vehicletoBedConfigSearchService = vehicletoBedConfigSearchService;
            _vehicletoBodyStyleConfigSearchService = vehicletoBodyStyleConfigSearchService;
            _vehicleToDriveTypeIndexingService = vehicleToDriveTypeIndexingService;
            _vehicletoDriveTypeSearchService = vehicletoDriveTypeSearchService;
            _vehicleToMfrBodyCodeIndexingService = vehicleToMfrBodyCodeIndexingService;
            _vehicletoMfrBodyCodeSearchService = vehicletoMfrBodyCodeSearchService;
            _vehicleToWheelBaseIndexingService = vehicleToWheelBaseIndexingService;
            _vehicletoWheelBaseSearchService = vehicletoWheelBaseSearchService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var modelRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<Model>() as
                                IVcdbSqlServerEfRepositoryService<Model>;

            if (modelRepositoryService == null)
            {
                return;
            }

            var updatedModels =
                                await
                                    modelRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000);

            if (updatedModels == null || !updatedModels.Any())
            {
                throw new InvalidOperationException(
                    "Model Index cannot be updated before the transactional table is updated");
            }

            var updatedModel = updatedModels.First();

            await UpdateVehicleDocuments(updatedModel);

            await UpdateVehicleToBrakeConfigDocuments(updatedModel);
            await UpdateVehicleToBedConfigDocuments(updatedModel);
            await UpdateVehicleToBodyStyleConfigDocuments(updatedModel);
            await UpdateVehicleToDriveTypeDocuments(updatedModel);
            await UpdateVehicleToMfrBodyCodeDocuments(updatedModel);
            await UpdateVehicleToWheelBaseDocuments(updatedModel);
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

        private async Task UpdateVehicleDocuments(Model updatedModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleSearchResult =
                    await
                        _vehicleSearchService.SearchAsync(null,
                            $"modelId eq {updatedModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleDocuments = vehicleSearchResult.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        existingVehicleDocument.ModelName = updatedModel.Name;
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

        private async Task UpdateVehicleToBrakeConfigDocuments(Model updatedModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                    await
                        _vehicletoBrakeConfigSearchService.SearchAsync(null,
                            $"modelId eq {updatedModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBrakeConfigDocuments != null &&
                    existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.ModelName = updatedModel.Name;
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
        private async Task UpdateVehicleToBodyStyleConfigDocuments(Model updatedModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBodyStyleConfigSearchResult =
                    await
                        _vehicletoBodyStyleConfigSearchService.SearchAsync(null,
                            $"modelId eq {updatedModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;

                if (existingVehicleToBodyStyleConfigDocuments != null &&
                    existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBodyStyleConfigDocument in
                            existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.ModelName = updatedModel.Name;
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
        private async Task UpdateVehicleToBedConfigDocuments(Model updatedModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                    await
                        _vehicletoBedConfigSearchService.SearchAsync(null,
                            $"modelId eq {updatedModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;

                if (existingVehicleToBedConfigDocuments != null &&
                    existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.ModelName = updatedModel.Name;
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

        //Start
        private async Task UpdateVehicleToDriveTypeDocuments(Model updatedModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToDriveTypeSearchResult =
                    await
                        _vehicletoDriveTypeSearchService.SearchAsync(null,
                            $"modelId eq {updatedModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToDriveTypeDocuments = vehicleToDriveTypeSearchResult.Documents;

                if (existingVehicleToDriveTypeDocuments != null &&
                    existingVehicleToDriveTypeDocuments.Any())
                {
                    foreach (
                        var existingVehicleToDriveTypeDocument in
                            existingVehicleToDriveTypeDocuments)
                    {
                        existingVehicleToDriveTypeDocument.ModelName = updatedModel.Name;
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

        private async Task UpdateVehicleToMfrBodyCodeDocuments(Model updatedModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToMfrBodyCodeSearchResult =
                    await
                        _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                            $"modelId eq {updatedModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;

                if (existingVehicleToMfrBodyCodeDocuments != null &&
                    existingVehicleToMfrBodyCodeDocuments.Any())
                {
                    foreach (
                        var existingVehicleToMfrBodyCodeDocument in
                            existingVehicleToMfrBodyCodeDocuments)
                    {
                        existingVehicleToMfrBodyCodeDocument.ModelName = updatedModel.Name;
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

        private async Task UpdateVehicleToWheelBaseDocuments(Model updatedModel)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToWheelBaseSearchResult =
                    await
                        _vehicletoWheelBaseSearchService.SearchAsync(null,
                            $"modelId eq {updatedModel.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;

                if (existingVehicleToWheelBaseDocuments != null &&
                    existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (
                        var existingVehicleToWheelBaseDocument in
                            existingVehicleToWheelBaseDocuments)
                    {
                        existingVehicleToWheelBaseDocument.ModelName = updatedModel.Name;
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

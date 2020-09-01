using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Search.Model;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class BaseVehicleDataIndexer : IBaseVehicleDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleSearchService _vehicleSearchService;
        private readonly IVehicleIndexingService _vehicleIndexingService;
        private readonly IVehicleToBrakeConfigSearchService _vehicleToBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;
        private readonly IVehicleToBedConfigSearchService _vehicleToBedConfigSearchService;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService;
        private readonly IVehicleToBodyStyleConfigSearchService _vehicleToBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService;

        public BaseVehicleDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleSearchService vehicleSearchService,
            IVehicleIndexingService vehicleIndexingService,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService,
            IVehicleToBodyStyleConfigSearchService vehicletoBodyStyleConfigSearchService,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyStyleConfigIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleSearchService = vehicleSearchService;
            _vehicleIndexingService = vehicleIndexingService;
            _vehicleToBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
            _vehicleToBedConfigSearchService = vehicletoBedConfigSearchService;
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
            _vehicleToBodyStyleConfigSearchService = vehicletoBodyStyleConfigSearchService;
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyStyleConfigIndexingService;
        }

        //Raja: This function logic needs to be revisited
        //Pushkar: Revised and unit testing
        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var baseVehicleRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BaseVehicle>() as
                                IVcdbSqlServerEfRepositoryService<BaseVehicle>;

            if (baseVehicleRepositoryService == null)
            {
                return;
            }

            var addedBaseVehicles =
                                await
                                    baseVehicleRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "Make",
                                        "Model");

            if (addedBaseVehicles == null || !addedBaseVehicles.Any())
            {
                throw new InvalidOperationException(
                    "Base Vehicle Index cannot be updated before the transactional table is updated");
            }

            var addedBaseVehicle = addedBaseVehicles.First();
            var vehicleSearchResult =
                                await
                                    _vehicleSearchService.SearchAsync(null,
                                        $"baseVehicleId eq {addedBaseVehicle.Id}");

            var existingVehicleDocuments = vehicleSearchResult?.Documents;

            if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
            {
                throw new InvalidOperationException("Base Vehicle already exist in VehicleIndex. So, this change request cannot be an add request");
            }

            //base vehicle is new and therefore not yet available in "vehicles" azure search index
            var newVehicleDocument = new VehicleDocument
            {
                VehicleId = Guid.NewGuid().ToString(),
                BaseVehicleId = addedBaseVehicle.Id,
                MakeId = addedBaseVehicle.MakeId,
                MakeName = addedBaseVehicle.Make.Name,
                ModelId = addedBaseVehicle.ModelId,
                ModelName = addedBaseVehicle.Model.Name,
                YearId = addedBaseVehicle.YearId,
            };

            await this._vehicleIndexingService.UploadDocumentAsync(newVehicleDocument);
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var baseVehicleRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BaseVehicle>() as
                                IVcdbSqlServerEfRepositoryService<BaseVehicle>;

            if (baseVehicleRepositoryService == null)
            {
                return;
            }

            var updatedBaseVehicles =
                                await
                                    baseVehicleRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000,
                                        "Make",
                                        "Model");

            if (updatedBaseVehicles == null || !updatedBaseVehicles.Any())
            {
                throw new InvalidOperationException(
                    "Base Vehicle Index cannot be updated before the transactional table is updated");
            }

            var updatedBaseVehicle = updatedBaseVehicles.First();

            await UpdateVehicleDocuments(updatedBaseVehicle);

            await UpdateVehicleToBrakeConfigDocuments(updatedBaseVehicle);
            await UpdateVehicleToBedConfigDocuments(updatedBaseVehicle);
            await UpdateVehicleToBodyStyleConfigDocuments(updatedBaseVehicle);
            
        }
        
        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: This following code will delete vehicle document form vehicles index with base vehicle only information (no associated vehicles)
            var baseVehicleRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BaseVehicle>() as
                                IVcdbSqlServerEfRepositoryService<BaseVehicle>;

            if (baseVehicleRepositoryService == null)
            {
                return;
            }

            var deletedBaseVehicles =
                                await
                                    baseVehicleRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (deletedBaseVehicles == null || !deletedBaseVehicles.Any())
            {
                throw new InvalidOperationException(
                    "Base Vehicle Index cannot be updated before the transactional table is updated");
            }

            var deletedBaseVehicle = deletedBaseVehicles.First();

            if(deletedBaseVehicle.Vehicles == null || deletedBaseVehicle.Vehicles.Count == 0)
            {
                var vehicleSearchResult =
                await
                    _vehicleSearchService.SearchAsync(null,
                        $"baseVehicleId eq {deletedBaseVehicle.Id}");

                var existingVehicleDocuments = vehicleSearchResult.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await this._vehicleIndexingService.DeleteDocumentByVehicleIdAsync(existingVehicleDocument.VehicleId);
                    }
                }
            }
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var baseVehicleRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<BaseVehicle>() as
                                IVcdbSqlServerEfRepositoryService<BaseVehicle>;

            if (baseVehicleRepositoryService == null)
            {
                return;
            }

            var updatedBaseVehicles =
                                await
                                    baseVehicleRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedBaseVehicles == null || !updatedBaseVehicles.Any())
            {
                throw new InvalidOperationException(
                    "Base Vehicle Index cannot be updated before the transactional table is updated");
            }

            var updatedBaseVehicle = updatedBaseVehicles.First();

            var vehicleSearchResult =
                await
                    _vehicleSearchService.SearchAsync(null,
                        $"baseVehicleId eq {updatedBaseVehicle.Id}");

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
            }
        }

        private async Task UpdateVehicleDocuments(BaseVehicle updatedBaseVehicle)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleSearchResult =
                    await
                        _vehicleSearchService.SearchAsync(null,
                            $"baseVehicleId eq {updatedBaseVehicle.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleDocuments = vehicleSearchResult.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        existingVehicleDocument.BaseVehicleChangeRequestId = -1;
                        existingVehicleDocument.MakeId = updatedBaseVehicle.MakeId;
                        existingVehicleDocument.MakeName = updatedBaseVehicle.Make.Name;
                        existingVehicleDocument.ModelId = updatedBaseVehicle.ModelId;
                        existingVehicleDocument.ModelName = updatedBaseVehicle.Model.Name;
                        existingVehicleDocument.YearId = updatedBaseVehicle.YearId;
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

        private async Task UpdateVehicleToBrakeConfigDocuments(BaseVehicle updatedBaseVehicle)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                    await
                        _vehicleToBrakeConfigSearchService.SearchAsync(null,
                            $"baseVehicleId eq {updatedBaseVehicle.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBrakeConfigDocuments != null &&
                    existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.MakeId = updatedBaseVehicle.MakeId;
                        existingVehicleToBrakeConfigDocument.MakeName = updatedBaseVehicle.Make.Name;
                        existingVehicleToBrakeConfigDocument.ModelId = updatedBaseVehicle.ModelId;
                        existingVehicleToBrakeConfigDocument.ModelName = updatedBaseVehicle.Model.Name;
                        existingVehicleToBrakeConfigDocument.YearId = updatedBaseVehicle.YearId;
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

        private async Task UpdateVehicleToBedConfigDocuments(BaseVehicle updatedBaseVehicle)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBedConfigSearchResult =
                    await
                        _vehicleToBedConfigSearchService.SearchAsync(null,
                            $"baseVehicleId eq {updatedBaseVehicle.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;

                if (existingVehicleToBedConfigDocuments != null &&
                    existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBedConfigDocument in
                            existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBedConfigDocument.MakeId = updatedBaseVehicle.MakeId;
                        existingVehicleToBedConfigDocument.MakeName = updatedBaseVehicle.Make.Name;
                        existingVehicleToBedConfigDocument.ModelId = updatedBaseVehicle.ModelId;
                        existingVehicleToBedConfigDocument.ModelName = updatedBaseVehicle.Model.Name;
                        existingVehicleToBedConfigDocument.YearId = updatedBaseVehicle.YearId;
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

        private async Task UpdateVehicleToBodyStyleConfigDocuments(BaseVehicle updatedBaseVehicle)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBodyStyleConfigSearchResult =
                    await
                        _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                            $"baseVehicleId eq {updatedBaseVehicle.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;

                if (existingVehicleToBodyStyleConfigDocuments != null &&
                    existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBodyStyleConfigDocument in
                            existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.MakeId = updatedBaseVehicle.MakeId;
                        existingVehicleToBodyStyleConfigDocument.MakeName = updatedBaseVehicle.Make.Name;
                        existingVehicleToBodyStyleConfigDocument.ModelId = updatedBaseVehicle.ModelId;
                        existingVehicleToBodyStyleConfigDocument.ModelName = updatedBaseVehicle.Model.Name;
                        existingVehicleToBodyStyleConfigDocument.YearId = updatedBaseVehicle.YearId;
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

    }
}

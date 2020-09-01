using System;
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
    public class WheelBaseDataIndexer : IWheelBaseDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToWheelBaseSearchService _vehicletoWheelBaseSearchService;
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService;

        public WheelBaseDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToWheelBaseSearchService vehicletoWheelBaseSearchService,
            IVehicleToWheelBaseIndexingService  vehicleToWheelBaseIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicletoWheelBaseSearchService = vehicletoWheelBaseSearchService;
            _vehicleToWheelBaseIndexingService = vehicleToWheelBaseIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var wheelBaseRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<WheelBase>() as
                                IVcdbSqlServerEfRepositoryService<WheelBase>;

            if (wheelBaseRepositoryService == null)
            {
                return;
            }

            var addedWheelBases =
                                await
                                    wheelBaseRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000
                                        );

            if (addedWheelBases == null || !addedWheelBases.Any())
            {
                throw new InvalidOperationException(
                    "WheelBase Index cannot be updated before the transactional table is updated");
            }

            var addedWheelBase = addedWheelBases.First();
            var vehicleToWheelBaseSearchResult =
                                    await
                                        _vehicletoWheelBaseSearchService.SearchAsync(null,
                                            $"wheelBaseId eq {addedWheelBase.Id}");
            var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;

            if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
            {
                throw new InvalidOperationException(
                    "WheelBase already exisit in VehicleToWheelBaseIndex. So, this change request cannot be an add request");
            }

            //WheelBase is new and therefore not yet available in "vehicletoWheelBase" azure search index
            VehicleToWheelBaseDocument newVehicleToWheelBaseDocument = new VehicleToWheelBaseDocument
            {
                VehicleToWheelBaseId = Guid.NewGuid().ToString(),
                WheelBaseId = addedWheelBase.Id,
                WheelBaseName = addedWheelBase.Base,
                WheelBaseMetric = addedWheelBase.WheelBaseMetric,
              };

            await this._vehicleToWheelBaseIndexingService.UploadDocumentAsync(newVehicleToWheelBaseDocument);
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            //NOTE: This following code will delete vehicletowheelBase document form vehicletowheelbase index with wheelbase only information (no associated vehicles)
            var wheelBaseRepositoryService =
                            _vcdbUnitOfWork.GetRepositoryService<WheelBase>() as
                                IVcdbSqlServerEfRepositoryService<WheelBase>;

            if (wheelBaseRepositoryService == null)
            {
                return;
            }

            var deletedWheelBases =
                                await
                                    wheelBaseRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (deletedWheelBases == null || !deletedWheelBases.Any())
            {
                throw new InvalidOperationException(
                    "Wheel Base Index cannot be updated before the transactional table is updated");
            }

            var deletedWheelBase = deletedWheelBases.First();

            if (deletedWheelBase.VehicleToWheelBases == null || deletedWheelBase.VehicleToWheelBases.Count == 0)
            {
                var vehicleToWheelBaseSearchResult =
                await
                    _vehicletoWheelBaseSearchService.SearchAsync(null,
                        $"wheelBaseId eq {deletedWheelBase.Id}");

                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
                    {
                        //existingVehicleToWheelBaseDocument.VehicleToWheelBaseId must be a GUID string
                        await this._vehicleToWheelBaseIndexingService.DeleteDocumentByVehicleToWheelBaseIdAsync(existingVehicleToWheelBaseDocument.VehicleToWheelBaseId);
                    }
                }
            }
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var wheelBaseRepositoryService =
               _vcdbUnitOfWork.GetRepositoryService<WheelBase>() as
                   IVcdbSqlServerEfRepositoryService<WheelBase>;

            if (wheelBaseRepositoryService == null)
            {
                return;
            }

            var updatedWheelBases =
                        await
                            wheelBaseRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId,
                                100000);


            if (updatedWheelBases == null || !updatedWheelBases.Any())
            {
                throw new InvalidOperationException(
                    "WheelBases  Index cannot be updated before the transactional table is updated");
            }

            var updatedWheelBase = updatedWheelBases.First();

            await UpdateVehicleToWheelBaseDocuments(updatedWheelBase);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }
        private async Task UpdateVehicleToWheelBaseDocuments(WheelBase updatedWheelBase)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToWheelBaseSearchResult =
                                    await
                                        _vehicletoWheelBaseSearchService.SearchAsync(null,
                                            $"wheelBaseId eq {updatedWheelBase.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;

                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
                    {
                        existingVehicleToWheelBaseDocument.WheelBaseChangeRequestId = -1;
                        existingVehicleToWheelBaseDocument.WheelBaseId = updatedWheelBase.Id;
                        existingVehicleToWheelBaseDocument.WheelBaseName = updatedWheelBase.Base;
                        existingVehicleToWheelBaseDocument.WheelBaseMetric = updatedWheelBase.WheelBaseMetric;
                    }

                    await this._vehicleToWheelBaseIndexingService.UploadDocumentsAsync(existingVehicleToWheelBaseDocuments.ToList());
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

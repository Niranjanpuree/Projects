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
    public class BrakeABSDataIndexer : IBrakeABSDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;

        public BrakeABSDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicletoBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
            
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var brakeAbsRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<BrakeABS>() as
                    IVcdbSqlServerEfRepositoryService<BrakeABS>;

            if (brakeAbsRepositoryService == null)
            {
                return;
            }

            var updatedBrakeAbses =
                        await
                            brakeAbsRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId,
                                100000);


            if (updatedBrakeAbses == null || !updatedBrakeAbses.Any())
            {
                throw new InvalidOperationException(
                    "Brake ABS Index cannot be updated before the transactional table is updated");
            }

            var updatedBrakeAbs = updatedBrakeAbses.First();

            await UpdateVehicleToBrakeConfigDocuments(updatedBrakeAbs);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        private async Task UpdateVehicleToBrakeConfigDocuments(BrakeABS updatedBrakeAbs)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                await
                    _vehicletoBrakeConfigSearchService.SearchAsync(null,
                    $"brakeABSId eq {updatedBrakeAbs.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBrakeConfigDocuments != null &&
                    existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.BrakeABSName = updatedBrakeAbs.Name;
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
    }
}

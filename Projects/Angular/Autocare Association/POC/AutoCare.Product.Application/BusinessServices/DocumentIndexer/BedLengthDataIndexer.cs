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
    public class BedLengthDataIndexer : IBedLengthDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBedConfigSearchService _vehicletoBedConfigSearchService;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService;

        public BedLengthDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBedConfigIndexingService  vehicleToBedConfigIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicletoBedConfigSearchService = vehicletoBedConfigSearchService;
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
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
            var bedLengthRepositoryService =
               _vcdbUnitOfWork.GetRepositoryService<BedLength>() as
                   IVcdbSqlServerEfRepositoryService<BedLength>;

            if (bedLengthRepositoryService == null)
            {
                return;
            }

            var updatedBedLengths =
                        await
                            bedLengthRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId,
                                100000);


            if (updatedBedLengths == null || !updatedBedLengths.Any())
            {
                throw new InvalidOperationException(
                    "Bed Length  Index cannot be updated before the transactional table is updated");
            }

            var updatedBrakeAbs = updatedBedLengths.First();

            await UpdateVehicleToBedConfigDocuments(updatedBrakeAbs);
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        private async Task UpdateVehicleToBedConfigDocuments(BedLength updatedBedLength)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                await
                    _vehicletoBedConfigSearchService.SearchAsync(null,
                    $"bedLengthId eq {updatedBedLength.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBedConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBedConfigDocuments != null &&
                    existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBedConfigDocument in
                            existingVehicleToBedConfigDocuments)
                    {
                        existingVehicleToBedConfigDocument.BedLength = updatedBedLength.Length;
                        existingVehicleToBedConfigDocument.BedLengthMetric = updatedBedLength.BedLengthMetric;
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
    }
}

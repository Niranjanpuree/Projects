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
    public class BrakeSystemDataIndexer : IBrakeSystemDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;

        public BrakeSystemDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
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

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var brakeSystemRepositoryService =
                _vcdbUnitOfWork.GetRepositoryService<BrakeSystem>() as
                    IVcdbSqlServerEfRepositoryService<BrakeSystem>;

            if (brakeSystemRepositoryService == null)
            {
                return;
            }

            var updatedBrakeSystems =
                        await
                            brakeSystemRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId,
                                100000);


            if (updatedBrakeSystems == null || !updatedBrakeSystems.Any())
            {
                //Pushkar: need to test this condition
                throw new InvalidOperationException(
                    "Brake System Index cannot be updated before the transactional table is updated");
            }

            var updatedBrakeSystem = updatedBrakeSystems.First();

            await UpdateVehicleToBrakeConfigDocuments(updatedBrakeSystem);
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

        private async Task UpdateVehicleToBrakeConfigDocuments(BrakeSystem updatedBrakeSystem)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResult =
                    await
                        _vehicletoBrakeConfigSearchService.SearchAsync(null,
                            $"brakeSystemId eq {updatedBrakeSystem.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;

                if (existingVehicleToBrakeConfigDocuments != null &&
                    existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBrakeConfigDocuments)
                    {
                        existingVehicleToBrakeConfigDocument.BrakeSystemName = updatedBrakeSystem.Name;
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

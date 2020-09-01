using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.Search.Model;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class BodyNumDoorsDataIndexer : IBodyNumDoorsDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBodyStyleConfigSearchService _vehicleToBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService;

        public BodyNumDoorsDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
            IVehicleToBodyStyleConfigSearchService vehicleToBodyStyleConfigSearchService,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyStyleConfigIndexingService)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _vehicleToBodyStyleConfigSearchService = vehicleToBodyStyleConfigSearchService;
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyStyleConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var bodyNumDoorsRepositoryService =
                   _vcdbUnitOfWork.GetRepositoryService<BodyNumDoors>() as
                       IVcdbSqlServerEfRepositoryService<BodyNumDoors>;

            if (bodyNumDoorsRepositoryService == null)
            {
                return;
            }

            var updatedBodyNumDoors =
                        await
                            bodyNumDoorsRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId,
                                100000);


            if (updatedBodyNumDoors == null || !updatedBodyNumDoors.Any())
            {
                throw new InvalidOperationException(
                    "Body Num Doors Index cannot be updated before the transactional table is updated");
            }

            var updatedBodyNumDoor = updatedBodyNumDoors.First();

            await UpdateVehicleToBodyStyleConfigDocuments(updatedBodyNumDoor);

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

        private async Task UpdateVehicleToBodyStyleConfigDocuments(BodyNumDoors updatedBodyNumDoor)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToBodyStyleConfigSearchResult =
                await
                    _vehicleToBodyStyleConfigSearchService.SearchAsync(null,
                    $"bodyNumDoorsId eq {updatedBodyNumDoor.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });
                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;

                if (existingVehicleToBodyStyleConfigDocuments != null &&
                    existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (
                        var existingVehicleToBodyStyleConfigDocument in
                            existingVehicleToBodyStyleConfigDocuments)
                    {
                        existingVehicleToBodyStyleConfigDocument.BodyNumDoors = updatedBodyNumDoor.NumDoors;
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

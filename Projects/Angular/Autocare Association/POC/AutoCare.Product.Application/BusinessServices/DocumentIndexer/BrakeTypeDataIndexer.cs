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
    public class BrakeTypeDataIndexer : IBrakeTypeDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;

        public BrakeTypeDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork,
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
            var brakeTypeRepositoryService =
                   _vcdbUnitOfWork.GetRepositoryService<BrakeType>() as
                       IVcdbSqlServerEfRepositoryService<BrakeType>;

            if (brakeTypeRepositoryService == null)
            {
                return;
            }

            var updatedBrakeTypes =
                        await
                            brakeTypeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId,
                                100000);


            if (updatedBrakeTypes == null || !updatedBrakeTypes.Any())
            {
                throw new InvalidOperationException(
                    "Brake Type Index cannot be updated before the transactional table is updated");
            }

            var updatedBrakeType = updatedBrakeTypes.First();

            await UpdateVehicleToBrakeConfigDocuments(updatedBrakeType);
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

        private async Task UpdateVehicleToBrakeConfigDocuments(BrakeType updatedBrakeType)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                // get front brake type result
                var vehicleToBrakeConfigSearchResultFront =
                    await
                        _vehicletoBrakeConfigSearchService.SearchAsync(null,
                            $"frontBrakeTypeId eq {updatedBrakeType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBrakeConfigDocumentsFront = vehicleToBrakeConfigSearchResultFront.Documents;

                if (existingVehicleToBrakeConfigDocumentsFront != null &&
                    existingVehicleToBrakeConfigDocumentsFront.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBrakeConfigDocumentsFront)
                    {
                        existingVehicleToBrakeConfigDocument.FrontBrakeTypeName = updatedBrakeType.Name;
                    }

                    await
                        this._vehicleToBrakeConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBrakeConfigDocumentsFront.ToList());
                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);

            // get rear brake type result
            isEndReached = false;
            pageNumber = 1;
            do
            {
                var vehicleToBrakeConfigSearchResultRear =
                await
                    _vehicletoBrakeConfigSearchService.SearchAsync(null,
                    $"rearBrakeTypeId eq {updatedBrakeType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToBrakeConfigDocumentsRear = vehicleToBrakeConfigSearchResultRear.Documents;

                if (existingVehicleToBrakeConfigDocumentsRear != null &&
                    existingVehicleToBrakeConfigDocumentsRear.Any())
                {
                    foreach (
                        var existingVehicleToBrakeConfigDocument in
                            existingVehicleToBrakeConfigDocumentsRear)
                    {
                        existingVehicleToBrakeConfigDocument.RearBrakeTypeName = updatedBrakeType.Name;
                    }

                    await
                        this._vehicleToBrakeConfigIndexingService.UploadDocumentsAsync(
                            existingVehicleToBrakeConfigDocumentsRear.ToList());
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

using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class BedTypeDataIndexer : IBedTypeDataIndexer
    {
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService;

        public BedTypeDataIndexer(IVcdbUnitOfWork vcdbUnitOfWork
            /*IVehicleToBedConfigSearchService vehicletoBedConfigSearchService,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService*/)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            //_vehicletoBrakeConfigSearchService = vehicletoBedConfigSearchService;
            //_vehicleToBrakeConfigIndexingService = vehicleToBedConfigIndexingService;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            //var brakeTypeRepositoryService =
            //       _vcdbUnitOfWork.GetRepositoryService<BedType>() as
            //           IVcdbSqlServerEfRepositoryService<BedType>;

            //if (brakeTypeRepositoryService == null)
            //{
            //    return;
            //}

            //var updatedBedTypes =
            //            await
            //                bedTypeRepositoryService.GetAsync(item => item.ChangeRequestId == changeRequestId,
            //                    100000);


            //if (updatedBedTypes == null || !updatedBedTypes.Any())
            //{
            //    throw new InvalidOperationException(
            //        "Bed Type Index cannot be updated before the transactional table is updated");
            //}

            //var updatedBedType = updatedBedTypes.First();

            //await UpdateVehicleToBedConfigDocuments(updatedBedType);
            return;
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

        private async Task UpdateVehicleToBedConfigDocuments(BedType updatedBedType)
        {
            //bool isEndReached = false;
            //int pageNumber = 1;
            //do
            //{
            //    // get front brake type result
            //    var vehicleToBedConfigSearchResult =
            //        await
            //            _vehicletoBedConfigSearchService.SearchAsync(null,
            //                $"bedTypeId eq {updatedBedType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

            //    var existingVehicleToBedConfigDocumentsFront = vehicleToBedConfigSearchResultFront.Documents;

            //    if (existingVehicleToBedConfigDocumentsFront != null &&
            //        existingVehicleToBedConfigDocumentsFront.Any())
            //    {
            //        foreach (
            //            var existingVehicleToBedConfigDocument in
            //                existingVehicleToBedConfigDocumentsFront)
            //        {
            //            existingVehicleToBedConfigDocument.BedTypeName = updatedBedType.Name;
            //        }

            //        await
            //            this._vehicleToBedConfigIndexingService.UploadDocumentsAsync(
            //                existingVehicleToBedConfigDocumentsFront.ToList());
            //        pageNumber++;
            //    }
            //    else
            //    {
            //        isEndReached = true;
            //    }
            //} while (!isEndReached);

            //// get rear bed type result
            //isEndReached = false;
            //pageNumber = 1;
            //do
            //{
            //    var vehicleToBrakeConfigSearchResultRear =
            //    await
            //        _vehicletoBrakeConfigSearchService.SearchAsync(null,
            //        $"rearBrakeTypeId eq {updatedBrakeType.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

            //    var existingVehicleToBrakeConfigDocumentsRear = vehicleToBrakeConfigSearchResultRear.Documents;

            //    if (existingVehicleToBrakeConfigDocumentsRear != null &&
            //        existingVehicleToBrakeConfigDocumentsRear.Any())
            //    {
            //        foreach (
            //            var existingVehicleToBrakeConfigDocument in
            //                existingVehicleToBrakeConfigDocumentsRear)
            //        {
            //            existingVehicleToBrakeConfigDocument.RearBrakeTypeName = updatedBrakeType.Name;
            //        }

            //        await
            //            this._vehicleToBrakeConfigIndexingService.UploadDocumentsAsync(
            //                existingVehicleToBrakeConfigDocumentsRear.ToList());
            //        pageNumber++;
            //    }
            //    else
            //    {
            //        isEndReached = true;
            //    }
            //} while (!isEndReached);

            return;
        }
    }
}

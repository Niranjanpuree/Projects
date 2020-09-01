using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Search.Model;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearchIndex.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.DocumentIndexer
{
    public class MfrBodyCodeDataIndexer : IMfrBodyCodeDataIndexer
    {
        private readonly IVcdbUnitOfWork _repositories;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicletoMfrBodyCodeSearchService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService;
        private readonly IVehicleToMfrBodyCodeDataIndexer _vehicleToMfrBodyCodeDataIndexer;

        public MfrBodyCodeDataIndexer(IVcdbUnitOfWork repositories,
            IVehicleToMfrBodyCodeSearchService vehicletoMfrBodyCodeSearchService,
        IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService,
        IVehicleToMfrBodyCodeDataIndexer vehicleToMfrBodyCodeDataIndexer
            )
        {
            _repositories = repositories;
            _vehicletoMfrBodyCodeSearchService = vehicletoMfrBodyCodeSearchService;
            _vehicleToMfrBodyCodeIndexingService = vehicleToMfrBodyCodeIndexingService;
            _vehicleToMfrBodyCodeDataIndexer = vehicleToMfrBodyCodeDataIndexer;
        }

        public async Task AddChangeRequestIndexerAsync(long changeRequestId)
        {
            var mfrBodyCodeRepositoryService =
                            _repositories.GetRepositoryService<MfrBodyCode>() as
                                IVcdbSqlServerEfRepositoryService<MfrBodyCode>;

            if (mfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var addedMfrBodyCodes =
                                await
                                    mfrBodyCodeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000
                                        
                                        );

            if (addedMfrBodyCodes == null || !addedMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            var addedMfrBodyCode = addedMfrBodyCodes.First();
            var vehicleToMfrBodyCodeSearchResult =
                                    await
                                        _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                                            $"mfrBodyCodeId eq {addedMfrBodyCode.Id}");
            var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;

            if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
            {
                throw new InvalidOperationException(
                    "Mfr Body Code already exisit in VehicleToMfrBodyCodeIndex. So, this change request cannot be an add request");
            }

            //MfrBodyCode is new and therefore not yet available in "vehicletoMfrBodyCode" azure search index
            VehicleToMfrBodyCodeDocument newMfrBodyCodeConfigDocument = new VehicleToMfrBodyCodeDocument
            {
                VehicleToMfrBodyCodeId = Guid.NewGuid().ToString(),
                MfrBodyCodeId = addedMfrBodyCode.Id,
                MfrBodyCodeName = addedMfrBodyCode.Name
            };

            await this._vehicleToMfrBodyCodeIndexingService.UploadDocumentAsync(newMfrBodyCodeConfigDocument);
        }
        public async Task ModifyChangeRequestIndexerAsync(long changeRequestId)
        {
            var mfrBodyCodeRepositoryService =
                            _repositories.GetRepositoryService<MfrBodyCode>() as
                                IVcdbSqlServerEfRepositoryService<MfrBodyCode>;

            if (mfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var updatedMfrBodyCodes =
                                await
                                    mfrBodyCodeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId, 100000
                                        );

            if (updatedMfrBodyCodes == null || !updatedMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            var updatedMfrBodyCode = updatedMfrBodyCodes.First();

            await UpdateVehicleToMfrBodyCodeDocuments(updatedMfrBodyCode);
        }

        public async Task ReplaceChangeRequestIndexerAsync(long changeRequestId)
        {
            return;
        }

        public async Task DeleteChangeRequestIndexerAsync(long changeRequestId)
        {
            var mfrBodyCodeRepositoryService =
                _repositories.GetRepositoryService<MfrBodyCode>() as
                    IVcdbSqlServerEfRepositoryService<MfrBodyCode>;

            if (mfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var deletedMfrBodyCodes =
                await mfrBodyCodeRepositoryService.GetAsync(
                    item => item.ChangeRequestId == changeRequestId);

            if (deletedMfrBodyCodes == null || !deletedMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            var deletedMfrBodyCode = deletedMfrBodyCodes.First();

            if (deletedMfrBodyCode.VehicleToMfrBodyCodes == null || deletedMfrBodyCode.VehicleToMfrBodyCodes.Count == 0)
            {
                var vehicleToDriveTypeSearchResult =
                await
                    _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                        $"mfrBodyCodeId eq {deletedMfrBodyCode.Id}");

                var existingVehicleToMfrBodyCodeDocuments = vehicleToDriveTypeSearchResult.Documents;
                if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingVehicleToMfrBodyCodeDocument in existingVehicleToMfrBodyCodeDocuments)
                    {
                        //existingVehicleToMfrBodyCodeDocument.VehicleToMfrBodyCodeId must be a GUID string
                        await this._vehicleToMfrBodyCodeIndexingService.DeleteDocumentByVehicleToMfrBodyCodeIdAsync(existingVehicleToMfrBodyCodeDocument.VehicleToMfrBodyCodeId);
                    }
                }
            }
        }

        public async Task RejectChangeRequestIndexerAsync(long changeRequestId)
        {
            var mfrBodyCodeRepositoryService =
                            _repositories.GetRepositoryService<MfrBodyCode>() as
                                IVcdbSqlServerEfRepositoryService<MfrBodyCode>;

            if (mfrBodyCodeRepositoryService == null)
            {
                return;
            }

            var updatedMfrBodyCodes =
                                await
                                    mfrBodyCodeRepositoryService.GetAsync(
                                        item => item.ChangeRequestId == changeRequestId);

            if (updatedMfrBodyCodes == null || !updatedMfrBodyCodes.Any())
            {
                throw new InvalidOperationException(
                    "Mfr Body Code Index cannot be updated before the transactional table is updated");
            }

            var updatedMfrBodyCode = updatedMfrBodyCodes.First();

            var vehicleToMfrBodyCodeSearchResult =
                                    await
                                        _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                                            $"mfrBodyCodeId eq {updatedMfrBodyCode.Id}");

            var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;

            if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
            {
                foreach (var existingVehicleToMfrBodyCodeDocument in existingVehicleToMfrBodyCodeDocuments)
                {
                    existingVehicleToMfrBodyCodeDocument.MfrBodyCodeChangeRequestId = -1;
                }

                await
                    this._vehicleToMfrBodyCodeIndexingService.UploadDocumentsAsync(
                        existingVehicleToMfrBodyCodeDocuments.ToList());
            }
        }

        private async Task UpdateVehicleToMfrBodyCodeDocuments(MfrBodyCode updatedMfrBodyCode)
        {
            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var vehicleToMfrBodyCodeSearchResult =
                                    await
                                        _vehicletoMfrBodyCodeSearchService.SearchAsync(null,
                                            $"mfrBodyCodeId eq {updatedMfrBodyCode.Id}", new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                var existingVehicleToMfrBodyCodeDocuments = vehicleToMfrBodyCodeSearchResult.Documents;

                if (existingVehicleToMfrBodyCodeDocuments != null && existingVehicleToMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingVehicleToMfrBodyCodeDocument in existingVehicleToMfrBodyCodeDocuments)
                    {
                        existingVehicleToMfrBodyCodeDocument.MfrBodyCodeChangeRequestId = -1;
                        existingVehicleToMfrBodyCodeDocument.MfrBodyCodeId = updatedMfrBodyCode.Id;
                        existingVehicleToMfrBodyCodeDocument.MfrBodyCodeName = updatedMfrBodyCode.Name;
                    }

                    await this._vehicleToMfrBodyCodeIndexingService.UploadDocumentsAsync(existingVehicleToMfrBodyCodeDocuments.ToList());
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

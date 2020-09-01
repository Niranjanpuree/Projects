using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Linq.Expressions;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class MfrBodyCodeBusinessService : VcdbBusinessService<MfrBodyCode>, IMfrBodyCodeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<MfrBodyCode> _mfrBodyCodeRepositoryService;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService = null;
        private readonly IVehicleToMfrBodyCodeSearchService _vehicleToMfrBodyCodeSearchService = null;

        public MfrBodyCodeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService,
            ITextSerializer serializer, IVehicleToMfrBodyCodeSearchService vehicleToMfrBodyCodeSearchService)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _mfrBodyCodeRepositoryService = Repositories.GetRepositoryService<MfrBodyCode>() as ISqlServerEfRepositoryService<MfrBodyCode>;
            _vehicleToMfrBodyCodeIndexingService = vehicleToMfrBodyCodeIndexingService;
            _vehicleToMfrBodyCodeSearchService = vehicleToMfrBodyCodeSearchService;
        }

        public override async Task<List<MfrBodyCode>> GetAllAsync(int topCount = 0)
        {
            return await _mfrBodyCodeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        
        public override async Task<MfrBodyCode> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var mfrBodyCode = await FindAsync(id);

            if (mfrBodyCode == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            mfrBodyCode.VehicleToMfrBodyCodeCount =
                await _mfrBodyCodeRepositoryService.GetCountAsync(mfrBodyCode, x => x.VehicleToMfrBodyCodes, y => y.DeleteDate == null);

            return mfrBodyCode;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(MfrBodyCode entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateMfrBodyCodeIsNotDuplicate(entity, ChangeType.Add);
            await ValidateMfrBodyCodeHasNoChangeRequest(entity, ChangeType.Add);
            

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(MfrBodyCode).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);

        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(MfrBodyCode entity, TId id,
            string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid MfrBodyCode Id");
            }

            var mfrBodyCodeDb = await FindAsync(id);

            if (mfrBodyCodeDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateMfrBodyCodeIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateMfrBodyCodeHasNoChangeRequest(entity, ChangeType.Modify);
            //await ValidateMfrBodyCodeLookUpHasNoChangeRequest(entity);

            // fill up audit information
            entity.InsertDate = mfrBodyCodeDb.InsertDate;
            entity.LastUpdateDate = mfrBodyCodeDb.LastUpdateDate;
            entity.DeleteDate = mfrBodyCodeDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new MfrBodyCode()
            {
                Id = mfrBodyCodeDb.Id,
                Name = mfrBodyCodeDb.Name,
                ChangeRequestId = mfrBodyCodeDb.ChangeRequestId,
                VehicleToMfrBodyCodeCount = mfrBodyCodeDb.VehicleToMfrBodyCodeCount,
                DeleteDate = mfrBodyCodeDb.DeleteDate,
                InsertDate = mfrBodyCodeDb.InsertDate,
                LastUpdateDate = mfrBodyCodeDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(MfrBodyCode).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , mfrBodyCodeDb.Name
                , entity.Name);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            mfrBodyCodeDb.ChangeRequestId = changeRequestId;
            _mfrBodyCodeRepositoryService.Update(mfrBodyCodeDb);
            Repositories.SaveChanges();

            IList<VehicleToMfrBodyCode> existingVehicleToMfrBodyCodes =
                await
                    base.Repositories.GetRepositoryService<VehicleToMfrBodyCode>()
                        .GetAsync(item => item.MfrBodyCodeId.Equals(mfrBodyCodeDb.Id) && item.DeleteDate == null);
            
            //NOTE: updating change request id in child dependent tables is not valid

            if (existingVehicleToMfrBodyCodes == null || existingVehicleToMfrBodyCodes.Count == 0)
            {
                var mfrBodyCodeSearchResult = await _vehicleToMfrBodyCodeSearchService.SearchAsync(null, $"mfrBodyCodeId eq {mfrBodyCodeDb.Id}");
                var existingMfrBodyCodeDocuments = mfrBodyCodeSearchResult.Documents;
                if (existingMfrBodyCodeDocuments != null && existingMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingMfrBodyCodeDocument in existingMfrBodyCodeDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToMfrBodyCodeIndexingService.UpdateMfrBodyCodeChangeRequestIdAsync(existingMfrBodyCodeDocument.VehicleToMfrBodyCodeId, changeRequestId);
                    }
                }
            }
            else
            {
                foreach (var vehicleToMfrBodyCode in existingVehicleToMfrBodyCodes)
                {
                    await _vehicleToMfrBodyCodeIndexingService.
                         UpdateMfrBodyCodeChangeRequestIdAsync(vehicleToMfrBodyCode.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitReplaceChangeRequestAsync<TId>(MfrBodyCode entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid MfrBodyCode Id");
            }

            var mfrBodyCodeFromDb = await FindAsync(id);
            if (mfrBodyCodeFromDb == null)
            {
                throw new NoRecordFound("No MFR Body Code exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            //await ValidateMfrBodyCodeLookUpHasNoChangeRequest(entity);

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            var vehicleToMfrBodyCodeRepository = base.Repositories.GetRepositoryService<VehicleToMfrBodyCode>();
            foreach (var vehicleToMfrBodyCode in entity.VehicleToMfrBodyCodes)
            {
                
                var vehicleToMfrBodyCodeFromDb = await vehicleToMfrBodyCodeRepository.GetAsync(x => x.Id == vehicleToMfrBodyCode.Id && x.DeleteDate == null);
                if (vehicleToMfrBodyCodeFromDb == null || !vehicleToMfrBodyCodeFromDb.Any()) continue;

                vehicleToMfrBodyCode.InsertDate = vehicleToMfrBodyCodeFromDb.First().InsertDate;
            }

            changeRequestItemStagings.AddRange(entity.VehicleToMfrBodyCodes.Select(item => new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = item.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToMfrBodyCode).Name,
                Payload = base.Serializer.Serialize(item),
                EntityFullName = typeof(VehicleToMfrBodyCode).AssemblyQualifiedName
            }));

            changeContent = string.Format("{0}"
                , mfrBodyCodeFromDb.Name);

            var changeRequestId = await base.SubmitReplaceChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            mfrBodyCodeFromDb.ChangeRequestId = changeRequestId;
            _mfrBodyCodeRepositoryService.Update(mfrBodyCodeFromDb);
            Repositories.SaveChanges();

            //NOTE: all vehicletomfrbodycodes linked to the mfrbodycode need to be updated with MfrBodyCodeChangeRequestId = CR
            var linkedVehicles = await vehicleToMfrBodyCodeRepository.GetAsync(x => x.MfrBodyCode.Id == mfrBodyCodeFromDb.Id && x.DeleteDate == null);
            //NOTE: updating change request id in child dependent tables is not valid

            if (linkedVehicles == null || linkedVehicles.Count == 0)
            {
                var mfrBodyCodeSearchResult = await _vehicleToMfrBodyCodeSearchService.SearchAsync(null, $"mfrBodyCodeId eq {mfrBodyCodeFromDb.Id}");
                var existingMfrBodyCodeDocuments = mfrBodyCodeSearchResult.Documents;
                if (existingMfrBodyCodeDocuments != null && existingMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingMfrBodyCodeDocument in existingMfrBodyCodeDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToMfrBodyCodeIndexingService.UpdateMfrBodyCodeChangeRequestIdAsync(existingMfrBodyCodeDocument.VehicleToMfrBodyCodeId, changeRequestId);
                    }
                }
            }
            else
            {
                foreach (var vehicleToMfrBodyCode in linkedVehicles)
                {
                    await _vehicleToMfrBodyCodeIndexingService.
                         UpdateMfrBodyCodeChangeRequestIdAsync(vehicleToMfrBodyCode.Id.ToString(), changeRequestId);
                }
            }


            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(MfrBodyCode entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var mfrBodyCodeFromDb = await FindAsync(id);
            if (mfrBodyCodeFromDb == null)
            {
                throw new NoRecordFound("No Mfr Body Code exist");
            }

            MfrBodyCode mfrBodyCodeSubmit = new MfrBodyCode()
            {
                Id = mfrBodyCodeFromDb.Id,
                VehicleToMfrBodyCodeCount = entity.VehicleToMfrBodyCodeCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();
            //await ValidateMfrBodyCodeLookUpHasNoChangeRequest(mfrBodyCodeFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = mfrBodyCodeSubmit.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(MfrBodyCode).Name,
                Payload = base.Serializer.Serialize(mfrBodyCodeSubmit)
            });

            IList<VehicleToMfrBodyCode> existingVehicleToMfrBodyCodes =
                await
                    base.Repositories.GetRepositoryService<VehicleToMfrBodyCode>()
                        .GetAsync(item => item.MfrBodyCodeId.Equals(mfrBodyCodeSubmit.Id) && item.DeleteDate == null);

            if (existingVehicleToMfrBodyCodes != null && existingVehicleToMfrBodyCodes.Count > 0)
            {
                changeRequestItemStagings.AddRange(
                    existingVehicleToMfrBodyCodes.Select(vehicleToMfrBodyCode => new ChangeRequestItemStaging()
                    {
                        ChangeType = ChangeType.Delete,
                        EntityId = vehicleToMfrBodyCode.Id.ToString(),
                        CreatedDateTime = DateTime.UtcNow,
                        Entity = typeof(VehicleToMfrBodyCode).Name,
                        Payload = base.Serializer.Serialize(new VehicleToMfrBodyCode()
                        {
                            Id = vehicleToMfrBodyCode.Id,
                            MfrBodyCodeId = vehicleToMfrBodyCode.MfrBodyCodeId,
                            VehicleId = vehicleToMfrBodyCode.VehicleId
                        })
                    }));
            }
            changeContent = string.Format("{0}"
                , mfrBodyCodeFromDb.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(mfrBodyCodeSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            mfrBodyCodeFromDb.ChangeRequestId = changeRequestId;
            _mfrBodyCodeRepositoryService.Update(mfrBodyCodeFromDb);
            Repositories.SaveChanges();
            
            //NOTE: updating change request id in child dependent tables is not valid

            if (existingVehicleToMfrBodyCodes == null || existingVehicleToMfrBodyCodes.Count == 0)
            {
                var mfrBodyCodeSearchResult = await _vehicleToMfrBodyCodeSearchService.SearchAsync(null, $"mfrBodyCodeId eq {mfrBodyCodeFromDb.Id}");
                var existingMfrBodyCodeDocuments = mfrBodyCodeSearchResult.Documents;
                if (existingMfrBodyCodeDocuments != null && existingMfrBodyCodeDocuments.Any())
                {
                    foreach (var existingMfrBodyCodeDocument in existingMfrBodyCodeDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToMfrBodyCodeIndexingService.UpdateMfrBodyCodeChangeRequestIdAsync(existingMfrBodyCodeDocument.VehicleToMfrBodyCodeId, changeRequestId);
                    }
                }
            }
            else
            {
                foreach (var vehicleToMfrBodyCode in existingVehicleToMfrBodyCodes)
                {
                    await _vehicleToMfrBodyCodeIndexingService.
                         UpdateMfrBodyCodeChangeRequestIdAsync(vehicleToMfrBodyCode.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public new async Task<MfrBodyCodeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            var result = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<MfrBodyCode, TId>(changeRequestId);

            List<VehicleToMfrBodyCode> vehicleToMfrBodyCodes = null;
            if (result.StagingItem.ChangeType == ChangeType.Replace.ToString())
            {
                result.EntityStaging = result.EntityCurrent;
                var changeRequestIdLong = Convert.ToInt64(changeRequestId);
                var vehicleToMfrBodyCodeChangeRequestItems = await this.ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(item => item.ChangeRequestId == changeRequestIdLong);

                if (vehicleToMfrBodyCodeChangeRequestItems != null && vehicleToMfrBodyCodeChangeRequestItems.Any())
                {
                    var vehicleToMfrBodyCodeIds = vehicleToMfrBodyCodeChangeRequestItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                    vehicleToMfrBodyCodes = await base.Repositories.GetRepositoryService<VehicleToMfrBodyCode>()
                        .GetAsync(item => vehicleToMfrBodyCodeIds.Any(id => id == item.Id) && item.DeleteDate == null);

                    //1. Extract the replacement mfrBodyCode to mfrBodyCodeid from the first deserialized vehicleToMfrBodyCodeChangeRequestItems
                    var vehicleToMfrBodyCode = Serializer.Deserialize<VehicleToMfrBodyCode>(vehicleToMfrBodyCodeChangeRequestItems[0].Payload);

                    //2. fill result.EntityStaging with the replacement MfrBodyCode details
                    result.EntityStaging = await FindAsync(vehicleToMfrBodyCode.MfrBodyCodeId);

                    // 3. fill currentEntity
                    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                }
                else
                {
                    var vehicleToMfrBodyCodeChangeRequestStoreItems = await this.ChangeRequestBusinessService.GetChangeRequestItemAsync(item =>
                        item.ChangeRequestId == changeRequestIdLong);

                    if (vehicleToMfrBodyCodeChangeRequestStoreItems != null && vehicleToMfrBodyCodeChangeRequestStoreItems.Any())
                    {
                        var vehicleToMfrBodyCodeIds = vehicleToMfrBodyCodeChangeRequestStoreItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                        vehicleToMfrBodyCodes =
                            await
                                base.Repositories.GetRepositoryService<VehicleToMfrBodyCode>()
                                    .GetAsync(item => vehicleToMfrBodyCodeIds.Any(id => id == item.Id) && item.DeleteDate == null);

                        //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                        var vehicleToMfrBodyCode = Serializer.Deserialize<VehicleToMfrBodyCode>(vehicleToMfrBodyCodeChangeRequestStoreItems[0].Payload);

                        //2. fill result.EntityStaging with the replacement base vehicle details
                        result.EntityStaging = await FindAsync(vehicleToMfrBodyCode.MfrBodyCodeId);

                        // 3. fill currentEntity
                        result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                    }
                }
            }

            MfrBodyCodeChangeRequestStagingModel staging = new MfrBodyCodeChangeRequestStagingModel
            {
                EntityCurrent = result.EntityCurrent,
                EntityStaging = result.EntityStaging,
                //RequestorComments = result.RequestorComments,
                //ReviewerComments = result.ReviewerComments,
                Comments = result.Comments,
                StagingItem = result.StagingItem,
                ReplacementVehicleToMfrBodyCodes = vehicleToMfrBodyCodes,
                Attachments = result.Attachments
            };
            return staging;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                MfrBodyCode deserializedEntry = Serializer.Deserialize<MfrBodyCode>(payload);
                count.VehicleToMfrBodyCodeCount = deserializedEntry.VehicleToMfrBodyCodeCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _mfrBodyCodeRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _mfrBodyCodeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }

            var vehicleToMfrBodyCodeRepositoryService = base.Repositories.GetRepositoryService<VehicleToMfrBodyCode>();
            List<VehicleToMfrBodyCode> approvedReplacementVehicleToMfrBodyCodes = await vehicleToMfrBodyCodeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (approvedReplacementVehicleToMfrBodyCodes != null && approvedReplacementVehicleToMfrBodyCodes.Any())
            {
                foreach (var approvedReplacementVehicleToMfrBodyCode in approvedReplacementVehicleToMfrBodyCodes)
                {
                    approvedReplacementVehicleToMfrBodyCode.ChangeRequestId = null;
                    vehicleToMfrBodyCodeRepositoryService.Update(approvedReplacementVehicleToMfrBodyCode);
                }
                Repositories.SaveChanges();
            }
        }

        public override async Task<List<MfrBodyCode>> GetPendingAddChangeRequests(Expression<Func<MfrBodyCode, bool>> whereCondition = null, int topCount = 0)
        {
            var existingChangeRequestItemStagings = await ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(
                x => x.Entity.Equals(typeof(MfrBodyCode).Name, StringComparison.CurrentCultureIgnoreCase)
                && x.ChangeType == ChangeType.Add);

            List<MfrBodyCode> pendingMfrBodyCodes = new List<MfrBodyCode>();

            foreach (var existingChangeRequestItemStaging in existingChangeRequestItemStagings)
            {
                var pendingMfrBodyCode = Serializer.Deserialize<MfrBodyCode>(existingChangeRequestItemStaging.Payload);
                pendingMfrBodyCode.ChangeRequestId = existingChangeRequestItemStaging.ChangeRequestId;
                pendingMfrBodyCodes.Add(pendingMfrBodyCode);
            }

            if (whereCondition != null)
            {
                var predicate = whereCondition.Compile();

                pendingMfrBodyCodes = pendingMfrBodyCodes.Where(predicate).ToList();
            }

            return pendingMfrBodyCodes;
        }
        private async Task ValidateConfigurationDoesNotMatchWithExistingMfrBodyCode(MfrBodyCode mfrBodyCode)
        {
            // 1. Validate that there is no mfr body code config with same configuration
            IList<MfrBodyCode> mfrBodyCodeFromDb =
                await
                    _mfrBodyCodeRepositoryService.GetAsync(
                        x => x.Name.Equals(mfrBodyCode.Name)
                            
                             && x.DeleteDate == null);

            if (mfrBodyCodeFromDb != null && mfrBodyCodeFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Mfr Body Code with same configuration already exists.");
            }
        }
        
        private async Task<MfrBodyCode> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _mfrBodyCodeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
        private async Task ValidateMfrBodyCodeIsNotDuplicate(MfrBodyCode mfrBodyCode, ChangeType changeType)
        {
            // validate no duplicate MfrBodyCode Name.
            if (changeType == ChangeType.Add)
            {
                IList<MfrBodyCode> mfrBodyCodeFromDb =
                    await
                        _mfrBodyCodeRepositoryService.GetAsync(
                            x => x.Name.Trim().Equals(mfrBodyCode.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (mfrBodyCodeFromDb != null && mfrBodyCodeFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a MfrBodyCode Name: {mfrBodyCode.Name}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var mfrBodyCodeFromDb = await FindAsync(mfrBodyCode.Id);

                if (mfrBodyCode.Name.Trim().Equals(mfrBodyCodeFromDb.Name.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a MfrBodyCode Name: {mfrBodyCode.Name}.");
                }
                if (!mfrBodyCode.Name.Trim().Equals(mfrBodyCodeFromDb.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<MfrBodyCode> mfrBodyCodesFromDb =
                        await
                            _mfrBodyCodeRepositoryService.GetAsync(
                                x => x.Name.Trim().Equals(mfrBodyCode.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (mfrBodyCodesFromDb != null && mfrBodyCodesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a MfrBodyCode Name: {mfrBodyCode.Name}.");
                    }
                }
            }
        }
        private async Task ValidateMfrBodyCodeHasNoChangeRequest(MfrBodyCode mfrBodyCode, ChangeType changeType)
        {
            // validate no CR for this MfrBodyCode Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<MfrBodyCode>(item => item.Id == mfrBodyCode.Id));

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with MfrBodyCode ID : {mfrBodyCode.Id}.");
                }
            }

            // validate no CR for this MfrBodyCode Name.
            var changerequestid1 =
            await
                ChangeRequestBusinessService.ChangeRequestExist<MfrBodyCode>(
                    x => x.Name.Trim().Equals(mfrBodyCode.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (changerequestid1 > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same MfrBodyCode Name : {mfrBodyCode.Name}.");
            }
        }
        private async Task ValidateMfrBodyCodeLookUpHasNoChangeRequest(MfrBodyCode mfrBodyCode)
        {
            // validate no CR for base vehicle using this MfrBodyCode.
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<VehicleToMfrBodyCode>(x => x.MfrBodyCode.Id == mfrBodyCode.Id);
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for Vehicle To Mfr Body Code using this MfrBodyCode.");
            }
        }
       

    }
}
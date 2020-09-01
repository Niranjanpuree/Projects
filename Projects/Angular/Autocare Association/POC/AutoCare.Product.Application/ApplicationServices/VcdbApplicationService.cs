using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public abstract class VcdbApplicationService<T>: ApplicationService<T>, IVcdbApplicationService<T>
        where T:class
    {
        private readonly IVcdbBusinessService<T> _vcdbBusinessService;

        protected VcdbApplicationService(IVcdbBusinessService<T> vcdbBusinessService) 
            : base(vcdbBusinessService)
        {
            _vcdbBusinessService = vcdbBusinessService;
        }

        public override async Task<long> AddAsync(T entity, string requestedBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null)
        {
            return await _vcdbBusinessService.SubmitAddChangeRequestAsync(entity, requestedBy, null, comment, attachments);
        }

        public override async Task<long> UpdateAsync<TId>(T entity, TId id, string requestedBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null)
        {
            return await _vcdbBusinessService.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, null, comment,attachments);
        }

        public override async Task<long> DeleteAsync<TId>(T entity, TId id, string requestedBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null)
        {
            return await _vcdbBusinessService.SubmitDeleteChangeRequestAsync(entity, id, requestedBy, null, comment, attachments);
        }

        public override async Task<long> ReplaceAsync<TId>(T entity, TId id, string requestdBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null)
        {
            return await _vcdbBusinessService.SubmitReplaceChangeRequestAsync(entity, id, requestdBy, null, comment, attachments);
        }

        public virtual async Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings)
        {
            return await _vcdbBusinessService.GetAssociatedCount(selectedChangeRequestStagings);
        }
    }
}

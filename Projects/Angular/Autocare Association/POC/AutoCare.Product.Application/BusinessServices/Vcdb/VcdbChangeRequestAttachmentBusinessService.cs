using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VcdbChangeRequestAttachmentBusinessService : ChangeRequestAttachmentBusinessService<AttachmentsStaging>, IVcdbChangeRequestAttachmentBusinessService
    {
        private readonly ISqlServerEfRepositoryService<AttachmentsStaging> _attachmentsStagingRepositoryService;
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;

        public VcdbChangeRequestAttachmentBusinessService(IVcdbUnitOfWork vcdbUnitOfWork, ITextSerializer serializer)
            : base (vcdbUnitOfWork, serializer)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _attachmentsStagingRepositoryService = vcdbUnitOfWork.GetRepositoryService<AttachmentsStaging>() 
                as ISqlServerEfRepositoryService<AttachmentsStaging>;
        }

        public override async Task<List<AttachmentsStaging>> GetChangeRequestAttachmentsAsync(Expression<Func<AttachmentsStaging, bool>> whereCondition, int topCount = 0)
        {
            return await this._attachmentsStagingRepositoryService.GetAsync(whereCondition, topCount);
        }

        public override void Add(AttachmentsStaging entity)
        {
            this._attachmentsStagingRepositoryService.Add(entity);
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await this._vcdbUnitOfWork.SaveChangesAsync();
        }
    }
}

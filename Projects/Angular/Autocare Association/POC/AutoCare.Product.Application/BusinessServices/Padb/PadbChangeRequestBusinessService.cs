using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoMapper;

namespace AutoCare.Product.Application.BusinessServices.Padb
{
    // todo: note: use concrete classes of staging, item & comments for pcdb
    // todo: note: remove abstract property 
    public abstract class PadbChangeRequestBusinessService<T, TItem, TComment, TAttachment> :
        ChangeRequestBusinessService<T, TItem, TComment, TAttachment>, IPadbChangeRequestBusinessService<T, TItem, TComment, TAttachment>
        where T : class
        where TItem : class
        where TComment : class
        where TAttachment : class
    {
        public PadbChangeRequestBusinessService(IVcdbUnitOfWork entityRepositoryService, IMapper autoMapper,
            ITextSerializer serializer,
            IChangeRequestItemBusinessService<TItem> changeRequestItemBusinessService,
            IChangeRequestCommentsBusinessService<TComment> changeRequestCommentsBusinessService,
            IChangeRequestIndexingService changeRequestIndexingService)
            : base(entityRepositoryService, autoMapper, serializer,
                changeRequestItemBusinessService, changeRequestCommentsBusinessService, changeRequestIndexingService)
        {
        }

        public override Task<AssociationCount> GetAssociatedCount(
            List<ChangeRequestStaging> selectedChangeRequestStagings)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> AssignReviewer(AssignReviewerBusinessModel assignReviewerBusinessModel)
        {
            throw new NotImplementedException();
        }
    }
}

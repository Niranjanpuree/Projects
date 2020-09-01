using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Application.BusinessServices
{
    public abstract class ChangeRequestAttachmentBusinessService<T>: ChangeRequestStagingBase<T>, IChangeRequestAttachmentBusinessService<T>
        where T:class
    {
        private readonly IRepositoryService<T> _repositoryService;

        protected ChangeRequestAttachmentBusinessService(IUnitOfWork repositories, ITextSerializer serializer) : base(repositories, serializer)
        {
            _repositoryService = repositories.GetRepositoryService<T>();
        }

        public virtual async Task<List<T>> GetChangeRequestAttachmentsAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0)
        {
            return await _repositoryService.GetAsync(whereCondition, topCount);
        }
    }
}

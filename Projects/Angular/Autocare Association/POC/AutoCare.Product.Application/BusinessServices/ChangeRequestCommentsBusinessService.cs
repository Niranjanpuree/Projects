using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Application.BusinessServices
{
    public abstract class ChangeRequestCommentsBusinessService<T>: ChangeRequestStagingBase<T>, IChangeRequestCommentsBusinessService<T>
        where T:class
    {
        private readonly IRepositoryService<T> _repositoryService;

        protected ChangeRequestCommentsBusinessService(IUnitOfWork repositories, ITextSerializer serializer) : base(repositories, serializer)
        {
            _repositoryService = repositories.GetRepositoryService<T>();
        }

        public virtual async Task<List<T>> GetChangeRequestCommentsAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0)
        {
            return await _repositoryService.GetAsync(whereCondition, topCount);
        }
    }
}

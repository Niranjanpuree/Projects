using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using System.Linq.Expressions;
using System;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices
{
    public abstract class BusinessServiceBase<T> : IBusinessService<T>, IChangeRequestReviewEventHandler
        where T : class
    {
        protected readonly IUnitOfWork Repositories;
        protected readonly ITextSerializer Serializer;

        private readonly IRepositoryService<T> _repositoryService = null;

        protected BusinessServiceBase(IUnitOfWork repositories,
            ITextSerializer serializer)
        {
            Repositories = repositories;
            Serializer = serializer;
            _repositoryService = repositories.GetRepositoryService<T>();
        }

        public virtual async Task<List<T>> GetAllAsync(int topCount = 0)
        {
            return await _repositoryService.GetAllAsync(topCount);
        }

        public virtual async Task<T> GetAsync<TKey>(TKey id)
        {
            return await _repositoryService.FindAsync(id);
        }

        public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0)
        {
            return await _repositoryService.GetAsync(whereCondition, topCount);
        }

        public async Task<bool> AddAsync(T entity)
        {
            _repositoryService.Add(entity);
            return await Repositories.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddAllAsync(List<T> entities)
        {
            _repositoryService.AddAll(entities);
            return await Repositories.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync<TId>(T entity)
        {
            _repositoryService.Update(entity);
            return await Repositories.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync<TKey>(TKey id)
        {
            await _repositoryService.RemoveAsync(id);
            return await Repositories.SaveChangesAsync() > 0;
        }

        public async Task<int> GetCountAsync<TElement>(T entity,
            Expression<Func<T, ICollection<TElement>>> collectionProperty)
            where TElement : class
        {
            return await _repositoryService.GetCountAsync(entity, collectionProperty);
        }

        public abstract Task<ChangeRequestStagingModel<T>> GetChangeRequestStaging<TId>(TId changeRequestId);
        public abstract Task<List<T>> GetPendingAddChangeRequests(Expression<Func<T, bool>> whereCondition = null, int topCount = 0);
        public abstract Task<bool> SubmitChangeRequestReviewAsync<TId>(TId changeRequestId, ChangeRequestReviewModel review);
        public abstract Task<List<ChangeEntityFacet>> GetAllChangeTypes();
        public abstract Task<long> ChangeRequestExist<TId>(T entity, TId id);

        public abstract bool Handle(ChangeRequestApprovedEvent changeRequestApprovedEvent);
        public abstract bool Handle(ChangeRequestRejectedEvent changeRequestRejectedEvent);
        public abstract bool Handle(ChangeRequestPrelimApprovedEvent changeRequestPrelimApprovedEvent);

        public abstract Task<bool> SubmitLikeAsync(long crId, string likedBy, string likedStatus);

        public abstract AssociationCount AssociatedCount(string payload);

        public abstract Task ClearChangeRequestId(long changeRequestId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Application.BusinessServices
{
    public abstract class ChangeRequestStagingBase<T> : IChangeRequestStagingBase<T>
        where T: class
    {
        private readonly IUnitOfWork _repositories;
        private readonly IRepositoryService<T> _repositoryService;

        protected ChangeRequestStagingBase(IUnitOfWork repositories, ITextSerializer serializer)
        {
            _repositories = repositories;
            _repositoryService = repositories.GetRepositoryService<T>();
        }

        public virtual async Task<List<T>> GetAllAsync(int topCount = 0)
        {
            return await this._repositoryService.GetAllAsync(topCount);
        }

        public virtual async Task<T> GetAsync<TKey>(TKey id)
        {
            return await this._repositoryService.FindAsync(id);
        }

        public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0)
        {
            return await this._repositoryService.GetAsync(whereCondition, topCount);
        }

        public virtual void Add(T entity)
        {
            this._repositoryService.Add(entity);
        }

        public virtual void Update<TId>(T entity)
        {
            this._repositoryService.Update(entity);
        }

        public virtual async Task<T> RemoveAsync<TKey>(TKey id)
        {
            return await this._repositoryService.RemoveAsync(id);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await this._repositories.SaveChangesAsync();
        }
    }
}

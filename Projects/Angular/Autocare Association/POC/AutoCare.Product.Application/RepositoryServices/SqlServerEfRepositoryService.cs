using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Application.RepositoryServices
{
    public abstract class SqlServerEfRepositoryService<TEntity> : IRepositoryService<TEntity>
        where TEntity : class
    {
        protected readonly DbContext Context;
        private readonly ITextSerializer _serializer;

        protected SqlServerEfRepositoryService(DbContext context, ITextSerializer serializer)
        {
            Context = context;
            _serializer = serializer;
        }

        public async Task<List<TEntity>> GetAllAsync(int topCount = 0)
        {
            if (topCount == 0)
            {
                return await Context.Set<TEntity>().ToListAsync();
            }

            return await Context.Set<TEntity>()
                                .Take(topCount)
                                .ToListAsync();
        }

        public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await Context.Set<TEntity>()
                                .Where(whereCondition)
                                .ToListAsync();
        }

        public Task<TEntity> FindAsync<TId>(TId id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddAll(List<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<TEntity>> RemoveAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            var itemsToBeRemoved = await Context.Set<TEntity>()
                                        .Where(whereCondition)
                                        .ToListAsync();

            return Context.Set<TEntity>().RemoveRange(itemsToBeRemoved);
        }

        public async Task<TEntity> RemoveAsync<TKey>(TKey id)
        {
            var itemToBeRemoved = await Context.Set<TEntity>().FindAsync(id);

            return Context.Set<TEntity>().Remove(itemToBeRemoved);
        }

        //public async Task<ChangeRequestStaging> BuildChangeRequestStagingAsync(TEntity entity, string requestedBy)
        //{
        //    return await BuildChangeRequestStaging(entity, requestedBy);
        //}

        //public Task<ChangeRequestStaging> BuildChangeRequestStaging(TEntity entity, string requestedBy)
        //{
        //    return Task.Run(() =>
        //    {
        //        var changeRequestStagingList = new List<ChangeRequestStaging>();

        //        var enSet = Context.Set<TEntity>();
        //        enSet.AddOrUpdate(entity);

        //        var entityColumns = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || 
        //        e.State == EntityState.Modified || e.State == EntityState.Deleted);

        //        bool changeOccurred = false;
        //        ChangeRequestStaging changeRequestStaging = null;
        //        foreach (var column in entityColumns)
        //        {
        //            if (changeRequestStaging == null)
        //            {
        //                changeRequestStaging = new ChangeRequestStaging()
        //                {
        //                    ChangeType = column.State.ToChangeType(),
        //                    Entity = entity.GetType().Name,
        //                    CreatedDateTime = DateTime.UtcNow,
        //                    RequestedBy = requestedBy
        //                };
        //            }

        //            foreach (var propertyName in column.CurrentValues.PropertyNames)
        //            {
        //                var newValue = column.CurrentValues[propertyName]?.ToString().Trim();
        //                var oldValue = column.OriginalValues[propertyName]?.ToString().Trim();

        //                if (!String.Equals(newValue, oldValue, StringComparison.CurrentCultureIgnoreCase))
        //                {

        //                    changeOccurred = true;
        //                    break;
        //                }
        //            }

        //            if (changeOccurred)
        //            {
        //                break;
        //            }
        //        }

        //        if (changeOccurred)
        //        {
        //            changeRequestStaging.Payload = _serializer.Serialize(entity);
        //            return changeRequestStaging;
        //        }

        //        return null;
        //    });
        //}
    }
}

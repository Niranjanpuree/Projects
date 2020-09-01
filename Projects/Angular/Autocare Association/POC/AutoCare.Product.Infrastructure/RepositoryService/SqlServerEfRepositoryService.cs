using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Infrastructure.RepositoryService
{
    public abstract class SqlServerEfRepositoryService<TEntity> : ISqlServerEfRepositoryService<TEntity>
        where TEntity : class
    {
        private readonly DbContext _context;
        private readonly ITextSerializer _serializer;

        protected SqlServerEfRepositoryService(DbContext context, ITextSerializer serializer)
        {
            _context = context;
            _serializer = serializer;
        }

        public async Task<List<TEntity>> GetAllAsync(int topCount = 0)
        {
            if (topCount == 0)
            {
                return await _context.Set<TEntity>().ToListAsync();
            }

            return await _context.Set<TEntity>()
                                .Take(topCount)
                                .ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> whereCondition, int topCount = 0,
            params string[] includeProperties)
        {
            var entityDbSet = _context.Set<TEntity>()
                                .Where(whereCondition);
            topCount = topCount == 0 ? 100000 : topCount;
            //var query = _context.Set<TEntity>().Where(whereCondition)as IQueryable; //for checking what SQL query is generated
            entityDbSet = entityDbSet.Take(topCount);
            entityDbSet = entityDbSet.Select(x => x);
            includeProperties?.ToList().ForEach(property => entityDbSet = entityDbSet.Include(property));

            return await entityDbSet.ToListAsync();
        }

        public Task<TEntity> FindAsync<TId>(TId id)
        {
            return _context.Set<TEntity>().FindAsync(id);
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddAll(List<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<TEntity>> RemoveAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            var itemsToBeRemoved = await _context.Set<TEntity>()
                                        .Where(whereCondition)
                                        .ToListAsync();

            return _context.Set<TEntity>().RemoveRange(itemsToBeRemoved);
        }

        public async Task<TEntity> RemoveAsync<TKey>(TKey id)
        {
            var itemToBeRemoved = await _context.Set<TEntity>().FindAsync(id);

            return _context.Set<TEntity>().Remove(itemToBeRemoved);
        }

        public async Task<int> GetCountAsync<TElement>(TEntity entity,
            Expression<Func<TEntity, ICollection<TElement>>> collectionProperty, Expression<Func<TElement, bool>> whereExpression = null) where TElement : class
        {
            try
            {
                if (whereExpression == null)
                {
                    return await _context.Entry(entity)
                        .Collection(collectionProperty)
                        .Query()
                        .CountAsync();
                }
                else
                {
                    return
                        await
                            _context.Entry(entity)
                                .Collection(collectionProperty)
                                .Query()
                                .Where(whereExpression)
                                .CountAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int GetCountAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            var predicate = whereExpression.Compile();
            return _context.Set<TEntity>().Where(predicate).Count();
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            IQueryable<TEntity> entity = _context.Set<TEntity>();
            return entity;
        }

        public int GetMax(Expression<Func<TEntity, int>> propertyExpression)
        {
            if (_context.Set<TEntity>().Any())
                return _context.Set<TEntity>().Max(propertyExpression);
            else return 0;
        }

        public int GetMax(PropertyInfo propertyInfo)
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            var body = Expression.Property(parameter, propertyInfo.Name);
            var lambda = Expression.Lambda<Func<TEntity, int>>(body, parameter);
            var result = _context.Set<TEntity>().Max(lambda);
            return result;
        }

        public TEntity RemoveAsync(TEntity entity)
        {
            var entityFromContext =_context.Set<TEntity>().Attach(entity);

            return _context.Set<TEntity>().Remove(entity);
        }
    }
}

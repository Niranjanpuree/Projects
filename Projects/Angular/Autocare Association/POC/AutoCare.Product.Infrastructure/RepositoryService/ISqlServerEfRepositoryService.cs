using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoCare.Product.Infrastructure.RepositoryService
{
    public interface ISqlServerEfRepositoryService<TEntity> : IRepositoryService<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAllQueryable();
        int GetMax(Expression<Func<TEntity, int>> predicateExpression);
        int GetMax(PropertyInfo propertyInfo);
        int GetCountAsync(Expression<Func<TEntity, bool>> whereExpression);
    }
}

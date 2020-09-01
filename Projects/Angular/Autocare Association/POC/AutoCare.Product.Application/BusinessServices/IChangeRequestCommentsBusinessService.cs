using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace AutoCare.Product.Application.BusinessServices
{
    public interface IChangeRequestCommentsBusinessService<T> : IChangeRequestStagingBase<T>
        where T: class 
    {
        Task<List<T>> GetChangeRequestCommentsAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0);
    }
}

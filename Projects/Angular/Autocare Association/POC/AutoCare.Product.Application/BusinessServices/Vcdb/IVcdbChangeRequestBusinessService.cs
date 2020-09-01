using AutoCare.Product.Vcdb.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IVcdbChangeRequestBusinessService : IChangeRequestBusinessService<ChangeRequestStaging, ChangeRequestItemStaging, CommentsStaging, AttachmentsStaging>
    {
        Task<List<ChangeRequestItem>> GetChangeRequestItemAsync(Expression<Func<ChangeRequestItem, bool>> whereCondition, int topCount = 0);
    }
}

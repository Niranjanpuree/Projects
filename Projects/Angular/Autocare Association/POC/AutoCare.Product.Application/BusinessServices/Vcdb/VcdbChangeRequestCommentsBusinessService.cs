using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VcdbChangeRequestCommentsBusinessService: ChangeRequestCommentsBusinessService<CommentsStaging>, IVcdbChangeRequestCommentsBusinessService
    {
        private readonly ISqlServerEfRepositoryService<CommentsStaging> _commentsStagingRepositoryService;
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;

        public VcdbChangeRequestCommentsBusinessService(IVcdbUnitOfWork vcdbUnitOfWork, ITextSerializer serializer)
            : base (vcdbUnitOfWork, serializer)
        {
            _vcdbUnitOfWork = vcdbUnitOfWork;
            _commentsStagingRepositoryService = vcdbUnitOfWork.GetRepositoryService<CommentsStaging>() 
                as ISqlServerEfRepositoryService<CommentsStaging>;
        }

        public override async Task<List<CommentsStaging>> GetChangeRequestCommentsAsync(Expression<Func<CommentsStaging, bool>> whereCondition, int topCount = 0)
        {
            return await this._commentsStagingRepositoryService.GetAsync(whereCondition, topCount);
        }

        public override void Add(CommentsStaging entity)
        {
            this._commentsStagingRepositoryService.Add(entity);
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await this._vcdbUnitOfWork.SaveChangesAsync();
        }
    }
}

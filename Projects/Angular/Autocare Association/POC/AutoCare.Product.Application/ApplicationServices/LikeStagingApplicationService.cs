using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class LikeStagingApplicationService : VcdbApplicationService<LikesStaging>, ILikeStagingApplicationService
    {
        private readonly ILikeStagingBusinessService _likeStagingBusinessService;
        public LikeStagingApplicationService(ILikeStagingBusinessService likeStagingBusinessService)
            : base(likeStagingBusinessService)
        {
            _likeStagingBusinessService = likeStagingBusinessService;
        }

        public Task<LikesModel> GetLikesByChangeRequestId(long changeRequestId,string currentUser)
        {
           return _likeStagingBusinessService.GetLikesByChangeRequestId(changeRequestId, currentUser);
        }

        public  Task<IList<LikesModel>> GetAllLikedBy(long changeRequestId)
        {
            return _likeStagingBusinessService.GetAllLikedBy(changeRequestId);
        }
    }
}

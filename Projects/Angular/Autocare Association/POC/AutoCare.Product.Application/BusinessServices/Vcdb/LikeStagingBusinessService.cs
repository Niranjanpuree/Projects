using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;


namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class LikeStagingBusinessService : VcdbBusinessService<LikesStaging>, ILikeStagingBusinessService
    {
        private readonly ISqlServerEfRepositoryService<LikesStaging> _likeStagingRepositoryService;
        private readonly ISqlServerEfRepositoryService<Likes> _likeRepositoryService;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;

        public LikeStagingBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer
            )
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _likeStagingRepositoryService = Repositories.GetRepositoryService<LikesStaging>() as ISqlServerEfRepositoryService<LikesStaging>;
            _likeRepositoryService = Repositories.GetRepositoryService<Likes>() as ISqlServerEfRepositoryService<Likes>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override async Task<bool> SubmitLikeAsync(long crId, String likedBy, string likedStatus)
        {
            //validation
            if (string.IsNullOrWhiteSpace(likedBy))
            {
                throw new ArgumentNullException(nameof(likedBy));
            }
            if (string.IsNullOrWhiteSpace(likedStatus))
            {
                throw new ArgumentNullException(nameof(likedStatus));
            }
            byte likeStatus = (byte)(LikeStatusType)Enum.Parse(typeof(LikeStatusType), likedStatus);
            IList<LikesStaging> likesFromDb = await _likeStagingRepositoryService.GetAsync(x => x.ChangeRequestId == crId &&
             x.LikedBy == likedBy &&
             x.LikeStatus == likeStatus);
            if (likesFromDb != null && likesFromDb.Any())
            {
                throw new RecordAlreadyExist("Change Request: " + crId + " is already liked by " + likedBy);
            }
            return await base.SubmitLikeAsync(crId, likedBy, likedStatus);
        }

        public async Task<LikesModel>  GetLikesByChangeRequestId(long changeRequestId, string currentUser)
        {
            var count = _likeStagingRepositoryService.GetCountAsync(x => x.ChangeRequestId == changeRequestId);

            LikesModel likesModel = new LikesModel();
            if (count>0)
            {
                   List<LikesStaging> likeStagings =
               await _likeStagingRepositoryService.GetAsync(
                    m => m.LikedBy.ToLower() == currentUser && m.ChangeRequestId == changeRequestId);
                var firstOrDefault = likeStagings.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    likesModel.LikeStatus = (LikeStatusType)firstOrDefault.LikeStatus;
                    likesModel.LikedBy = firstOrDefault.LikedBy;
                }
                likesModel.LikeCount = count;
            }
            else
            {
                List<Likes> likes = await _likeRepositoryService.GetAsync(
                     m => m.LikedBy.ToLower() == currentUser && m.ChangeRequestId == changeRequestId);
                Likes firstOrDefault = likes.FirstOrDefault();
                var countStore= _likeRepositoryService.GetCountAsync(x => x.ChangeRequestId == changeRequestId);
                if (firstOrDefault != null)
                {
                    likesModel.LikeStatus = (LikeStatusType)firstOrDefault.LikeStatus;
                    likesModel.LikedBy = firstOrDefault.LikedBy;
                }
                likesModel.LikeCount = countStore;
            }
            return likesModel;
        }

        public async Task<IList<LikesModel>> GetAllLikedBy(long changeRequestId)
        {
            IList<LikesStaging> allLikesStagings = await _likeStagingRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId);
            IList<LikesModel> allLikesModel=new List<LikesModel>();
            if (allLikesStagings.Count > 0)
            {
                foreach (var likeModel in allLikesStagings.Select(likesStaging => new LikesModel
                {
                    LikedBy = likesStaging.LikedBy,
                    CreatedDatetime = likesStaging.CreatedDatetime
                }))
                {
                    allLikesModel.Add(likeModel);
                }
            }
            else
            {
                IList<Likes> allLikes = await _likeRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId);
                foreach (var likeModel in allLikes.Select(like => new LikesModel
                {
                    LikedBy = like.LikedBy,
                    CreatedDatetime = like.CreatedDatetime
                }))
                {
                    allLikesModel.Add(likeModel);
                }
            }
            return allLikesModel;
        }


    }
}

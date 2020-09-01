using System;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoMapper;


namespace AutoCare.Product.Web.Controllers.Api
{
    [RoutePrefix("likeStaging")]
    public class LikesController : ApiControllerBase
    {
        private readonly ILikeStagingApplicationService _likeStagingApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public LikesController(ILikeStagingApplicationService likeStagingApplicationService, IApplicationLogger logger)
        {
            _likeStagingApplicationService = likeStagingApplicationService;
            _applicationLogger = logger;
        }

        [Route("{changeRequestId:int}")]
        [HttpPost()]
        public async Task<IHttpActionResult> Post(int changeRequestId, LikeStagingInputModel like)
        {
            try
            {
                like.LikedBy = CurrentUser.CustomerId;
                bool isSubmitted =
                    await
                        this._likeStagingApplicationService.SubmitLikeAsync(changeRequestId, like.LikedBy,
                            like.LikeStatus);
                return Ok(isSubmitted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int changeRequestId)
        {
            LikesModel likesModel = await _likeStagingApplicationService.GetLikesByChangeRequestId(changeRequestId, CurrentUser.CustomerId);
            LikesViewModel likesViewModel = Mapper.Map<LikesViewModel>(likesModel);
            return Ok(likesViewModel);
        }

        [Route("allLikedBy/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetAllLikedBy(int changeRequestId)
        {
            IList<LikesModel>  likesModel = await _likeStagingApplicationService.GetAllLikedBy(changeRequestId);
           IList<LikesViewModel>  likesViewModel = Mapper.Map<IList<LikesViewModel>>(likesModel);
            return Ok(likesViewModel);
        }
    }
}

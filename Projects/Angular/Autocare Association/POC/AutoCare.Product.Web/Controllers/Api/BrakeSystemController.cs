using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("brakeSystems")]
    public class BrakeSystemController : ApiControllerBase
    {
        private readonly IBrakeSystemApplicationService _brakeSystemApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public BrakeSystemController(IBrakeSystemApplicationService brakeSystemApplicationService, IApplicationLogger applicationLogger)
        {
            _brakeSystemApplicationService = brakeSystemApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var brakeSystems = await _brakeSystemApplicationService.GetAllAsync();
            IEnumerable<BrakeSystemViewModel> brakeSystemList = Mapper.Map<IEnumerable<BrakeSystemViewModel>>(brakeSystems);

            return Ok(brakeSystemList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedBrakeSystem = await _brakeSystemApplicationService.GetAsync(id);

            BrakeSystemDetailViewModel brakeSystem = Mapper.Map<BrakeSystemDetailViewModel>(selectedBrakeSystem);

            return Ok(brakeSystem);
        }

        [Route("search/{brakeSystemNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string brakeSystemNameFilter)
        {
            if (string.IsNullOrWhiteSpace(brakeSystemNameFilter))
            {
                return await this.Get();
            }

            List<BrakeSystem> brakeSystems = await _brakeSystemApplicationService.GetAsync(m => m.Name.ToLower().Contains(brakeSystemNameFilter.ToLower()));

            IEnumerable<BrakeSystemViewModel> brakeSystemList = Mapper.Map<IEnumerable<BrakeSystemViewModel>>(brakeSystems);

            return Ok(brakeSystemList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string brakeSystemNameFilter)
        {
            if (string.IsNullOrWhiteSpace(brakeSystemNameFilter))
            {
                return await this.Get();
            }

            List<BrakeSystem> brakeSystems = await _brakeSystemApplicationService.GetAsync(m => m.Name.ToLower().Contains(brakeSystemNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<BrakeSystemViewModel> brakeSystemList = Mapper.Map<IEnumerable<BrakeSystemViewModel>>(brakeSystems);

            return Ok(brakeSystemList);
        }

        [Route("frontBrakeType/{frontBrakeTypeId:int}/rearBrakeType/{rearBrakeTypeId:int}/brakeABS/{brakeABSId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByFrontBrakeTypeIdRearBrakeTypeIdBrakeABSId(int frontBrakeTypeId, int rearBrakeTypeId, int brakeABSId)
        {
            List<BrakeSystem> brakeSystems = null;
            brakeSystems = await _brakeSystemApplicationService.GetAsync(item => item.BrakeConfigs.Any(bc =>
                bc.FrontBrakeTypeId == frontBrakeTypeId && bc.RearBrakeTypeId == rearBrakeTypeId && bc.BrakeABSId == brakeABSId));
            IEnumerable<BrakeSystemViewModel> brakeSystemList = Mapper.Map<IEnumerable<BrakeSystemViewModel>>(brakeSystems);
            return Ok(brakeSystemList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BrakeSystemInputModel model)
        {

            // create make and list of comments
            BrakeSystem brakeSystem = new BrakeSystem() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeSystemApplicationService.AddAsync(brakeSystem, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BrakeSystemInputModel model)
        {
            BrakeSystem brakeSystem = new BrakeSystem()
            {
                Id = model.Id,
                Name = model.Name,
                BrakeConfigCount = model.BrakeConfigCount,
                VehicleToBrakeConfigCount = model.VehicleToBrakeConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeSystemApplicationService.UpdateAsync(brakeSystem, brakeSystem.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BrakeSystemInputModel model)
        {
            BrakeSystem brakeSystem = new BrakeSystem() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeSystemApplicationService.DeleteAsync(brakeSystem, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<BrakeSystem> changeRequestStagingBrakeSystemModel =
                await this._brakeSystemApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingBrakeSystemViewModel changeRequestStagingBrakeSystemViewModel = Mapper.Map<ChangeRequestStagingBrakeSystemViewModel>(changeRequestStagingBrakeSystemModel);

            SetUpChangeRequestReview(changeRequestStagingBrakeSystemViewModel.StagingItem.Status,
                changeRequestStagingBrakeSystemViewModel.StagingItem.SubmittedBy, changeRequestStagingBrakeSystemViewModel);
                  return Ok(changeRequestStagingBrakeSystemViewModel);
        }
        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpPost()]
        public async Task<IHttpActionResult> Post(int changeRequestId, ChangeRequestReviewInputModel changeRequestReview)
        {
            // create change-request-review-model
            ChangeRequestReviewModel reviewModel = new ChangeRequestReviewModel()
            {
                ChangeRequestId = changeRequestReview.ChangeRequestId,
                ReviewedBy = CurrentUser.CustomerId,
                ReviewStatus = changeRequestReview.ReviewStatus,
                ReviewComment = new CommentsStagingModel()
                {
                    Comment = changeRequestReview.ReviewComment.Comment,
                    CreatedDatetime = changeRequestReview.ReviewComment.CreatedDatetime,
                    AddedBy = CurrentUser.CustomerId
                },
                //NOTE: CR Input model uses IList unlike other models hence the addition of .ToList()
                Attachments = SetUpAttachmentsModels(changeRequestReview.Attachments?.ToList())

            };

            // submit review
            bool isSubmitted = await
                this._brakeSystemApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }

    }
}
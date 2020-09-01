using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("brakeTypes")]
    public class BrakeTypeController : ApiControllerBase
    {
        private readonly IBrakeTypeApplicationService _brakeTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public BrakeTypeController(IBrakeTypeApplicationService brakeTypeApplicationService, IApplicationLogger applicationLogger)
        {
            _brakeTypeApplicationService = brakeTypeApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var brakeTypes = await _brakeTypeApplicationService.GetAllAsync();
            IEnumerable<BrakeTypeViewModel> brakeTypeList = Mapper.Map<IEnumerable<BrakeTypeViewModel>>(brakeTypes);

            return Ok(brakeTypeList);
        }

        // GET api/brakeTypes/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedBrakeType = await _brakeTypeApplicationService.GetAsync(id);

            BrakeTypeDetailViewModel brakeType = Mapper.Map<BrakeTypeDetailViewModel>(selectedBrakeType);

            return Ok(brakeType);
        }

        [Route("search/{brakeTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string brakeTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(brakeTypeNameFilter))
            {
                return await this.Get();
            }

            List<BrakeType> brakeTypes = await _brakeTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(brakeTypeNameFilter.ToLower()));

            IEnumerable<BrakeTypeViewModel> brakeTypeList = Mapper.Map<IEnumerable<BrakeTypeViewModel>>(brakeTypes);

            return Ok(brakeTypeList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string brakeTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(brakeTypeNameFilter))
            {
                return await this.Get();
            }

            List<BrakeType> brakeTypes = await _brakeTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(brakeTypeNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<BrakeTypeViewModel> brakeTypeList = Mapper.Map<IEnumerable<BrakeTypeViewModel>>(brakeTypes);

            return Ok(brakeTypeList);
        }

        [Route("frontBrakeType/{frontBrakeTypeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByFrontBrakeTypeId(int frontBrakeTypeId)
        {
            List<BrakeType> brakeTypes = null;
            brakeTypes = await _brakeTypeApplicationService.GetAsync(item => item.BrakeConfigs_RearBrakeTypeId.Any(bc =>
                bc.FrontBrakeTypeId == frontBrakeTypeId));
            IEnumerable<BrakeTypeViewModel> brakeTypeList = Mapper.Map<IEnumerable<BrakeTypeViewModel>>(brakeTypes);
            return Ok(brakeTypeList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BrakeTypeInputModel model)
        {
            // create brake type and list of comments
            BrakeType brakeType = new BrakeType() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeTypeApplicationService.AddAsync(brakeType, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BrakeTypeInputModel model)
        {
            BrakeType brakeType = new BrakeType() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeTypeApplicationService.DeleteAsync(brakeType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BrakeTypeInputModel brakeTypeInputModel)
        {
            BrakeType brakeType = new BrakeType()
            {
                Id = brakeTypeInputModel.Id,
                Name = brakeTypeInputModel.Name,
                FrontBrakeConfigCount = brakeTypeInputModel.FrontBrakeConfigCount,
                RearBrakeConfigCount = brakeTypeInputModel.RearBrakeConfigCount,
                VehicleToBrakeConfigCount = brakeTypeInputModel.VehicleToBrakeConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = brakeTypeInputModel.Comment };
            var attachments = SetUpAttachmentsModels(brakeTypeInputModel.Attachments);
            var changeRequestId = await _brakeTypeApplicationService.UpdateAsync(brakeType, brakeType.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<BrakeType> changeRequestStagingBrakeTypeModel =
                await this._brakeTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingBrakeTypeViewModel changeRequestStagingBrakeTypeViewModel = Mapper.Map<ChangeRequestStagingBrakeTypeViewModel>(changeRequestStagingBrakeTypeModel);

            SetUpChangeRequestReview(changeRequestStagingBrakeTypeViewModel.StagingItem.Status,
                changeRequestStagingBrakeTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingBrakeTypeViewModel);
            

            return Ok(changeRequestStagingBrakeTypeViewModel);
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
                    AddedBy = CurrentUser.CustomerId//changeRequestReview.ReviewComment.AddedBy
                },
                //NOTE: CR Input model uses IList unlike other models hence the addition of .ToList()
                Attachments = SetUpAttachmentsModels(changeRequestReview.Attachments?.ToList())
            };

            // submit review
            bool isSubmitted = await
                this._brakeTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
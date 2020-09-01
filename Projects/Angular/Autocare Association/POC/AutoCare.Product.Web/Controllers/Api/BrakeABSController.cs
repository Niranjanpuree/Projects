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
    [RoutePrefix("brakeABSes")]
    [Authorize]
    public class BrakeABSController : ApiControllerBase
    {
        private readonly IBrakeABSApplicationService _brakeABSApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public BrakeABSController(IBrakeABSApplicationService brakeABSApplicationService, IApplicationLogger applicationLogger)
        {
            _brakeABSApplicationService = brakeABSApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var brakeABSes = await _brakeABSApplicationService.GetAllAsync();
            IEnumerable<BrakeABSViewModel> brakeABSList = Mapper.Map<IEnumerable<BrakeABSViewModel>>(brakeABSes);

            return Ok(brakeABSList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedBrakeABS = await _brakeABSApplicationService.GetAsync(id);

            BrakeABSDetailViewModel brakeABS = Mapper.Map<BrakeABSDetailViewModel>(selectedBrakeABS);

            return Ok(brakeABS);
        }

        [Route("search/{brakeABSNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string brakeABSNameFilter)
        {
            if (string.IsNullOrWhiteSpace(brakeABSNameFilter))
            {
                return await this.Get();
            }

            List<BrakeABS> brakeABSs = await _brakeABSApplicationService.GetAsync(m => m.Name.ToLower().Contains(brakeABSNameFilter.ToLower()));

            IEnumerable<BrakeABSViewModel> brakeABSList = Mapper.Map<IEnumerable<BrakeABSViewModel>>(brakeABSs);

            return Ok(brakeABSList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string brakeABSNameFilter)
        {
            if (string.IsNullOrWhiteSpace(brakeABSNameFilter))
            {
                return await this.Get();
            }

            List<BrakeABS> brakeABSs = await _brakeABSApplicationService.GetAsync(m => m.Name.ToLower().Contains(brakeABSNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<BrakeABSViewModel> brakeABSList = Mapper.Map<IEnumerable<BrakeABSViewModel>>(brakeABSs);

            return Ok(brakeABSList);
        }

        [Route("frontBrakeType/{frontBrakeTypeId:int}/rearBrakeType/{rearBrakeTypeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByFrontBrakeTypeIdRearBrakeTypeId(int frontBrakeTypeId, int rearBrakeTypeId)
        {
            List<BrakeABS> brakeABSes = null;
            brakeABSes = await _brakeABSApplicationService.GetAsync(item => item.BrakeConfigs.Any(bc =>
                bc.FrontBrakeTypeId == frontBrakeTypeId && bc.RearBrakeTypeId == rearBrakeTypeId));
            IEnumerable<BrakeABSViewModel> brakeABSesList = Mapper.Map<IEnumerable<BrakeABSViewModel>>(brakeABSes);
            return Ok(brakeABSesList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BrakeABSInputModel model)
        {
            // create make and list of comments
            BrakeABS brakeABS = new BrakeABS() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);

            var changeRequestId = await _brakeABSApplicationService.AddAsync(brakeABS, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BrakeABSInputModel model)
        {
            BrakeABS brakeABS = new BrakeABS()
            {
                Id = model.Id,
                Name = model.Name,
                BrakeConfigCount = model.BrakeConfigCount,
                VehicleToBrakeConfigCount = model.VehicleToBrakeConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeABSApplicationService.UpdateAsync(brakeABS, brakeABS.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BrakeABSInputModel model)
        {
            BrakeABS brakeABS = new BrakeABS() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeABSApplicationService.DeleteAsync(brakeABS, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        
        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<BrakeABS> changeRequestStagingBrakeABSModel =
                await this._brakeABSApplicationService.GetChangeRequestStaging(changeRequestId);
           ChangeRequestStagingBrakeABSViewModel  changeRequestStagingBrakeABSViewModel = Mapper.Map<ChangeRequestStagingBrakeABSViewModel>(changeRequestStagingBrakeABSModel);

             SetUpChangeRequestReview(changeRequestStagingBrakeABSViewModel.StagingItem.Status,
                changeRequestStagingBrakeABSViewModel.StagingItem.SubmittedBy, changeRequestStagingBrakeABSViewModel);
                return Ok(changeRequestStagingBrakeABSViewModel);
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
                this._brakeABSApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
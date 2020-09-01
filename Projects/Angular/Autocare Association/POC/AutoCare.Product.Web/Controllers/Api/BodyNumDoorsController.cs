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
    [RoutePrefix("bodyNumDoors")]
    [Authorize]
    public class BodyNumDoorsController : ApiControllerBase
    {
        private readonly IBodyNumDoorsApplicationService _bodyNumDoorsApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public BodyNumDoorsController(IBodyNumDoorsApplicationService bodyNumDoorsApplicationService, IApplicationLogger applicationLogger)
        {
            _bodyNumDoorsApplicationService = bodyNumDoorsApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var bodyNumDoors = await _bodyNumDoorsApplicationService.GetAllAsync();
            IEnumerable<BodyNumDoorsViewModel> bodyNumDoorsList = Mapper.Map<IEnumerable<BodyNumDoorsViewModel>>(bodyNumDoors);

            return Ok(bodyNumDoorsList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedbodyNumDoorsList = await _bodyNumDoorsApplicationService.GetAsync(id);

            BodyNumDoorsDetailViewModel bodyNumDoors = Mapper.Map<BodyNumDoorsDetailViewModel>(selectedbodyNumDoorsList);

            return Ok(bodyNumDoors);
        }

        [Route("search/{bodyNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string bodyNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bodyNameFilter))
            {
                return await this.Get();
            }

            List<BodyNumDoors> bodyNumDoors = await _bodyNumDoorsApplicationService.GetAsync(m => m.NumDoors.ToLower().Contains(bodyNameFilter.ToLower()));

            IEnumerable<BodyNumDoorsViewModel> bodyNumDoorsList = Mapper.Map<IEnumerable<BodyNumDoorsViewModel>>(bodyNumDoors);

            return Ok(bodyNumDoorsList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string bodyNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bodyNameFilter))
            {
                return await this.Get();
            }

            List<BodyNumDoors> bodyNumDoors = await _bodyNumDoorsApplicationService.GetAsync(m => m.NumDoors.ToLower().Contains(bodyNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<BodyNumDoorsViewModel> bodyNumDoorsList = Mapper.Map<IEnumerable<BodyNumDoorsViewModel>>(bodyNumDoors);

            return Ok(bodyNumDoorsList);
        }

        [Route("bodyNumDoors/{bodyNumDoorsId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByNumDoorsID(int bodyNumDoorsId)
        {
            List<BodyNumDoors> bodyNumDoors = null;
            bodyNumDoors = await _bodyNumDoorsApplicationService.GetAsync(item => item.BodyStyleConfigs.Any(bc =>
                bc.BodyNumDoorsId == bodyNumDoorsId));
            IEnumerable<BodyNumDoorsViewModel> bodyNumDoorsList = Mapper.Map<IEnumerable<BodyNumDoorsViewModel>>(bodyNumDoors);
            return Ok(bodyNumDoorsList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BodyNumDoorsInputModel model)
        {
            // create make and list of comments
            BodyNumDoors bodyNumDoors = new BodyNumDoors() { Id = model.Id, NumDoors = model.NumDoors };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);

            var changeRequestId = await _bodyNumDoorsApplicationService.AddAsync(bodyNumDoors, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BodyNumDoorsInputModel model)
        {
            BodyNumDoors bodyNumDoors = new BodyNumDoors()
            {
                Id = model.Id,
                NumDoors = model.NumDoors,
                BodyStyleConfigCount = model.BodyStyleConfigCount,
                VehicleToBodyStyleConfigCount = model.VehicleToBodyStyleConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyNumDoorsApplicationService.UpdateAsync(bodyNumDoors, bodyNumDoors.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BodyNumDoorsInputModel model)
        {
            BodyNumDoors bodyNumDoors = new BodyNumDoors() { Id = model.Id, NumDoors = model.NumDoors };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyNumDoorsApplicationService.DeleteAsync(bodyNumDoors, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }


        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<BodyNumDoors> changeRequestStagingBodyNumDoorsModel =
                await this._bodyNumDoorsApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingBodyNumDoorsViewModel changeRequestStagingBodyNumDoorsViewModel = Mapper.Map<ChangeRequestStagingBodyNumDoorsViewModel>(changeRequestStagingBodyNumDoorsModel);

            SetUpChangeRequestReview(changeRequestStagingBodyNumDoorsViewModel.StagingItem.Status,
                changeRequestStagingBodyNumDoorsViewModel.StagingItem.SubmittedBy, changeRequestStagingBodyNumDoorsViewModel);
                return Ok(changeRequestStagingBodyNumDoorsViewModel);
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
                this._bodyNumDoorsApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
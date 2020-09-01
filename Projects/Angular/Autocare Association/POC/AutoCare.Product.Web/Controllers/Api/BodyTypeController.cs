using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.ViewModels;
using AutoCare.Product.Web.Models.InputModels;
using AutoMapper;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("bodyTypes")]
    public class BodyTypeController : ApiControllerBase
    {
        private readonly IBodyTypeApplicationService _bodyTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public BodyTypeController(IApplicationLogger applicationLogger, IBodyTypeApplicationService bodyTypeApplicationService)
        {
            _applicationLogger = applicationLogger;
            _bodyTypeApplicationService = bodyTypeApplicationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var bodyTypes = await _bodyTypeApplicationService.GetAllAsync();
            IEnumerable<BodyTypeViewModel> bodyTypeList = Mapper.Map<IEnumerable<BodyTypeViewModel>>(bodyTypes);

            return Ok(bodyTypeList);
        }

        // GET api/bodyType/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedBodyType = await _bodyTypeApplicationService.GetAsync(id);
            BodyTypeDetailViewModel bodyType = Mapper.Map<BodyTypeDetailViewModel>(selectedBodyType);

            return Ok(bodyType);
        }


        [Route("search/{bodyTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string bodyTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bodyTypeNameFilter))
            {
                return await this.Get();
            }

            List<BodyType> bodyTypes = await _bodyTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(bodyTypeNameFilter.ToLower()));

            IEnumerable<BodyTypeViewModel> bodyTypeList = Mapper.Map<IEnumerable<BodyTypeViewModel>>(bodyTypes);

            return Ok(bodyTypeList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string bodyTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bodyTypeNameFilter))
            {
                return await this.Get();
            }

            List<BodyType> bodyTypes = await _bodyTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(bodyTypeNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<BodyTypeViewModel> bodyTypeList = Mapper.Map<IEnumerable<BodyTypeViewModel>>(bodyTypes);

            return Ok(bodyTypeList);
        }

        [Route("bodyNumDoors/{bodyNumDoorsId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBodyNumDoorsTypeId(int bodyNumDoorsId)
        {
            List<BodyType> bodyTypes = null;
            bodyTypes = await _bodyTypeApplicationService.GetAsync(item => item.BodyStyleConfigs.Any(bc =>
                bc.BodyNumDoorsId == bodyNumDoorsId));
            IEnumerable<BodyTypeViewModel> bodyTypeList = Mapper.Map<IEnumerable<BodyTypeViewModel>>(bodyTypes);
            return Ok(bodyTypeList);
        }

        [Route("bodyType/{bodyTypeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBodyTypeId(int bodyTypeId)
        {
            List<BodyType> bodyTypes = null;
            bodyTypes = await _bodyTypeApplicationService.GetAsync(item => item.BodyStyleConfigs.Any(bc =>
                bc.BodyTypeId == bodyTypeId));
            IEnumerable<BodyTypeViewModel> bodyTypeList = Mapper.Map<IEnumerable<BodyTypeViewModel>>(bodyTypes);
            return Ok(bodyTypeList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BodyTypeInputModel model)
        {
            // create body type and list of comments
            BodyType bodyType = new BodyType() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyTypeApplicationService.AddAsync(bodyType, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BodyTypeInputModel model)
        {
            BodyType bodyType = new BodyType() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyTypeApplicationService.DeleteAsync(bodyType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BodyTypeInputModel bodyTypeInputModel)
        {
            BodyType bodyType = new BodyType()
            {
                Id = bodyTypeInputModel.Id,
                Name = bodyTypeInputModel.Name,
                BodyStyleConfigCount = bodyTypeInputModel.BodyStyleConfigCount,
                VehicleToBodyStyleConfigCount = bodyTypeInputModel.VehicleToBodyStyleConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = bodyTypeInputModel.Comment };
            var attachments = SetUpAttachmentsModels(bodyTypeInputModel.Attachments);
            var changeRequestId = await _bodyTypeApplicationService.UpdateAsync(bodyType, bodyType.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<BodyType> changeRequestStagingBodyTypeModel =
                await this._bodyTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingBodyTypeViewModel changeRequestStagingBodyTypeViewModel = Mapper.Map<ChangeRequestStagingBodyTypeViewModel>(changeRequestStagingBodyTypeModel);

            SetUpChangeRequestReview(changeRequestStagingBodyTypeViewModel.StagingItem.Status,
                changeRequestStagingBodyTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingBodyTypeViewModel);
                       return Ok(changeRequestStagingBodyTypeViewModel);
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
                this._bodyTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
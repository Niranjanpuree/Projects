using System;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("bodyStyleConfigs")]
    public class BodyStyleConfigController : ApiControllerBase
    {
        private readonly IBodyStyleConfigApplicationService _bodyStyleConfigApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        private readonly ITextSerializer _serializer;
        public BodyStyleConfigController(IBodyStyleConfigApplicationService bodyStyleConfigApplicationService, IApplicationLogger applicationLogger, ITextSerializer serializer)
        {
            _bodyStyleConfigApplicationService = bodyStyleConfigApplicationService;
            _applicationLogger = applicationLogger;
            _serializer = serializer;
        }
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<BodyStyleConfig> bodyStyleConfigs = await _bodyStyleConfigApplicationService.GetAllAsync();

            IEnumerable<BodyStyleConfigViewModel> bodyStyleConfigList = Mapper.Map<IEnumerable<BodyStyleConfigViewModel>>(bodyStyleConfigs);

            return Ok(bodyStyleConfigList);
        }
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedBodyStyleConfig = await _bodyStyleConfigApplicationService.GetAsync(id);

                var bodyStyleConfig = Mapper.Map<BodyStyleConfigViewModel>(selectedBodyStyleConfig);

                return Ok(bodyStyleConfig);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("bodyNumDoors/{bodyNumberDoorsId:int}/bodyType/{bodyTypeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByChildIds(int bodyNumberDoorsId, int bodyTypeId)
        {
            var bodyStyleConfigs = await _bodyStyleConfigApplicationService.GetAsync(item => item.BodyNumDoorsId == bodyNumberDoorsId
            && item.BodyTypeId == bodyTypeId);
            BodyStyleConfigViewModel bodyStyleConfigViewModel = Mapper.Map<BodyStyleConfigViewModel>(bodyStyleConfigs.FirstOrDefault());

            return Ok(bodyStyleConfigViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BodyStyleConfigInputModel model)
        {
            BodyStyleConfig bodyStyleConfig = new BodyStyleConfig()
            {
                Id = model.Id,
                BodyNumDoorsId= model.BodyNumDoorsId,
                BodyTypeId= model.BodyTypeId
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyStyleConfigApplicationService.AddAsync(bodyStyleConfig, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BodyStyleConfigInputModel model)
        {
            BodyStyleConfig bodyStyleConfig = new BodyStyleConfig()
            {
                Id = model.Id,
                BodyNumDoorsId = model.BodyNumDoorsId,
                BodyTypeId = model.BodyTypeId,
                VehicleToBodyStyleConfigCount = model.VehicleToBodyStyleConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyStyleConfigApplicationService.UpdateAsync(bodyStyleConfig, bodyStyleConfig.Id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("replace/{id:int}")]
        public async Task<IHttpActionResult> Replace(int id, BodyStyleConfigInputModel model)
        {
            BodyStyleConfig bodyStyleConfig = new BodyStyleConfig()
            {
                Id = model.Id,
                BodyNumDoorsId = model.BodyNumDoorsId,
                BodyTypeId = model.BodyTypeId,
                VehicleToBodyStyleConfigs = model.VehicleToBodyStyleConfigs.Select(item => new VehicleToBodyStyleConfig
                {
                    BodyStyleConfigId= item.BodyStyleConfigId,
                    Id = item.Id,
                    VehicleId = item.VehicleId
                }).ToList(),

            };

            CommentsStagingModel comment = new CommentsStagingModel { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyStyleConfigApplicationService.ReplaceAsync(bodyStyleConfig, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("pendingChangeRequests")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingChangeRequests()
        {
            var pendingBodyStyleConfigs = await _bodyStyleConfigApplicationService.GetPendingAddChangeRequests(null);

            List<BodyStyleConfigViewModel> bodyStyleConfigViewModels = Mapper.Map<List<BodyStyleConfigViewModel>>(pendingBodyStyleConfigs);

            return Ok(bodyStyleConfigViewModels);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BodyStyleConfigInputModel model)
        {
            BodyStyleConfig bodyStyleConfig = new BodyStyleConfig()
            {
                Id = model.Id,
                VehicleToBodyStyleConfigCount = model.VehicleToBodyStyleConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment, };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bodyStyleConfigApplicationService.DeleteAsync(bodyStyleConfig, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            BodyStyleConfigChangeRequestStagingModel changeRequestStagingBodyStyleConfigModel =
                await this._bodyStyleConfigApplicationService.GetChangeRequestStaging(changeRequestId);

            ChangeRequestStagingBodyStyleConfigViewModel changeRequestStagingBodyStyleConfigViewModel = Mapper.Map<ChangeRequestStagingBodyStyleConfigViewModel>(changeRequestStagingBodyStyleConfigModel);

             SetUpChangeRequestReview(changeRequestStagingBodyStyleConfigViewModel.StagingItem.Status,
            changeRequestStagingBodyStyleConfigViewModel.StagingItem.SubmittedBy, changeRequestStagingBodyStyleConfigViewModel);
            
            return Ok(changeRequestStagingBodyStyleConfigViewModel);
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
                Attachments = SetUpAttachmentsModels(changeRequestReview.Attachments?.ToList())
            };

            // submit review
            bool isSubmitted = await
                this._bodyStyleConfigApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

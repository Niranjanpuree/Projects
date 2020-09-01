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
using System;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("bedConfigs")]
    public class BedConfigController : ApiControllerBase
    {
        private readonly IBedConfigApplicationService _bedConfigApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        private readonly ITextSerializer _serializer;
        public BedConfigController(IBedConfigApplicationService bedConfigApplicationService, IApplicationLogger applicationLogger, ITextSerializer serializer)
        {
            _bedConfigApplicationService = bedConfigApplicationService;
            _applicationLogger = applicationLogger;
            _serializer = serializer;
        }
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<BedConfig> bedConfigs = await _bedConfigApplicationService.GetAllAsync();

            IEnumerable<BedConfigViewModel> bedConfigList = Mapper.Map<IEnumerable<BedConfigViewModel>>(bedConfigs);

            return Ok(bedConfigList);
        }
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedBedConfig = await _bedConfigApplicationService.GetAsync(id);

                var bedConfig = Mapper.Map<BedConfigViewModel>(selectedBedConfig);

                return Ok(bedConfig);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [Route("bedLength/{bedLengthId:int}/bedType/{bedTypeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByChildIds(int bedLengthId, int bedTypeId)
        {
            var bedConfigs = await _bedConfigApplicationService.GetAsync(item => item.BedLengthId == bedLengthId
            && item.BedTypeId == bedTypeId);
            BedConfigViewModel bedConfigViewModel = Mapper.Map<BedConfigViewModel>(bedConfigs.FirstOrDefault());

            return Ok(bedConfigViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BedConfigInputModel model)
        {
            BedConfig bedConfig = new BedConfig()
            {
                Id = model.Id,
                BedLengthId=model.BedLengthId,
                BedTypeId=model.BedTypeId
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedConfigApplicationService.AddAsync(bedConfig, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BedConfigInputModel model)
        {
            BedConfig bedConfig = new BedConfig()
            {
                Id = model.Id,
                BedLengthId = model.BedLengthId,
                BedTypeId = model.BedTypeId,
                VehicleToBedConfigCount = model.VehicleToBedConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedConfigApplicationService.UpdateAsync(bedConfig, bedConfig.Id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("replace/{id:int}")]
        public async Task<IHttpActionResult> Replace(int id, BedConfigInputModel model)
        {
            BedConfig bedConfig = new BedConfig()
            {
                Id = model.Id,
                BedLengthId = model.BedLengthId,
                BedTypeId = model.BedTypeId,
                VehicleToBedConfigs = model.VehicleToBedConfigs.Select(item => new VehicleToBedConfig
                {
                    BedConfigId= item.BedConfigId,
                    Id = item.Id,
                    VehicleId = item.VehicleId
                }).ToList(),

            };

            CommentsStagingModel comment = new CommentsStagingModel { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedConfigApplicationService.ReplaceAsync(bedConfig, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("pendingChangeRequests")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingChangeRequests()
        {
            var pendingBedConfigs = await _bedConfigApplicationService.GetPendingAddChangeRequests(null);

            List<BedConfigViewModel> bedConfigViewModels = Mapper.Map<List<BedConfigViewModel>>(pendingBedConfigs);

            return Ok(bedConfigViewModels);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BedConfigInputModel model)
        {
            BedConfig bedConfig = new BedConfig()
            {
                Id = model.Id,
                VehicleToBedConfigCount = model.VehicleToBedConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment, };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedConfigApplicationService.DeleteAsync(bedConfig, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            BedConfigChangeRequestStagingModel changeRequestStagingBedConfigModel =
                await this._bedConfigApplicationService.GetChangeRequestStaging(changeRequestId);

            ChangeRequestStagingBedConfigViewModel changeRequestStagingBedConfigViewModel = Mapper.Map<ChangeRequestStagingBedConfigViewModel>(changeRequestStagingBedConfigModel);

             SetUpChangeRequestReview(changeRequestStagingBedConfigViewModel.StagingItem.Status,
                changeRequestStagingBedConfigViewModel.StagingItem.SubmittedBy, changeRequestStagingBedConfigViewModel);
            
            return Ok(changeRequestStagingBedConfigViewModel);
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
                this._bedConfigApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

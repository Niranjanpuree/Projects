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
    [RoutePrefix("brakeConfigs")]
    public class BrakeConfigController : ApiControllerBase
    {
        private readonly IBrakeConfigApplicationService _brakeConfigApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        private readonly ITextSerializer _serializer;

        public BrakeConfigController(IBrakeConfigApplicationService brakeConfigApplicationService, IApplicationLogger applicationLogger, ITextSerializer serializer)
        {
            _brakeConfigApplicationService = brakeConfigApplicationService;
            _applicationLogger = applicationLogger;
            _serializer = serializer;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<BrakeConfig> brakeConfigs = await _brakeConfigApplicationService.GetAllAsync();

            IEnumerable<BrakeConfigViewModel> brakeConfigList = Mapper.Map<IEnumerable<BrakeConfigViewModel>>(brakeConfigs);

            return Ok(brakeConfigList);
        }

        // GET api/brakeConfigs/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedBrakeConfig = await _brakeConfigApplicationService.GetAsync(id);

                var brakeConfig = Mapper.Map<BrakeConfigViewModel>(selectedBrakeConfig);

                return Ok(brakeConfig);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("frontBrakeType/{frontBrakeTypeId:int}/rearBrakeType/{rearBrakeTypeId:int}/brakeABS/{brakeABSId:int}/brakeSystem/{brakeSystemId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByChildIds(int frontBrakeTypeId, int rearBrakeTypeId, int brakeABSId, int brakeSystemId)
        {
            var brakeConfigs = await _brakeConfigApplicationService.GetAsync(item => item.FrontBrakeTypeId == frontBrakeTypeId
            && item.RearBrakeTypeId == rearBrakeTypeId
            && item.BrakeABSId == brakeABSId
            && item.BrakeSystemId == brakeSystemId);
            BrakeConfigViewModel brakeConfigViewModel = Mapper.Map<BrakeConfigViewModel>(brakeConfigs.FirstOrDefault());

            return Ok(brakeConfigViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BrakeConfigInputModel model)
        {
            BrakeConfig brakeConfig = new BrakeConfig()
            {
                Id = model.Id,
                FrontBrakeTypeId = model.FrontBrakeTypeId,
                RearBrakeTypeId = model.RearBrakeTypeId,
                BrakeABSId = model.BrakeABSId,
                BrakeSystemId = model.BrakeSystemId
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeConfigApplicationService.AddAsync(brakeConfig, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BrakeConfigInputModel model)
        {
            BrakeConfig brakeConfig = new BrakeConfig()
            {
                Id = model.Id,
                FrontBrakeTypeId = model.FrontBrakeTypeId,
                RearBrakeTypeId = model.RearBrakeTypeId,
                BrakeABSId = model.BrakeABSId,
                BrakeSystemId = model.BrakeSystemId,
                VehicleToBrakeConfigCount = model.VehicleToBrakeConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeConfigApplicationService.UpdateAsync(brakeConfig, brakeConfig.Id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("replace/{id:int}")]
        public async Task<IHttpActionResult> Replace(int id, BrakeConfigInputModel model)
        {
            BrakeConfig brakeConfig = new BrakeConfig()
            {
                Id = model.Id,
                FrontBrakeTypeId = model.FrontBrakeTypeId,
                RearBrakeTypeId = model.RearBrakeTypeId,
                BrakeABSId = model.BrakeABSId,
                BrakeSystemId = model.BrakeSystemId,
                VehicleToBrakeConfigs = model.VehicleToBrakeConfigs.Select(item => new VehicleToBrakeConfig
                {
                    BrakeConfigId = item.BrakeConfigId,
                    Id = item.Id,
                    VehicleId = item.VehicleId
                }).ToList(),

            };

            CommentsStagingModel comment = new CommentsStagingModel { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeConfigApplicationService.ReplaceAsync(brakeConfig, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }


        [Route("pendingChangeRequests")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingChangeRequests()
        {
            var pendingBrakeConfigs = await _brakeConfigApplicationService.GetPendingAddChangeRequests(null);

            List<BrakeConfigViewModel> brakeConfigViewModels = Mapper.Map<List<BrakeConfigViewModel>>(pendingBrakeConfigs);

            return Ok(brakeConfigViewModels);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BrakeConfigInputModel model)
        {
            BrakeConfig brakeConfig = new BrakeConfig()
            {
                Id = model.Id,
                VehicleToBrakeConfigCount = model.VehicleToBrakeConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment, };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _brakeConfigApplicationService.DeleteAsync(brakeConfig, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            BrakeConfigChangeRequestStagingModel changeRequestStagingBrakeConfigModel =
                await this._brakeConfigApplicationService.GetChangeRequestStaging(changeRequestId);

            ChangeRequestStagingBrakeConfigViewModel changeRequestStagingBrakeConfigViewModel = Mapper.Map<ChangeRequestStagingBrakeConfigViewModel>(changeRequestStagingBrakeConfigModel);

             SetUpChangeRequestReview(changeRequestStagingBrakeConfigViewModel.StagingItem.Status,
                changeRequestStagingBrakeConfigViewModel.StagingItem.SubmittedBy, changeRequestStagingBrakeConfigViewModel);
            

            return Ok(changeRequestStagingBrakeConfigViewModel);
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
                this._brakeConfigApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
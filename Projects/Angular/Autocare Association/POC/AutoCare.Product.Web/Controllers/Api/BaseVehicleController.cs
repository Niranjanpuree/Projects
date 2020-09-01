using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using System;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("baseVehicles")]
    public class BaseVehicleController : ApiControllerBase
    {
        private readonly IBaseVehicleApplicationService _baseVehicleApplicationService;

        private readonly ITextSerializer _serializer;

        public BaseVehicleController(IBaseVehicleApplicationService baseVehicleApplicationService, ITextSerializer serializer)
        {
            _baseVehicleApplicationService = baseVehicleApplicationService;
            _serializer = serializer;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var baseVehicles = await _baseVehicleApplicationService.GetAllAsync(10);
            IEnumerable<BaseVehicleViewModel> baseVehicleList = Mapper.Map<IEnumerable<BaseVehicleViewModel>>(baseVehicles);

            return Ok(baseVehicleList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedBaseVehicle = await _baseVehicleApplicationService.GetAsync(id);
                BaseVehicleViewModel baseVehicle = Mapper.Map<BaseVehicleViewModel>(selectedBaseVehicle);

                return Ok(baseVehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("~/years/{yearId:int}/makes/{makeId:int}/models")]
        [HttpGet]
        public async Task<IHttpActionResult> GetModelsByYearIdAndMakeId(int yearId, int makeId)
        {
            List<BaseVehicle> baseVehicles = null;

            baseVehicles = await _baseVehicleApplicationService.GetAsync(b => b.YearId == yearId && b.MakeId == makeId);

            IEnumerable<ModelViewModel> modelsList = Mapper.Map<IEnumerable<ModelViewModel>>(baseVehicles);

            return Ok(modelsList);
        }

        [Route("pendingChangeRequests")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingChangeRequests()
        {
            var pendingBaseVehicles = await _baseVehicleApplicationService.GetPendingAddChangeRequests(null);

            List<BaseVehicleViewModel> baseVehicleViewModels = Mapper.Map<List<BaseVehicleViewModel>>(pendingBaseVehicles);

            return Ok(baseVehicleViewModels);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BaseVehicleInputModel newBaseVehicle)
        {
            BaseVehicle baseVehicle = new BaseVehicle()
            {
                MakeId = newBaseVehicle.MakeId,
                ModelId = newBaseVehicle.ModelId,
                YearId = newBaseVehicle.YearId
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = newBaseVehicle.Comment };
            var attachments = SetUpAttachmentsModels(newBaseVehicle.Attachments);

            var changeRequestId = await _baseVehicleApplicationService.AddAsync(baseVehicle, CurrentUser.Email, comment,attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BaseVehicleInputModel changeBaseVehicle)
        {
            BaseVehicle baseVehicle = new BaseVehicle()
            {
                Id = changeBaseVehicle.Id,
                MakeId = changeBaseVehicle.MakeId,
                ModelId = changeBaseVehicle.ModelId,
                YearId = changeBaseVehicle.YearId,
                VehicleCount = changeBaseVehicle.VehicleCount,
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = changeBaseVehicle.Comment };
            var attachments = SetUpAttachmentsModels(changeBaseVehicle.Attachments);

            var changeRequestId = await _baseVehicleApplicationService.UpdateAsync(baseVehicle, baseVehicle.Id, CurrentUser.Email, comment,attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("replace/{id:int}")]
        public async Task<IHttpActionResult> Replace(int id, BaseVehicleInputModel replaceBaseVehicle)
        {
            BaseVehicle baseVehicle = new BaseVehicle()
            {
                Id = replaceBaseVehicle.Id,
                MakeId = replaceBaseVehicle.MakeId,
                ModelId = replaceBaseVehicle.ModelId,
                YearId = replaceBaseVehicle.YearId,
                Vehicles = replaceBaseVehicle.vehicles.Select(item => new Vehicle
                {
                    BaseVehicleId = item.BaseVehicleId,
                    Id = item.Id,
                    RegionId = item.RegionId,
                    SubModelId = item.SubModelId,
                }).ToList(),
            };
            CommentsStagingModel comment = new CommentsStagingModel { Comment = replaceBaseVehicle.Comment };
            var attachments = SetUpAttachmentsModels(replaceBaseVehicle.Attachments);

            var changeRequestId = await _baseVehicleApplicationService.ReplaceAsync(baseVehicle, baseVehicle.Id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);

        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BaseVehicleInputModel newBaseVehicle)
        {
            BaseVehicle baseVehicle = new BaseVehicle()
            {
                Id = newBaseVehicle.Id,
                MakeId = newBaseVehicle.MakeId,
                ModelId = newBaseVehicle.ModelId,
                YearId = newBaseVehicle.YearId,
                VehicleCount = newBaseVehicle.VehicleCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = newBaseVehicle.Comment };
            var attachments = SetUpAttachmentsModels(newBaseVehicle.Attachments);

            var changeRequestId = await _baseVehicleApplicationService.DeleteAsync(baseVehicle, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            BaseVehicleChangeRequestStagingModel changeRequestStagingModel = await this._baseVehicleApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingBaseVehicleViewModel changeRequestStagingBaseVehicleViewModel = Mapper.Map<ChangeRequestStagingBaseVehicleViewModel>(changeRequestStagingModel);

            
            // setup change request review properties
            SetUpChangeRequestReview(changeRequestStagingBaseVehicleViewModel.StagingItem.Status,
                changeRequestStagingBaseVehicleViewModel.StagingItem.SubmittedBy, changeRequestStagingBaseVehicleViewModel);
                       
            return Ok(changeRequestStagingBaseVehicleViewModel);
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
                this._baseVehicleApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }

    }
}
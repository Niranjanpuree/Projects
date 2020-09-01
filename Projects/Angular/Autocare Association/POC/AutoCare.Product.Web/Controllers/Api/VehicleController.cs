using System;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicles")]
    public class VehicleController : ApiControllerBase
    {
        private readonly IVehicleApplicationService _vehicleApplicationService;

        private readonly ILogger _logger;

        private readonly ITextSerializer _serializer;

        public VehicleController(IVehicleApplicationService vehicleApplicationService, ILogger logger, ITextSerializer serializer)
        {
            _vehicleApplicationService = vehicleApplicationService;
            _serializer = serializer;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var vehicles = await _vehicleApplicationService.GetAllAsync(10);
            var vehicleList = Mapper.Map<IEnumerable<VehicleViewModel>>(vehicles);
            _logger.Info("GetVehicles method called");
            return Ok(vehicleList);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedvehicle = await _vehicleApplicationService.GetAsync(id);
                var vehicleDetail = Mapper.Map<VehicleViewModel>(selectedvehicle);
                _logger.Info("GetVehicleById method called");
                return Ok(vehicleDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("~/baseVehicles/{baseVehicleId:int}/vehicles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVehiclesByBaseVehicleId(int baseVehicleId)
        {
            var vehicles = await _vehicleApplicationService.GetAsync(item => item.BaseVehicleId == baseVehicleId);

            List<VehicleViewModel> vehicleViewModels = Mapper.Map<List<VehicleViewModel>>(vehicles);

            return Ok(vehicleViewModels);
        }

        [Route("~/baseVehicles/{baseVehicleId:int}/subModels/{subModelId:int}/regions/{regionId:int}/vehicles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVehicleBySubModelIdAndRegionId(int baseVehicleId, int subModelId, int regionId)
        {
            var vehicles = await _vehicleApplicationService.GetAsync(item => item.BaseVehicleId == baseVehicleId && item.SubModelId == subModelId && item.RegionId == regionId);

            VehicleViewModel vehicleViewModel = Mapper.Map<VehicleViewModel>(vehicles.FirstOrDefault());

            return Ok(vehicleViewModel);
        }

        [Route("pendingChangeRequests/{baseVehicleId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingAddChangeRequests(int baseVehicleId)
        {
            var pendingVehicles = await _vehicleApplicationService.GetPendingAddChangeRequests(item=>item.BaseVehicleId == baseVehicleId);

            List<VehicleViewModel> vehicleViewModels = Mapper.Map<List<VehicleViewModel>>(pendingVehicles);

            return Ok(vehicleViewModels);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleInputModel model)
        {
            Vehicle vehicle = new Vehicle()
            {
                BaseVehicleId = model.BaseVehicleId,
                SubModelId = model.SubModelId,
                SourceId = model.SourceId,
                RegionId = model.RegionId
            };
            var attachments = SetUpAttachmentsModels(model.Attachments);

            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };

            var changeRequestId = await _vehicleApplicationService.AddAsync(vehicle, CurrentUser.Email, comment,attachments);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, VehicleInputModel model)
        {
            Vehicle vehicle = new Vehicle() { Id = model.Id,VehicleToBrakeConfigCount=model.VehicleToBrakeConfigCount };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _vehicleApplicationService.DeleteAsync(vehicle, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, VehicleInputModel updatedVehicle)
        {
            Vehicle vehicle = new Vehicle()
            {
                Id = updatedVehicle.Id,
                BaseVehicleId = updatedVehicle.BaseVehicleId,
                SubModelId = updatedVehicle.SubModelId,
                SourceId = updatedVehicle.SourceId,
                RegionId = updatedVehicle.RegionId,
                VehicleToBrakeConfigCount=updatedVehicle.VehicleToBrakeConfigCount,
                VehicleToBedConfigCount=updatedVehicle.VehicleToBedConfigCount,
                VehicleToBodyStyleConfigCount=updatedVehicle.VehicleToBodyStyleConfigCount

            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = updatedVehicle.Comment };
            var attachments = SetUpAttachmentsModels(updatedVehicle.Attachments);
            var changeRequestId = await _vehicleApplicationService.UpdateAsync(vehicle, id, CurrentUser.Email, comment,attachments);
            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<Vehicle> changeRequestStagingModel = await this._vehicleApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleViewModel changeRequestStagingVehicleViewModel = Mapper.Map<ChangeRequestStagingVehicleViewModel>(changeRequestStagingModel);

             SetUpChangeRequestReview(changeRequestStagingVehicleViewModel.StagingItem.Status,
                changeRequestStagingVehicleViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleViewModel);
            
            // return view model
            return Ok(changeRequestStagingVehicleViewModel);
        }


        [HttpPost]
        [Route("~/vehicles/changeRequestStaging/{changeRequestId:int}")]
        public async Task<IHttpActionResult> Post(int changeRequestId, ChangeRequestReviewInputModel changeRequestReview)
        {
            //create change request review model
            ChangeRequestReviewModel reviewModel = new ChangeRequestReviewModel()
            {
                ChangeRequestId = changeRequestReview.ChangeRequestId,
                ReviewedBy = CurrentUser.CustomerId,
                ReviewStatus = changeRequestReview.ReviewStatus,
                ReviewComment = new CommentsStagingModel()
                {
                    Comment = changeRequestReview.ReviewComment.Comment,
                    AddedBy = CurrentUser.CustomerId,//changeRequestReview.ReviewComment.AddedBy,
                    CreatedDatetime = changeRequestReview.ReviewComment.CreatedDatetime
                },
                //NOTE: CR Input model uses IList unlike other models hence the addition of .ToList()
                Attachments = SetUpAttachmentsModels(changeRequestReview.Attachments?.ToList())

            };
            // submit review
            bool isSubmitted =
                await
                    this._vehicleApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);
            return Ok(isSubmitted);

        }
    }
}

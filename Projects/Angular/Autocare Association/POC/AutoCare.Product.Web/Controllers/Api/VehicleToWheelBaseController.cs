using AutoCare.Product.Infrastructure.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using AutoCare.Product.Web.Models.InputModels;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicleToWheelBases")]
    public class VehicleToWheelBaseController : ApiControllerBase
    {
        private readonly IVehicleToWheelBaseApplicationService _vehicleToWheelBaseApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public VehicleToWheelBaseController(IVehicleToWheelBaseApplicationService vehicleToWheelBaseApplicationService, 
            IApplicationLogger applicationLogger)
        {
            _vehicleToWheelBaseApplicationService = vehicleToWheelBaseApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<VehicleToWheelBase> vehicleToWheelBases = await _vehicleToWheelBaseApplicationService.GetAllAsync(10);
            List<VehicleToWheelBaseViewModel> vehicleToWheelBase = Mapper.Map<List<VehicleToWheelBaseViewModel>>(vehicleToWheelBases);
            return Ok(vehicleToWheelBase);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedVehicleToWheelBase = await _vehicleToWheelBaseApplicationService.GetAsync(id);
            return Ok(selectedVehicleToWheelBase);
        }

        //TODO: use GetAssociationsByWheelBaseId() from VehicleToWheelBaseSearchController
        [Route("wheelBase/{wheelbaseid:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByWheelBaseId(int wheelBaseId)
        {
            List<VehicleToWheelBase> vehicleToWheelBases = await _vehicleToWheelBaseApplicationService.GetAsync(item => item.WheelBase.Id.Equals(wheelBaseId));
            List<VehicleToWheelBaseViewModel> vehicleToWheelBaseList = Mapper.Map<List<VehicleToWheelBaseViewModel>>(vehicleToWheelBases);
            return Ok(vehicleToWheelBaseList);
        }

        [Route("~/vehicles/{vehicleId:int}/vehicleToWheelBases")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(int vehicleId)
        {
            var vehicleToWheelBases = await _vehicleToWheelBaseApplicationService.GetAsync(item => item.VehicleId == vehicleId);
            List<VehicleToWheelBaseViewModel> vehicleToWheelBaseList = vehicleToWheelBases.Select(item =>
                new VehicleToWheelBaseViewModel()
                {
                    Id = item.Id,
                    Vehicle = new VehicleViewModel()
                    {
                        Id = item.VehicleId,
                        BaseVehicleId = item.Vehicle.BaseVehicleId,
                        ModelId = item.Vehicle.BaseVehicle.ModelId,
                        MakeId = item.Vehicle.BaseVehicle.MakeId,
                        ChangeRequestId = -1,
                        MakeName = item.Vehicle.BaseVehicle.Make.Name,
                        ModelName = item.Vehicle.BaseVehicle.Model.Name,
                        PublicationEnvironment = item.Vehicle.PublicationEnvironment,
                        PublicationStageDate = item.Vehicle.PublicationStageDate,
                        PublicationStageId = item.Vehicle.PublicationStageId,
                        PublicationStageName = item.Vehicle.PublicationStage.Name,
                        PublicationStageSource = item.Vehicle.PublicationStageSource,
                        RegionId = item.Vehicle.RegionId,
                        RegionName = item.Vehicle.Region.Name,
                        SourceId = item.Vehicle.SourceId,
                        SourceName = item.Vehicle.SourceName,
                        SubModelId = item.Vehicle.SubModelId,
                        SubModelName = item.Vehicle.SubModel.Name,
                        YearId = item.Vehicle.BaseVehicle.YearId,
                    },
                    WheelBase = new WheelBaseViewModel()
                    {
                        Id = item.WheelBaseId,
                        Base = item.WheelBase.Base,
                        WheelBaseMetric = item.WheelBase.WheelBaseMetric,
                    },
                    ChangeRequestId = item.ChangeRequestId
                }).ToList();
            return Ok(vehicleToWheelBaseList);
        }

        [Route("~/vehicles/{vehicleIds}/wheelBases/{wheelbaseids}/vehicleToWheelBases")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(string vehicleIds, string wheelBaseIds)
        {
            List<VehicleToWheelBase> vehicleToWheelBases = null;
            if (!string.IsNullOrWhiteSpace(vehicleIds) && !string.IsNullOrWhiteSpace(wheelBaseIds))
            {
                string[] vehicleIdArray = vehicleIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] wheelBaseIdArray = wheelBaseIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (vehicleIdArray.Length > 0 && wheelBaseIdArray.Length > 0)
                {
                    vehicleToWheelBases = await _vehicleToWheelBaseApplicationService.GetAsync(item =>
                    vehicleIdArray.Any(v => v == item.VehicleId.ToString()) && 
                    wheelBaseIdArray.Any(v => v == item.WheelBaseId.ToString()));
                }
            }
            List<VehicleToWheelBaseViewModel> vehicleToWheelBaseViewModel = Mapper.Map<List<VehicleToWheelBaseViewModel>>(vehicleToWheelBases);
            return Ok(vehicleToWheelBaseViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleToWheelBaseInputModel model)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _vehicleToWheelBaseApplicationService.AddAsync(new VehicleToWheelBase()
            {
                Id = model.Id,
                WheelBaseId = model.WheelBase.Id,
                VehicleId = model.Vehicle.Id,
            }, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, VehicleToWheelBaseInputModel model)
        {
            var changeRequestId = await _vehicleToWheelBaseApplicationService.UpdateAsync(new VehicleToWheelBase()
            {
                Id = model.Id,
                VehicleId = model.Vehicle.Id,
                WheelBaseId = model.WheelBase.Id
            }, id, "update-requestor");
            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, VehicleToWheelBaseInputModel vehicleToWheelBaseInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleToWheelBaseInputModel.Comment };
            var attachments = SetUpAttachmentsModels(vehicleToWheelBaseInputModel.Attachments);
            var changeRequestId = await _vehicleToWheelBaseApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleToWheelBase> changeRequestStagingVehicleToWheelBaseModel = await this._vehicleToWheelBaseApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleToWheelBaseViewModel changeRequestStagingVehicleToWheelBaseViewModel = Mapper.Map<ChangeRequestStagingVehicleToWheelBaseViewModel>(changeRequestStagingVehicleToWheelBaseModel);
             SetUpChangeRequestReview(changeRequestStagingVehicleToWheelBaseViewModel.StagingItem.Status,
                changeRequestStagingVehicleToWheelBaseViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleToWheelBaseViewModel);
                        // return view model
            return Ok(changeRequestStagingVehicleToWheelBaseViewModel);
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
            bool isSubmitted = await
                this._vehicleToWheelBaseApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);
            return Ok(isSubmitted);
        }
    }
}

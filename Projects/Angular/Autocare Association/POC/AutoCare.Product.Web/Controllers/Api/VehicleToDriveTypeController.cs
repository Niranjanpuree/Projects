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

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicletodrivetypes")]
    public class VehicleToDriveTypeController : ApiControllerBase
    {
        private readonly IVehicleToDriveTypeApplicationService _vehicleToDriveTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public VehicleToDriveTypeController(IVehicleToDriveTypeApplicationService vehicleToDriveTypeApplicationService, IApplicationLogger applicationLogger)
        {
            _vehicleToDriveTypeApplicationService = vehicleToDriveTypeApplicationService;
            _applicationLogger = applicationLogger;
        }
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<VehicleToDriveType> vehicleToDriveTypes = await _vehicleToDriveTypeApplicationService.GetAllAsync(10);
            List<VehicleToDriveTypeViewModel> vehicleToDriveType = Mapper.Map<List<VehicleToDriveTypeViewModel>>(vehicleToDriveTypes);
            return Ok(vehicleToDriveType);
        }
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedVehicleToDriveType = await _vehicleToDriveTypeApplicationService.GetAsync(id);

            return Ok(selectedVehicleToDriveType);
        }
        [Route("driveType/{driveTypeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByDriveTypeId(int driveTypeId)
        {
            List<VehicleToDriveType> vehicleToDriveTypes = await _vehicleToDriveTypeApplicationService.GetAsync(item => item.DriveType.Id.Equals(driveTypeId));

            List<VehicleToDriveTypeViewModel> vehicleToDriveTypeList = Mapper.Map<List<VehicleToDriveTypeViewModel>>(vehicleToDriveTypes);
            return Ok(vehicleToDriveTypeList);
        }
        [Route("~/vehicles/{vehicleId:int}/vehicletodrivetypes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(int vehicleId)
        {
            var vehicleToDriveTypes = await _vehicleToDriveTypeApplicationService.GetAsync(item => item.VehicleId == vehicleId);
            List<VehicleToDriveTypeViewModel> vehicleToDriveTypeList = vehicleToDriveTypes.Select(item =>
                new VehicleToDriveTypeViewModel()
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
                        SourceName = item.Vehicle.SourceName,   //TODO: check
                        SubModelId = item.Vehicle.SubModelId,
                        SubModelName = item.Vehicle.SubModel.Name,
                        YearId = item.Vehicle.BaseVehicle.YearId,
                    },
                    DriveType = new DriveTypeViewModel()
                    {
                        Id = item.DriveTypeId,
                        Name = item.DriveType.Name,
                        VehicleToDriveTypeCount = 0,
                    },
                    ChangeRequestId = item.ChangeRequestId
                }).ToList();
            //Mapper.Map<List<VehicleToDriveTypeViewModel>>(VehicleToDriveTypes);

            return Ok(vehicleToDriveTypeList);
        }

        [Route("~/vehicles/{vehicleIds}/driveTypes/{driveTypeIds}/VehicleToDriveTypes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(string vehicleIds, string driveTypeIds)
        {
            List<VehicleToDriveType> vehicleToDriveTypes = null;

            if (!string.IsNullOrWhiteSpace(vehicleIds) && !string.IsNullOrWhiteSpace(driveTypeIds))
            {
                string[] vehicleIdArray = vehicleIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] driveTypeIdArray = driveTypeIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (vehicleIdArray.Length > 0 && driveTypeIdArray.Length > 0)
                {
                    vehicleToDriveTypes = await _vehicleToDriveTypeApplicationService.GetAsync(item => vehicleIdArray.Any(v => v == item.VehicleId.ToString())
                    && driveTypeIdArray.Any(v => v == item.DriveTypeId.ToString()));
                }
            }
            List<VehicleToDriveTypeViewModel> vehicleToDriveTypeViewModel = Mapper.Map<List<VehicleToDriveTypeViewModel>>(vehicleToDriveTypes);

            return Ok(vehicleToDriveTypeViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleToDriveTypeInputModel model)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _vehicleToDriveTypeApplicationService.AddAsync(new VehicleToDriveType()
            {
                Id = model.Id,
                DriveTypeId = model.DriveType.Id,
                VehicleId = model.Vehicle.Id
            }, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, VehicleToDriveTypeInputModel model)
        {
            var changeRequestId = await _vehicleToDriveTypeApplicationService.UpdateAsync(new VehicleToDriveType()
            {
                Id = model.Id,
                VehicleId = model.Vehicle.Id,
                DriveTypeId = model.DriveType.Id
            }, id,CurrentUser.Email);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, VehicleToDriveTypeInputModel vehicleToDriveTypeInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleToDriveTypeInputModel.Comment };
            var attachments = SetUpAttachmentsModels(vehicleToDriveTypeInputModel.Attachments);
            var changeRequestId = await _vehicleToDriveTypeApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleToDriveType> changeRequestStagingVehicleToDriveTypeModel = await this._vehicleToDriveTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleToDriveTypeViewModel changeRequestStagingVehicleToDriveTypeViewModel = Mapper.Map<ChangeRequestStagingVehicleToDriveTypeViewModel>(changeRequestStagingVehicleToDriveTypeModel);

            SetUpChangeRequestReview(changeRequestStagingVehicleToDriveTypeViewModel.StagingItem.Status,
                changeRequestStagingVehicleToDriveTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleToDriveTypeViewModel);
           
            // return view model
            return Ok(changeRequestStagingVehicleToDriveTypeViewModel);
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
                this._vehicleToDriveTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            return Ok(isSubmitted);
        }
    }
}

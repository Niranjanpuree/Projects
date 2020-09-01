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
    [RoutePrefix("vehicletobrakeconfigs")]
    public class VehicleToBrakeConfigController : ApiControllerBase
    {
        private readonly IVehicleToBrakeConfigApplicationService _vehicleToBrakeConfigApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public VehicleToBrakeConfigController(IVehicleToBrakeConfigApplicationService vehicleToBrakeConfigApplicationService, IApplicationLogger applicationLogger)
        {
            _vehicleToBrakeConfigApplicationService = vehicleToBrakeConfigApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<VehicleToBrakeConfig> vehicleToBrakeConfigs = await _vehicleToBrakeConfigApplicationService.GetAllAsync(10);
            List<VehicleToBrakeConfigViewModel> vehicleToBrakeConfig = Mapper.Map<List<VehicleToBrakeConfigViewModel>>(vehicleToBrakeConfigs);

            return Ok(vehicleToBrakeConfig);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedVehicleToBrakeConfig = await _vehicleToBrakeConfigApplicationService.GetAsync(id);

            return Ok(selectedVehicleToBrakeConfig);
        }

        //TODO: use GetAssociationsByBrakeConfigId() from VehicleToBrakeConfigSearchController
        [Route("brakeConfig/{brakeConfigId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBrakeConfigId(int brakeConfigId)
        {
            List<VehicleToBrakeConfig> vehicleToBrakeConfigs = await _vehicleToBrakeConfigApplicationService.GetAsync(item => item.BrakeConfig.Id.Equals(brakeConfigId));

            List<VehicleToBrakeConfigViewModel> vehicleToBrakeConfigList = Mapper.Map<List<VehicleToBrakeConfigViewModel>>(vehicleToBrakeConfigs);
            return Ok(vehicleToBrakeConfigList);
        }

        [Route("~/vehicles/{vehicleId:int}/vehicletobrakeconfigs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(int vehicleId)
        {
            var vehicleToBrakeConfigs = await _vehicleToBrakeConfigApplicationService.GetAsync(item => item.VehicleId == vehicleId);
            List<VehicleToBrakeConfigViewModel> vehicleToBrakeConfigList = vehicleToBrakeConfigs.Select(item =>
                new VehicleToBrakeConfigViewModel()
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
                    BrakeConfig = new BrakeConfigViewModel()
                    {
                        Id = item.BrakeConfigId,
                        FrontBrakeTypeId = item.BrakeConfig.FrontBrakeTypeId,
                        BrakeSystemId = item.BrakeConfig.BrakeSystemId,
                        BrakeABSId = item.BrakeConfig.BrakeABSId,
                        RearBrakeTypeId = item.BrakeConfig.RearBrakeTypeId,
                        BrakeABSName = item.BrakeConfig.BrakeABS.Name,
                        BrakeSystemName = item.BrakeConfig.BrakeSystem.Name,
                        FrontBrakeTypeName = item.BrakeConfig.FrontBrakeType.Name,
                        VehicleToBrakeConfigCount = 0,
                        RearBrakeTypeName = item.BrakeConfig.RearBrakeType.Name
                    },
                    ChangeRequestId = item.ChangeRequestId
                }).ToList();
            //Mapper.Map<List<VehicleToBrakeConfigViewModel>>(vehicleToBrakeConfigs);

            return Ok(vehicleToBrakeConfigList);
        }

        [Route("~/vehicles/{vehicleIds}/brakeConfigs/{brakeConfigIds}/vehicletobrakeconfigs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(string vehicleIds, string brakeConfigIds)
        {
            List<VehicleToBrakeConfig> vehicleToBrakeConfigs = null;

            if (!string.IsNullOrWhiteSpace(vehicleIds) && !string.IsNullOrWhiteSpace(brakeConfigIds))
            {
                string[] vehicleIdArray = vehicleIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] brakeConfigIdArray = brakeConfigIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (vehicleIdArray.Length > 0 && brakeConfigIdArray.Length > 0)
                {
                    vehicleToBrakeConfigs = await _vehicleToBrakeConfigApplicationService.GetAsync(item => vehicleIdArray.Any(v => v == item.VehicleId.ToString())
                    && brakeConfigIdArray.Any(v => v == item.BrakeConfigId.ToString()));
                }
            }
            List<VehicleToBrakeConfigViewModel> vehicleToBrakeConfigViewModel = Mapper.Map<List<VehicleToBrakeConfigViewModel>>(vehicleToBrakeConfigs);

            return Ok(vehicleToBrakeConfigViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleToBrakeConfigInputModel model)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _vehicleToBrakeConfigApplicationService.AddAsync(new VehicleToBrakeConfig()
            {
                Id = model.Id,
                BrakeConfigId = model.BrakeConfig.Id,
                VehicleId = model.Vehicle.Id
            }, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, VehicleToBrakeConfigInputModel model)
        {
            var changeRequestId = await _vehicleToBrakeConfigApplicationService.UpdateAsync(new VehicleToBrakeConfig()
            {
                Id = model.Id,
                VehicleId = model.Vehicle.Id,
                BrakeConfigId = model.BrakeConfig.Id
            }, id, CurrentUser.Email);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, VehicleToBrakeConfigInputModel vehicleToBrakeConfigInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleToBrakeConfigInputModel.Comment };
            var attachments = SetUpAttachmentsModels(vehicleToBrakeConfigInputModel.Attachments);
            var changeRequestId = await _vehicleToBrakeConfigApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleToBrakeConfig> changeRequestStagingVehicleToBrakeConfigModel = await this._vehicleToBrakeConfigApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleToBrakeConfigViewModel changeRequestStagingVehicleToBrakeConfigViewModel = Mapper.Map<ChangeRequestStagingVehicleToBrakeConfigViewModel>(changeRequestStagingVehicleToBrakeConfigModel);

             SetUpChangeRequestReview(changeRequestStagingVehicleToBrakeConfigViewModel.StagingItem.Status,
                changeRequestStagingVehicleToBrakeConfigViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleToBrakeConfigViewModel);
            
            // return view model
            return Ok(changeRequestStagingVehicleToBrakeConfigViewModel);
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
                this._vehicleToBrakeConfigApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            return Ok(isSubmitted);
        }
    }
}
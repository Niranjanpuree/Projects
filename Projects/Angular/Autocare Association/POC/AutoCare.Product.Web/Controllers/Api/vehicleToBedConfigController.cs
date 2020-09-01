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

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicletobedconfigs")]
    public class VehicleToBedConfigController : ApiControllerBase
    {
        private readonly IVehicleToBedConfigApplicationService _vehicleToBedConfigApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public VehicleToBedConfigController(IVehicleToBedConfigApplicationService vehicleToBedConfigApplicationService, IApplicationLogger applicationLogger)
        {
            _vehicleToBedConfigApplicationService = vehicleToBedConfigApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<VehicleToBedConfig> vehicleToBedConfigs = await _vehicleToBedConfigApplicationService.GetAllAsync(10);
            List<VehicleToBedConfigViewModel> vehicleToBedConfig = Mapper.Map<List<VehicleToBedConfigViewModel>>(vehicleToBedConfigs);

            return Ok(vehicleToBedConfig);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedVehicleToBedConfig = await _vehicleToBedConfigApplicationService.GetAsync(id);

                return Ok(selectedVehicleToBedConfig);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //TODO: use GetAssociationsByBedConfigId() from VehicleToBedConfigSearchController
        [Route("bedConfig/{bedConfigId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBedConfigId(int BedConfigId)
        {
            List<VehicleToBedConfig> vehicleToBedConfigs = await _vehicleToBedConfigApplicationService.GetAsync(item => item.BedConfig.Id.Equals(BedConfigId));

            List<VehicleToBedConfigViewModel> vehicleToBedConfigList = Mapper.Map<List<VehicleToBedConfigViewModel>>(vehicleToBedConfigs);
            return Ok(vehicleToBedConfigList);
        }


        [Route("~/vehicles/{vehicleId:int}/vehicletobedconfigs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(int vehicleId)
        {
            var vehicleToBedConfigs = await _vehicleToBedConfigApplicationService.GetAsync(item => item.VehicleId == vehicleId);
            List<VehicleToBedConfigViewModel> vehicleToBedConfigList = vehicleToBedConfigs.Select(item =>
                new VehicleToBedConfigViewModel()
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
                    BedConfig = new BedConfigViewModel()
                    {
                        BedLengthId = item.BedConfig.BedLengthId,
                        Length = item.BedConfig.BedLength.Length,
                        BedLengthMetric = item.BedConfig.BedLength.BedLengthMetric,
                        BedTypeId = item.BedConfig.BedType.Id,
                        BedTypeName = item.BedConfig.BedType.Name,
                        VehicleToBedConfigCount = 0,
                    },
                    ChangeRequestId = item.ChangeRequestId
                }).ToList();
            //Mapper.Map<List<VehicleToBedConfigViewModel>>(vehicleToBedConfigs);

            return Ok(vehicleToBedConfigList);
        }

        [Route("~/vehicles/{vehicleIds}/bedConfigs/{bedConfigIds}/vehicleToBedConfigs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(string vehicleIds, string BedConfigIds)
        {
            List<VehicleToBedConfig> vehicleToBedConfigs = null;

            if (!string.IsNullOrWhiteSpace(vehicleIds) && !string.IsNullOrWhiteSpace(BedConfigIds))
            {
                string[] vehicleIdArray = vehicleIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] BedConfigIdArray = BedConfigIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (vehicleIdArray.Length > 0 && BedConfigIdArray.Length > 0)
                {
                    vehicleToBedConfigs = await _vehicleToBedConfigApplicationService.GetAsync(item => vehicleIdArray.Any(v => v == item.VehicleId.ToString())
                    && BedConfigIdArray.Any(v => v == item.BedConfigId.ToString()));
                }
            }
            List<VehicleToBedConfigViewModel> vehicleToBedConfigViewModel = Mapper.Map<List<VehicleToBedConfigViewModel>>(vehicleToBedConfigs);

            return Ok(vehicleToBedConfigViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleToBedConfigInputModel model)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _vehicleToBedConfigApplicationService.AddAsync(new VehicleToBedConfig()
            {
                Id = model.Id,
                BedConfigId = model.BedConfig.Id,
                VehicleId = model.Vehicle.Id
            }, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, VehicleToBedConfigInputModel model)
        {
            var changeRequestId = await _vehicleToBedConfigApplicationService.UpdateAsync(new VehicleToBedConfig()
            {
                Id = model.Id,
                VehicleId = model.Vehicle.Id,
                BedConfigId = model.BedConfig.Id
            }, id, "update-requestor");

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, VehicleToBedConfigInputModel vehicleToBedConfigInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleToBedConfigInputModel.Comment };
            var attachments = SetUpAttachmentsModels(vehicleToBedConfigInputModel.Attachments);
            var changeRequestId = await _vehicleToBedConfigApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleToBedConfig> changeRequestStagingVehicleToBedConfigModel = await this._vehicleToBedConfigApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleToBedConfigViewModel changeRequestStagingVehicleToBedConfigViewModel = Mapper.Map<ChangeRequestStagingVehicleToBedConfigViewModel>(changeRequestStagingVehicleToBedConfigModel);

             SetUpChangeRequestReview(changeRequestStagingVehicleToBedConfigViewModel.StagingItem.Status,
                changeRequestStagingVehicleToBedConfigViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleToBedConfigViewModel);
            
            // return view model
            return Ok(changeRequestStagingVehicleToBedConfigViewModel);
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
                this._vehicleToBedConfigApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            return Ok(isSubmitted);
        }
    }
}

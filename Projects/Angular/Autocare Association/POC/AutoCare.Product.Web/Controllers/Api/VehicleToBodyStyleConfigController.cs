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
    [RoutePrefix("vehicletobodyStyleconfigs")]
    public class VehicleToBodyStyleConfigController : ApiControllerBase
    {
        private readonly IVehicleToBodyStyleConfigApplicationService _vehicleToBodyStyleConfigApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public VehicleToBodyStyleConfigController(IVehicleToBodyStyleConfigApplicationService vehicleToBodyStyleConfigApplicationService, IApplicationLogger applicationLogger)
        {
            _vehicleToBodyStyleConfigApplicationService = vehicleToBodyStyleConfigApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<VehicleToBodyStyleConfig> vehicleToBodyStyleConfigs = await _vehicleToBodyStyleConfigApplicationService.GetAllAsync(10);
            List<VehicleToBodyStyleConfigViewModel> vehicleToBodyStyleConfig = Mapper.Map<List<VehicleToBodyStyleConfigViewModel>>(vehicleToBodyStyleConfigs);

            return Ok(vehicleToBodyStyleConfig);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedVehicleToBodyStyleConfig = await _vehicleToBodyStyleConfigApplicationService.GetAsync(id);

            return Ok(selectedVehicleToBodyStyleConfig);
        }

        //TODO: use GetAssociationsByBodyStyleConfigId() from VehicleToBodyStyleConfigSearchController
        [Route("bodyStyleConfig/{bodyStyleConfigId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBodyStyleConfigId(int bodyStyleConfigId)
        {
            List<VehicleToBodyStyleConfig> vehicleToBodyStyleConfigs = await _vehicleToBodyStyleConfigApplicationService.GetAsync(item => item.BodyStyleConfig.Id.Equals(bodyStyleConfigId));

            List<VehicleToBodyStyleConfigViewModel> vehicleToBodyStyleConfigList = Mapper.Map<List<VehicleToBodyStyleConfigViewModel>>(vehicleToBodyStyleConfigs);
            return Ok(vehicleToBodyStyleConfigList);
        }


        [Route("~/vehicles/{vehicleId:int}/vehicletobodyStyleconfigs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(int vehicleId)
        {
            var vehicleToBodyStyleConfigs = await _vehicleToBodyStyleConfigApplicationService.GetAsync(item => item.VehicleId == vehicleId);
            List<VehicleToBodyStyleConfigViewModel> vehicleToBodyStyleConfigList = vehicleToBodyStyleConfigs.Select(item =>
                new VehicleToBodyStyleConfigViewModel()
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
                    BodyStyleConfig = new BodyStyleConfigViewModel()
                    {
                        Id = item.BodyStyleConfigId,
                        BodyNumDoorsId = item.BodyStyleConfig.BodyNumDoorsId,
                        NumDoors = item.BodyStyleConfig.BodyNumDoors.NumDoors,
                        BodyTypeId = item.BodyStyleConfig.BodyTypeId,
                        BodyTypeName = item.BodyStyleConfig.BodyType.Name,
                        VehicleToBodyStyleConfigCount = 0,

                    },
                    ChangeRequestId = item.ChangeRequestId
                }).ToList();
           
            return Ok(vehicleToBodyStyleConfigList);
        }

        [Route("~/vehicles/{vehicleIds}/bodyStyleConfigs/{bodyStyleConfigIds}/vehicletobodyStyleconfigs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(string vehicleIds, string bodyStyleConfigIds)
        {
            List<VehicleToBodyStyleConfig> vehicleToBodyStyleConfigs = null;

            if (!string.IsNullOrWhiteSpace(vehicleIds) && !string.IsNullOrWhiteSpace(bodyStyleConfigIds))
            {
                string[] vehicleIdArray = vehicleIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] bodyStyleConfigIdArray = bodyStyleConfigIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (vehicleIdArray.Length > 0 && bodyStyleConfigIdArray.Length > 0)
                {
                    vehicleToBodyStyleConfigs = await _vehicleToBodyStyleConfigApplicationService.GetAsync(item => vehicleIdArray.Any(v => v == item.VehicleId.ToString())
                    && bodyStyleConfigIdArray.Any(v => v == item.BodyStyleConfigId.ToString()));
                }
            }
            List<VehicleToBodyStyleConfigViewModel> vehicleToBodyStyleConfigViewModel = Mapper.Map<List<VehicleToBodyStyleConfigViewModel>>(vehicleToBodyStyleConfigs);

            return Ok(vehicleToBodyStyleConfigViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleToBodyStyleConfigInputModel model)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _vehicleToBodyStyleConfigApplicationService.AddAsync(new VehicleToBodyStyleConfig()
            {
                Id = model.Id,
                BodyStyleConfigId = model.BodyStyleConfig.Id,
                VehicleId = model.Vehicle.Id
            }, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, VehicleToBodyStyleConfigInputModel model)
        {
            var changeRequestId = await _vehicleToBodyStyleConfigApplicationService.UpdateAsync(new VehicleToBodyStyleConfig()
            {
                Id = model.Id,
                VehicleId = model.Vehicle.Id,
                BodyStyleConfigId = model.BodyStyleConfig.Id
            }, id, "update-requestor");

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, VehicleToBodyStyleConfigInputModel vehicleToBodyStyleConfigInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleToBodyStyleConfigInputModel.Comment };
            var attachments = SetUpAttachmentsModels(vehicleToBodyStyleConfigInputModel.Attachments);
            var changeRequestId = await _vehicleToBodyStyleConfigApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleToBodyStyleConfig> changeRequestStagingVehicleToBodyStyleConfigModel = await this._vehicleToBodyStyleConfigApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleToBodyStyleConfigViewModel changeRequestStagingVehicleToBodyStyleConfigViewModel = Mapper.Map<ChangeRequestStagingVehicleToBodyStyleConfigViewModel>(changeRequestStagingVehicleToBodyStyleConfigModel);

             SetUpChangeRequestReview(changeRequestStagingVehicleToBodyStyleConfigViewModel.StagingItem.Status,
                changeRequestStagingVehicleToBodyStyleConfigViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleToBodyStyleConfigViewModel);
           
            // return view model
            return Ok(changeRequestStagingVehicleToBodyStyleConfigViewModel);
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
                this._vehicleToBodyStyleConfigApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            return Ok(isSubmitted);
        }
    }
}

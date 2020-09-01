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
    [RoutePrefix("vehicletomfrbodycodes")]
    public class VehicleToMfrBodyCodeController : ApiControllerBase
    {
        private readonly IVehicleToMfrBodyCodeApplicationService _vehicleToMfrBodyCodeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public VehicleToMfrBodyCodeController(IVehicleToMfrBodyCodeApplicationService vehicleToMfrBodyCodeApplicationService, IApplicationLogger applicationLogger)
        {
            _vehicleToMfrBodyCodeApplicationService = vehicleToMfrBodyCodeApplicationService;
            _applicationLogger = applicationLogger;
        }
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<VehicleToMfrBodyCode> vehicleToMfrBodyCodes = await _vehicleToMfrBodyCodeApplicationService.GetAllAsync(10);
            List<VehicleToMfrBodyCodeViewModel> vehicleToMfrBodyCode = Mapper.Map<List<VehicleToMfrBodyCodeViewModel>>(vehicleToMfrBodyCodes);
            return Ok(vehicleToMfrBodyCode);
        }
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedVehicleToMfrBodyCode = await _vehicleToMfrBodyCodeApplicationService.GetAsync(id);

            return Ok(selectedVehicleToMfrBodyCode);
        }
        [Route("mfrBodyCode/{mfrBodyCodeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByMfrBodyCodeId(int mfrBodyCodeId)
        {
            List<VehicleToMfrBodyCode> vehicleToMfrBodyCodes = await _vehicleToMfrBodyCodeApplicationService.GetAsync(item => item.MfrBodyCode.Id.Equals(mfrBodyCodeId));

            List<VehicleToMfrBodyCodeViewModel> vehicleToMfrBodyCodeList = Mapper.Map<List<VehicleToMfrBodyCodeViewModel>>(vehicleToMfrBodyCodes);
            return Ok(vehicleToMfrBodyCodeList);
        }
        [Route("~/vehicles/{vehicleId:int}/vehicletomfrbodycodes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(int vehicleId)
        {
            var vehicleToMfrBodyCodes = await _vehicleToMfrBodyCodeApplicationService.GetAsync(item => item.VehicleId == vehicleId);
            List<VehicleToMfrBodyCodeViewModel> vehicleToMfrBodyCodeList = vehicleToMfrBodyCodes.Select(item =>
                new VehicleToMfrBodyCodeViewModel()
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
                    MfrBodyCode = new MfrBodyCodeViewModel()
                    {
                        Id = item.MfrBodyCodeId,
                        Name = item.MfrBodyCode.Name,
                        VehicleToMfrBodyCodeCount = 0,
                    },
                    ChangeRequestId = item.ChangeRequestId
                }).ToList();
            //Mapper.Map<List<VehicleToMfrBodyCodeViewModel>>(vehicleToMfrBodyCodes);

            return Ok(vehicleToMfrBodyCodeList);
        }

        [Route("~/vehicles/{vehicleIds}/mfrBodyCodes/{mfrBodyCodeIds}/vehicletomfrbodycodes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByVehicleId(string vehicleIds, string mfrBodyCodeIds)
        {
            List<VehicleToMfrBodyCode> vehicleToMfrBodyCodes = null;

            if (!string.IsNullOrWhiteSpace(vehicleIds) && !string.IsNullOrWhiteSpace(mfrBodyCodeIds))
            {
                string[] vehicleIdArray = vehicleIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] mfrBodyCodeIdArray = mfrBodyCodeIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (vehicleIdArray.Length > 0 && mfrBodyCodeIdArray.Length > 0)
                {
                    vehicleToMfrBodyCodes = await _vehicleToMfrBodyCodeApplicationService.GetAsync(item => vehicleIdArray.Any(v => v == item.VehicleId.ToString())
                    && mfrBodyCodeIdArray.Any(v => v == item.MfrBodyCodeId.ToString()));
                }
            }
            List<VehicleToMfrBodyCodeViewModel> vehicleToMfrBodyCodeViewModel = Mapper.Map<List<VehicleToMfrBodyCodeViewModel>>(vehicleToMfrBodyCodes);

            return Ok(vehicleToMfrBodyCodeViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleToMfrBodyCodeInputModel model)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _vehicleToMfrBodyCodeApplicationService.AddAsync(new VehicleToMfrBodyCode()
            {
                Id = model.Id,
                MfrBodyCodeId = model.MfrBodyCode.Id,
                VehicleId = model.Vehicle.Id
            }, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, VehicleToMfrBodyCodeInputModel model)
        {
            var changeRequestId = await _vehicleToMfrBodyCodeApplicationService.UpdateAsync(new VehicleToMfrBodyCode()
            {
                Id = model.Id,
                VehicleId = model.Vehicle.Id,
                MfrBodyCodeId = model.MfrBodyCode.Id
            }, id,CurrentUser.Email);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, VehicleToMfrBodyCodeInputModel vehicleToMfrBodyCodeInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleToMfrBodyCodeInputModel.Comment };
            var attachments = SetUpAttachmentsModels(vehicleToMfrBodyCodeInputModel.Attachments);
            var changeRequestId = await _vehicleToMfrBodyCodeApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleToMfrBodyCode> changeRequestStagingVehicleToMfrBodyCodeModel = await this._vehicleToMfrBodyCodeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleToMfrBodyCodeViewModel changeRequestStagingVehicleToMfrBodyCodeViewModel = Mapper.Map<ChangeRequestStagingVehicleToMfrBodyCodeViewModel>(changeRequestStagingVehicleToMfrBodyCodeModel);

            SetUpChangeRequestReview(changeRequestStagingVehicleToMfrBodyCodeViewModel.StagingItem.Status,
                changeRequestStagingVehicleToMfrBodyCodeViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleToMfrBodyCodeViewModel);
                        // return view model
            return Ok(changeRequestStagingVehicleToMfrBodyCodeViewModel);
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
                this._vehicleToMfrBodyCodeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            return Ok(isSubmitted);
        }
    }
}

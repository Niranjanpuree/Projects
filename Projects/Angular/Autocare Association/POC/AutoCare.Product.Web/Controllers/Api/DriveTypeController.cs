using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("driveTypes")]
    public class DriveTypeController : ApiControllerBase
    {
        private readonly IDriveTypeApplicationService _driveTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        private readonly ITextSerializer _serializer;

        public DriveTypeController(IDriveTypeApplicationService driveTypeApplicationService, IApplicationLogger applicationLogger, ITextSerializer serializer)
        {
            _driveTypeApplicationService = driveTypeApplicationService;
            _applicationLogger = applicationLogger;
            _serializer = serializer;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<DriveType> driveTypes = await _driveTypeApplicationService.GetAllAsync();

            IEnumerable<DriveTypeViewModel> driveTypeList = Mapper.Map<IEnumerable<DriveTypeViewModel>>(driveTypes);

            return Ok(driveTypeList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedDriveType = await _driveTypeApplicationService.GetAsync(id);

                var driveType = Mapper.Map<DriveTypeViewModel>(selectedDriveType);

                return Ok(driveType);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(DriveTypeInputModel driveTypeInputModel)
        {
            DriveType driveType = new DriveType()
            {
                Id = driveTypeInputModel.Id,
                Name = driveTypeInputModel.Name
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = driveTypeInputModel.Comment };
            var attachments = SetUpAttachmentsModels(driveTypeInputModel.Attachments);
            var changeRequestId = await _driveTypeApplicationService.AddAsync(driveType, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, DriveTypeInputModel driveTypeInputModel)
        {
            DriveType driveType = new DriveType()
            {
                Id = driveTypeInputModel.Id,
                Name = driveTypeInputModel.Name,
                VehicleToDriveTypeCount = driveTypeInputModel.VehicleToDriveTypeCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = driveTypeInputModel.Comment };
            var attachments = SetUpAttachmentsModels(driveTypeInputModel.Attachments);
            var changeRequestId = await _driveTypeApplicationService.UpdateAsync(driveType, driveType.Id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("replace/{id:int}")]
        public async Task<IHttpActionResult> Replace(int id, DriveTypeInputModel model)
        {
            DriveType driveType = new DriveType()
            {
                Id = model.Id,
                Name = model.Name,
                VehicleToDriveTypes = model.VehicleToDriveTypes.Select(item => new VehicleToDriveType
                {
                    DriveTypeId = item.DriveTypeId,
                    Id = item.Id,
                    VehicleId = item.VehicleId
                }).ToList(),

            };

            CommentsStagingModel comment = new CommentsStagingModel { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _driveTypeApplicationService.ReplaceAsync(driveType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("pendingChangeRequests")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingChangeRequests()
        {
            var pendingDriveTypes = await _driveTypeApplicationService.GetPendingAddChangeRequests(null);

            List<DriveTypeViewModel> driveTypeViewModels = Mapper.Map<List<DriveTypeViewModel>>(pendingDriveTypes);

            return Ok(driveTypeViewModels);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, DriveTypeInputModel model)
        {
            DriveType driveType = new DriveType()
            {
                Id = model.Id,
                VehicleToDriveTypeCount = model.VehicleToDriveTypeCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment, };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _driveTypeApplicationService.DeleteAsync(driveType, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            DriveTypeChangeRequestStagingModel changeRequestStagingDriveTypeModel =
                 await this._driveTypeApplicationService.GetChangeRequestStaging(changeRequestId);

            ChangeRequestStagingDriveTypeViewModel changeRequestStagingDriveTypeViewModel = Mapper.Map<ChangeRequestStagingDriveTypeViewModel>(changeRequestStagingDriveTypeModel);

            SetUpChangeRequestReview(changeRequestStagingDriveTypeViewModel.StagingItem.Status,
                changeRequestStagingDriveTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingDriveTypeViewModel);
                        return Ok(changeRequestStagingDriveTypeViewModel);
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
                this._driveTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

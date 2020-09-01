using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.ErrorMessage;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicleTypes")]
    public class VehicleTypeController : ApiControllerBase
    {
        private readonly IVehicleTypeApplicationService _vehicleTypeApplicationService;

        public VehicleTypeController(IVehicleTypeApplicationService vehicleTypeApplicationService)
        {
            _vehicleTypeApplicationService = vehicleTypeApplicationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var vehicleTypes = await _vehicleTypeApplicationService.GetAllAsync();
            IEnumerable<VehicleTypeViewModel> vehicleTypeList = Mapper.Map<IEnumerable<VehicleTypeViewModel>>(vehicleTypes);

            return Ok(vehicleTypeList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedVehicleType = await _vehicleTypeApplicationService.GetAsync(id);

            VehicleTypeViewModel vehicleType = Mapper.Map<VehicleTypeViewModel>(selectedVehicleType);

            return Ok(vehicleType);
        }

        [Route("~/vehicleTypes/search/{vehicleTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string vehicleTypeNameFilter)
        {
            if(string.IsNullOrWhiteSpace(vehicleTypeNameFilter))
            {
                return await this.Get();
            }

            List<VehicleType> vehicleTypes = await _vehicleTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(vehicleTypeNameFilter.ToLower()));

            IEnumerable<VehicleTypeViewModel> vehicleTypeList = Mapper.Map<IEnumerable<VehicleTypeViewModel>>(vehicleTypes);

            return Ok(vehicleTypeList);
        }

        [Route("~/vehicleTypes/search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string vehicleTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(vehicleTypeNameFilter))
            {
                return await this.Get();
            }

            List<VehicleType> vehicleTypes = await _vehicleTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(vehicleTypeNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<VehicleTypeViewModel> vehicleTypeList = Mapper.Map<IEnumerable<VehicleTypeViewModel>>(vehicleTypes);

            return Ok(vehicleTypeList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleTypeInputModel model)
        {
            if (model == null)
            {
                return BadRequest(ErrorMessage.Type.RequiredTypeInputModel);
            }

            if (model.Name == null)
            {
                return BadRequest(ErrorMessage.Type.RequiredType);
            }

            VehicleType type = new VehicleType()
            {
                Id = model.Id,
                Name = model.Name,
                VehicleTypeGroupId = model.VehicleTypeGroupId
            };

            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = model.Comment
            };

            var attachments = SetUpAttachmentsModels(model.Attachments);

            var changeRequestId = await _vehicleTypeApplicationService.AddAsync(type, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, VehicleTypeInputModel m)
        {
            if (m == null)
            {
                return BadRequest(ErrorMessage.Type.RequiredTypeInputModel);
            }

            if (m.Name == null)
            {
                return BadRequest(ErrorMessage.Type.RequiredType);
            }

            VehicleType type = new VehicleType()
            {
                Id = m.Id,
                Name = m.Name,
                VehicleTypeGroupId = m.VehicleTypeGroupId,
                VehicleCount = m.VehicleCount,
                BaseVehicleCount = m.BaseVehicleCount,
                ModelCount = m.ModelCount
            };

            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = m.Comment
            };

            var attachments = SetUpAttachmentsModels(m.Attachments);

            var changeRequestId = await _vehicleTypeApplicationService.UpdateAsync(type, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("~/vehicleTypes/delete/{id:int}")]
       public async Task<IHttpActionResult> Delete(int id,VehicleTypeInputModel vehicleInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleInputModel.Comment };

            var attachments = SetUpAttachmentsModels(vehicleInputModel.Attachments);

            var changeRequestId = await _vehicleTypeApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("~/vehicleTypes/changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleType> changeRequestStagingVehicleTypeModel = await this._vehicleTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleTypeViewModel changeRequestStagingVehicleTypeViewModel = Mapper.Map<ChangeRequestStagingVehicleTypeViewModel>(changeRequestStagingVehicleTypeModel);

            SetUpChangeRequestReview(changeRequestStagingVehicleTypeViewModel.StagingItem.Status,
                changeRequestStagingVehicleTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleTypeViewModel);
            
            // return view model
            return Ok(changeRequestStagingVehicleTypeViewModel);
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
                this._vehicleTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
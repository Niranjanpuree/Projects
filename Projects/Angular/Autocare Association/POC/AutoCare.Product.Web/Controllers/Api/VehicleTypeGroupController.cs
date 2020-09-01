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
using CommentsStagingModel = AutoCare.Product.Application.Models.BusinessModels.CommentsStagingModel;


namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicleTypeGroups")]
    public class VehicleTypeGroupController : ApiControllerBase
    {
        private readonly IVehicleTypeGroupApplicationService _vehicleTypeGroupApplicationService;

        public VehicleTypeGroupController(IVehicleTypeGroupApplicationService vehicleTypeGroupApplicationService)
        {
            _vehicleTypeGroupApplicationService = vehicleTypeGroupApplicationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var vehicleTypeGroups = await _vehicleTypeGroupApplicationService.GetAllAsync();
            IEnumerable<VehicleTypeGroupViewModel> vehicleTypeGroupList = Mapper.Map<IEnumerable<VehicleTypeGroupViewModel>>(vehicleTypeGroups);

            return Ok(vehicleTypeGroupList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedVehicleTypeGroup = await _vehicleTypeGroupApplicationService.GetAsync(id);

            VehicleTypeGroupViewModel vehicleTypeGroup = Mapper.Map<VehicleTypeGroupViewModel>(selectedVehicleTypeGroup);

            return Ok(vehicleTypeGroup);
        }

        [Route("search/{vehicleTypeGroupNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string vehicleTypeGroupNameFilter)
        {
            if (string.IsNullOrWhiteSpace(vehicleTypeGroupNameFilter))
            {
                return await this.Get();
            }

            List<VehicleTypeGroup> vehicleTypeGroups = await _vehicleTypeGroupApplicationService.GetAsync(m => m.Name.ToLower().Contains(vehicleTypeGroupNameFilter.ToLower()));

            IEnumerable<VehicleTypeGroupViewModel> vehicleTypeGroupList = Mapper.Map<IEnumerable<VehicleTypeGroupViewModel>>(vehicleTypeGroups);

            return Ok(vehicleTypeGroupList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string vehicleTypeGroupNameFilter)
        {
            if (string.IsNullOrWhiteSpace(vehicleTypeGroupNameFilter))
            {
                return await this.Get();
            }

            List<VehicleTypeGroup> vehicleTypeGroups = await _vehicleTypeGroupApplicationService.GetAsync(m => m.Name.ToLower().Contains(vehicleTypeGroupNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<VehicleTypeGroupViewModel> vehicleTypeGroupList = Mapper.Map<IEnumerable<VehicleTypeGroupViewModel>>(vehicleTypeGroups);

            return Ok(vehicleTypeGroupList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(VehicleTypeGroupInputModel model)
        {
            if (model == null)
            {
                return BadRequest(ErrorMessage.TypeGroup.RequiredTypeGroupInputModel);
            }

            if (model.Name == null)
            {
                return BadRequest(ErrorMessage.TypeGroup.RequiredTypeGroup);
            }

            VehicleTypeGroup typeGroup = new VehicleTypeGroup()
            {
                Id = model.Id,
                Name = model.Name,
            };

            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = model.Comment
            };

            var attachments = SetUpAttachmentsModels(model.Attachments);

            var changeRequestId = await _vehicleTypeGroupApplicationService.AddAsync(typeGroup, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, VehicleTypeGroupInputModel m)
        {
            if (m == null)
            {
                return BadRequest(ErrorMessage.TypeGroup.RequiredTypeGroupInputModel);
            }

            if (m.Name == null)
            {
                return BadRequest(ErrorMessage.TypeGroup.RequiredTypeGroup);
            }

            VehicleTypeGroup typeGroup = new VehicleTypeGroup()
            {
                Id = m.Id,
                Name = m.Name,
                VehicleTypeCount = m.VehicleTypeCount
            };

            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = m.Comment
            };

            var attachments = SetUpAttachmentsModels(m.Attachments);

            var changeRequestId = await _vehicleTypeGroupApplicationService.UpdateAsync(typeGroup, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }
        
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id, VehicleTypeGroupInputModel vehicleInputModel)
        {
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = vehicleInputModel.Comment };

            var attachments = SetUpAttachmentsModels(vehicleInputModel.Attachments);

            var changeRequestId = await _vehicleTypeGroupApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<VehicleTypeGroup> changeRequestStagingVehicleTypeGroupModel = await this._vehicleTypeGroupApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingVehicleTypeGroupViewModel changeRequestStagingVehicleTypeGroupViewModel = Mapper.Map<ChangeRequestStagingVehicleTypeGroupViewModel>(changeRequestStagingVehicleTypeGroupModel);

                      
            // setup change request review properties
            ReviewViewModel review = SetUpChangeRequestReview(changeRequestStagingVehicleTypeGroupViewModel.StagingItem.Status,
                changeRequestStagingVehicleTypeGroupViewModel.StagingItem.SubmittedBy, changeRequestStagingVehicleTypeGroupViewModel);
           
            // return view model
            return Ok(changeRequestStagingVehicleTypeGroupViewModel);
        }

        [HttpPost]
        [Route("changeRequestStaging/{changeRequestId:int}")]
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
                    AddedBy = CurrentUser.CustomerId,
                    CreatedDatetime = changeRequestReview.ReviewComment.CreatedDatetime
                },
                //NOTE: CR Input model uses IList unlike other models hence the addition of .ToList()
                Attachments = SetUpAttachmentsModels(changeRequestReview.Attachments?.ToList())
            };

            // submit review
            bool isSubmitted =
                await
                    this._vehicleTypeGroupApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);
            return Ok(isSubmitted);

        }
    }
}
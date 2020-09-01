using AutoCare.Product.Application.ApplicationServices;
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
    [RoutePrefix("submodels")]
    public class SubModelController : ApiControllerBase
    {
        private readonly ISubModelApplicationService _subModelApplicationService;
        private readonly ILogger _logger;
        public SubModelController(ISubModelApplicationService subModelApplicationService, ILogger logger)
        {
            _subModelApplicationService = subModelApplicationService;
            _logger = logger;
        }

        [Route("")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get()
        {
            var submodel = await _subModelApplicationService.GetAllAsync();
            var submodelList = Mapper.Map<IEnumerable<SubModelViewModel>>(submodel);
            _logger.Info("GetSubmodel method called");
            return Ok(submodelList);
        }

        [Route("~/submodels/count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
            List<SubModel> subModels = await _subModelApplicationService.GetAllAsync(count);

            IEnumerable<SubModelViewModel> subModelsList = Mapper.Map<IEnumerable<SubModelViewModel>>(subModels);

            return Ok(subModelsList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetSubModelById(int id)
        {
            var selectedSubModel = await _subModelApplicationService.GetAsync(id);
            SubModelViewModel subModel = Mapper.Map<SubModelViewModel>(selectedSubModel);
            _logger.Info("GetSubmodelById method called");
            return Ok(subModel);
        }

        [Route("~/submodels/search/{subModelNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string subModelNameFilter)
        {
            if (string.IsNullOrWhiteSpace(subModelNameFilter))
            {
                return await this.Get();
            }

            List<SubModel> subModels = await _subModelApplicationService.GetAsync(m => m.Name.ToLower().Contains(subModelNameFilter.ToLower()));

            IEnumerable<SubModelViewModel> subModelsList = Mapper.Map<IEnumerable<SubModelViewModel>>(subModels);

            return Ok(subModelsList);
        }

        [Route("~/baseVehicles/{baseVehicleId:int}/subModels")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSubModelsByBaseVehicleId(int baseVehicleId)
        {
            List<SubModel> subModels = null;
            if (baseVehicleId != default(int))
            {
                subModels = await _subModelApplicationService.GetAsync(item => item.Vehicles.Any(v => v.BaseVehicleId == baseVehicleId));
            }
            IEnumerable<SubModelViewModel> subModelsList = Mapper.Map<IEnumerable<SubModelViewModel>>(subModels);

            return Ok(subModelsList);
        }

        [Route("~/makes/{makeIds}/models/{modelIds}/subModels")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByMakeIdsAndModelIds(string makeIds, string modelIds)
        {
            List<SubModel> subModels = null;
            if (!string.IsNullOrWhiteSpace(makeIds) && !string.IsNullOrWhiteSpace(modelIds))
            {
                string[] makeIdArray = makeIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] modelIdArray = modelIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (makeIdArray.Length > 0 && modelIdArray.Length > 0)
                {
                    subModels = await _subModelApplicationService.GetAsync(item => item.Vehicles.Any(b => makeIdArray.Contains(b.BaseVehicle.MakeId.ToString())
                    && modelIdArray.Contains(b.BaseVehicle.ModelId.ToString())));
                }
            }
            IEnumerable<SubModelViewModel> subModelsList = Mapper.Map<IEnumerable<SubModelViewModel>>(subModels);

            return Ok(subModelsList);
        }

        [Route("~/submodels/search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string subModelNameFilter)
        {
            if (string.IsNullOrWhiteSpace(subModelNameFilter))
            {
                return await this.Get();
            }

            List<SubModel> subModels = await _subModelApplicationService.GetAsync(m => m.Name.ToLower().Contains(subModelNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<SubModelViewModel> subModelsList = Mapper.Map<IEnumerable<SubModelViewModel>>(subModels);

            return Ok(subModelsList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(SubModelInputModel newSubModel)
        {
            SubModel subModel = new SubModel() { Id = newSubModel.Id, Name = newSubModel.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = newSubModel.Comment };
            var attachments = SetUpAttachmentsModels(newSubModel.Attachments);

            var changeRequestId = await _subModelApplicationService.AddAsync(subModel, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("~/submodels/delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, SubModelInputModel deleteSubModel)
        {
            SubModel subModel = new SubModel() { Id = deleteSubModel.Id };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = deleteSubModel.Comment };

            var attachments = SetUpAttachmentsModels(deleteSubModel.Attachments);

            var changeRequestId = await _subModelApplicationService.DeleteAsync(subModel, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, SubModelInputModel updateSubModel)
        {
            SubModel subModel = new SubModel()
            {
                Id = updateSubModel.Id,
                Name = updateSubModel.Name,
                VehicleCount = updateSubModel.VehicleCount

            };
           
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = updateSubModel.Comment };

            var attachments = SetUpAttachmentsModels(updateSubModel.Attachments);

            var changeRequestId = await _subModelApplicationService.UpdateAsync(subModel, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        
        [Route("~/submodels/changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<SubModel> changeRequestStagingSubModelModel = await this._subModelApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingSubModelViewModel changeRequestStagingSubModelViewModel = Mapper.Map<ChangeRequestStagingSubModelViewModel>(changeRequestStagingSubModelModel);

             SetUpChangeRequestReview(changeRequestStagingSubModelViewModel.StagingItem.Status,
                changeRequestStagingSubModelViewModel.StagingItem.SubmittedBy, changeRequestStagingSubModelViewModel);
                       // return view model
            return Ok(changeRequestStagingSubModelViewModel);
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
                this._subModelApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Infrastructure.ErrorMessage;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("models")]
    public class ModelController : ApiControllerBase
    {
        private readonly IModelApplicationService _modelApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public ModelController(IModelApplicationService modelApplicationService,
            IApplicationLogger applicationLogger)
        {
            _modelApplicationService = modelApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Model> models = await _modelApplicationService.GetAllAsync();

            IEnumerable<ModelViewModel> modelsList = Mapper.Map<IEnumerable<ModelViewModel>>(models);

            return Ok(modelsList);
        }

        [Route("~/models/count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
            //if (count <= 0) count = 20;
            List<Model> models = await _modelApplicationService.GetAllAsync(count);

            IEnumerable<ModelViewModel> modelsList = Mapper.Map<IEnumerable<ModelViewModel>>(models);

            return Ok(modelsList);
        }

        [Route("~/makes/{makeIds}/models")]
        [HttpGet]
        public async Task<IHttpActionResult> GetModelsByMakeIds(string makeIds)
        {
            List<Model> models = null;
            if (!string.IsNullOrWhiteSpace(makeIds))
            {
                string[] makeIdArray = makeIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (makeIdArray.Length > 0)
                {
                    models = await _modelApplicationService.GetAsync(item => item.BaseVehicles.Any(b => makeIdArray.Contains(b.MakeId.ToString())));
                }
            }
            IEnumerable<ModelViewModel> modelsList = Mapper.Map<IEnumerable<ModelViewModel>>(models);

            return Ok(modelsList);
        }

        // GET api/models/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetModelById(int id)
        {
            var selectedModel = await _modelApplicationService.GetAsync(id);

            ModelDetailViewModel model = Mapper.Map<ModelDetailViewModel>(selectedModel);

            return Ok(model);
        }

        [Route("~/models/search/{modelNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string modelNameFilter)
        {
            if (string.IsNullOrWhiteSpace(modelNameFilter))
            {
                return await this.Get();
            }

            List<Model> models = await _modelApplicationService.GetAsync(m => m.Name.ToLower().Contains(modelNameFilter.ToLower()), 20);

            IEnumerable<ModelViewModel> modelsList = Mapper.Map<IEnumerable<ModelViewModel>>(models);

            return Ok(modelsList);
        }

        [Route("~/models/search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string modelNameFilter)
        {
            if (string.IsNullOrWhiteSpace(modelNameFilter))
            {
                return await this.Get();
            }

            List<Model> models = await _modelApplicationService.GetAsync(m => m.Name.ToLower().Contains(modelNameFilter.ToLower()) && m.DeleteDate == null, 20);

            IEnumerable<ModelViewModel> modelsList = Mapper.Map<IEnumerable<ModelViewModel>>(models);

            return Ok(modelsList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(ModelInputModel newModel)
        {
            if (newModel == null)
            {
                return BadRequest(ErrorMessage.Model.RequiredModelInputModel);
            }
            if (string.IsNullOrWhiteSpace(newModel.Name))
            {
                return BadRequest(ErrorMessage.Model.RequiredModelName);
            }
            //Create model and comments
            Model model = new Model()
            {
                Id = newModel.Id,
                Name = newModel.Name,
                VehicleTypeId = newModel.VehicleTypeId
            };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = newModel.Comment
            };
            var attachments = SetUpAttachmentsModels(newModel.Attachments);

            var changeRequestId = await _modelApplicationService.AddAsync(model, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, ModelInputModel model)
        {
            if (id == default(int))
            {
                return BadRequest(ErrorMessage.Model.InvalidModelId);
            }
            if (model == null)
            {
                return BadRequest(ErrorMessage.Model.RequiredModelInputModel);
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest(ErrorMessage.Model.RequiredModelName);
            }
            //Create Model and Comments
            Model _model = new Model()
            {
                Id = model.Id,
                Name = model.Name,
                VehicleTypeId = model.VehicleTypeId,
                BaseVehicleCount = model.BaseVehicleCount,
                VehicleCount = model.VehicleCount
            };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = model.Comment
            };

            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _modelApplicationService.UpdateAsync(_model, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("~/models/delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, ModelInputModel modelInputModel)
        {
            //System.Threading.Thread.Sleep(2000);
            //return Ok(-1);
            // create model and list of comments
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = modelInputModel.Comment };
            var attachments = SetUpAttachmentsModels(modelInputModel.Attachments);
            var changeRequestId = await _modelApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("~/models/changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<Model> changeRequestStagingModelModel = await this._modelApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingModelViewModel changeRequestStagingModelViewModel = Mapper.Map<ChangeRequestStagingModelViewModel>(changeRequestStagingModelModel);

            SetUpChangeRequestReview(changeRequestStagingModelViewModel.StagingItem.Status,
                changeRequestStagingModelViewModel.StagingItem.SubmittedBy, changeRequestStagingModelViewModel);
                       
            return Ok(changeRequestStagingModelViewModel);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpPost()]
        public async Task<IHttpActionResult> Post(int changeRequestId, ChangeRequestReviewInputModel changeRequestReview)
        {
            // create change-request-review-model
            ChangeRequestReviewModel reviewModel = new ChangeRequestReviewModel()
            {
                ChangeRequestId = changeRequestReview.ChangeRequestId,
                ReviewedBy = CurrentUser.CustomerId,// changeRequestReview.ReviewedBy,
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
                this._modelApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
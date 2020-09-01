using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.ViewModels;
using AutoCare.Product.Web.Models.InputModels;
using AutoMapper;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("engineDesignations")]
    public class EngineDesignationController : ApiControllerBase
    {
        private readonly IEngineDesignationApplicationService _engineDesignationApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public EngineDesignationController(IApplicationLogger applicationLogger, IEngineDesignationApplicationService engineDesignationApplicationService)
        {
            _applicationLogger = applicationLogger;
            _engineDesignationApplicationService = engineDesignationApplicationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var engineDesignations = await _engineDesignationApplicationService.GetAllAsync();
            IEnumerable<EngineDesignationViewModel> engineDesignationList = Mapper.Map<IEnumerable<EngineDesignationViewModel>>(engineDesignations);

            return Ok(engineDesignationList);
        }

        // GET api/EngineDesignation/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedEngineDesignation = await _engineDesignationApplicationService.GetAsync(id);
            EngineDesignationDetailViewModel engineDesignation = Mapper.Map<EngineDesignationDetailViewModel>(selectedEngineDesignation);

            return Ok(engineDesignation);
        }


        [Route("search/{engineDesignationNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string engineDesignationNameFilter)
        {
            if (string.IsNullOrWhiteSpace(engineDesignationNameFilter))
            {
                return await this.Get();
            }

            List<EngineDesignation> engineDesignations = await _engineDesignationApplicationService.GetAsync(m => m.EngineDesignationName.ToLower().Contains(engineDesignationNameFilter.ToLower()));

            IEnumerable<EngineDesignationViewModel> engineDesignationList = Mapper.Map<IEnumerable<EngineDesignationViewModel>>(engineDesignations);

            return Ok(engineDesignationList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string engineDesignationNameFilter)
        {
            if (string.IsNullOrWhiteSpace(engineDesignationNameFilter))
            {
                return await this.Get();
            }

            List<EngineDesignation> engineDesignations = await _engineDesignationApplicationService.GetAsync(m => m.EngineDesignationName.ToLower().Contains(engineDesignationNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<EngineDesignationViewModel> engineDesignationList = Mapper.Map<IEnumerable<EngineDesignationViewModel>>(engineDesignations);

            return Ok(engineDesignationList);
        }

        [Route("engineDesignation/{engineDesignationId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByEngineDesignationId(int engineDesignationId)
        {
            List<EngineDesignation> engineDesignations = null;
            engineDesignations = await _engineDesignationApplicationService.GetAsync(item => item.EngineConfigs.Any(bc =>
                bc.EngineDesignationId == engineDesignationId));
            IEnumerable<EngineDesignationViewModel> engineDesignationList = Mapper.Map<IEnumerable<EngineDesignationViewModel>>(engineDesignations);
            return Ok(engineDesignationList);
        }
        //[Route("engineDesignationByEngineDesignationId/{engineDesignationId:int}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetByBedLengthId(int engineDesignationId)
        //{
        //    List<EngineDesignation> engineDesignations = null;
        //    engineDesignations = await _engineDesignationApplicationService.GetAsync(item => item.EngineConfigs.Any(bc =>
        //        bc.EngineDesignationId == engineDesignationId));
        //    IEnumerable<EngineDesignationViewModel> engineDesignationList = Mapper.Map<IEnumerable<EngineDesignationViewModel>>(engineDesignations);
        //    return Ok(engineDesignationList);
        //}

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(EngineDesignationInputModel model)
        {
            // create bed type and list of comments
            EngineDesignation engineDesignation = new EngineDesignation() { Id = model.Id, EngineDesignationName = model.EngineDesignationName };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _engineDesignationApplicationService.AddAsync(engineDesignation, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, EngineDesignationInputModel model)
        {
            EngineDesignation engineDesignation = new EngineDesignation() { Id = model.Id, EngineDesignationName = model.EngineDesignationName };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _engineDesignationApplicationService.DeleteAsync(engineDesignation, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, EngineDesignationInputModel engineDesignationInputModel)
        {
            EngineDesignation engineDesignation = new EngineDesignation()
            {
                Id = id,
                EngineDesignationName = engineDesignationInputModel.EngineDesignationName,
                EngineConfigCount = engineDesignationInputModel.EngineConfigCount,
                VehicleToEngineConfigCount = engineDesignationInputModel.VehicleToEngineConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = engineDesignationInputModel.Comment };
            var attachments = SetUpAttachmentsModels(engineDesignationInputModel.Attachments);
            var changeRequestId = await _engineDesignationApplicationService.UpdateAsync(engineDesignation, engineDesignation.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<EngineDesignation> changeRequestStagingEngineDesignationModel =
                await this._engineDesignationApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingEngineDesignationViewModel changeRequestStagingEngineDesignationViewModel = Mapper.Map<ChangeRequestStagingEngineDesignationViewModel>(changeRequestStagingEngineDesignationModel);

             SetUpChangeRequestReview(changeRequestStagingEngineDesignationViewModel.StagingItem.Status,
            changeRequestStagingEngineDesignationViewModel.StagingItem.SubmittedBy, changeRequestStagingEngineDesignationViewModel);
           
            return Ok(changeRequestStagingEngineDesignationViewModel);
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
                this._engineDesignationApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
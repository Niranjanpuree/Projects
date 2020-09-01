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
    [RoutePrefix("engineVersions")]
    public class EngineVersionController : ApiControllerBase
    {
        private readonly IEngineVersionApplicationService _engineVersionApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public EngineVersionController(IApplicationLogger applicationLogger, IEngineVersionApplicationService engineVersionApplicationService)
        {
            _applicationLogger = applicationLogger;
            _engineVersionApplicationService = engineVersionApplicationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var engineVersions = await _engineVersionApplicationService.GetAllAsync();
            IEnumerable<EngineVersionViewModel> engineVersionList = Mapper.Map<IEnumerable<EngineVersionViewModel>>(engineVersions);

            return Ok(engineVersionList);
        }

        // GET api/EngineVersion/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedEngineVersion = await _engineVersionApplicationService.GetAsync(id);
            EngineVersionDetailViewModel engineVersion = Mapper.Map<EngineVersionDetailViewModel>(selectedEngineVersion);

            return Ok(engineVersion);
        }


        [Route("search/{engineVersionNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string engineVersionNameFilter)
        {
            if (string.IsNullOrWhiteSpace(engineVersionNameFilter))
            {
                return await this.Get();
            }

            List<EngineVersion> engineVersions = await _engineVersionApplicationService.GetAsync(m => m.EngineVersionName.ToLower().Contains(engineVersionNameFilter.ToLower()));

            IEnumerable<EngineVersionViewModel> engineVersionList = Mapper.Map<IEnumerable<EngineVersionViewModel>>(engineVersions);

            return Ok(engineVersionList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string engineVersionNameFilter)
        {
            if (string.IsNullOrWhiteSpace(engineVersionNameFilter))
            {
                return await this.Get();
            }

            List<EngineVersion> engineVersions = await _engineVersionApplicationService.GetAsync(m => m.EngineVersionName.ToLower().Contains(engineVersionNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<EngineVersionViewModel> engineVersionList = Mapper.Map<IEnumerable<EngineVersionViewModel>>(engineVersions);

            return Ok(engineVersionList);
        }

        [Route("engineVersion/{engineVersionId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByEngineVersionId(int engineVersionId)
        {
            List<EngineVersion> engineVersions = null;
            engineVersions = await _engineVersionApplicationService.GetAsync(item => item.EngineConfigs.Any(bc =>
                bc.EngineVersionId == engineVersionId));
            IEnumerable<EngineVersionViewModel> engineVersionList = Mapper.Map<IEnumerable<EngineVersionViewModel>>(engineVersions);
            return Ok(engineVersionList);
        }
        //[Route("engineVersionByEngineVersionId/{engineVersionId:int}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetByBedLengthId(int engineVersionId)
        //{
        //    List<EngineVersion> engineVersions = null;
        //    engineVersions = await _engineVersionApplicationService.GetAsync(item => item.EngineConfigs.Any(bc =>
        //        bc.EngineVersionId == engineVersionId));
        //    IEnumerable<EngineVersionViewModel> engineVersionList = Mapper.Map<IEnumerable<EngineVersionViewModel>>(engineVersions);
        //    return Ok(engineVersionList);
        //}

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(EngineVersionInputModel model)
        {
            // create bed type and list of comments
            EngineVersion engineVersion = new EngineVersion() { Id = model.Id, EngineVersionName = model.EngineVersionName };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _engineVersionApplicationService.AddAsync(engineVersion, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, EngineVersionInputModel model)
        {
            EngineVersion engineVersion = new EngineVersion() { Id = model.Id, EngineVersionName = model.EngineVersionName };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _engineVersionApplicationService.DeleteAsync(engineVersion, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, EngineVersionInputModel engineVersionInputModel)
        {
            EngineVersion engineVersion = new EngineVersion()
            {
                Id = id,
                EngineVersionName = engineVersionInputModel.EngineVersionName,
                EngineConfigCount = engineVersionInputModel.EngineConfigCount,
                VehicleToEngineConfigCount = engineVersionInputModel.VehicleToEngineConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = engineVersionInputModel.Comment };
            var attachments = SetUpAttachmentsModels(engineVersionInputModel.Attachments);
            var changeRequestId = await _engineVersionApplicationService.UpdateAsync(engineVersion, engineVersion.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<EngineVersion> changeRequestStagingEngineVersionModel =
                await this._engineVersionApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingEngineVersionViewModel changeRequestStagingEngineVersionViewModel = Mapper.Map<ChangeRequestStagingEngineVersionViewModel>(changeRequestStagingEngineVersionModel);

             SetUpChangeRequestReview(changeRequestStagingEngineVersionViewModel.StagingItem.Status,
            changeRequestStagingEngineVersionViewModel.StagingItem.SubmittedBy, changeRequestStagingEngineVersionViewModel);
           
            return Ok(changeRequestStagingEngineVersionViewModel);
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
                this._engineVersionApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
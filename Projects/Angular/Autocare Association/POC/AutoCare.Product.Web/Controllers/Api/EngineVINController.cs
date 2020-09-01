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
    [RoutePrefix("EngineVins")]
    public class EngineVinController : ApiControllerBase
    {
        private readonly IEngineVinApplicationService _EngineVinApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public EngineVinController(IApplicationLogger applicationLogger, IEngineVinApplicationService EngineVinApplicationService)
        {
            _applicationLogger = applicationLogger;
            _EngineVinApplicationService = EngineVinApplicationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var EngineVins = await _EngineVinApplicationService.GetAllAsync();
            IEnumerable<EngineVinViewModel> EngineVinList = Mapper.Map<IEnumerable<EngineVinViewModel>>(EngineVins);

            return Ok(EngineVinList);
        }

        // GET api/EngineVin/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedEngineVin = await _EngineVinApplicationService.GetAsync(id);
            EngineVinDetailViewModel EngineVin = Mapper.Map<EngineVinDetailViewModel>(selectedEngineVin);

            return Ok(EngineVin);
        }


        [Route("search/{EngineVinNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string EngineVinNameFilter)
        {
            if (string.IsNullOrWhiteSpace(EngineVinNameFilter))
            {
                return await this.Get();
            }

            List<EngineVin> EngineVins = await _EngineVinApplicationService.GetAsync(m => m.EngineVinName.ToLower().Contains(EngineVinNameFilter.ToLower()));

            IEnumerable<EngineVinViewModel> EngineVinList = Mapper.Map<IEnumerable<EngineVinViewModel>>(EngineVins);

            return Ok(EngineVinList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string EngineVinNameFilter)
        {
            if (string.IsNullOrWhiteSpace(EngineVinNameFilter))
            {
                return await this.Get();
            }

            List<EngineVin> EngineVins = await _EngineVinApplicationService.GetAsync(m => m.EngineVinName.ToLower().Contains(EngineVinNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<EngineVinViewModel> EngineVinList = Mapper.Map<IEnumerable<EngineVinViewModel>>(EngineVins);

            return Ok(EngineVinList);
        }

        [Route("EngineVin/{EngineVinId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByEngineVinId(int EngineVinId)
        {
            List<EngineVin> EngineVins = null;
            EngineVins = await _EngineVinApplicationService.GetAsync(item => item.EngineConfigs.Any(bc =>
                bc.EngineVinId == EngineVinId));
            IEnumerable<EngineVinViewModel> EngineVinList = Mapper.Map<IEnumerable<EngineVinViewModel>>(EngineVins);
            return Ok(EngineVinList);
        }
        //[Route("EngineVinByEngineVinId/{EngineVinId:int}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetByBedLengthId(int EngineVinId)
        //{
        //    List<EngineVin> EngineVins = null;
        //    EngineVins = await _EngineVinApplicationService.GetAsync(item => item.EngineConfigs.Any(bc =>
        //        bc.EngineVinId == EngineVinId));
        //    IEnumerable<EngineVinViewModel> EngineVinList = Mapper.Map<IEnumerable<EngineVinViewModel>>(EngineVins);
        //    return Ok(EngineVinList);
        //}

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(EngineVinInputModel model)
        {
            // create bed type and list of comments
            EngineVin EngineVin = new EngineVin() { Id = model.Id, EngineVinName = model.EngineVinName };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _EngineVinApplicationService.AddAsync(EngineVin, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, EngineVinInputModel model)
        {
            EngineVin EngineVin = new EngineVin() { Id = model.Id, EngineVinName = model.EngineVinName };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _EngineVinApplicationService.DeleteAsync(EngineVin, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, EngineVinInputModel EngineVinInputModel)
        {
            EngineVin EngineVin = new EngineVin()
            {
                Id = id,
                EngineVinName = EngineVinInputModel.EngineVinName,
                EngineConfigCount = EngineVinInputModel.EngineConfigCount,
                VehicleToEngineConfigCount = EngineVinInputModel.VehicleToEngineConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = EngineVinInputModel.Comment };
            var attachments = SetUpAttachmentsModels(EngineVinInputModel.Attachments);
            var changeRequestId = await _EngineVinApplicationService.UpdateAsync(EngineVin, EngineVin.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<EngineVin> changeRequestStagingEngineVinModel =
                await this._EngineVinApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingEngineVinViewModel changeRequestStagingEngineVinViewModel = Mapper.Map<ChangeRequestStagingEngineVinViewModel>(changeRequestStagingEngineVinModel);

             SetUpChangeRequestReview(changeRequestStagingEngineVinViewModel.StagingItem.Status,
            changeRequestStagingEngineVinViewModel.StagingItem.SubmittedBy, changeRequestStagingEngineVinViewModel);
           
            return Ok(changeRequestStagingEngineVinViewModel);
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
                this._EngineVinApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
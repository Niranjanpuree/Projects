using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Logging;
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
    [RoutePrefix("bedLengths")]
    public class BedLengthController : ApiControllerBase
    {
        private readonly IBedLengthApplicationService _bedLengthApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public BedLengthController(IBedLengthApplicationService bedLengthApplicationService, IApplicationLogger applicationLogger)
        {
            _bedLengthApplicationService = bedLengthApplicationService;
            _applicationLogger = applicationLogger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var bedlengths = await _bedLengthApplicationService.GetAllAsync();
            IEnumerable<BedLengthViewModel> bedLengthlists = Mapper.Map<IEnumerable<BedLengthViewModel>>(bedlengths);

            return Ok(bedLengthlists);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BedLengthInputModel model)
        {
            BedLength bedLength = new BedLength() { Id = model.Id, Length = model.Length, BedLengthMetric = model.BedLengthMetric };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedLengthApplicationService.AddAsync(bedLength, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedBedlength = await _bedLengthApplicationService.GetAsync(id);

            BedLengthDetailViewModel bedLength = Mapper.Map<BedLengthDetailViewModel>(selectedBedlength);

            return Ok(bedLength);
        }
        [Route("bedLength/{bedLengthId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBedLengthId(int bedLengthId)
        {
            List<BedLength> bedLengths = null;
            bedLengths = await _bedLengthApplicationService.GetAsync(item => item.BedConfigs.Any(bc =>
                bc.BedLengthId == bedLengthId));
            IEnumerable<BedLengthViewModel> bedLengthList = Mapper.Map<IEnumerable<BedLengthViewModel>>(bedLengths);
            return Ok(bedLengthList);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BedLengthInputModel bedLengthInputModel)
        {
            BedLength bedlength = new BedLength()
            {
                Id = bedLengthInputModel.Id,
                Length = bedLengthInputModel.Length,
                BedLengthMetric = bedLengthInputModel.BedLengthMetric,
                VehicleToBedConfigCount = bedLengthInputModel.VehicleToBedConfigCount,
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = bedLengthInputModel.Comment };
            var attachments = SetUpAttachmentsModels(bedLengthInputModel.Attachments);
            var changeRequestId = await _bedLengthApplicationService.UpdateAsync(bedlength, bedlength.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BedLengthInputModel model)
        {
            BedLength bedlength = new BedLength() { Id = model.Id, Length = model.Length, BedLengthMetric = model.BedLengthMetric };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedLengthApplicationService.DeleteAsync(bedlength, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string bedLengthNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bedLengthNameFilter))
            {
                return await this.Get();
            }

            List<BedLength> bedlengths = await _bedLengthApplicationService.GetAsync(m => m.Length.Contains(bedLengthNameFilter) && m.DeleteDate == null);

            IEnumerable<BedLengthViewModel> bedlengthlists = Mapper.Map<IEnumerable<BedLengthViewModel>>(bedlengths);

            return Ok(bedlengthlists);
        }

        [Route("search/{bedLengthNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string bedLengthNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bedLengthNameFilter))
            {
                return await this.Get();
            }

            List<BedLength> bedlength = await _bedLengthApplicationService.GetAsync(m => m.Length.Contains(bedLengthNameFilter));

            IEnumerable<BedLengthViewModel> bedlengthlists = Mapper.Map<IEnumerable<BedLengthViewModel>>(bedlength);

            return Ok(bedlengthlists);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<BedLength> changeRequestStagingBedLengthModel =
                await this._bedLengthApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingBedLengthViewModel changeRequestStagingBedLengthViewModel = Mapper.Map<ChangeRequestStagingBedLengthViewModel>(changeRequestStagingBedLengthModel);

             SetUpChangeRequestReview(changeRequestStagingBedLengthViewModel.StagingItem.Status,
                changeRequestStagingBedLengthViewModel.StagingItem.SubmittedBy, changeRequestStagingBedLengthViewModel);
           
            return Ok(changeRequestStagingBedLengthViewModel);
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
                this._bedLengthApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }


    }
}
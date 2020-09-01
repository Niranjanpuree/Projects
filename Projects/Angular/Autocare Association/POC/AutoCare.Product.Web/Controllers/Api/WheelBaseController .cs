using System;
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
    [RoutePrefix("wheelBase")]
    public class WheelBaseController : ApiControllerBase
    {
        private readonly IWheelBaseApplicationService _wheelBaseApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public WheelBaseController(IWheelBaseApplicationService wheelBaseApplicationService,
            IApplicationLogger applicationLogger)
        {
            _wheelBaseApplicationService = wheelBaseApplicationService;
            _applicationLogger = applicationLogger;
        }
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(WheelBaseInputModel model)
        {
            WheelBase wheelBase = new WheelBase()
            {
                Id = model.Id,
                Base = model.Base,
                WheelBaseMetric = model.WheelBaseMetric
            };
            CommentsStagingModel comment = new CommentsStagingModel() {Comment = model.Comment};
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId =
                await _wheelBaseApplicationService.AddAsync(wheelBase, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var wheelBase = await _wheelBaseApplicationService.GetAllAsync();
            IEnumerable<WheelBaseViewModel> wheelBaselists = Mapper.Map<IEnumerable<WheelBaseViewModel>>(wheelBase);

            return Ok(wheelBaselists);
        }


        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedWheelBase = await _wheelBaseApplicationService.GetAsync(id);

            WheelBaseViewModel wheelBase = Mapper.Map<WheelBaseDetailViewModel>(selectedWheelBase);

            return Ok(wheelBase);
        }

        [Route("getByWheelBaseName")]
        [HttpPost()]
        public async Task<IHttpActionResult> Post([FromBody]string baseName)
        {
            List<WheelBase> wheelBases = null;
            wheelBases = await _wheelBaseApplicationService.GetAsync(item => item.Base.Equals(baseName, StringComparison.CurrentCultureIgnoreCase));
            IEnumerable<WheelBaseViewModel> wheelBaseList = Mapper.Map<IEnumerable<WheelBaseViewModel>>(wheelBases);
            return Ok(wheelBaseList);
        }

        [Route("getByChildNames")]
        [HttpPost()]
        public async Task<IHttpActionResult> Post(WheelBaseSearchInputModel childModel)
        {
            var wheelBases =
                await
                    _wheelBaseApplicationService.GetAsync(
                        item => item.Base.Equals(childModel.Base, StringComparison.CurrentCultureIgnoreCase)
                                &&
                                item.WheelBaseMetric.Equals(childModel.WheelBaseMetric, StringComparison.CurrentCultureIgnoreCase));
            WheelBaseViewModel wheelBaseViewModel = Mapper.Map<WheelBaseDetailViewModel>(wheelBases.FirstOrDefault());
            return Ok(wheelBaseViewModel);
        }

        [Route("wheelBase/{WheelBaseId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByWheelBaseId(int wheelBaseId)
        {
            List<WheelBase> wheelBases = null;
            wheelBases = await _wheelBaseApplicationService.GetAsync(bc =>
                bc.Id == wheelBaseId);
            IEnumerable<WheelBaseViewModel> wheelBaseList = Mapper.Map<IEnumerable<WheelBaseViewModel>>(wheelBases);
            return Ok(wheelBaseList);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, WheelBaseInputModel wheelBaseInputModel)
        {
            WheelBase wheelBase = new WheelBase()
            {
                Id = wheelBaseInputModel.Id,
                Base = wheelBaseInputModel.Base,
                WheelBaseMetric = wheelBaseInputModel.WheelBaseMetric,
                VehicleToWheelBaseCount = wheelBaseInputModel.VehicleToWheelBaseCount,
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = wheelBaseInputModel.Comment };
            var attachments = SetUpAttachmentsModels(wheelBaseInputModel.Attachments);
            var changeRequestId = await _wheelBaseApplicationService.UpdateAsync(wheelBase, wheelBase.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("replace/{id:int}")]
        public async Task<IHttpActionResult> Replace(int id, WheelBaseInputModel model)
        {
            WheelBase brakeConfig = new WheelBase()
            {
                Id = model.Id,
                Base = model.Base,
                WheelBaseMetric = model.WheelBaseMetric,
                VehicleToWheelBases = model.VehicleToWheelBases.Select(item => new VehicleToWheelBase
                {
                    WheelBaseId = item.WheelBaseId,
                    Id = item.Id,
                    VehicleId = item.VehicleId
                }).ToList(),
            };

            CommentsStagingModel comment = new CommentsStagingModel { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);

            var changeRequestId = await _wheelBaseApplicationService.ReplaceAsync(brakeConfig, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }


        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, WheelBaseInputModel model)
        {
            WheelBase wheelBaselength = new WheelBase() { Id = model.Id, Base = model.Base, WheelBaseMetric = model.WheelBaseMetric };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _wheelBaseApplicationService.DeleteAsync(wheelBaselength, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        //[Route("search")]
        //[HttpPost]
        //public async Task<IHttpActionResult> Post([FromBody]string bedLengthNameFilter)
        //{
        //    if (string.IsNullOrWhiteSpace(bedLengthNameFilter))
        //    {
        //        return await this.Get();
        //    }

        //    List<BedLength> bedlengths = await _bedLengthApplicationService.GetAsync(m => m.Length.Contains(bedLengthNameFilter) && m.DeleteDate == null);

        //    IEnumerable<BedLengthViewModel> bedlengthlists = Mapper.Map<IEnumerable<BedLengthViewModel>>(bedlengths);

        //    return Ok(bedlengthlists);
        //}

        //[Route("search/{bedLengthNameFilter}")]
        //[HttpGet()]
        //public async Task<IHttpActionResult> Get(string bedLengthNameFilter)
        //{
        //    if (string.IsNullOrWhiteSpace(bedLengthNameFilter))
        //    {
        //        return await this.Get();
        //    }

        //    List<BedLength> bedlength = await _bedLengthApplicationService.GetAsync(m => m.Length.Contains(bedLengthNameFilter));

        //    IEnumerable<BedLengthViewModel> bedlengthlists = Mapper.Map<IEnumerable<BedLengthViewModel>>(bedlength);

        //    return Ok(bedlengthlists);
        //}

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)

        {
            ChangeRequestStagingModel<WheelBase> changeRequestStagingWheelBaseModel =
                await this._wheelBaseApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingWheelBaseViewModel changeRequestStagingWheelBaseViewModel = Mapper.Map<ChangeRequestStagingWheelBaseViewModel>(changeRequestStagingWheelBaseModel);

                 
            
            SetUpChangeRequestReview(changeRequestStagingWheelBaseViewModel.StagingItem.Status,
                changeRequestStagingWheelBaseViewModel.StagingItem.SubmittedBy, changeRequestStagingWheelBaseViewModel);

            
            return Ok(changeRequestStagingWheelBaseViewModel);
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
                this._wheelBaseApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }


    }
}

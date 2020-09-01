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
    [RoutePrefix("bedTypes")]
    public class BedTypeController : ApiControllerBase
    {
        private readonly IBedTypeApplicationService _bedTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public BedTypeController(IApplicationLogger applicationLogger, IBedTypeApplicationService bedTypeApplicationService)
        {
            _applicationLogger = applicationLogger;
            _bedTypeApplicationService = bedTypeApplicationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var bedTypes = await _bedTypeApplicationService.GetAllAsync();
            IEnumerable<BedTypeViewModel> bedTypeList = Mapper.Map<IEnumerable<BedTypeViewModel>>(bedTypes);

            return Ok(bedTypeList);
        }

        // GET api/bedType/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedBedType = await _bedTypeApplicationService.GetAsync(id);
            BedTypeDetailViewModel bedType = Mapper.Map<BedTypeDetailViewModel>(selectedBedType);

            return Ok(bedType);
        }


        [Route("search/{bedTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string bedTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bedTypeNameFilter))
            {
                return await this.Get();
            }

            List<BedType> bedTypes = await _bedTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(bedTypeNameFilter.ToLower()));

            IEnumerable<BedTypeViewModel> bedTypeList = Mapper.Map<IEnumerable<BedTypeViewModel>>(bedTypes);

            return Ok(bedTypeList);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string bedTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(bedTypeNameFilter))
            {
                return await this.Get();
            }

            List<BedType> bedTypes = await _bedTypeApplicationService.GetAsync(m => m.Name.ToLower().Contains(bedTypeNameFilter.ToLower()) && m.DeleteDate == null);

            IEnumerable<BedTypeViewModel> bedTypeList = Mapper.Map<IEnumerable<BedTypeViewModel>>(bedTypes);

            return Ok(bedTypeList);
        }

        [Route("bedType/{bedTypeId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBedTypeId(int bedTypeId)
        {
            List<BedType> bedTypes = null;
            bedTypes = await _bedTypeApplicationService.GetAsync(item => item.BedConfigs.Any(bc =>
                bc.BedTypeId == bedTypeId));
            IEnumerable<BedTypeViewModel> bedTypeList = Mapper.Map<IEnumerable<BedTypeViewModel>>(bedTypes);
            return Ok(bedTypeList);
        }
        [Route("bedTypeByBedLength/{bedLengthId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByBedLengthId(int bedLengthId)
        {
            List<BedType> bedTypes = null;
            bedTypes = await _bedTypeApplicationService.GetAsync(item => item.BedConfigs.Any(bc =>
                bc.BedLengthId == bedLengthId));
            IEnumerable<BedTypeViewModel> bedTypeList = Mapper.Map<IEnumerable<BedTypeViewModel>>(bedTypes);
            return Ok(bedTypeList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(BedTypeInputModel model)
        {
            // create bed type and list of comments
            BedType bedType = new BedType() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedTypeApplicationService.AddAsync(bedType, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, BedTypeInputModel model)
        {
            BedType bedType = new BedType() { Id = model.Id, Name = model.Name };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _bedTypeApplicationService.DeleteAsync(bedType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, BedTypeInputModel bedTypeInputModel)
        {
            BedType bedType = new BedType()
            {
                Id = bedTypeInputModel.Id,
                Name = bedTypeInputModel.Name,
                BedConfigCount = bedTypeInputModel.BedConfigCount,
                VehicleToBedConfigCount = bedTypeInputModel.VehicleToBedConfigCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = bedTypeInputModel.Comment };
            var attachments = SetUpAttachmentsModels(bedTypeInputModel.Attachments);
            var changeRequestId = await _bedTypeApplicationService.UpdateAsync(bedType, bedType.Id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            ChangeRequestStagingModel<BedType> changeRequestStagingBedTypeModel =
                await this._bedTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            ChangeRequestStagingBedTypeViewModel changeRequestStagingBedTypeViewModel = Mapper.Map<ChangeRequestStagingBedTypeViewModel>(changeRequestStagingBedTypeModel);

             SetUpChangeRequestReview(changeRequestStagingBedTypeViewModel.StagingItem.Status,
            changeRequestStagingBedTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingBedTypeViewModel);
           
            return Ok(changeRequestStagingBedTypeViewModel);
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
                this._bedTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}
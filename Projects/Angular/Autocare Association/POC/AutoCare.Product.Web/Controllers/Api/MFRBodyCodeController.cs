using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("mfrBodyCodes")]
    public class MfrBodyCodeController : ApiControllerBase
    {
        private readonly IMfrBodyCodeApplicationService _mfrBodyCodeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        private readonly ITextSerializer _serializer;

        public MfrBodyCodeController(IMfrBodyCodeApplicationService mfrBodyCodeApplicationService, IApplicationLogger applicationLogger, ITextSerializer serializer)
        {
            _mfrBodyCodeApplicationService = mfrBodyCodeApplicationService;
            _applicationLogger = applicationLogger;
            _serializer = serializer;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<MfrBodyCode> mfrBodyCodes = await _mfrBodyCodeApplicationService.GetAllAsync();

            IEnumerable<MfrBodyCodeViewModel> mfrBodyCodeList = Mapper.Map<IEnumerable<MfrBodyCodeViewModel>>(mfrBodyCodes);

            return Ok(mfrBodyCodeList);
        }

        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var selectedMfrBodyCode = await _mfrBodyCodeApplicationService.GetAsync(id);

                var mfrBodyCode = Mapper.Map<MfrBodyCodeViewModel>(selectedMfrBodyCode);

                return Ok(mfrBodyCode);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(MfrBodyCodeInputModel model)
        {
            MfrBodyCode mfrBodyCode = new MfrBodyCode()
            {
                Id = model.Id,
                Name = model.Name
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _mfrBodyCodeApplicationService.AddAsync(mfrBodyCode, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, MfrBodyCodeInputModel model)
        {
            MfrBodyCode mfrBodyCode = new MfrBodyCode()
            {
                Id = model.Id,
                Name = model.Name,
                VehicleToMfrBodyCodeCount = model.VehicleToMfrBodyCodeCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _mfrBodyCodeApplicationService.UpdateAsync(mfrBodyCode, mfrBodyCode.Id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("replace/{id:int}")]
        public async Task<IHttpActionResult> Replace(int id, MfrBodyCodeInputModel model)
        {
            MfrBodyCode mfrBodyCode = new MfrBodyCode()
            {
                Id = model.Id,
                Name = model.Name,
                VehicleToMfrBodyCodes = model.VehicleToMfrBodyCodes.Select(item => new VehicleToMfrBodyCode
                {
                    MfrBodyCodeId = item.MfrBodyCodeId,
                    Id = item.Id,
                    VehicleId = item.VehicleId
                }).ToList(),

            };

            CommentsStagingModel comment = new CommentsStagingModel { Comment = model.Comment };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _mfrBodyCodeApplicationService.ReplaceAsync(mfrBodyCode, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("pendingChangeRequests")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingChangeRequests()
        {
            var pendingMfrBodyCodes = await _mfrBodyCodeApplicationService.GetPendingAddChangeRequests(null);

            List<MfrBodyCodeViewModel> mfrBodyCodeViewModels = Mapper.Map<List<MfrBodyCodeViewModel>>(pendingMfrBodyCodes);

            return Ok(mfrBodyCodeViewModels);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, MfrBodyCodeInputModel model)
        {
            MfrBodyCode mfrBodyCode = new MfrBodyCode()
            {
                Id = model.Id,
                VehicleToMfrBodyCodeCount = model.VehicleToMfrBodyCodeCount
            };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = model.Comment, };
            var attachments = SetUpAttachmentsModels(model.Attachments);
            var changeRequestId = await _mfrBodyCodeApplicationService.DeleteAsync(mfrBodyCode, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            MfrBodyCodeChangeRequestStagingModel changeRequestStagingMfrBodyCodeModel =
                await this._mfrBodyCodeApplicationService.GetChangeRequestStaging(changeRequestId);

            ChangeRequestStagingMfrBodyCodeViewModel changeRequestStagingMfrBodyCodeViewModel = Mapper.Map<ChangeRequestStagingMfrBodyCodeViewModel>(changeRequestStagingMfrBodyCodeModel);

             SetUpChangeRequestReview(changeRequestStagingMfrBodyCodeViewModel.StagingItem.Status,
                changeRequestStagingMfrBodyCodeViewModel.StagingItem.SubmittedBy, changeRequestStagingMfrBodyCodeViewModel);
                        return Ok(changeRequestStagingMfrBodyCodeViewModel);
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
                this._mfrBodyCodeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

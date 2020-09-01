using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using System;
using System.Linq;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    //[Authorize(Roles = "Admin, Researcher")]
    [RoutePrefix("fuelDeliveryTypes")]
    public class FuelDeliveryTypeController : ApiControllerBase
    {
        private readonly IFuelDeliveryTypeApplicationService _fuelDeliveryTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public FuelDeliveryTypeController(IFuelDeliveryTypeApplicationService fuelDeliveryTypeApplicationService,
            IApplicationLogger logger)
        {
            _fuelDeliveryTypeApplicationService = fuelDeliveryTypeApplicationService;
            _applicationLogger = logger;

        }

        [Route("")]
        [HttpGet]
        //[CustomAuthorize]
        public async Task<IHttpActionResult> GetAll()
        {
            List<FuelDeliveryType> fuelDeliveryTypes = await _fuelDeliveryTypeApplicationService.GetAllAsync();
            IEnumerable<FuelDeliveryTypeViewModel> fuelDeliveryTypeViewModels = Mapper.Map<IEnumerable<FuelDeliveryTypeViewModel>>(fuelDeliveryTypes);

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelDeliveryTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelDeliveryTypeViewModels);
        }


        //TODO: remove if no longer used
        [Route("count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
            //if (count <= 0) count = 20;
            List<FuelDeliveryType> fuelDeliveryTypes = await _fuelDeliveryTypeApplicationService.GetAllAsync(count);
            IEnumerable<FuelDeliveryTypeViewModel> fuelDeliveryTypesList = Mapper.Map<IEnumerable<FuelDeliveryTypeViewModel>>(fuelDeliveryTypes);

            //mock the data for Change request and revision date,
            //until we finalize the changeRequest Process
            int i = 0;
            IList<FuelDeliveryTypeViewModel> fuelDeliveryTypeViewModels = fuelDeliveryTypesList as IList<FuelDeliveryTypeViewModel> ?? fuelDeliveryTypesList.ToList();
            foreach (var fuelDeliveryType in fuelDeliveryTypeViewModels)
            {
                fuelDeliveryType.LastUpdateDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                if (i == 1 || i == 3)
                {
                    fuelDeliveryType.ChangeRequestExists = true;
                }
                else
                {
                    fuelDeliveryType.ChangeRequestExists = false;
                }
                i++;
            }

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelDeliveryTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelDeliveryTypeViewModels);
        }


        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetFuelDeliveryTypeById(int id)
        {
            FuelDeliveryType fuelDeliveryType = await _fuelDeliveryTypeApplicationService.GetAsync(id);
            FuelDeliveryTypeViewModel fuelDeliveryTypeViewModel = Mapper.Map<FuelDeliveryTypeViewModel>(fuelDeliveryType);

            return Ok(fuelDeliveryTypeViewModel);
        }


        [Route("search/{fuelDeliveryTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string fuelDeliveryTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelDeliveryTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelDeliveryType> fuelDeliveryTypes = await _fuelDeliveryTypeApplicationService.GetAsync(m => m.FuelDeliveryTypeName.ToLower().Contains(fuelDeliveryTypeNameFilter.ToLower()));
            IEnumerable<FuelDeliveryTypeViewModel> fuelDeliveryTypeViewModels = Mapper.Map<IEnumerable<FuelDeliveryTypeViewModel>>(fuelDeliveryTypes);

            return Ok(fuelDeliveryTypeViewModels);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string fuelDeliveryTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelDeliveryTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelDeliveryType> fuelDeliveryTypes = await _fuelDeliveryTypeApplicationService.GetAsync(m => m.FuelDeliveryTypeName.ToLower().Contains(fuelDeliveryTypeNameFilter.ToLower()) && m.DeleteDate == null);
            IEnumerable<FuelDeliveryTypeViewModel> fuelDeliveryTypeViewModels = Mapper.Map<IEnumerable<FuelDeliveryTypeViewModel>>(fuelDeliveryTypes);

            return Ok(fuelDeliveryTypeViewModels);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(FuelDeliveryTypeInputModel fuelDeliveryTypeInputModel)
        {
            // create fuelDeliveryType and list of comments
            FuelDeliveryType fuelDeliveryType = new FuelDeliveryType() { Id = fuelDeliveryTypeInputModel.Id, FuelDeliveryTypeName = fuelDeliveryTypeInputModel.Name };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelDeliveryTypeInputModel.Comment
            };
            var attachments = SetUpAttachmentsModels(fuelDeliveryTypeInputModel.Attachments);

            var changeRequestId = await _fuelDeliveryTypeApplicationService.AddAsync(fuelDeliveryType, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, FuelDeliveryTypeInputModel fuelDeliveryTypeInputModel)
        {
            // create fuelDeliveryType and list of comments
            FuelDeliveryType fuelDeliveryType = new FuelDeliveryType()
            {
                Id = fuelDeliveryTypeInputModel.Id,
                FuelDeliveryTypeName = fuelDeliveryTypeInputModel.Name,
                FuelDeliveryConfigCount = fuelDeliveryTypeInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelDeliveryTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelDeliveryTypeInputModel.Comment
            };

            var changeRequestId = await _fuelDeliveryTypeApplicationService.UpdateAsync(fuelDeliveryType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, FuelDeliveryTypeInputModel fuelDeliveryTypeInputModel)
        {
            // create fuelDeliveryType and list of comments
            FuelDeliveryType fuelDeliveryType = new FuelDeliveryType()
            {
                Id = fuelDeliveryTypeInputModel.Id,
                FuelDeliveryConfigCount = fuelDeliveryTypeInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelDeliveryTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = fuelDeliveryTypeInputModel.Comment };

            var changeRequestId = await _fuelDeliveryTypeApplicationService.DeleteAsync(fuelDeliveryType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<FuelDeliveryType> changeRequestStagingFuelDeliveryTypeModel = await this._fuelDeliveryTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingFuelDeliveryTypeViewModel changeRequestStagingFuelDeliveryTypeViewModel = Mapper.Map<ChangeRequestStagingFuelDeliveryTypeViewModel>(changeRequestStagingFuelDeliveryTypeModel);

            SetUpChangeRequestReview(changeRequestStagingFuelDeliveryTypeViewModel.StagingItem.Status,
                changeRequestStagingFuelDeliveryTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingFuelDeliveryTypeViewModel);

            // return view model
            return Ok(changeRequestStagingFuelDeliveryTypeViewModel);
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
                this._fuelDeliveryTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

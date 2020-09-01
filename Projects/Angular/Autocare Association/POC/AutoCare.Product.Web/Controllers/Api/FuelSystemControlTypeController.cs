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
    [RoutePrefix("fuelSystemControlTypes")]
    public class FuelSystemControlTypeController : ApiControllerBase
    {
        private readonly IFuelSystemControlTypeApplicationService _fuelSystemControlTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public FuelSystemControlTypeController(IFuelSystemControlTypeApplicationService fuelSystemControlTypeApplicationService,
            IApplicationLogger logger)
        {
            _fuelSystemControlTypeApplicationService = fuelSystemControlTypeApplicationService;
            _applicationLogger = logger;

        }

        [Route("")]
        [HttpGet]
        //[CustomAuthorize]
        public async Task<IHttpActionResult> GetAll()
        {
            List<FuelSystemControlType> fuelSystemControlTypes = await _fuelSystemControlTypeApplicationService.GetAllAsync();
            IEnumerable<FuelSystemControlTypeViewModel> fuelSystemControlTypeViewModels = Mapper.Map<IEnumerable<FuelSystemControlTypeViewModel>>(fuelSystemControlTypes);

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelSystemControlTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelSystemControlTypeViewModels);
        }


        //TODO: remove if no longer used
        [Route("count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
            //if (count <= 0) count = 20;
            List<FuelSystemControlType> fuelSystemControlTypes = await _fuelSystemControlTypeApplicationService.GetAllAsync(count);
            IEnumerable<FuelSystemControlTypeViewModel> fuelSystemControlTypesList = Mapper.Map<IEnumerable<FuelSystemControlTypeViewModel>>(fuelSystemControlTypes);

            //mock the data for Change request and revision date,
            //until we finalize the changeRequest Process
            int i = 0;
            IList<FuelSystemControlTypeViewModel> fuelSystemControlTypeViewModels = fuelSystemControlTypesList as IList<FuelSystemControlTypeViewModel> ?? fuelSystemControlTypesList.ToList();
            foreach (var fuelSystemControlType in fuelSystemControlTypeViewModels)
            {
                fuelSystemControlType.LastUpdateDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                if (i == 1 || i == 3)
                {
                    fuelSystemControlType.ChangeRequestExists = true;
                }
                else
                {
                    fuelSystemControlType.ChangeRequestExists = false;
                }
                i++;
            }

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelSystemControlTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelSystemControlTypeViewModels);
        }


        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetFuelSystemControlTypeById(int id)
        {
            FuelSystemControlType fuelSystemControlType = await _fuelSystemControlTypeApplicationService.GetAsync(id);
            FuelSystemControlTypeViewModel fuelSystemControlTypeViewModel = Mapper.Map<FuelSystemControlTypeViewModel>(fuelSystemControlType);

            return Ok(fuelSystemControlTypeViewModel);
        }


        [Route("search/{fuelSystemControlTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string fuelSystemControlTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelSystemControlTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelSystemControlType> fuelSystemControlTypes = await _fuelSystemControlTypeApplicationService.GetAsync(m => m.FuelSystemControlTypeName.ToLower().Contains(fuelSystemControlTypeNameFilter.ToLower()));
            IEnumerable<FuelSystemControlTypeViewModel> fuelSystemControlTypeViewModels = Mapper.Map<IEnumerable<FuelSystemControlTypeViewModel>>(fuelSystemControlTypes);

            return Ok(fuelSystemControlTypeViewModels);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string fuelSystemControlTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelSystemControlTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelSystemControlType> fuelSystemControlTypes = await _fuelSystemControlTypeApplicationService.GetAsync(m => m.FuelSystemControlTypeName.ToLower().Contains(fuelSystemControlTypeNameFilter.ToLower()) && m.DeleteDate == null);
            IEnumerable<FuelSystemControlTypeViewModel> fuelSystemControlTypeViewModels = Mapper.Map<IEnumerable<FuelSystemControlTypeViewModel>>(fuelSystemControlTypes);

            return Ok(fuelSystemControlTypeViewModels);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(FuelSystemControlTypeInputModel fuelSystemControlTypeInputModel)
        {
            // create fuelSystemControlType and list of comments
            FuelSystemControlType fuelSystemControlType = new FuelSystemControlType() { Id = fuelSystemControlTypeInputModel.Id, FuelSystemControlTypeName = fuelSystemControlTypeInputModel.Name };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelSystemControlTypeInputModel.Comment
            };
            var attachments = SetUpAttachmentsModels(fuelSystemControlTypeInputModel.Attachments);

            var changeRequestId = await _fuelSystemControlTypeApplicationService.AddAsync(fuelSystemControlType, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, FuelSystemControlTypeInputModel fuelSystemControlTypeInputModel)
        {
            // create fuelSystemControlType and list of comments
            FuelSystemControlType fuelSystemControlType = new FuelSystemControlType()
            {
                Id = fuelSystemControlTypeInputModel.Id,
                FuelSystemControlTypeName = fuelSystemControlTypeInputModel.Name,
                FuelDeliveryConfigCount = fuelSystemControlTypeInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelSystemControlTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelSystemControlTypeInputModel.Comment
            };

            var changeRequestId = await _fuelSystemControlTypeApplicationService.UpdateAsync(fuelSystemControlType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, FuelSystemControlTypeInputModel fuelSystemControlTypeInputModel)
        {
            // create fuelSystemControlType and list of comments
            FuelSystemControlType fuelSystemControlType = new FuelSystemControlType()
            {
                Id = fuelSystemControlTypeInputModel.Id,
                FuelDeliveryConfigCount = fuelSystemControlTypeInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelSystemControlTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = fuelSystemControlTypeInputModel.Comment };

            var changeRequestId = await _fuelSystemControlTypeApplicationService.DeleteAsync(fuelSystemControlType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<FuelSystemControlType> changeRequestStagingFuelSystemControlTypeModel = await this._fuelSystemControlTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingFuelSystemControlTypeViewModel changeRequestStagingFuelSystemControlTypeViewModel = Mapper.Map<ChangeRequestStagingFuelSystemControlTypeViewModel>(changeRequestStagingFuelSystemControlTypeModel);

            SetUpChangeRequestReview(changeRequestStagingFuelSystemControlTypeViewModel.StagingItem.Status,
                changeRequestStagingFuelSystemControlTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingFuelSystemControlTypeViewModel);

            // return view model
            return Ok(changeRequestStagingFuelSystemControlTypeViewModel);
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
                this._fuelSystemControlTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

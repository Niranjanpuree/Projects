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
    [RoutePrefix("fuelDeliverySubTypes")]
    public class FuelDeliverySubTypeController : ApiControllerBase
    {
        private readonly IFuelDeliverySubTypeApplicationService _fuelDeliverySubTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;     
        public FuelDeliverySubTypeController(IFuelDeliverySubTypeApplicationService fuelDeliverySubTypeApplicationService,
            IApplicationLogger logger)
        {
            _fuelDeliverySubTypeApplicationService = fuelDeliverySubTypeApplicationService;
            _applicationLogger = logger;
         
        }

        [Route("")]
        [HttpGet]
        //[CustomAuthorize]
        public async Task<IHttpActionResult> GetAll()
        {
            List<FuelDeliverySubType> fuelDeliverySubTypes = await _fuelDeliverySubTypeApplicationService.GetAllAsync();
            IEnumerable<FuelDeliverySubTypeViewModel> fuelDeliverySubTypeViewModels = Mapper.Map<IEnumerable<FuelDeliverySubTypeViewModel>>(fuelDeliverySubTypes);

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelDeliverySubTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelDeliverySubTypeViewModels);
        }


        //TODO: remove if no longer used
        [Route("count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
            //if (count <= 0) count = 20;
            List<FuelDeliverySubType> fuelDeliverySubTypes = await _fuelDeliverySubTypeApplicationService.GetAllAsync(count);
            IEnumerable<FuelDeliverySubTypeViewModel> fuelDeliverySubTypesList = Mapper.Map<IEnumerable<FuelDeliverySubTypeViewModel>>(fuelDeliverySubTypes);

            //mock the data for Change request and revision date,
            //until we finalize the changeRequest Process
            int i = 0;
            IList<FuelDeliverySubTypeViewModel> fuelDeliverySubTypeViewModels = fuelDeliverySubTypesList as IList<FuelDeliverySubTypeViewModel> ?? fuelDeliverySubTypesList.ToList();
            foreach (var fuelDeliverySubType in fuelDeliverySubTypeViewModels)
            {
                fuelDeliverySubType.LastUpdateDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                if (i == 1 || i == 3)
                {
                    fuelDeliverySubType.ChangeRequestExists = true;
                }
                else
                {
                    fuelDeliverySubType.ChangeRequestExists = false;
                }
                i++;
            }

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelDeliverySubTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelDeliverySubTypeViewModels);
        }

        
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetFuelDeliverySubTypeById(int id)
        {
            FuelDeliverySubType fuelDeliverySubType = await _fuelDeliverySubTypeApplicationService.GetAsync(id);
            FuelDeliverySubTypeViewModel fuelDeliverySubTypeViewModel = Mapper.Map<FuelDeliverySubTypeViewModel>(fuelDeliverySubType);

            return Ok(fuelDeliverySubTypeViewModel);
        }


        [Route("search/{fuelDeliverySubTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string fuelDeliverySubTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelDeliverySubTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelDeliverySubType> fuelDeliverySubTypes = await _fuelDeliverySubTypeApplicationService.GetAsync(m => m.FuelDeliverySubTypeName.ToLower().Contains(fuelDeliverySubTypeNameFilter.ToLower()));
            IEnumerable<FuelDeliverySubTypeViewModel> fuelDeliverySubTypeViewModels = Mapper.Map<IEnumerable<FuelDeliverySubTypeViewModel>>(fuelDeliverySubTypes);

            return Ok(fuelDeliverySubTypeViewModels);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string fuelDeliverySubTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelDeliverySubTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelDeliverySubType> fuelDeliverySubTypes = await _fuelDeliverySubTypeApplicationService.GetAsync(m => m.FuelDeliverySubTypeName.ToLower().Contains(fuelDeliverySubTypeNameFilter.ToLower()) && m.DeleteDate == null);
            IEnumerable<FuelDeliverySubTypeViewModel> fuelDeliverySubTypeViewModels = Mapper.Map<IEnumerable<FuelDeliverySubTypeViewModel>>(fuelDeliverySubTypes);

            return Ok(fuelDeliverySubTypeViewModels);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(FuelDeliverySubTypeInputModel fuelDeliverySubTypeInputModel)
            {
            // create fuelDeliverySubType and list of comments
            FuelDeliverySubType fuelDeliverySubType = new FuelDeliverySubType() { Id = fuelDeliverySubTypeInputModel.Id, FuelDeliverySubTypeName = fuelDeliverySubTypeInputModel.Name };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelDeliverySubTypeInputModel.Comment
            };
            var attachments = SetUpAttachmentsModels(fuelDeliverySubTypeInputModel.Attachments);

            var changeRequestId = await _fuelDeliverySubTypeApplicationService.AddAsync(fuelDeliverySubType, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, FuelDeliverySubTypeInputModel fuelDeliverySubTypeInputModel)
        {
            // create fuelDeliverySubType and list of comments
            FuelDeliverySubType fuelDeliverySubType = new FuelDeliverySubType()
            {
                Id = fuelDeliverySubTypeInputModel.Id,
                FuelDeliverySubTypeName = fuelDeliverySubTypeInputModel.Name,
                FuelDeliveryConfigCount = fuelDeliverySubTypeInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelDeliverySubTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelDeliverySubTypeInputModel.Comment
            };

            var changeRequestId = await _fuelDeliverySubTypeApplicationService.UpdateAsync(fuelDeliverySubType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, FuelDeliverySubTypeInputModel fuelDeliverySubTypeInputModel)
        {
            // create fuelDeliverySubType and list of comments
            FuelDeliverySubType fuelDeliverySubType = new FuelDeliverySubType()
            {
                Id = fuelDeliverySubTypeInputModel.Id,
                FuelDeliveryConfigCount = fuelDeliverySubTypeInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelDeliverySubTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = fuelDeliverySubTypeInputModel.Comment };

            var changeRequestId = await _fuelDeliverySubTypeApplicationService.DeleteAsync(fuelDeliverySubType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<FuelDeliverySubType> changeRequestStagingFuelDeliverySubTypeModel = await this._fuelDeliverySubTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingFuelDeliverySubTypeViewModel changeRequestStagingFuelDeliverySubTypeViewModel = Mapper.Map<ChangeRequestStagingFuelDeliverySubTypeViewModel>(changeRequestStagingFuelDeliverySubTypeModel);

            SetUpChangeRequestReview(changeRequestStagingFuelDeliverySubTypeViewModel.StagingItem.Status,
                changeRequestStagingFuelDeliverySubTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingFuelDeliverySubTypeViewModel);
           
            // return view model
            return Ok(changeRequestStagingFuelDeliverySubTypeViewModel);
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
                this._fuelDeliverySubTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }   
    }
}

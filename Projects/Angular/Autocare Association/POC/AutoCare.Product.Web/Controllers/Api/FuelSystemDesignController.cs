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
    [RoutePrefix("fuelSystemDesigns")]
    public class FuelSystemDesignController : ApiControllerBase
    {
        private readonly IFuelSystemDesignApplicationService _fuelSystemDesignApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public FuelSystemDesignController(IFuelSystemDesignApplicationService fuelSystemDesignApplicationService,
            IApplicationLogger logger)
        {
            _fuelSystemDesignApplicationService = fuelSystemDesignApplicationService;
            _applicationLogger = logger;

        }

        [Route("")]
        [HttpGet]
        //[CustomAuthorize]
        public async Task<IHttpActionResult> GetAll()
        {
            List<FuelSystemDesign> fuelSystemDesigns = await _fuelSystemDesignApplicationService.GetAllAsync();
            IEnumerable<FuelSystemDesignViewModel> fuelSystemDesignViewModels = Mapper.Map<IEnumerable<FuelSystemDesignViewModel>>(fuelSystemDesigns);

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelSystemDesigns, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelSystemDesignViewModels);
        }


        //TODO: remove if no longer used
        [Route("count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
            //if (count <= 0) count = 20;
            List<FuelSystemDesign> fuelSystemDesigns = await _fuelSystemDesignApplicationService.GetAllAsync(count);
            IEnumerable<FuelSystemDesignViewModel> fuelSystemDesignsList = Mapper.Map<IEnumerable<FuelSystemDesignViewModel>>(fuelSystemDesigns);

            //mock the data for Change request and revision date,
            //until we finalize the changeRequest Process
            int i = 0;
            IList<FuelSystemDesignViewModel> fuelSystemDesignViewModels = fuelSystemDesignsList as IList<FuelSystemDesignViewModel> ?? fuelSystemDesignsList.ToList();
            foreach (var fuelSystemDesign in fuelSystemDesignViewModels)
            {
                fuelSystemDesign.LastUpdateDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                if (i == 1 || i == 3)
                {
                    fuelSystemDesign.ChangeRequestExists = true;
                }
                else
                {
                    fuelSystemDesign.ChangeRequestExists = false;
                }
                i++;
            }

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelSystemDesigns, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelSystemDesignViewModels);
        }


        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetFuelSystemDesignById(int id)
        {
            FuelSystemDesign fuelSystemDesign = await _fuelSystemDesignApplicationService.GetAsync(id);
            FuelSystemDesignViewModel fuelSystemDesignViewModel = Mapper.Map<FuelSystemDesignViewModel>(fuelSystemDesign);

            return Ok(fuelSystemDesignViewModel);
        }


        [Route("search/{fuelSystemDesignNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string fuelSystemDesignNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelSystemDesignNameFilter))
            {
                return await this.Get();
            }

            List<FuelSystemDesign> fuelSystemDesigns = await _fuelSystemDesignApplicationService.GetAsync(m => m.FuelSystemDesignName.ToLower().Contains(fuelSystemDesignNameFilter.ToLower()));
            IEnumerable<FuelSystemDesignViewModel> fuelSystemDesignViewModels = Mapper.Map<IEnumerable<FuelSystemDesignViewModel>>(fuelSystemDesigns);

            return Ok(fuelSystemDesignViewModels);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string fuelSystemDesignNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelSystemDesignNameFilter))
            {
                return await this.Get();
            }

            List<FuelSystemDesign> fuelSystemDesigns = await _fuelSystemDesignApplicationService.GetAsync(m => m.FuelSystemDesignName.ToLower().Contains(fuelSystemDesignNameFilter.ToLower()) && m.DeleteDate == null);
            IEnumerable<FuelSystemDesignViewModel> fuelSystemDesignViewModels = Mapper.Map<IEnumerable<FuelSystemDesignViewModel>>(fuelSystemDesigns);

            return Ok(fuelSystemDesignViewModels);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(FuelSystemDesignInputModel fuelSystemDesignInputModel)
        {
            // create fuelSystemDesign and list of comments
            FuelSystemDesign fuelSystemDesign = new FuelSystemDesign() { Id = fuelSystemDesignInputModel.Id, FuelSystemDesignName = fuelSystemDesignInputModel.Name };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelSystemDesignInputModel.Comment
            };
            var attachments = SetUpAttachmentsModels(fuelSystemDesignInputModel.Attachments);

            var changeRequestId = await _fuelSystemDesignApplicationService.AddAsync(fuelSystemDesign, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, FuelSystemDesignInputModel fuelSystemDesignInputModel)
        {
            // create fuelSystemDesign and list of comments
            FuelSystemDesign fuelSystemDesign = new FuelSystemDesign()
            {
                Id = fuelSystemDesignInputModel.Id,
                FuelSystemDesignName = fuelSystemDesignInputModel.Name,
                FuelDeliveryConfigCount = fuelSystemDesignInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelSystemDesignInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelSystemDesignInputModel.Comment
            };

            var changeRequestId = await _fuelSystemDesignApplicationService.UpdateAsync(fuelSystemDesign, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, FuelSystemDesignInputModel fuelSystemDesignInputModel)
        {
            // create fuelSystemDesign and list of comments
            FuelSystemDesign fuelSystemDesign = new FuelSystemDesign()
            {
                Id = fuelSystemDesignInputModel.Id,
                FuelDeliveryConfigCount = fuelSystemDesignInputModel.FuelDeliveryConfigCount,
            };
            var attachments = SetUpAttachmentsModels(fuelSystemDesignInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = fuelSystemDesignInputModel.Comment };

            var changeRequestId = await _fuelSystemDesignApplicationService.DeleteAsync(fuelSystemDesign, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<FuelSystemDesign> changeRequestStagingFuelSystemDesignModel = await this._fuelSystemDesignApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingFuelSystemDesignViewModel changeRequestStagingFuelSystemDesignViewModel = Mapper.Map<ChangeRequestStagingFuelSystemDesignViewModel>(changeRequestStagingFuelSystemDesignModel);

            SetUpChangeRequestReview(changeRequestStagingFuelSystemDesignViewModel.StagingItem.Status,
                changeRequestStagingFuelSystemDesignViewModel.StagingItem.SubmittedBy, changeRequestStagingFuelSystemDesignViewModel);

            // return view model
            return Ok(changeRequestStagingFuelSystemDesignViewModel);
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
                this._fuelSystemDesignApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

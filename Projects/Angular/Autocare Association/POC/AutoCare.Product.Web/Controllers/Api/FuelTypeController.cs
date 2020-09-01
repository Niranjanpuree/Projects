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
    [RoutePrefix("fuelTypes")]
    public class FuelTypeController : ApiControllerBase
    {
        private readonly IFuelTypeApplicationService _fuelTypeApplicationService;
        private readonly IApplicationLogger _applicationLogger;
        public FuelTypeController(IFuelTypeApplicationService fuelTypeApplicationService,
            IApplicationLogger logger)
        {
            _fuelTypeApplicationService = fuelTypeApplicationService;
            _applicationLogger = logger;

        }

        [Route("")]
        [HttpGet]
        //[CustomAuthorize]
        public async Task<IHttpActionResult> GetAll()
        {
            List<FuelType> fuelTypes = await _fuelTypeApplicationService.GetAllAsync();
            IEnumerable<FuelTypeViewModel> fuelTypeViewModels = Mapper.Map<IEnumerable<FuelTypeViewModel>>(fuelTypes);

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelTypeViewModels);
        }


        //TODO: remove if no longer used
        [Route("count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
         
            ////if (count <= 0) count = 20;
            List<FuelType> fuelTypes = await _fuelTypeApplicationService.GetAllAsync(count);
            IEnumerable<FuelTypeViewModel> fuelTypesList = Mapper.Map<IEnumerable<FuelTypeViewModel>>(fuelTypes);

            //mock the data for Change request and revision date,
            //until we finalize the changeRequest Process
            int i = 0;
            IList<FuelTypeViewModel> fuelTypeViewModels = fuelTypesList as IList<FuelTypeViewModel> ?? fuelTypesList.ToList();
            foreach (var fuelType in fuelTypeViewModels)
            {
                fuelType.LastUpdateDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                if (i == 1 || i == 3)
                {
                    fuelType.ChangeRequestExists = true;
                }
                else
                {
                    fuelType.ChangeRequestExists = false;
                }
                i++;
            }

            _applicationLogger.WriteLog("Api Invoked: url=api/fuelTypes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(fuelTypeViewModels);
        }


        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetFuelTypeById(int id)
        {
            FuelType fuelType = await _fuelTypeApplicationService.GetAsync(id);
            FuelTypeViewModel fuelTypeViewModel = Mapper.Map<FuelTypeViewModel>(fuelType);

            return Ok(fuelTypeViewModel);
        }


        [Route("search/{fuelTypeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string fuelTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelType> fuelTypes = await _fuelTypeApplicationService.GetAsync(m => m.FuelTypeName.ToLower().Contains(fuelTypeNameFilter.ToLower()));
            IEnumerable<FuelTypeViewModel> fuelTypeViewModels = Mapper.Map<IEnumerable<FuelTypeViewModel>>(fuelTypes);

            return Ok(fuelTypeViewModels);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string fuelTypeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(fuelTypeNameFilter))
            {
                return await this.Get();
            }

            List<FuelType> fuelTypes = await _fuelTypeApplicationService.GetAsync(m => m.FuelTypeName.ToLower().Contains(fuelTypeNameFilter.ToLower()) && m.DeleteDate == null);
            IEnumerable<FuelTypeViewModel> fuelTypeViewModels = Mapper.Map<IEnumerable<FuelTypeViewModel>>(fuelTypes);

            return Ok(fuelTypeViewModels);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(FuelTypeInputModel fuelTypeInputModel)
        {
            // create fuelType and list of comments
            FuelType fuelType = new FuelType() { Id = fuelTypeInputModel.Id, FuelTypeName = fuelTypeInputModel.Name };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelTypeInputModel.Comment
            };
            var attachments = SetUpAttachmentsModels(fuelTypeInputModel.Attachments);

            var changeRequestId = await _fuelTypeApplicationService.AddAsync(fuelType, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, FuelTypeInputModel fuelTypeInputModel)
        {
            // create fuelType and list of comments
            FuelType fuelType = new FuelType()
            {
                Id = fuelTypeInputModel.Id,
                FuelTypeName = fuelTypeInputModel.Name,
                EngineConfigCount= fuelTypeInputModel.EngineConfigCount,
                VehicleToEngineConfigCount=fuelTypeInputModel.VehicleToEngineConfigCount
                
            };
            var attachments = SetUpAttachmentsModels(fuelTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = fuelTypeInputModel.Comment
            };

            var changeRequestId = await _fuelTypeApplicationService.UpdateAsync(fuelType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, FuelTypeInputModel fuelTypeInputModel)
        {
            // create fuelType and list of comments
            FuelType fuelType = new FuelType()
            {
                Id = fuelTypeInputModel.Id,
               
            };
            var attachments = SetUpAttachmentsModels(fuelTypeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = fuelTypeInputModel.Comment };

            var changeRequestId = await _fuelTypeApplicationService.DeleteAsync(fuelType, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<FuelType> changeRequestStagingFuelTypeModel = await this._fuelTypeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingFuelTypeViewModel changeRequestStagingFuelTypeViewModel = Mapper.Map<ChangeRequestStagingFuelTypeViewModel>(changeRequestStagingFuelTypeModel);

            SetUpChangeRequestReview(changeRequestStagingFuelTypeViewModel.StagingItem.Status,
                changeRequestStagingFuelTypeViewModel.StagingItem.SubmittedBy, changeRequestStagingFuelTypeViewModel);

            // return view model
            return Ok(changeRequestStagingFuelTypeViewModel);
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
                this._fuelTypeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

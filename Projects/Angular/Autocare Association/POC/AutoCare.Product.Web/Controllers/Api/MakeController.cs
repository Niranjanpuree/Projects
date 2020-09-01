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
    [RoutePrefix("makes")]
    public class MakeController : ApiControllerBase
    {
        private readonly IMakeApplicationService _makeApplicationService;
        private readonly IApplicationLogger _applicationLogger;     
        public MakeController(IMakeApplicationService makeApplicationService,
            IApplicationLogger logger)
        {
            _makeApplicationService = makeApplicationService;
            _applicationLogger = logger;
         
        }

        [Route("")]
        [HttpGet]
        //[CustomAuthorize]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Make> makes = await _makeApplicationService.GetAllAsync();
            IEnumerable<MakeViewModel> makeViewModels = Mapper.Map<IEnumerable<MakeViewModel>>(makes);

            _applicationLogger.WriteLog("Api Invoked: url=api/makes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(makeViewModels);
        }


        //TODO: remove if no longer used
        [Route("count/{count:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int count = 0)
        {
            //if (count <= 0) count = 20;
            List<Make> makes = await _makeApplicationService.GetAllAsync(count);
            IEnumerable<MakeViewModel> makesList = Mapper.Map<IEnumerable<MakeViewModel>>(makes);

            //mock the data for Change request and revision date,
            //until we finalize the changeRequest Process
            int i = 0;
            IList<MakeViewModel> makeViewModels = makesList as IList<MakeViewModel> ?? makesList.ToList();
            foreach (var make in makeViewModels)
            {
                make.LastUpdateDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                if (i == 1 || i == 3)
                {
                    make.ChangeRequestExists = true;
                }
                else
                {
                    make.ChangeRequestExists = false;
                }
                i++;
            }

            _applicationLogger.WriteLog("Api Invoked: url=api/makes, Verb=Get, User=user-name", GetType(), LogType.Info);

            return Ok(makeViewModels);
        }

        [Route("~/years/{yearId:int}/makes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMakesByYearId(int yearId)
        {
            List<Make> makes = null;

            makes = await _makeApplicationService.GetAsync(item => item.BaseVehicles.Any(b => b.YearId == yearId));
            IEnumerable<MakeViewModel> makesList = Mapper.Map<IEnumerable<MakeViewModel>>(makes);

            return Ok(makesList);
        }

        // GET api/makes/1
        [Route("{id:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetMakeById(int id)
        {
            Make make = await _makeApplicationService.GetAsync(id);
            MakeViewModel makeViewModel = Mapper.Map<MakeViewModel>(make);

            return Ok(makeViewModel);
        }


        [Route("search/{makeNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> Get(string makeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(makeNameFilter))
            {
                return await this.Get();
            }

            List<Make> makes = await _makeApplicationService.GetAsync(m => m.Name.ToLower().Contains(makeNameFilter.ToLower()));
            IEnumerable<MakeViewModel> makeViewModels = Mapper.Map<IEnumerable<MakeViewModel>>(makes);

            return Ok(makeViewModels);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string makeNameFilter)
        {
            if (string.IsNullOrWhiteSpace(makeNameFilter))
            {
                return await this.Get();
            }

            List<Make> makes = await _makeApplicationService.GetAsync(m => m.Name.ToLower().Contains(makeNameFilter.ToLower()) && m.DeleteDate == null);
            IEnumerable<MakeViewModel> makeViewModels = Mapper.Map<IEnumerable<MakeViewModel>>(makes);

            return Ok(makeViewModels);
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(MakeInputModel makeInputModel)
            {
            // create make and list of comments
            Make make = new Make() { Id = makeInputModel.Id, Name = makeInputModel.Name };
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = makeInputModel.Comment
            };
            var attachments = SetUpAttachmentsModels(makeInputModel.Attachments);

            var changeRequestId = await _makeApplicationService.AddAsync(make, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, MakeInputModel makeInputModel)
        {
            // create make and list of comments
            Make make = new Make()
            {
                Id = makeInputModel.Id,
                Name = makeInputModel.Name,
                BaseVehicleCount = makeInputModel.BaseVehicleCount,
                VehicleCount = makeInputModel.VehicleCount
            };
            var attachments = SetUpAttachmentsModels(makeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = makeInputModel.Comment
            };

            var changeRequestId = await _makeApplicationService.UpdateAsync(make, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, MakeInputModel makeInputModel)
        {
            // create make and list of comments
            Make make = new Make()
            {
                Id = makeInputModel.Id,
                BaseVehicleCount = makeInputModel.BaseVehicleCount,
                VehicleCount = makeInputModel.VehicleCount
            };
            var attachments = SetUpAttachmentsModels(makeInputModel.Attachments);
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = makeInputModel.Comment };

            var changeRequestId = await _makeApplicationService.DeleteAsync(make, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<Make> changeRequestStagingMakeModel = await this._makeApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingMakeViewModel changeRequestStagingMakeViewModel = Mapper.Map<ChangeRequestStagingMakeViewModel>(changeRequestStagingMakeModel);

            SetUpChangeRequestReview(changeRequestStagingMakeViewModel.StagingItem.Status,
                changeRequestStagingMakeViewModel.StagingItem.SubmittedBy, changeRequestStagingMakeViewModel);
           
            // return view model
            return Ok(changeRequestStagingMakeViewModel);
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
                this._makeApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }   
    }
}

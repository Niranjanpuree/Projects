using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Web.Models.ViewModels;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Vcdb.Model;
using AutoMapper;
using AutoCare.Product.Infrastructure.ErrorMessage;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("years")]
    public class YearController : ApiControllerBase
    {
        private readonly IYearApplicationService _yearApplicationService;
        private readonly IApplicationLogger _applicationLogger;

        public YearController(IYearApplicationService yearApplicationService,
            IApplicationLogger applicationLogger
            )
        {
            _yearApplicationService = yearApplicationService;
            _applicationLogger = applicationLogger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetYears()
        {
            var years = await _yearApplicationService.GetAllAsync();
            //years.Add(new Year() {
            //    Id= 9999
            //});
            //TODO: remove this looping and get the dependencies at a single db call
            //while fetching years, check if it has dependencies or not
            //foreach (var year in years)
            //{
            //    try
            //    {
            //        var _year = await _yearApplicationService.GetAsync(year.Id);
            //        year.BaseVehicleCount = _year.BaseVehicleCount;
            //    }
            //    catch (Exception)
            //    {
            //        //simply catch the exception, and continue
            //    }
            //}

            var yearsViewModel = Mapper.Map<IEnumerable<YearViewModel>>(years);

            return Ok(yearsViewModel.ToArray());
        }

        [HttpGet]
        [Route("dependencies/{id:int}")]
        public async Task<IHttpActionResult> GetDependencies(int id)
        {

            if (id == default(int))
            {
                return BadRequest(ErrorMessage.Make.InvalidMakeId);
            }

            YearViewModel yearViewModel = new YearViewModel();

            try
            {
                var year = await _yearApplicationService.GetAsync(id);
                yearViewModel.Id = year.Id;
                yearViewModel.BaseVehicleCount = year.BaseVehicleCount;
            }
            catch (Exception)
            {
                //no dependencies
            }

            return Ok(yearViewModel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(YearInputModel yearInputModel)
        {
            if (yearInputModel == null)
            {
                return BadRequest(ErrorMessage.Year.RequiredYearInputModel);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check length
            if (yearInputModel.Id.ToString().Length != 4)
            {
                ModelState.AddModelError("", ErrorMessage.Year.InvalidYearLength);
                return BadRequest(ModelState);
            }
            Year year = new Year() { Id = yearInputModel.Id};
            //var year = Mapper.Map<Year>(yearInputModel);
            var changeRequestId = await _yearApplicationService.AddAsync(year, CurrentUser.Email);

            return Ok(changeRequestId);
        }

        //NOTE: this method is not required.
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (id == default(int))
            {
                return BadRequest(ErrorMessage.Year.RequiredYear);
            }
            try
            {
                var _year = await _yearApplicationService.GetAsync(id);
                if (_year != null)
                {
                    return BadRequest("cannot delete year, because of it's dependencies ");
                }
            }
            catch (NoRecordFound)
            {
                //simply ignore the record Not Found 
            }
            //get dependencies

            var changeRequestId = await _yearApplicationService.DeleteAsync(null, id, CurrentUser.Email);

            return Ok(changeRequestId);
        }


        [HttpPost]
        [Route("~/years/delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, YearInputModel yearInputModel)
        {
            // create make and list of comments          

            CommentsStagingModel comment = new CommentsStagingModel() { Comment = yearInputModel.Comment };
            var attachments = SetUpAttachmentsModels(yearInputModel.Attachments);
            var changeRequestId = await _yearApplicationService.DeleteAsync(null, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("~/years/changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<Year> changeRequestStagingYearModel = await this._yearApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingYearViewModel changeRequestStagingYearViewModel = Mapper.Map<ChangeRequestStagingYearViewModel>(changeRequestStagingYearModel);

           SetUpChangeRequestReview(changeRequestStagingYearViewModel.StagingItem.Status,
                changeRequestStagingYearViewModel.StagingItem.SubmittedBy, changeRequestStagingYearViewModel);
                        // return view model
            return Ok(changeRequestStagingYearViewModel);
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
                this._yearApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }

}


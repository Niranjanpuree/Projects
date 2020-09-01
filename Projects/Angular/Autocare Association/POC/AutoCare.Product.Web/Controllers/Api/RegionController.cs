using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using AutoCare.Product.Infrastructure.ErrorMessage;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("regions")]
    public class RegionController : ApiControllerBase
    {
        private readonly IRegionApplicationService _regionApplicationService;
        private readonly ILogger _logger;
        public RegionController(IRegionApplicationService regionApplicationService, ILogger logger)
        {
            _regionApplicationService = regionApplicationService;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var region = await _regionApplicationService.GetAllAsync();
            var regionList = Mapper.Map<IEnumerable<RegionViewModel>>(region);
            _logger.Info("GetRegion method called");
            return Ok(regionList);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedRegion = await _regionApplicationService.GetAsync(id);
            var regionDetail = Mapper.Map<RegionViewModel>(selectedRegion);
            _logger.Info("GetRegionById method called");
            return Ok(regionDetail);
        }

        [Route("~/regions/search/{regionNameFilter}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetByFilter(string regionNameFilter)
        {
            if (string.IsNullOrWhiteSpace(regionNameFilter))
            {
                return await this.Get();
            }
            var region = await _regionApplicationService.GetAsync(m => m.Name.ToLower().Contains(regionNameFilter.ToLower()));
            var regionList = Mapper.Map<IEnumerable<RegionViewModel>>(region);

            return Ok(regionList);
        }

        [Route("~/regions/search")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string regionNameFilter)
        {
            if (string.IsNullOrWhiteSpace(regionNameFilter))
            {
                return await this.Get();
            }
            var region = await _regionApplicationService.GetAsync(m => m.Name.ToLower().Contains(regionNameFilter.ToLower()) && m.DeleteDate == null);
            var regionList = Mapper.Map<IEnumerable<RegionViewModel>>(region);

            return Ok(regionList);
        }

        [Route("~/baseVehicles/{baseVehicleId:int}/subModels/{subModelId:int}/regions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRegionsByBaseVehicleIdAndSubModelId(int baseVehicleId, int subModelId)
        {
            List<Region> regions = null;
            if (baseVehicleId != default(int) && subModelId != default(int))
            {
                regions = await _regionApplicationService.GetAsync(item => item.Vehicles.Any(v => v.BaseVehicleId == baseVehicleId && v.SubModelId == subModelId));
            }
            IEnumerable<RegionViewModel> regionList = Mapper.Map<IEnumerable<RegionViewModel>>(regions);

            return Ok(regionList);
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(RegionInputModel newRegion)
        {
            if (newRegion == null)
            {
                return BadRequest(ErrorMessage.Region.RequiredRegionInputModel);
            }

            if (newRegion.Name == null)
            {
                return BadRequest(ErrorMessage.Region.RequiredRegion);
            }
            if (newRegion.RegionAbbr == null)
            {
                return BadRequest(ErrorMessage.Region.RequiredRegion);
            }

            Region region = new Region()
            {
                Id = newRegion.Id,
                Name = newRegion.Name,
                ParentId = newRegion.ParentId,
                RegionAbbr = newRegion.RegionAbbr,
                RegionAbbr_2 = newRegion.RegionAbbr_2
            };

            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = newRegion.Comment
            };
            var attachments = SetUpAttachmentsModels(newRegion.Attachments);


            var changeRequestId = await _regionApplicationService.AddAsync(region, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, RegionInputModel updateRegion)
        {
            if (updateRegion == null)
            {
                return BadRequest(ErrorMessage.Region.RequiredRegionInputModel);
            }

            if (updateRegion.Name == null)
            {
                return BadRequest(ErrorMessage.Region.RequiredRegion);
            }
            if (updateRegion.RegionAbbr == null)
            {
                return BadRequest(ErrorMessage.Region.RegionAbbreviation);
            }

            if (updateRegion.RegionAbbr == null)
            {
                return BadRequest(ErrorMessage.Region.RegionAbbreviation);
            }

            Region region = new Region()
            {
                Id = updateRegion.Id,
                Name = updateRegion.Name,
                ParentId = updateRegion.ParentId,
                RegionAbbr = updateRegion.RegionAbbr,
                RegionAbbr_2 = updateRegion.RegionAbbr_2,
                VehicleCount = updateRegion.VehicleCount
            };

            CommentsStagingModel comment = new CommentsStagingModel()
            {
                Comment = updateRegion.Comment
            };
            var attachments = SetUpAttachmentsModels(updateRegion.Attachments);

            var changeRequestId = await _regionApplicationService.UpdateAsync(region, id, CurrentUser.Email, comment, attachments);
            return Ok(changeRequestId);
        }

        //NOTE: this method is not required.
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var changeRequestId = await _regionApplicationService.DeleteAsync(null, id, CurrentUser.Email);

            return Ok(changeRequestId);
        }

        [HttpPost]
        [Route("~/regions/delete/{id:int}")]
        public async Task<IHttpActionResult> Post(int id, RegionInputModel regionInputModel)
        {
            // create make and list of comments          
            Region region = new Region() { Id = regionInputModel.Id };
            CommentsStagingModel comment = new CommentsStagingModel() { Comment = regionInputModel.Comment };
            var attachments = SetUpAttachmentsModels(regionInputModel.Attachments);

            var changeRequestId = await _regionApplicationService.DeleteAsync(region, id, CurrentUser.Email, comment, attachments);

            return Ok(changeRequestId);
        }

        [Route("~/regions/changeRequestStaging/{changeRequestId:int}")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetChangeRequestStaging(int changeRequestId)
        {
            // retrieve staging information
            ChangeRequestStagingModel<Region> changeRequestStagingModelRegion = await this._regionApplicationService.GetChangeRequestStaging(changeRequestId);
            // convert to view model
            ChangeRequestStagingRegionViewModel changeRequestStagingRegionViewModel = Mapper.Map<ChangeRequestStagingRegionViewModel>(changeRequestStagingModelRegion);

             SetUpChangeRequestReview(changeRequestStagingRegionViewModel.StagingItem.Status,
                changeRequestStagingRegionViewModel.StagingItem.SubmittedBy, changeRequestStagingRegionViewModel);
           
            return Ok(changeRequestStagingRegionViewModel);
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
                this._regionApplicationService.SubmitChangeRequestReviewAsync(changeRequestId, reviewModel);

            // return view model
            return Ok(isSubmitted);
        }
    }
}

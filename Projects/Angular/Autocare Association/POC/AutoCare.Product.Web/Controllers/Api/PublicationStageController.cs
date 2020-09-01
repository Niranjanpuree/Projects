using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;
using NLog;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("publicationstages")]
    public class PublicationStageController : ApiController
    {
        private readonly IPublicationStageApplicationService _publicationStageApplicationService;
        private readonly ILogger _logger;

        public PublicationStageController(IPublicationStageApplicationService publicationStageApplicationService, ILogger logger)
        {
            _publicationStageApplicationService = publicationStageApplicationService;
            _logger = logger;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var publicationStage = await _publicationStageApplicationService.GetAllAsync();
            var publicationStageList = Mapper.Map<IEnumerable<PublicationStageViewModel>>(publicationStage);
            _logger.Info("GetPublicationStage method called");
            return Ok(publicationStageList);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedpublicationStage = await _publicationStageApplicationService.GetAsync(id);
            var publicationStageDetail = Mapper.Map<PublicationStageViewModel>(selectedpublicationStage);
            _logger.Info("GetPublicationStageById method called");
            return Ok(publicationStageDetail);
        }
    }
}

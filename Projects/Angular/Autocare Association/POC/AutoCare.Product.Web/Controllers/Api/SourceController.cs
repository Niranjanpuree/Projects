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
    [RoutePrefix("sources")]
    public class SourceController : ApiController
    {
        private readonly ISourceApplicationService _sourceApplicationService;
        private readonly ILogger _logger;
        public SourceController(ISourceApplicationService sourceApplicationService, ILogger logger)
        {
            _sourceApplicationService = sourceApplicationService;
            _logger = logger;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var source = await _sourceApplicationService.GetAllAsync();
            var sourceList = Mapper.Map<IEnumerable<SourceViewModel>>(source);
            _logger.Info("GetSource method called");
            return Ok(sourceList);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var selectedSource = await _sourceApplicationService.GetAsync(id);
            var sourceDetail = Mapper.Map<SourceViewModel>(selectedSource);
            _logger.Info("GetSourceById method called");
            return Ok(sourceDetail);
        }
    }
}

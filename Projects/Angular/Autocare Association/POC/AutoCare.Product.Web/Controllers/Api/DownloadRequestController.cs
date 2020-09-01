using AutoCare.Product.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("downloadRequest")]
    public class DownloadRequestController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetDownloadRequests()
        {            
            //downloadRequests.push({
            //    id: 3498,
            //    fileType: "Access",
            //    content: "VCDB,ait duty",
            //    date: "06-12-2016 10.41 am PST",
            //    status: "Download"
            //});

            var downloadRequests = new List<DownloadRequestViewModel>() {
                new DownloadRequestViewModel() {
                     Id = 1767,
                     FileType = "SQL",
                     Content = "VCDB, all",
                     Date = "06-12-2016 8.32 am PS",
                     Status= "Request"
                },
                new DownloadRequestViewModel() {
                     Id = 428,
                     FileType = "Comma/ASCII",
                     Content = "Powersport",
                     Date = "06-12-2016 10.44 am PST",
                     Status= "Download"
                },
                new DownloadRequestViewModel() {
                     Id = 3498,
                     FileType = "Comma/ASCII",
                     Content = "Powersport",
                     Date = "06-12-2016 10.44 am PST",
                     Status= "Download"
                }
                };

            return Ok(downloadRequests.ToArray());

        }
    }
}
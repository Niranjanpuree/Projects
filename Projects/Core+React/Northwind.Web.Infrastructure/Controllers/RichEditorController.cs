using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities.RichEditor;
using Northwind.Core.Interfaces.RichEditor;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Controllers
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    [Route("/RichEditor")]
    public class RichEditorController: Controller
    {
        private readonly IRichEditorService _richEditorService;

        public RichEditorController(IRichEditorService richEditorService)
        {
            _richEditorService = richEditorService;
            _richEditorService.VerifyRootFolder();
        }

        [Route("/RichEditor/Folders")]
        public async Task<IEnumerable<RichEditorFolder>> Folders(Dictionary<string, string> param)
        {
            var relativePath = string.IsNullOrEmpty(param["path"]) ? "" : "/" + param["path"];
            if(param.ContainsKey("name"))
            {
                relativePath += param["name"];
            }
            return await _richEditorService.GetFolders(relativePath);
        }

        [Route("/RichEditor/files")]
        public async Task<ActionResult> Files([FromQuery] Dictionary<string, string> param)
        {
            var relativePath = param["file"];
            var output = await _richEditorService.GetFile(relativePath);
            output.Save(Response.Body, ImageFormat.Png);
            return File(Response.Body, "image/png");
        }

        [Route("/RichEditor/Thumbnail")]
        public async Task<ActionResult> Thumbnail(Dictionary<string, string> param)
        {
            var relativePath = string.IsNullOrEmpty(param["path"]) ? "" : "/" + param["path"];
            var output = await _richEditorService.GetThumbnail(relativePath);
            output.Save(Response.Body, ImageFormat.Png);
            return File(Response.Body, "image/png");
        }

        [Route("/RichEditor/Upload")]
        public async Task<ActionResult> Upload(Dictionary<string, string> param)
        {
            var files = Request.Form.Files;
            var path = param["path"] == null? "": param["path"];
            if (!path.StartsWith("/"))
                path = "/" + path;
            foreach(var file in files)
            {
                var result = await _richEditorService.SaveFileAsync(path, file);
                return Ok(result);
            }
            return Ok();
        }

    }
}

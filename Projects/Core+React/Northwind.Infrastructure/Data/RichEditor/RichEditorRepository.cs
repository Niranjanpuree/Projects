using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities.RichEditor;
using Northwind.Core.Interfaces.RichEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Data.RichEditor
{
    public class RichEditorRepository : IRichEditorRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public RichEditorRepository(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<Image> GetFile(string relativePath)
        {
            var rootPath = _environment.ContentRootPath;
            var path = _configuration.GetSection("PublicDocumentPath").Value;
            var lst = new List<RichEditorFile>();
            if (!relativePath.StartsWith("/"))
                relativePath = "/" + relativePath;
            return await Task.Run(() => {
                var allPath = rootPath + path + relativePath;
                return new Bitmap(allPath); 
            });
        }

        public async Task<IEnumerable<RichEditorFolder>> GetFolders(string relativePath)
        {
            var rootPath = _environment.ContentRootPath;
            var path = _configuration.GetSection("PublicDocumentPath").Value;
            var lst = new List<RichEditorFolder>();

            await Task.Run(() => {
                if(!Directory.Exists(rootPath + path + relativePath))
                {
                    Directory.CreateDirectory(rootPath + path + relativePath);
                }
                var folders = Directory.GetDirectories(rootPath + path + relativePath);
                
                var files = Directory.GetFiles(rootPath + path + relativePath);
                foreach (var folder in folders)
                {
                    var finfo = new FileInfo(folder);
                    if (Directory.Exists(folder))
                    {
                        lst.Add(new RichEditorFolder { Name = finfo.Name, Path = path + relativePath + finfo.Name, Type = "d" });
                    }
                    else
                    {
                        lst.Add(new RichEditorFolder { Name = finfo.Name, Path = path + relativePath + finfo.Name, Type = "f" });
                    }
                }

                foreach (var folder in files)
                {
                    var finfo = new FileInfo(folder);
                    if (Directory.Exists(folder))
                    {
                        lst.Add(new RichEditorFolder { Name = finfo.Name, Path = path + relativePath + finfo.Name, Type = "d" });
                    }
                    else
                    {
                        lst.Add(new RichEditorFolder { Name = finfo.Name, Path = path + relativePath + finfo.Name, Type = "f" });
                    }
                }
            });
            return lst;
        }

        public async Task<Image> GetThumbnail(string path)
        {
            var rootPath = _environment.ContentRootPath;
            var doc = _configuration.GetSection("PublicDocumentPath").Value;
            var section = _configuration.GetSection("thumbnail");
            var size = section.GetSection("size");
            var width = 0;
            var height = 0;
            int.TryParse(size.GetSection("width").Value,out width);
            int.TryParse(size.GetSection("height").Value,out height);

            return await Task.Run(() =>
            {
                var orgBitmap = new Bitmap(rootPath + doc + path);
                var orgSize = orgBitmap.Size;
                var bitHeight = (int) orgSize.Height * width / orgSize.Width;
                var bitmap = new Bitmap(width, bitHeight);
                var gr = Graphics.FromImage(bitmap);
                gr.DrawImage(orgBitmap, new Rectangle(0,0,width,bitHeight), new Rectangle(0,0, orgSize.Width, orgSize.Height), GraphicsUnit.Pixel);
                gr.Flush();
                orgBitmap.Dispose();
                return bitmap;
            });
        }

        public async Task<dynamic> SaveFile(string relativePath, IFormFile file)
        {
            return await Task.Run(() =>
            {
                var rootPath = _environment.ContentRootPath;
                var path = _configuration.GetSection("PublicDocumentPath").Value;
                var fullPath = rootPath + path + relativePath;
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                var filename = GetFilename(fullPath + file.FileName);
                var f = File.OpenWrite(filename);
                file.OpenReadStream().CopyTo(f);
                f.Close();
                f.Dispose();
                var finfo = new FileInfo(filename);
                return new { Name = finfo.Name, Type = "f" };
            });
            
        }

        public async Task<bool> VerifyRootFolder()
        {
            var rootPath = _environment.ContentRootPath;
            var path = _configuration.GetSection("PublicDocumentPath").Value;
            if (!Directory.Exists(rootPath + path))
            {
                await Task.Run(() => { return Directory.CreateDirectory(rootPath + path); });
            }
            return true;
        }

        private string GetFilename(string filename, int index = 0)
        {
            var finfo = new FileInfo(filename);
            var fName = "";
            if (index == 0)
            {
                fName = finfo.FullName;
            }
            else
            {
                fName = finfo.FullName.Replace(finfo.Name, "")+ finfo.Name.Replace(finfo.Extension,"") + "(" + index.ToString() + ")" + finfo.Extension;
            }
            
            if (File.Exists(fName))
            {
                return GetFilename(filename, index + 1);
            }
            else
            {
                return fName;
            }
        }
    }
}

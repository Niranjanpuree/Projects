using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Northwind.Core.Interfaces;
using System.Linq;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Core.Entities;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileService(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        private string GetMimeType(string fileName)
        {
            // Make Sure Microsoft.AspNetCore.StaticFiles Nuget Package is installed
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public FileContentResult GetFile(string filename, string filePath)
        {
            var filepath = Path.Combine(filePath);

            //filename = filePath.Split(Path.DirectorySeparatorChar).Last();

            var mimeType = this.GetMimeType(filename);

            byte[] fileBytes = null;

            if (File.Exists(filepath))
            {
                fileBytes = File.ReadAllBytes(filepath);

                return new FileContentResult(fileBytes, mimeType)
                {
                    FileDownloadName = filename
                };
            }
            else
            {
                // Code to handle if file is not present
            }

            return null;
        }

        public string FilePost(string location, IFormFile fileToUpload)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            //edge browser get full path in file name ...
            var fullPath = fileToUpload.FileName.Split("\\");
            var fileName = fullPath[fullPath.Length - 1];
            var relativePath = Path.Combine(location, fileName);
            using (var stream = new FileStream(relativePath, FileMode.Create))
            {
                fileToUpload.CopyTo(stream);
            }
            return fileName;
        }

        public string SaveFile(string location, IFormFile fileToUpload)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            //edge browser get full path in file name ...
            var fullPath = fileToUpload.FileName.Split("\\");
            var fileName = fullPath[fullPath.Length - 1];
            var relativePath = Path.Combine(location, fileName);

            var newFileName = GetNewFilePath(relativePath, 0);

            using (var stream = new FileStream(Path.Combine(location, newFileName), FileMode.Create))
            {
                fileToUpload.CopyTo(stream);
            }
            //var finfo = new FileInfo(Path.Combine(location, newFileName));
            return newFileName;
        }

        private string GetNewFilePath(string filePath, int index)
        {
            var finfo = new FileInfo(filePath);
            var name = finfo.Name;
            var ext = finfo.Extension;
            if (!string.IsNullOrEmpty(ext))
                name = name.Replace(ext, "");

            var newPath = finfo.Directory.FullName + "\\" + name + ext;
            if (System.IO.File.Exists(newPath))
            {
                name += "_" + index.ToString();
                var dupFilePath = finfo.Directory.FullName + "\\" + name + ext;
                return GetNewFilePath(dupFilePath, ++index);
            }

            var newFileName = name + ext;
            return newFileName;
        }

        public bool UploadFileTypeCheck(IFormFile file)
        {
            var supportedTypes = new[] { "CSV" };
            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            if (!supportedTypes.Contains(fileExt.ToUpper()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string MoveFile(string location, string previousFile, IFormFile fileToUpload)
        {
            var filename = string.Empty;

            if (fileToUpload != null)
                filename = fileToUpload.FileName;

            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            else if (!string.IsNullOrEmpty(previousFile))
            {
                var destinationPath = Path.Combine(location, "OldFiles");
                var sourceFile = Path.Combine(location, previousFile);
                var destinationFile = Path.Combine(destinationPath, previousFile);

                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                }
                Directory.CreateDirectory(destinationPath);
                if (File.Exists(sourceFile))
                {
                    System.IO.File.Move(sourceFile, destinationFile);
                }
            }
            var relativePath = Path.Combine(location, filename);
            using (var stream = new FileStream(relativePath, FileMode.Create))
            {
                fileToUpload.CopyTo(stream);
            }
            return filename;
        }

        public bool DeleteFileFromDirectory(string location)
        {
            if (File.Exists(location))
            {
                var physicalFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, location);
                // If file found, delete it    
                File.Delete(location);
            }
            return true;
        }
    }
}

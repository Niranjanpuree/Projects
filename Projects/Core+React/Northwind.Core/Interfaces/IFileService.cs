
using Attribute = Northwind.Core.Entities.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Core.Interfaces
{
    public interface IFileService
    {
        FileContentResult GetFile(string filename, string filePath);
        string FilePost(string location, IFormFile FileToUpload);
        string SaveFile(string location, IFormFile fileToUpload);
        string MoveFile(string location,string previousFile, IFormFile fileToUpload);
        bool UploadFileTypeCheck(IFormFile file);
        bool DeleteFileFromDirectory(string location);
        //string UploadFileDataCheck(string path, UploadMethodName methodaName);
        //string UploadFileEditDataCheck(UploadMethodName methodaName, List<GridHeaderModel> model);
    }
}

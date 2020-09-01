using Microsoft.AspNetCore.Http;
using Northwind.Core.Entities.RichEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces.RichEditor
{
    public interface IRichEditorService
    {
        Task<bool> VerifyRootFolder();
        Task<IEnumerable<RichEditorFolder>> GetFolders(string relativePath);
        Task<Image> GetFile(string relativePath);
        Task<dynamic> SaveFileAsync(string relativePath, IFormFile file);
        Task<Image> GetThumbnail(string path);
    }
}

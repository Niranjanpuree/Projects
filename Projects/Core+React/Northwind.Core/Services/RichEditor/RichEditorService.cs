using Microsoft.AspNetCore.Http;
using Northwind.Core.Entities.RichEditor;
using Northwind.Core.Interfaces.RichEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Core.Services.RichEditor
{
    public class RichEditorService : IRichEditorService
    {
        private readonly IRichEditorRepository _richEditorRepository;

        public RichEditorService(IRichEditorRepository richEditorRepository)
        {
            _richEditorRepository = richEditorRepository;
        }

        public async Task<Image> GetFile(string relativePath)
        {
            return await _richEditorRepository.GetFile(relativePath);
        }

        public async Task<IEnumerable<RichEditorFolder>> GetFolders(string relativePath)
        {
            return await _richEditorRepository.GetFolders(relativePath);
        }

        public async Task<Image> GetThumbnail(string path)
        {
            return await _richEditorRepository.GetThumbnail(path);
        }

        public async Task<dynamic> SaveFileAsync(string relativePath, IFormFile file)
        {
            return await _richEditorRepository.SaveFile(relativePath, file);
        }

        public async Task<bool> VerifyRootFolder()
        {
            return await _richEditorRepository.VerifyRootFolder();
        }
    }
}

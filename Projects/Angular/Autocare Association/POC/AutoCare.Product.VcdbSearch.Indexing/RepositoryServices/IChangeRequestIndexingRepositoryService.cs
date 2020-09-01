using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public interface IChangeRequestIndexingRepositoryService
    {
        Task<DocumentIndexResult> UploadDocumentsAsync(List<ChangeRequestDocument> documents);
        Task UploadDocumentAsync(ChangeRequestDocument document);
        Task<DocumentIndexResult> UpdateDocumentAsync(List<ChangeRequestDocument> documents);
        Task DeleteDocumentByChangeRequestIdAsync(string changeRequestId);
        Task UpdateDocumentAsync(ChangeRequestDocument document);
    }
}

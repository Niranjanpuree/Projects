using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IChangeRequestIndexingService
    {
        Task UploadDocumentAsync(ChangeRequestDocument document);
        Task UpdateDocumentAsync(ChangeRequestDocument document);
        Task UpdateDocumentAsync(List<ChangeRequestDocument> documents);
    }
}

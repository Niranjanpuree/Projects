using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices
{
    public interface IDocumentIndexer
    {
        Task AddChangeRequestIndexerAsync(long changeRequestId);
        Task ModifyChangeRequestIndexerAsync(long changeRequestId);
        Task ReplaceChangeRequestIndexerAsync(long changeRequestId);
        Task DeleteChangeRequestIndexerAsync(long changeRequestId);
        Task RejectChangeRequestIndexerAsync(long changeRequestId);
    }
}

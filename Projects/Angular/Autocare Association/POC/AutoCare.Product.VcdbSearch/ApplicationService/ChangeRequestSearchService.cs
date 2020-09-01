using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class ChangeRequestSearchService : IChangeRequestSearchService
    {
        private readonly IChangeRequestSearchRepositoryService _changeRequestSearchRepositoryService;

        public ChangeRequestSearchService(IChangeRequestSearchRepositoryService changeRequestSearchRepositoryService)
        {
            _changeRequestSearchRepositoryService = changeRequestSearchRepositoryService;
        }

        public async Task<ChangeRequestSearchResult> SearchAsync(string searchText, string filter = null,
            SearchOptions searchOptions = null)
        {
            var result = await _changeRequestSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);

            ChangeRequestSearchResult changeRequestSearchResult = new ChangeRequestSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return changeRequestSearchResult;
        }
    }
}

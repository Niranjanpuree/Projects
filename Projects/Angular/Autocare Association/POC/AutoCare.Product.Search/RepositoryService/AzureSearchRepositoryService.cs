using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.Search.Infrastructure;
using AutoCare.Product.Search.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.Search.RepositoryService
{
    public class AzureSearchRepositoryService<T> : ISearchRepositoryService<T>
        where T : class
    {
        protected ISearchIndexClient SearchService;

        public AzureSearchRepositoryService(string serviceName, string apiKey, string indexName)
        {
            ISearchServiceClient searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            SearchService = searchServiceClient.Indexes.GetClient(indexName);
        }

        public async Task<Model.SearchResult<T>> SearchAsync(string searchText,
            Expression<Func<T, bool>> filterExpression = null, SearchOptions searchOptions = null)
        {
            string filter = null;
            if (filterExpression != null)
            {
                filter = filterExpression.ToAzureSearchFilter();
            }

            return await SearchAsync(searchText, filter, searchOptions);
        }

        public async Task<Model.SearchResult<T>> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            var searchParameters = searchOptions == null ? new SearchParameters() : searchOptions.ToSearchParameters();

            if (!String.IsNullOrWhiteSpace(filter))
            {
                searchParameters.Filter = filter;
            }

            var documentsSearchResult = await SearchService.Documents.SearchAsync<T>(searchText, searchParameters);

            var searchResult = new Model.SearchResult<T>()
            {
                Documents = documentsSearchResult.Results.Select(x => x.Document).ToList(),
                Facets = ExtractFacets(documentsSearchResult.Facets),
                TotalCount = documentsSearchResult.Count    //documentsSearchResult.Results.Count
            };

            return searchResult;
        }

        private IList<Facet> ExtractFacets(FacetResults facetResults)
        {
            if (facetResults == null || !facetResults.Keys.Any())
            {
                return null;
            }

            var facets = new List<Facet>();
            foreach (var facetItem in facetResults)
            {
                var facet = new Facet()
                {
                    Name = facetItem.Key,
                    Value = new List<FacetValue>()
                };

                if (facetItem.Value != null && facetItem.Value.Any())
                {
                    foreach (var value in facetItem.Value)
                    {
                        if (value.Type == FacetType.Value)
                        {
                            facet.Value.Add(new SimpleValue() { Value = value.Value, Count = value.Count });
                        }

                        if (value.Type == FacetType.Range)
                        {
                            facet.Value.Add(new RangeValue() { From = value.From, To = value.To, Count = value.Count });
                        }
                    }
                }

                facets.Add(facet);
            }

            return facets;
        }
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.Search.Model;
using Microsoft.Azure.Search.Models;
using SearchMode = AutoCare.Product.Search.Model.SearchMode;

namespace AutoCare.Product.Search.Infrastructure
{
    public static class AzureSearchExtenstions
    {
        public static SearchParameters ToSearchParameters(this SearchOptions searchOptions)
        {
            if (searchOptions == null)
            {
                return null;
            }

            var searchParameters = new SearchParameters();
            AddSearchMode(searchOptions, searchParameters);
            AddOrderBy(searchOptions, searchParameters);
            AddStartingPosition(searchOptions, searchParameters);
            AddRecordCount(searchOptions, searchParameters);
            AddFacets(searchOptions, searchParameters);
            EnableTotalCount(searchOptions, searchParameters);
            AddSearchFields(searchOptions, searchParameters);
            return searchParameters;
        }

        private static void AddFacets(SearchOptions searchOptions, SearchParameters searchParameters)
        {
            searchParameters.Facets = searchOptions.FacetsToInclude;
        }

        private static void EnableTotalCount(SearchOptions searchOptions, SearchParameters searchParameters)
        {
            if (!searchOptions.ReturnTotalCount)
            {
                return;
            }

            searchParameters.IncludeTotalResultCount = searchOptions.ReturnTotalCount;
        }

        private static void AddOrderBy(SearchOptions searchOptions, SearchParameters searchParameters)
        {
            if (searchOptions.OrderBy == null || !searchOptions.OrderBy.Any())
            {
                return;
            }

            searchParameters.OrderBy = searchOptions.OrderBy;
        }

        private static void AddSearchMode(SearchOptions searchOptions, SearchParameters searchParameters)
        {
            searchParameters.SearchMode = searchOptions.SearchMode == SearchMode.ContainsAnyWord
                ? Microsoft.Azure.Search.Models.SearchMode.Any
                : Microsoft.Azure.Search.Models.SearchMode.All;
        }

        private static void AddStartingPosition(SearchOptions searchOptions, SearchParameters searchParameters)
        {
            if (searchOptions.RecordCount <= 0 || searchOptions.PageNumber <= 0)
            {
                return;
            }

            var documentsToSkip = searchOptions.RecordCount * (searchOptions.PageNumber - 1);
            searchParameters.Skip = documentsToSkip;
        }

        private static void AddRecordCount(SearchOptions searchOptions, SearchParameters searchParameters)
        {
            if (searchOptions.RecordCount < 0)
            {
                return;
            }

            searchParameters.Top = searchOptions.RecordCount;
        }

        private static void AddSearchFields(SearchOptions searchOptions, SearchParameters searchParameters)
        {
            searchParameters.SearchFields = searchOptions.SearchFields;
        }

        public static string QueryToExactMatch<T, TProp, TMatch>(this T indexDocument, 
            Expression<Func<T, TProp>> propertyExpression,
            TMatch valueToMatch)
        {
            var memberEx = propertyExpression.Body as MemberExpression;
            if (memberEx == null)
            {
                return null;
            }

            return $"{memberEx.Member.Name.ToCamelCase()} eq {valueToMatch}";
        }
    }
}

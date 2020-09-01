using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Search.Model;

namespace AutoCare.Product.Search.RepositoryService
{
    public interface ISearchRepositoryService<T>
        where T: class
    {
        Task<Model.SearchResult<T>> SearchAsync(string searchText, Expression<Func<T, bool>> filterExpression = null,
            SearchOptions searchOptions = null);

        Task<Model.SearchResult<T>> SearchAsync(string searchText, string filter = null,
            SearchOptions searchOptions = null);
    }
}

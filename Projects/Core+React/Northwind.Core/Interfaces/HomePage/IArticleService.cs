using Northwind.Core.Entities.HomePage;
using Northwind.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces.HomePage
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleType>> GetArticleTypes();
        Task<ArticleType> GetArticleType(int articleTypeId);
        Task<ArticleType> GetArticleTypeByName(string title);
        Task<ArticleType> GetArticleType(string articleType);
        Task<IEnumerable<Article>> GetArticles(int articleTypeId, SearchSpec searchSpec, bool includeDraft = false);
        Task<IEnumerable<Article>> GetArticlesForHomePage(ArticleTypes articletypes);
        Task<Article> GetArticle(int articleId);
        Task<int> Add(Article article);
        Task<int> Update(Article article);
        Task<int> Delete(Article article);
        void  DeleteMultiple(int[] ids);
        Task<int> TotalArticleRecord(string searchValue);
    }
} 

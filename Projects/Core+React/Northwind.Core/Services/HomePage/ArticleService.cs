using Northwind.Core.Entities.HomePage;
using Northwind.Core.Interfaces.HomePage;
using Northwind.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Core.Services.HomePage
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<int> Add(Article article)
        {
            return await _articleRepository.Add(article);
        }

        public async Task<int> Delete(Article article)
        {
            return await _articleRepository.Delete(article);
        }

        public void  DeleteMultiple(int [] ids)
        {
            _articleRepository.DeleteMultiple(ids);
        }

        public async Task<Article> GetArticle(int articleId)
        {
            return await _articleRepository.GetArticle(articleId);
        }

        public async Task<IEnumerable<Article>> GetArticles(int articleTypeId, SearchSpec searchSpec, bool includeDraft = false)
        {
            return await _articleRepository.GetArticles(articleTypeId, searchSpec, includeDraft);
        }

        public async Task<IEnumerable<Article>> GetArticlesForHomePage(ArticleTypes articles)
        {
            return await _articleRepository.GetArticlesForHomePage(articles);
        }


        public async Task<ArticleType> GetArticleType(int articleTypeId)
        {
            return await _articleRepository.GetArticleType(articleTypeId);
        }

        public async Task<ArticleType> GetArticleType(string articleType)
        {
            return await _articleRepository.GetArticleType(articleType);
        }

        public async Task<ArticleType> GetArticleTypeByName(string title)
        {
            return await _articleRepository.GetArticleTypeByName(title);
        }

        public async Task<IEnumerable<ArticleType>> GetArticleTypes()
        {
            return await _articleRepository.GetArticleTypes();
        }

        public async Task<int> TotalArticleRecord(string searchValue)
        {
            return await _articleRepository.TotalArticleRecord(searchValue);
        }

        public async Task<int> Update(Article article)
        {
            return await _articleRepository.Update(article);
        }

    }
}

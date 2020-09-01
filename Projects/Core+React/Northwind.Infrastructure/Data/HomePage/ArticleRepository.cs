using Northwind.Core.Entities.HomePage;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.HomePage;
using Northwind.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Northwind.Infrastructure.Data.HomePage
{
    public class ArticleRepository : IArticleRepository
    {
        public IDatabaseContext _context;

        public ArticleRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Article article)
        {
            var sql = @"INSERT INTO [Article]
               ([ArticleTypeId]
               ,[Name]
               ,[Title]
               ,[Excerpt]
               ,[Body]
               ,[IsLocalMedia]
               ,[PrimaryMediaPath]
               ,[PrimaryMediaUrl]
               ,[MediaCaption]
               ,[Status]
               ,[IsFeatured]
               ,[ShowInArchive]
               ,[CreatedBy]
               ,[CreatedOn]
               ,[UpdatedBy]
               ,[UpdatedOn]
               ,[IsDeleted])
            VALUES
               (@ArticleTypeId,
               @Name,
               @Title,
               @Excerpt,
               @Body,
               @IsLocalMedia,
               @PrimaryMediaPath,
               @PrimaryMediaUrl,
               @MediaCaption,
               @Status,
               @IsFeatured,
               @ShowInArchive,
               @CreatedBy,
               @CreatedOn,
               @UpdatedBy,
               @UpdatedOn,
               @IsDeleted)";
            try
            {
                return await _context.Connection.ExecuteAsync(sql, article);
            }
            catch (Exception ex)
            {
                return await _context.Connection.ExecuteAsync(sql, article);
            }
        }

        public async Task<int> Delete(Article article)
        {
            var sql = @"DELETE FROM [Article] WHERE [ArticleId]=@ArticleId";
            return await _context.Connection.ExecuteAsync(sql, article);
        }

        public void DeleteMultiple(int[] ids)
        {
            foreach(var i in ids)
            {
                var sql = @"DELETE FROM [Article] WHERE [ArticleId] = @id";
                _context.Connection.ExecuteAsync(sql, new {id=i });
            }
            
        }

        public async Task<Article> GetArticle(int articleId)
        {
            var sql = @"SELECT * FROM [Article] WHERE [ArticleId]=@ArticleId";
            var result = await _context.Connection.QueryAsync<Article>(sql, new { ArticleId = articleId });
            if (result.AsList().Count > 0)
            {
                return result.AsList()[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Article>> GetArticles(int articleTypeId, SearchSpec searchSpec, bool includeDraft = false)
        {
            var whereClause = "WHERE IsDeleted=0";

            if(articleTypeId >0)
            {
                searchSpec.SearchText = "%" + searchSpec.SearchText + "%";
                whereClause += " AND (article.ArticleTypeId = " + articleTypeId+") ";
            }
            if (!string.IsNullOrEmpty(searchSpec.SearchText))
            {
                searchSpec.SearchText = "%" + searchSpec.SearchText + "%";
                whereClause += " AND (article.Name LIKE @searchText OR article.Title LIKE @searchText OR Excerpt LIKE @searchText OR Body LIKE @searchText) ";
            }
            var sortField = searchSpec.SortField;
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "article.Name";
            }
            var showDraft = "";
            if (!includeDraft)
            {
                if (!string.IsNullOrEmpty(whereClause))
                {
                    showDraft += " AND ";
                }
                showDraft += $@" Status<> {(int)ArticleStatus.Draft} ";
            }
            else
            {
                if (!string.IsNullOrEmpty(whereClause))
                {
                    showDraft += " AND ";
                }
                showDraft += $@" Status<> {(int)ArticleStatus.Published} ";
            }
            var sql = $@"SELECT article.*,
                                    createdByUser.Displayname CreatedByName,
                                    updatedByUser.Displayname UpdatedByName,
                                    articleType.Name ArticleTypeName
                                    FROM [Article] article
                                    LEFT JOIN [Users] createdByUser
                                    ON createdByUser.UserGuid = article.CreatedBy
                                    LEFT JOIN [Users] updatedByUser
                                    ON updatedByUser.UserGuid = article.CreatedBy
                                    LEFT JOIN [ArticleType] articleType
                                    ON articleType.ArticleTypeId = article.ArticleTypeId 
                                {whereClause} {showDraft} ORDER BY {sortField} {searchSpec.Dir} OFFSET {searchSpec.Skip} ROWS FETCH NEXT {searchSpec.Take} ROWS ONLY";
            return await _context.Connection.QueryAsync<Article>(sql, searchSpec);

        }

        public async Task<int> TotalArticleRecord(string searchValue)
        {
            string searchParam = "";
            string searchText = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchText = "%" + searchValue + "%";
                searchParam += " AND (Name LIKE @searchValue OR Title LIKE @searchValue OR Excerpt LIKE @searchValue OR Body LIKE @searchValue) ";
            }
            var sql = $@" select COUNT(1) From ARTICLE WHERE IsDeleted = 0 {searchParam}";
            var count = _context.Connection.ExecuteScalarAsync<int>(sql, new { searchValue = searchText });
            return await count;
        }

        public async Task<ArticleType> GetArticleType(int articleTypeId)
        {
            var sql = @"SELECT * FROM [ArticleType] WHERE [ArticleTypeId]=@ArticleTypeId";
            var result = await _context.Connection.QueryAsync<ArticleType>(sql, new { ArticleTypeId = articleTypeId });
            if (result.AsList().Count > 0)
            {
                return result.AsList()[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<ArticleType> GetArticleType(string articleType)
        {
            var sql = @"SELECT * FROM [ArticleType] WHERE [Name]=@ArticleType";
            var result = await _context.Connection.QueryAsync<ArticleType>(sql, new { ArticleType = articleType });
            if (result.AsList().Count > 0)
            {
                return result.AsList()[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ArticleType>> GetArticleTypes()
        {
            var sql = @"SELECT * FROM [ArticleType]";
            return await _context.Connection.QueryAsync<ArticleType>(sql);
        }

        public async Task<int> Update(Article article)
        {
            var sql = @"UPDATE [dbo].[Article]
                       SET [ArticleTypeId] = @ArticleTypeId, 
                          [Name] = @Name,
                          [Title] = @Title,
                          [Excerpt] = @Excerpt,
                          [Body] = @Body,
                          [IsLocalMedia] = @IsLocalMedia,
                          [PrimaryMediaPath] = @PrimaryMediaPath,
                          [PrimaryMediaUrl] = @PrimaryMediaUrl,
                          [MediaCaption] = @MediaCaption,
                          [Status] = @Status,
                          [IsFeatured] = @IsFeatured,
                          [ShowInArchive] = @ShowInArchive,
                          [CreatedBy] = @CreatedBy,
                          [CreatedOn] = @CreatedOn,
                          [UpdatedBy] = @UpdatedBy,
                          [UpdatedOn] = @UpdatedOn,
                          [IsDeleted] = @IsDeleted
                     WHERE [ArticleId]=@ArticleId";
            try
            {
                return await _context.Connection.ExecuteAsync(sql, article);
            }
            catch (Exception ex)
            {
                return await _context.Connection.ExecuteAsync(sql, article);
            }
        }

        public async Task<ArticleType> GetArticleTypeByName(string title)
        {
            var sql = @"SELECT * FROM [ArticleType] WHERE Name = @Name";
            return await _context.Connection.QueryFirstOrDefaultAsync<ArticleType>(sql,new {Name = title });
        }

        public async Task<IEnumerable<Article>> GetArticlesForHomePage(ArticleTypes articles)
        {
            var sql = $@"select * from (select top 4 * from Article where articletypeid = 1 order by createdon desc) a union select * from (select top 1 * from Article where articletypeid = 2 and isfeatured = 1 order by createdon desc) b union select * from (select top 1 * from Article where articletypeid = 3 and isfeatured = 1 order by createdon desc) c";
            return await _context.Connection.QueryAsync<Article>(sql);
        }
    }
}

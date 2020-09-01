using Northwind.Core.Entities.HomePage;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.HomePage;
using Northwind.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

namespace Northwind.Infrastructure.Data.HomePage
{
    public class ApplicationRepository : IApplicationRepository
    {
        public IDatabaseContext _context;

        public ApplicationRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Application application)
        {
            var sql = @"INSERT INTO [Application]
                        [Name]
                       ,[Title]
                       ,[Description]
                       ,[IconPath]
                       ,[Url]
                       ,[IsInternalApplication]
                       ,[ShowAppForAllUsers]
                       ,[UpdateBy]
                       ,[UpdatedOn])
                     VALUES
                           ( @Name,
                             @Title,
                             @Description,
                             @IconPath,
                             @Url,
                             @IsInternalApplication,
                             @ShowAppForAllUsers,
                             @UpdateBy,
                             @UpdatedOn)
                ";
            return await _context.Connection.ExecuteAsync(sql, application);
        }

        public async Task<int> Delete(Application application)
        {
            var sql = @"DELETE FROM [Application]
                        WHERE ApplicationId=@ApplicationId";
            return await _context.Connection.ExecuteAsync(sql, new { application.ApplicationId });
        }

        public async Task<Application> GetApplication(int applicationId)
        {
            var sql = @"SELECT * FROM [Application] WHERE ApplicationId=@ApplicationId";
            var result = await _context.Connection.QueryAsync<Application>(sql, new { ApplicationId = applicationId });
            if (result.AsList().Count > 0)
            {
                return result.AsList()[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationCategory>> GetApplicationCategories()
        {
            var sql = @"SELECT * FROM [ApplicationCategory] ORDER BY OrderIndex";
            return await _context.Connection.QueryAsync<ApplicationCategory>(sql);
        }

        public async Task<ApplicationCategory> GetApplicationCategory(int applicationCategoryId)
        {
            var sql = @"SELECT * FROM [ApplicationCategory] WHERE ApplicationCategoryId=@ApplicationCategoryId";
            var result = await _context.Connection.QueryAsync<ApplicationCategory>(sql, new { ApplicationCategoryId = applicationCategoryId });
            if (result.AsList().Count > 0)
            {
                return result.AsList()[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Application>> GetApplications(int applicationCategoryId, SearchSpec searchSpec)
        {
            var whereClause = "";
            if (!string.IsNullOrEmpty(searchSpec.SearchText))
            {
                searchSpec.SearchText = "%" + searchSpec.SearchText + "%";
                whereClause = " AND (Name like @searchText OR Title like @searchText OR Description like @searchText)";
            }
            var sortField = searchSpec.SortField;
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "Name";
            }
            var sql = $@"SELECT * FROM [Application] WHERE 1=1 {whereClause} ORDER BY {sortField} {searchSpec.Dir} OFFSET {searchSpec.Skip} ROWS FETCH NEXT {searchSpec.Take} ROWS ONLY";
            return await _context.Connection.QueryAsync<Application>(sql, searchSpec);
        }

        public async Task<IEnumerable<ApplicationCategory>> GetUserMenuTree(Guid userGuid)
        {
            var sql = $@"SELECT DISTINCT AC.* FROM [Application] A
                            INNER JOIN [ApplicationCategory] AC ON AC.ApplicationCategoryId=A.ApplicationCategoryId
                            INNER JOIN [Resources] RS ON A.Resource = RS.Name
                            INNER JOIN [ResourceActions] RA ON RA.Name = A.Action
                            INNER JOIN [GroupPermission] CP ON CP.ResourceGuid = RS.ResourceGuid AND CP.ResourceActionGuid=RA.ActionGuid
                            INNER JOIN [Group] G ON G.GroupGuid=CP.GroupGuid
                            INNER JOIN [GroupUser] GU ON GU.GroupGuid=G.GroupGuid
                            WHERE GU.UserGuid=@UserGuid;";
            sql += $@"SELECT DISTINCT A.* FROM [Application] A
                            INNER JOIN [Resources] RS ON A.Resource = RS.Name
                            INNER JOIN [ResourceActions] RA ON RA.Name = A.Action
                            INNER JOIN [GroupPermission] CP ON CP.ResourceGuid = RS.ResourceGuid AND CP.ResourceActionGuid=RA.ActionGuid
                            INNER JOIN [Group] G ON G.GroupGuid=CP.GroupGuid
                            INNER JOIN [GroupUser] GU ON GU.GroupGuid=G.GroupGuid
                            WHERE GU.UserGuid=@UserGuid;";
            var result = await _context.Connection.QueryMultipleAsync(sql, new { UserGuid = userGuid });
            var cats = result.Read<ApplicationCategory>().ToList();
            var apps = result.Read<Application>().ToList();
            cats.ForEach((c) => {
                c.Applications = apps.Where(a => a.ApplicationCategoryId == c.ApplicationCategoryId).ToList();
            });
            return cats;
        }

        public async Task<int> Update(Application application)
        {
            var sql = @"UPDATE [Application]
                        SET 
                        [Name] = @Name,
                        [Title] = @Title,
                        [Description] = @Description,
                        [IconPath] = @IconPath,
                        [Url] = @Url,
                        [IsInternalApplication] = @IsInternalApplication,
                        [ShowAppForAllUsers] = @ShowAppForAllUsers,
                        [UpdateBy] = @UpdateBy,
                        [UpdatedOn] = @UpdatedOn
                        WHERE ApplicationId=@ApplicationId";
            return await _context.Connection.ExecuteAsync(sql);
        }
    }
}

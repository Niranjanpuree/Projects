using System.Collections.Generic;
using System.Linq;
using Dapper;
using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;

namespace Northwind.Costpoint.Data
{
    public class ProjectModRepositoryCP : IProjectModRepositoryCP
    {
        //Costpoint does not have a Title column for a Mod, as far as we can tell
        private const string SelectQuery = @"SELECT
                                                PROJ_ID+PROJ_MOD_ID as ProjectModId,
                                                PROJ_ID as ProjectNumber,
                                                PROJ_MOD_ID as ModNumber,
                                                PROJ_MOD_DESC as Title,
                                                PROJ_MOD_DESC as Description,
                                                EFFECT_DT as AwardDate,
                                                PROJ_START_DT as POPStartDate,
                                                PROJ_END_DT as POPEndDate,
                                                PROJ_V_CST_AMT as AwardAmount,
                                                PROJ_F_CST_AMT as FundedAmount
                                        from DELTEK.PROJ_MOD";
        IPFSDBContext _context;

        public ProjectModRepositoryCP(IPFSDBContext context)
        {
            _context = context;
        }

        public ProjectModCP GetProjectModById(long projectModId)
        {
            //Column names need to match ProjectModCP
            var sql = $"{SelectQuery} Where PROJ_MOD_ID=@ProjectModId";
            var result = _context.Connection.QuerySingleOrDefault<ProjectModCP>(sql, new { ProjectModId = projectModId });
            return result;
        }

        public IEnumerable<ProjectModCP> GetProjectMods(string ProjectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue)
        {
            //var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            //var query = queryBuilder.getQuery();
            //var _builder = new SqlBuilder();
            //var selector = _builder.AddTemplate(" /**where**/");
            //foreach (dynamic d in query)
            //{
            //    _builder.Where(d.sql, d.value);
            //}
            //var where = selector.RawSql.Replace("WHERE"," AND ");
            //var parameters = (Dapper.DynamicParameters) selector.Parameters;
            //parameters.Add("PROJ_ID", ProjectNumber);

            var whereBuilder = ProjectModWhereBuilder(ProjectNumber, searchValue, postValue);
            //var sql = $"{SelectQuery} WHERE PROJ_ID='{ProjectNumber}' {where} Order By {orderBy}";
            var sql = $"{SelectQuery} {whereBuilder.where} Order By {orderBy}";
            return _context.Connection.Query<ProjectModCP>(sql, whereBuilder.parameters);
        }

        public int GetProjectModsCount(string ProjectNumber, string searchValue, List<AdvancedSearchRequest> postValue)
        {
            //To Do: get count logic
            var whereBuilder = ProjectModWhereBuilder(ProjectNumber, searchValue, postValue);
            var sql = $"select Count(1) from DELTEK.PROJ_MOD PF {whereBuilder.where}";

            return _context.Connection.ExecuteScalar<int>(sql,whereBuilder.parameters);
        }

        private class WhereBuildReturn {
            public string where { get; set; }
            public DynamicParameters parameters { get; set; }
        }
        private WhereBuildReturn ProjectModWhereBuilder(string ProjectNumber, string searchValue, List<AdvancedSearchRequest> postValue)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }
            var where = selector.RawSql.Replace("WHERE", " AND ");
            var parameters = (Dapper.DynamicParameters)selector.Parameters;
            parameters.Add("PROJ_ID", ProjectNumber);
            string sql = $"WHERE PROJ_ID='{ProjectNumber}' {where}";
            return new WhereBuildReturn { 
                where = sql,
                parameters = parameters
            };
        }

        public IEnumerable<ProjectModCP> GetCostPointProjectModsByProjectNumber(string ProjectNumber)
        {
            var sql = @"SELECT proj_mod.PROJ_ID as ProjectNumber, proj_mod.PROJ_MOD_ID as ProjectModId,  proj_mod.PROJ_MOD_DESC as Title, proj_mod.PROJ_MOD_DESC as Description, proj_mod.PROJ_START_DT as POPStartDate, proj_mod.PROJ_END_DT as POPEndDate, 
                        proj_mod.PROJ_MOD_ID as ModNumber,  EFFECT_DT as AwardDate,  isnull(proj_mod.PROJ_F_CST_AMT,0) + isnull(proj_mod.PROJ_F_FEE_AMT,0) as FundedAmount,  
                        isnull(proj_mod.PROJ_V_CST_AMT,0) as cost,  isnull(proj_mod.PROJ_V_FEE_AMT,0) as fee, cust.CUST_NAME ,PROJ_V_TOT_AMT as TotalAmount
                        FROM DELTEK.CUST AS cust 
                        RIGHT OUTER JOIN DELTEK.PROJ AS Proj ON cust.CUST_ID = Proj.CUST_ID 
                        RIGHT OUTER JOIN DELTEK.PROJ_MOD AS proj_mod ON Proj.PROJ_ID = proj_mod.PROJ_ID 
                        WHERE (proj_mod.PROJ_ID like @ProjectNumber ) ORDER BY proj_mod.EFFECT_DT";
            return _context.Connection.Query<ProjectModCP>(sql, new { ProjectNumber });
        }

        public ProjectModCP GetBriefedThroughModNo(string projectNumber)
        {
            var sql = @"SELECT DISTINCT PROJ_ID,
                              MAX(PROJ_MOD_ID) AS ModNumber,
                              MAX(EFFECT_DT) AS AwardDate
                            FROM DELTEK.PROJ_MOD
                            where PROJ_ID = @ProjectNumber
                            GROUP BY PROJ_ID
                            ORDER BY PROJ_ID";
            var result = _context.Connection.Query<ProjectModCP>(sql, new { ProjectNumber = projectNumber });
            if(result.ToList().Count > 0)
            {
                return result.SingleOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}

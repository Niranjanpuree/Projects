using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;

namespace Northwind.Costpoint.Data
{
    public class ProjectRepositoryCP : IProjectRepositoryCP
    {
        IPFSDBContext _context;
        public ProjectRepositoryCP(IPFSDBContext context)
        {
            _context = context;
        }

        public ProjectCP GetCostPointProjectByProjectNumber(string projectNumber)
        {
            var sql = @"SELECT PROJ_ID AS ProjectNumber, PROJ_NAME AS ProjectName, ORG_ID AS OrgId, PROJ_F_TOT_AMT AS FundedAmount, PROJ_TYPE_DC As ContractType, 
                        PROJ_V_CST_AMT AS EstimatedCost, PROJ_V_FEE_AMT AS EstimatedFee, PROJ_V_TOT_AMT as ContractAmount, PROJ_START_DT As POPStartDate,
                        PROJ_END_DT as POPEndDate, PRIME_CONTR_ID as ContractNumber
                        from DELTEK.PROJ Where PROJ_ID=@ProjectNumber";
            var result = _context.Connection.Query<ProjectCP>(sql, new { ProjectNumber = projectNumber });
            if (result.AsList().Count > 0)
                return result.AsList<ProjectCP>()[0];
            else
                return null;
        }

        public ProjectCP GetProjectByProjectNumber(string projectNumber)
        {
            var sql = "select * from Projects Where projectNumber=@ProjectNumber";
            var result = _context.Connection.Query<ProjectCP>(sql, new { ProjectNumber = projectNumber });
            if (result.AsList().Count > 0)
                return result.AsList<ProjectCP>()[0];
            else
                return null;
        }

        public int GetProjectCount(Guid UserGuid, string searchValue, List<AdvancedSearchRequest> postValue)
        {
            var sql = "select Count(1) from Projects";
            return _context.Connection.ExecuteScalar<int>(sql);
        }

        public IEnumerable<ProjectCP> GetProjects(Guid UserGuid, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }

            var sql = "select * from Projects";
            return _context.Connection.Query<ProjectCP>(sql, new { UserGuid });
        }

        public void UpdateContractGuidByProjectNumber(string projectNumber,Guid contractGuid)
        {
            var sql = @"UPDATE Projects
                        SET ContractGuid = @contractGuid
                        WHERE ProjectNumber = @projectNumber";
            _context.Connection.ExecuteScalar(sql,new { contractGuid = contractGuid , projectNumber = projectNumber});
        }
        
    }
}

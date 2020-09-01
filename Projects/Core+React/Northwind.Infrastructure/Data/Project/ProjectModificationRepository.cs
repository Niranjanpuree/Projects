using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System.Linq;
using System.Net.Sockets;
using System.Text;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Infrastructure.Data.Project
{
    public class ProjectModificationRepository : IProjectModificationRepository
    {
        private readonly IDatabaseContext _context;
        public ProjectModificationRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<ProjectModificationModel> GetAll(Guid projectGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();

            if (sortField.Equals("isActiveStatus"))
            {
                orderingQuery.Append($"ProjectModification.isActive {sortDirection}");  //Ambiguous if not done.. 
            }
            else
            {
                orderingQuery.Append($"{sortField} {sortDirection}");
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                conditionalQuery.Append($"and ProjectModification.ProjectReference like '%{searchValue}%'");
            }

            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                       (SELECT ROW_NUMBER() OVER (ORDER BY ProjectModification.ModificationNumber asc) AS RowNum, 
                                                                                       ProjectModification.ProjectModificationGuid					                ,
                                                                                       ProjectModification.ProjectGuid					                            ,
                                                                                       ProjectModification.ModificationNumber						                ,
                                                                                       ProjectModification.EnteredDate						                        ,
                                                                                       ProjectModification.EffectiveDate				                            ,
                                                                                       ProjectModification.AwardAmount			                            ,
                                                                                       ProjectModification.POPStart						                            ,
                                                                                       ProjectModification.POPEnd						                            ,
                                                                                       ProjectModification.Description						                            ,
                                                                                       ProjectModification.UpdatedOn						                        ,
                                                                                       ProjectModification.IsActive						                            ,
                                                                                       ProjectModification.ModificationTitle						                            ,
                                                                                       ProjectModification.IsAwardAmount						                ,
                                                                                       ProjectModification.IsFundingAmount						                ,
                                                                                       ProjectModification.IsPop						                ,
                                                                                       ProjectModification.FundingAmount						                ,
                                                                                       Contract.ProjectNumber                           						        ,
                                                                                       Contract.ContractTitle	                        
                                                                                        
                                                                                       from ProjectModification
                                                                                       left join Contract
                                                                                       on ProjectModification.ProjectGuid = Contract.ContractGuid
                                                                                       where ProjectModification.IsDeleted = 0
                                                                                        {conditionalQuery}
                                                                                       and   Contract.ContractGuid = @ProjectGuid
                                                                                       
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > {skip} 
                                            AND RowNum <= {pageSize + skip}  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<ProjectModificationModel>(pagingQuery, new { ProjectGuid = projectGuid });
            return pagedData;
        }
        public int TotalRecord(Guid projectGuid)
        {
            string sql = $@"SELECT Count(1) 
                            from ProjectModification
                            left join Project
                            on ProjectModification.ProjectGuid = Project.ProjectGuid
                            where ProjectModification.IsDeleted = 0
                            and   Project.ProjectGuid = @ProjectGuid";
            var result = _context.Connection.QuerySingle<int>(sql, new { ProjectGuid = projectGuid });
            return result;
        }
        public int Add(ProjectModificationModel projectModificationModel)
        {

            string insertQuery = @"INSERT INTO [dbo].[ProjectModification]
                                                                   (
                                                                    ProjectModificationGuid						                    ,
                                                                    ProjectGuid						                                ,
                                                                    ModificationNumber							                    ,
                                                                    EnteredDate			            			                    ,
                                                                    EffectiveDate				                                    ,
                                                                    AwardAmount				                            ,
                                                                    POPStart											            ,
                                                                    POPEnd						                                    ,     
                                                                    Description						                                    ,
                                                                    UploadedFileName						                            ,
                                                                    ModificationTitle						                            ,
                                                                    IsAwardAmount						                            ,
                                                                    IsFundingAmount						                            ,
                                                                    IsPop						                            ,
                                                                    FundingAmount						                            ,
                                                                    
                                                                    CreatedOn						                            ,
                                                                    UpdatedOn						                            ,
                                                                    CreatedBy						                            ,
                                                                    UpdatedBy						                            ,
                                                                    IsActive						                            ,
                                                                    IsDeleted                                                   
                                                                    )
                                  VALUES (
                                                                    @projectModificationGuid						                    ,
                                                                    @projectGuid						                                ,
                                                                    @ModificationNumber							                    ,
                                                                    @EnteredDate			            			                    ,
                                                                    @EffectiveDate				                                    ,
                                                                    @AwardAmount				                            ,
                                                                    @POPStart											            ,
                                                                    @POPEnd						                                    ,     
                                                                    @Description						                                    ,
                                                                    @UploadedFileName						                            ,
                                                                    @ModificationTitle						                            ,
                                                                    @IsAwardAmount						                            ,
                                                                    @IsFundingAmount						                            ,
                                                                    @IsPop						                            ,
                                                                    @FundingAmount						                            ,
                                                                    
                                                                    @CreatedOn						                            ,
                                                                    @UpdatedOn						                            ,
                                                                    @CreatedBy						                            ,
                                                                    @UpdatedBy						                            ,
                                                                    @IsActive						                            ,
                                                                    @IsDeleted                                                   
                                                                )";
            return _context.Connection.Execute(insertQuery, projectModificationModel);
        }
        public int Edit(ProjectModificationModel projectModificationModel)
        {
            string updateQuery = @"Update ProjectModification set 
                                                                    ProjectGuid					     = @ProjectGuid					,
                                                                    ModificationNumber			     = @ModificationNumber			,
                                                                    EnteredDate				         = @EnteredDate				    ,   
                                                                    EffectiveDate				     = @EffectiveDate				,   
                                                                    AwardAmount				         = @AwardAmount				    ,
                                                                    POPStart						 = @POPStart					,	
                                                                    POPEnd						     = @POPEnd						,
                                                                    Description					     = @Description					,
                                                                    UploadedFileName				 = @UploadedFileName			,	
                                                                    ModificationTitle				 = @ModificationTitle			,	
                                                                    IsAwardAmount				    = @IsAwardAmount			,	
                                                                    IsFundingAmount				    = @IsFundingAmount			,	
                                                                    IsPop				            = @IsPop			,	
                                                                    FundingAmount				    = @FundingAmount			,		
                                                                                                     
                                                                    UpdatedOn						 = @UpdatedOn					,	
                                                                    UpdatedBy						 = @UpdatedBy					,	
                                                                    IsActive						 = @IsActive					,	
                                                                    IsDeleted                        = @IsDeleted                   
                                                                    where ProjectModificationGuid    = @ProjectModificationGuid";


            return _context.Connection.Execute(updateQuery, projectModificationModel);
        }
        public int Delete(Guid[] ids)
        {
            foreach (var projectModificationGuid in ids)
            {
                var projectModification = new
                {
                    ProjectModificationGuid = projectModificationGuid
                };
                string disableQuery = @"Update ProjectModification set 
                                               IsDeleted   = 1
                                               where ProjectModificationGuid = @ProjectModificationGuid ";
                _context.Connection.Execute(disableQuery, projectModification);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int Disable(Guid[] ids)
        {
            foreach (var projectModificationGuid in ids)
            {
                var projectModification = new
                {
                    ProjectModificationGuid = projectModificationGuid
                };
                string disableQuery = @"Update ProjectModification set 
                                            IsActive   = 0
                                            where ProjectModificationGuid = @ProjectModificationGuid ";
                _context.Connection.Execute(disableQuery, projectModification);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int Enable(Guid[] ids)
        {
            foreach (var projectModificationGuid in ids)
            {
                var projectModification = new
                {
                    ProjectModificationGuid = projectModificationGuid
                };
                string disableQuery = @"Update ProjectModification set 
                                            IsActive   = 1
                                            where ProjectModificationGuid = @ProjectModificationGuid ";
                _context.Connection.Execute(disableQuery, projectModification);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public ProjectModificationModel GetDetailById(Guid id)
        {
            string sql = @"select distinct 
                            ProjectModification.ProjectModificationGuid		                                    ,
                            ProjectModification.ProjectGuid									                    ,
                            ProjectModification.ModificationNumber				                                ,
                            ProjectModification.ModificationTitle				                                ,
                            ProjectModification.IsAwardAmount				                                ,
                            ProjectModification.IsFundingAmount				                                ,
                            ProjectModification.IsPop				                                ,
                            ProjectModification.FundingAmount				                                ,
                            ProjectModification.EnteredDate						                                ,
                            ProjectModification.EffectiveDate				    				                ,
                            ProjectModification.AwardAmount			                                ,
                            ProjectModification.POPStart							                            ,
                            ProjectModification.POPEnd											                ,
                            ProjectModification.Description						                                ,
                            ProjectModification.UploadedFileName						                            ,
							ProjectModification.IsActive                                                        ,
							ProjectModification.UpdatedOn                                                       ,
                            Project.ProjectNumber                                                               ,
                            Project.ProjectTitle
                            from ProjectModification
                            left join Project
                            on ProjectModification.ProjectGuid = Project.ProjectGuid
                            WHERE ProjectModificationGuid =  @ProjectModificationGuid;";
            var projectModificationModel = _context.Connection.QuerySingle<ProjectModificationModel>(sql, new { ProjectModificationGuid = id });
            return projectModificationModel;
        }
        public bool IsExistModificationNumber(Guid projectGuid, string modificationNumber)
        {
            string modificationNumberQuery = $@"select modificationNumber from  ProjectModification  
                                              where  IsDeleted   = 0
                                                 and projectGuid = @projectGuid
                                                 and modificationNumber = @modificationNumber ";
            var result = _context.Connection.QueryFirstOrDefault<string>(modificationNumberQuery, new { projectGuid = projectGuid, modificationNumber = modificationNumber });
            return !string.IsNullOrEmpty(result) ? true : false;
        }
    }
}


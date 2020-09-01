using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.Questinoaries
{
    public class ContractQuestionariesRepository : IContractQuestionariesRepository
    {
        private readonly IDatabaseContext _context;

        public ContractQuestionariesRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int AddContractQuestionaires(ContractQuestionaire model)
        {
            string sql = $"SELECT Count(*) FROM ContractQuestionaire WHERE ContractGuid = @ContractGuid";
            var result = _context.Connection.QuerySingle<int>(sql, new { ContractGuid = model.ContractGuid });
            if (result == 0)
            {
                string insertQuery = $@"INSERT INTO [dbo].[ContractQuestionaire]
                                                                   (
                                                                        ContractGuid				   ,
                                                                        IsReportExecCompensation	   ,
                                                                        ReportLastReportDate		   ,
                                                                        ReportNextReportDate		   ,
                                                                        IsGSAschedulesale			   ,
                                                                        GSALastReportDate			   ,
                                                                        GSANextReportDate			   ,
                                                                        IsSBsubcontract			       ,
                                                                        SBLastReportDate			   ,
                                                                        SBNextReportDate			   ,
                                                                        IsGQAC					       ,
                                                                        GQACLastReportDate		       ,
                                                                        GQACNextReportDate			   ,
                                                                        IsCPARS					       ,
                                                                        CPARSLastReportDate		   ,
                                                                        CPARSNextReportDate		   ,
                                                                        IsWarranties				   ,
                                                                        IsStandardIndustryProvision	   ,
                                                                        WarrantyProvisionDescription   ,
                                                                        CreatedOn				       ,
                                                                        UpdatedOn		               ,
                                                                        CreatedBy                      ,
                                                                        UpdatedBy				       ,
                                                                        IsDeleted				       ,
                                                                        IsActive				       
                                                                    )
                                  VALUES (
                                                                        @ContractGuid				    ,
                                                                        @IsReportExecCompensation	    ,
                                                                        @ReportLastReportDate		    ,
                                                                        @ReportNextReportDate		    ,
                                                                        @IsGSAschedulesale			    ,
                                                                        @GSALastReportDate			    ,
                                                                        @GSANextReportDate			    ,
                                                                        @IsSBsubcontract			    ,
                                                                        @SBLastReportDate			    ,
                                                                        @SBNextReportDate			    ,
                                                                        @IsGQAC					        ,
                                                                        @GQACLastReportDate		    ,
                                                                        @GQACNextReportDate		    ,
                                                                        @IsCPARS					    ,
                                                                        @CPARSLastReportDate		    ,
                                                                        @CPARSNextReportDate		    ,
                                                                        @IsWarranties				    ,
                                                                        @IsStandardIndustryProvision    ,
                                                                        @WarrantyProvisionDescription   ,
                                                                        @CreatedOn				        ,
                                                                        @UpdatedOn		                ,
                                                                        @CreatedBy                      ,
                                                                        @UpdatedBy				        ,
                                                                        @IsDeleted				        ,
                                                                        @IsActive				                                             
                                                                )";
                 _context.Connection.Execute(insertQuery, model);
            }
            else
            {
                UpdateContractQuestionairesById(model);
            }
            return 1;
        }

        public bool AddQuestionaires(IList<Questionaires> model,Guid contractGuid, DateTime CreatedOn, Guid CreatedBy, DateTime UpdatedOn, Guid UpdatedBy)
        {
            string deleteQuery = "delete from QuestionaireUserAnswer where contractGuid = @contractGuid";
            _context.Connection.Execute(deleteQuery, new { contractGuid = @contractGuid });
            foreach (var data in model)
            {
                
                string insertQuery = "Insert into QuestionaireUserAnswer (ContractGuid,QuestionaireMasterGuid,Questions,Answer,CreatedOn,UpdatedOn,TextAnswer,DateOfLastReport,DateOfNextReport,checkboxanswer,ChildYesNoAnswer,createdBy,updatedBy) values ('" + contractGuid + "',@QuestionGuid,@Question,@YesNoAnswer,'" + CreatedOn + "','" + UpdatedOn + "',@Textanswer,@ReportLastReportDate,@ReportNextReportDate,@CheckBoxAnswer,@ChildYesNoAnswer,'" + CreatedBy + "','" + UpdatedBy + "')";
                 _context.Connection.Execute(insertQuery, data);
            }
            
            return true;
        }

        public ContractQuestionaire GetContractQuestionariesById(Guid id)
        {
            string sql = $"SELECT * FROM ContractQuestionaire WHERE ContractGuid = @ContractGuid;";
            var result = _context.Connection.QueryFirstOrDefault<ContractQuestionaire>(sql, new { ContractGuid = id });
            return result;
        }

        public IEnumerable<QuestionaireMaster> GetContractQuestionaries(string resourceType,string action,Guid contractGuid)
        {
            IEnumerable<QuestionaireMaster> result = null;
            if (!string.IsNullOrWhiteSpace(action) && action.ToLower() == "edit")
            {
                string sql = $@"SELECT * FROM(
								SELECT * FROM QuestionaireMaster where resourcetype = 'FARCONTRACT'  AND IsActive=1
								) q
								LEFT JOIN
								(SELECT * FROM  (
						         SELECT qa.* FROM QuestionaireMaster qm 
                                JOIN QuestionaireUserAnswer qa 
                                ON qm.QuestionaireMasterGuid = qa.QuestionaireMasterGuid   
                                WHERE ResourceType = @ResourceType 
                                AND ContractGuid = @ContractGuid 
                                AND qm.IsActive=1 ) userQA ) a
								ON q.QuestionaireMasterGuid = a.QuestionaireMasterGuid order by ordernumber";
                 result = _context.Connection.Query<QuestionaireMaster>(sql, new { ResourceType = resourceType, ContractGuid = contractGuid });
            }
            else
            {
                string sql = $"SELECT * FROM QuestionaireMaster WHERE ResourceType = @ResourceType AND IsActive=1 order by ordernumber;";
                 result = _context.Connection.Query<QuestionaireMaster>(sql, new { ResourceType = resourceType });
            }
            return result;
        }
  
        public int UpdateContractQuestionairesById(ContractQuestionaire ContractQuestionaire)
        {
            string updateQuery = $@"Update ContractQuestionaire       set 
                                                                    IsReportExecCompensation	 =     @IsReportExecCompensation	,
                                                                    ReportLastReportDate		 =     @ReportLastReportDate		,
                                                                    ReportNextReportDate		 =     @ReportNextReportDate		,
                                                                    IsGSAschedulesale			 =     @IsGSAschedulesale			,
                                                                    GSALastReportDate			 =     @GSALastReportDate			,
                                                                    GSANextReportDate			 =     @GSANextReportDate			,
                                                                    IsSBsubcontract			     =     @IsSBsubcontract			    ,
                                                                    SBLastReportDate			 =     @SBLastReportDate			,
                                                                    SBNextReportDate			 =     @SBNextReportDate			,
                                                                    IsGQAC					     =     @IsGQAC					    ,
                                                                    GQACLastReportDate		     =     @GQACLastReportDate		    ,
                                                                    GQACNextReportDate			 =     @GQACNextReportDate			,
                                                                    IsCPARS					     =     @IsCPARS					    ,
                                                                    CPARSLastReportDate		     =     @CPARSLastReportDate		,                                                                 
                                                                    CPARSNextReportDate		     =     @CPARSNextReportDate		,
                                                                    IsWarranties				 =     @IsWarranties				,
                                                                    IsStandardIndustryProvision	 =     @IsStandardIndustryProvision	,
                                                                    WarrantyProvisionDescription =     @WarrantyProvisionDescription,
                                                                    UpdatedOn		             =     @UpdatedOn		            ,
                                                                    UpdatedBy				     =     @UpdatedBy				    
                                                                      where ContractGuid = @ContractGuid ";
            return _context.Connection.Execute(updateQuery, ContractQuestionaire);
        }

    }
}

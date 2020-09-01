using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data.QustionaireContractClose
{
    public class QuestionaireUserAnswerRepository : IQuestionaireUserAnswerRepository
    {
        IDatabaseContext _context;


        public QuestionaireUserAnswerRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void Add(QuestionaireUserAnswer answer)
        {
            string sql = $@"SELECT count(*)
                            from QuestionaireUserAnswer
                            where ContractGuid= @ContractGuid and QuestionaireMasterGuid =@QuestionaireMasterGuid ";
            var result = _context.Connection.QueryFirstOrDefault<int>(sql,
                new
                {
                    ContractGuid = answer.ContractGuid,
                    QuestionaireMasterGuid = answer.QuestionaireMasterGuid
                });
            if (result == 0)
            {
                string insertQuery = $@"INSERT INTO [dbo].[QuestionaireUserAnswer]
                                                                   ( 
                                                                        ContractGuid,
                                                                        QuestionaireMasterGuid	   ,
                                                                        ManagerUserGuid	   ,
                                                                        ContractCloseApprovalGuid  ,			       
                                                                        RepresentativeType  ,			       
                                                                        Questions  ,			       
                                                                        Answer  ,			       
                                                                        Notes  ,			       
                                                                        CreatedBy  ,			       
                                                                        UpdatedBy  ,			       
                                                                        CreatedOn  ,			       
                                                                        UpdatedOn  ,			       
                                                                        Status  			       
                                                                    )
                                  VALUES (
                                                                        @ContractGuid	    ,
                                                                        @QuestionaireMasterGuid	    ,
                                                                        @ManagerUserGuid	    ,
                                                                        @ContractCloseApprovalGuid	,			                                             
                                                                        @RepresentativeType	,			                                             
                                                                        @Questions	,			                                             
                                                                        @Answer	,			                                             
                                                                        @Notes	,			                                             
                                                                        @CreatedBy	,			                                             
                                                                        @UpdatedBy	,			                                             
                                                                        @CreatedOn	,			                                             
                                                                        @UpdatedOn	,			                                             
                                                                        @Status				                                             
                                                                )";
                _context.Connection.Execute(insertQuery, answer);
            }
            else
            {
                Update(answer);
            }
        }

        public IEnumerable<QuestionaireUserAnswer> GetByContractAndUserGuid(Guid contractGuid, Guid userGuid)
        {
            string sql = $@"SELECT * FROM QuestionaireUserAnswer 
                        where ContractGuid = @ContractGuid and ManagerUserGuid = @UserGuid";
            var data = _context.Connection.Query<QuestionaireUserAnswer>
                (sql,
                new
                {
                    ContractGuid = contractGuid,
                    UserGuid = userGuid
                });
            return data;
        }

        public IEnumerable<QuestionaireUserAnswer> GetByContractGuid(Guid contractGuid)
        {
            string sql = $@"SELECT * FROM QuestionaireUserAnswer 
                        where ContractGuid = @ContractGuid";
            var data = _context.Connection.Query<QuestionaireUserAnswer>
                (sql,
                new
                {
                    ContractGuid = contractGuid
                });
            return data;
        }

        public QuestionaireUserAnswer GetDetailById(Guid contractGuid, string resourceType)
        {
            string sql = $@"SELECT  TOP(1)* FROM QuestionaireUserAnswer QUA
                                    RIGHT JOIN 
                                    QuestionaireMaster QM
                                    on QM.QuestionaireMasterGuid = QUA.QuestionaireMasterGuid
                                    AND QM.ResourceType = @ResourceType
                                    where ContractGuid = @ContractGuid   order by UpdatedOn desc ";
            var data = _context.Connection.QueryFirstOrDefault<QuestionaireUserAnswer>
                (sql,
                new
                {
                    ContractGuid = contractGuid,
                    ResourceType = resourceType
                });
            return data;
        }

        public IEnumerable<Guid> GetDistinctUserGuid(Guid contractGuid)
        {
            string sql = $@"SELECT Distinct(ManagerUserGuid) from QuestionaireUserAnswer 
                            where ContractGuid = @ContractGuid ";
            var data = _context.Connection.Query<Guid>
                   (sql,
                   new
                   {
                       ContractGuid = contractGuid
                   });
            return data;
        }

        public void Update(QuestionaireUserAnswer answer)
        {
            string updateQuery = @"update QuestionaireUserAnswer set 
                                                                        ContractGuid              =@ContractGuid,
                                                                        QuestionaireMasterGuid	  =@QuestionaireMasterGuid,	
                                                                        ManagerUserGuid	          =@ManagerUserGuid,
                                                                        ContractCloseApprovalGuid =@ContractCloseApprovalGuid,       
                                                                        RepresentativeType		  =@RepresentativeType,		    
                                                                        Questions 			      =@Questions, 			     
                                                                        Answer			          =@Answer,			       
                                                                        Notes			          =@Notes,			       
                                                                        CreatedBy 		          =@CreatedBy, 		       
                                                                        UpdatedBy		          =@UpdatedBy,		       
                                                                        CreatedOn 			      =@CreatedOn, 			     
                                                                        UpdatedOn 		          =@UpdatedOn, 		       
                                                                        Status  		          =@Status  		
                                                                where QuestionaireMasterGuid = @QuestionaireMasterGuid ";
            _context.Connection.Execute(updateQuery, answer);
        }

        public bool CheckQAByContractGuid(Guid contractGuid)
        {
            var sql = @"SELECT COUNT(*)
                        FROM QuestionaireUserAnswer
                        WHERE ContractGuid = @contractGuid";
            var count = _context.Connection.QueryFirstOrDefault<int>(sql, new { contractGuid = contractGuid });
            if (count > 0)
                return true;
            return false;
        }
    }
}

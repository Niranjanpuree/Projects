using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Infrastructure.Data.QustionaireContractClose
{
    public class QuestionaireMasterRepository : IQuestionaireMasterRepository
    {
        IDatabaseContext _context;


        public QuestionaireMasterRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void Add(QuestionaireMaster answer)
        {
        }

        public IEnumerable<QuestionaireMaster> GetAll(string resourceType)
        {
            string sql = $@"SELECT * FROM QuestionaireMaster where IsActive = 1 
                                    and ResourceType = @ResourceType";
            var data = _context.Connection.Query<QuestionaireMaster>(sql, new { ResourceType = resourceType });
            return data;
        }

        public QuestionaireMaster GetQuestionaireByQuestion(string question)
        {
            var sql = @"SELECT * 
                        FROM QuestionaireMaster 
                        WHERE Questions = @question
                        AND IsActive = 1";
            return _context.Connection.QueryFirstOrDefault<QuestionaireMaster>(sql, new { question = question });
        }

        //public IEnumerable<QuestionaireMaster> GetQuestionsByManagerType(Guid managerType,string resourceType)
        //{
        //    string sql = $@"SELECT * FROM QuestionaireMaster where IsActive = 1 
        //                            and QuestionaireManagerTypeGuid = @QuestionaireManagerTypeGuid
        //                            and ResourceType = @ResourceType";
        //    var data = _context.Connection.Query<QuestionaireMaster>(sql,new { QuestionaireManagerTypeGuid = managerType, ResourceType = resourceType });
        //    return data;
        //}
    }
}

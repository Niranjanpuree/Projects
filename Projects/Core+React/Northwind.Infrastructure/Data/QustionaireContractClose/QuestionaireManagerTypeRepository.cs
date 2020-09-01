using Dapper;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.QustionaireContractClose
{
    public class QuestionaireManagerTypeRepository : IQuestionaireManagerTypeRepository
    {
        IDatabaseContext _context;

        public QuestionaireManagerTypeRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public Guid GetGuidByNormailzedName(string managerTypeNormalize)
        {
            var data = _context.Connection.QueryFirstOrDefault<Guid>
                ("SELECT * FROM QuestionaireManagerType where managerTypeNormalize = @ManagerTypeNormalize",
                new { ManagerTypeNormalize = managerTypeNormalize });
            return data;
        }

        public string GetNameByGuid(Guid id)
        {
            string sql = $@"SELECT managerTypeNormalize
                            from QuestionaireManagerType
                            where  QuestionaireManagerTypeGuid= @QuestionaireManagerTypeGuid";
            var result = _context.Connection.QueryFirstOrDefault<string>(sql, new { QuestionaireManagerTypeGuid = id });
            return result;
        }
    }
}

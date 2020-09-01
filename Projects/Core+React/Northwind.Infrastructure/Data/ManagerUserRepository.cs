using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data
{
    public class ManagerUserRepository: IManagerUserRepository
    {
        private readonly IDatabaseContext _context;

        public ManagerUserRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void Insert(ManagerUser managerUser)
        {
            managerUser.ManagerUserGUID = Guid.NewGuid();
            _context.Connection.Execute(@"INSERT INTO [ManagerUser]
            ([ManagerUserGUID]
            ,[UserGUID]
            ,[ManagerGUID])
            VALUES
           (@ManagerUserGUID
           ,@UserGUID
           ,@ManagerGUID)", new {
                managerUser.ManagerUserGUID,
                managerUser.UserGUID,
                managerUser.ManagerGUID
            });
        }

        public IEnumerable<ManagerUser> GetManagerUser(ManagerUser managerUser)
        {
            return _context.Connection.Query<ManagerUser>("" +
                "SELECT ManagerUserGUID,UserGUID,ManagerGUID " +
                " FROM ManagerUser " +
                " WHERE UserGUID=@UserGUID AND ManagerGUID=@ManagerGUID", new
                {
                    managerUser.ManagerGUID,
                    managerUser.UserGUID
                });
        }

        public IEnumerable<ManagerUser> GetManagerUser(Guid UserGUID)
        {
            return _context.Connection.Query<ManagerUser>("" +
                "SELECT ManagerUserGUID,UserGUID,ManagerGUID " +
                "FROM ManagerUser " +
                " WHERE UserGUID=@UserGUID", new
                {
                    UserGUID
                });
        }

        public void Delete(ManagerUser managerUser)
        {
            throw new NotImplementedException();
        }

        public void DeleteByUserId(Guid userGuid)
        {
            var sql = "DELETE FROM ManagerUser WHERE UserGUID=@UserGUID";
            _context.Connection.Execute(sql, new { UserGUID = userGuid });
        }

        public void DeleteByManagerId(Guid managerGuid)
        {
            var sql = "DELETE FROM ManagerUser WHERE ManagerGUID=@ManagerGUID";
            _context.Connection.Execute(sql, new { ManagerGUID = managerGuid });
        }

        public bool IsManagerExists(Guid userGUID, Guid managerGuid)
        {
            var result = _context.Connection.Query<ManagerUser>("" +
                "SELECT ManagerUserGUID,UserGUID,ManagerGUID " +
                "FROM ManagerUser " +
                " WHERE UserGUID=@userGUID AND ManagerGUID=@managerGuid", new
                {
                    userGUID,
                    managerGuid
                });
            return result.AsList().Count > 0;
        }
    }
}

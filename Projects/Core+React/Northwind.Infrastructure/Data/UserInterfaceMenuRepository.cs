using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Dapper;

namespace Northwind.Infrastructure.Data
{
    public class UserInterfaceMenuRepository : IUserInterfaceMenuRepository
    {
        private readonly IDatabaseContext _context;
        public UserInterfaceMenuRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<UserInterfaceMenu> GetByClass(string menuClass)
        {
            return _context.Connection.Query<UserInterfaceMenu>("SELECT MenuGuid, MenuNameSpace, MenuText, MenuUrl, MenuDescription, ParentMenuNamespace, MenuClass FROM dbo.UserInterfaceMenu WHERE MenuClass=@MenuClass ORDER BY MenuText ASC ", new { @MenuClass = menuClass });
            
        }
    }
}

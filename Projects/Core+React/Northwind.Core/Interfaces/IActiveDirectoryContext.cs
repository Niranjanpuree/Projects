using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IActiveDirectoryContext
    {
       IActiveDirectoryUserRepository UserRepository { get; set; }
       IActiveDirectoryGroupRepository GroupRepository { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Utilities;

namespace Northwind.Core.Interfaces
{
    public interface IMenuService
    {
        TreeNode<UserInterfaceMenu> GetUserInterfaceMenuTreeByClass(string menuClass);
       
    }
}

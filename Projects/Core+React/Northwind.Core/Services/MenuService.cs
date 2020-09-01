using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using System.Linq;

namespace Northwind.Core.Services
{
    public class MenuService: IMenuService
    {
        private readonly IUserInterfaceMenuRepository _userInterfaceMenuRepo;
        public MenuService(IUserInterfaceMenuRepository userInterfaceMenuRepo)
        {
            _userInterfaceMenuRepo = userInterfaceMenuRepo;

        }

        public TreeNode<UserInterfaceMenu> GetUserInterfaceMenuTreeByClass(string menuClass)
        {
            
            var menuItems = _userInterfaceMenuRepo.GetByClass(menuClass);
            Dictionary<string, TreeNode<UserInterfaceMenu>> dict = new Dictionary<string, TreeNode<UserInterfaceMenu>>();
            List<TreeNode<UserInterfaceMenu>> rootNodes=new List<TreeNode<UserInterfaceMenu>>();


            foreach (var menu in menuItems)
            {
                if (dict.TryGetValue(menu.MenuNamespace, out TreeNode<UserInterfaceMenu> currentNode))
                {
                    currentNode.Data = menu;
                }
                else
                {
                    currentNode = new TreeNode<UserInterfaceMenu>(menu);
                    dict.Add(menu.MenuNamespace, currentNode);
                }

                if (menu.ParentMenuNamespace == null)
                {
                    rootNodes.Add(currentNode);
                }
                else
                {
                    if (!dict.TryGetValue(menu.ParentMenuNamespace, out TreeNode<UserInterfaceMenu> parentNode))
                    {
                        parentNode = new TreeNode<UserInterfaceMenu>(menu);
                        dict.Add(menu.ParentMenuNamespace, parentNode);
                    }
                    parentNode.Children.Add(currentNode);
                    currentNode.Parent = parentNode;

                }
            }

            return rootNodes.FirstOrDefault();

                

        }
 
    }
}

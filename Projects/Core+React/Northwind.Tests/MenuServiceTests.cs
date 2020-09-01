using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using Northwind.Core.Entities;
using Northwind.Core.Services;
using Moq;

namespace Northwind.Tests
{
    [TestClass]
    public class MenuServiceTests
    {

        [TestMethod]
        public void Test()
        {
            var listOfMenuItems = new List<UserInterfaceMenu>();
            listOfMenuItems.Add(new UserInterfaceMenu
            {
                MenuClass = "MainNav",
                MenuText = "Root",
                MenuNamespace = "Root",
                ParentMenuNamespace = null,
            });

            listOfMenuItems.Add(new UserInterfaceMenu {
                MenuClass = "MainNav",
                MenuText = "Root.1",
                MenuNamespace = "Root.1",
                ParentMenuNamespace = "Root",
            });

            listOfMenuItems.Add(new UserInterfaceMenu
            {
                MenuClass = "MainNav",
                MenuText = "Root.2",
                MenuNamespace = "Root.2",
                ParentMenuNamespace = "Root"
            });

            listOfMenuItems.Add(new UserInterfaceMenu
            {
                MenuClass="MainNav",
                MenuText = "Root.1.1",
                MenuNamespace="Root.1.1",
                ParentMenuNamespace = "Root.1"
            });
            var repo = new Mock<IUserInterfaceMenuRepository>();
            repo.Setup(x => x.GetByClass("MainNav")).Returns(listOfMenuItems);
            var menuService = new MenuService(repo.Object);
            var treeNode = menuService.GetUserInterfaceMenuTreeByClass("MainNav");
            Assert.IsNotNull(treeNode);
            
        }



    }
}

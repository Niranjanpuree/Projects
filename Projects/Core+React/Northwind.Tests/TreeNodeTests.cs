using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Utilities;
using System.Diagnostics;

namespace Northwind.Tests
{
    [TestClass]
    public class TreeNodeTests
    {
        
        private TreeBreadthFirstTraversal<string> treeVisitor;

        [TestInitialize]
        public void Init()
        {           
            treeVisitor = new TreeBreadthFirstTraversal<string>();
        }
        
       

        [TestMethod]
        public void AddingChildrenReturnsNotNull()
        {
            var rootNode = new TreeNode<string>("root");
            var child = rootNode.AddChild("root1child");
            Assert.IsNotNull(child);
        }

        [TestMethod]
        public void AddingChildrenReturnsChildrenOfTypeTreeNodeT()
        {
            var rootNode = new TreeNode<string>("root");
            var child = rootNode.AddChild("root1child");
            Assert.IsInstanceOfType(child, typeof(TreeNode<string>));
        }

        [TestMethod] 
        public void AddingChildrenTheCountShouldIncreaseBy1()
        {
            var rootNode = new TreeNode<string>("root");
            var child = rootNode.AddChild("root1child");
            Assert.AreEqual(rootNode.Children.Count, 1);
        }

        [TestMethod]
        public void TreeWithNoChildrenShouldReturnChildrenCountAs0()
        {
            var rootNode = new TreeNode<string>("root");
            Assert.AreEqual(rootNode.Children.Count, 0);
        }

        [TestMethod]
        public void AddingChildrenOneAtATimeMustBePossible()
        {
            var rootNode = new TreeNode<string>("root");
            var firstChild = rootNode.AddChild("oneitem");
            var secondChild = rootNode.AddChild("secondItem");
            Assert.IsTrue(firstChild != null);
            Assert.IsTrue(secondChild != null);
        }

        [TestMethod] 
        public void AddingChildrenMustSetParentToTheNode()
        {
            var rootNode = new TreeNode<string>("root");
            var childNode = rootNode.AddChild("child");
            Assert.AreEqual(childNode.Parent.Data,"root");
        }

        [TestMethod]
        public void LeafNodeIsLeafMustBeTrue()
        {
            var rootNode = new TreeNode<string>("root");
            var childNode = rootNode.AddChild("child");
            Assert.IsTrue(childNode.IsLeaf);
        }

        [TestMethod]
        public void IsLeafShouldBeTrueForRootNodeWhenItisTheOnlyNode()
        {
            var rootNode = new TreeNode<string>("root");
            Assert.IsTrue(rootNode.IsLeaf);
        }

        [TestMethod]
        public void IsRootShouldBeTrueForRootNode()
        {
            var rootNode = new TreeNode<string>("root");
            Assert.IsTrue(rootNode.IsRoot);
        }
        
       

    }
}

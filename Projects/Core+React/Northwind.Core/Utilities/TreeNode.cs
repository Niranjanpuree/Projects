using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Core.Utilities
{
   
        public class TreeNode<T> 
        {
            public T Data { get; set; }
            public TreeNode<T> Parent { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();
            public bool IsRoot
            {
                get { return Parent == null; }
            }

            public bool IsLeaf
            {
                get { return Children.Count == 0; }
            }

            public int Level
            {
                get
                {
                    if (this.IsRoot)
                        return 0;
                    return Parent.Level + 1;
                }
            }

            public TreeNode(T data)
            {
                this.Data = data;
               // this.Children = new LinkedList<TreeNode<T>>();               
            }

            public TreeNode<T> AddChild(T child)
            {
                TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
                this.Children.Add(childNode);
                return childNode;
            }

            
       
    }
}

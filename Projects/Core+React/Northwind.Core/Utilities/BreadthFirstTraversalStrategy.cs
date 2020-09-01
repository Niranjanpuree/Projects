using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Utilities
{
    public class TreeBreadthFirstTraversal<T> : ITreeVisitStrategy<T>   
    {

        public void Visit(TreeNode<T> treeNode, Action<TreeNode<T>> action) { 
            Queue<TreeNode<T>> q = new Queue<TreeNode<T>>();
            q.Enqueue(treeNode); 
            while(q.Count>0)
            {
                TreeNode<T> current = q.Dequeue();
                foreach(var child in current.Children)
                {
                    q.Enqueue(child);                    
                }
                action(current);
            }

        }
    }
}

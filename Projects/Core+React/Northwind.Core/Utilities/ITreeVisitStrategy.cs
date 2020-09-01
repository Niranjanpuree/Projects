using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Utilities
{
    public interface ITreeVisitStrategy<T>
    {
        void Visit(TreeNode<T> node, Action<TreeNode<T>> someAction);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IHangfireScheduler
    {
        void Register();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IDatabaseContextFactory
    {
       IDatabaseContext Create();
    }

    public interface IDatabaseSingletonContextFactory
    {
        IDatabaseContext Create();
    }
}

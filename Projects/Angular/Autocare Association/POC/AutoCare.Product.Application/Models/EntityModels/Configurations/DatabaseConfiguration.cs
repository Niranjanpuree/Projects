using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    internal class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () =>
            new SqlAzureExecutionStrategy(3,TimeSpan.FromSeconds(30))
            );
        }
    }
}

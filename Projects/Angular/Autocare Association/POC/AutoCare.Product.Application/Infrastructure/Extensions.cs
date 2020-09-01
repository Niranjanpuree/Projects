using System;
using System.Data.Entity;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Infrastructure
{
    public static class Extensions
    {
        public static ChangeType ToChangeType(this EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Added:
                    return ChangeType.Add;
                case EntityState.Deleted:
                    return ChangeType.Delete;
                case EntityState.Modified:
                    return ChangeType.Modify;
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    throw new ArgumentOutOfRangeException(nameof(entityState), entityState, null);
            }
        }
    }
}

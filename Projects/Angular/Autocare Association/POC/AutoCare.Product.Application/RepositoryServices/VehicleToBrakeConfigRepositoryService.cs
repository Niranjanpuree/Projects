using System.Data.Entity;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.RepositoryServices
{
    public class VehicleToBrakeConfigRepositoryService : VcdbSqlServerEfRepositoryService<VehicleToBrakeConfig>, IVehicleToBrakeConfigRepositoryService
    {
        public VehicleToBrakeConfigRepositoryService(DbContext context, ITextSerializer serializer) 
            : base(context, serializer)
        {
        }

        //public override async Task<List<VehicleToBrakeConfig>> GetAsync(Expression<Func<VehicleToBrakeConfig, bool>> whereCondition, int topCount = 0)
        //{
        //    return await Context.Set<VehicleToBrakeConfig>()
        //                        .Include(x => x.BrakeConfig)
        //                        .Include(x => x.Vehicle)
        //                        .Where(whereCondition)
        //                        .ToListAsync();
        //}
    }
}

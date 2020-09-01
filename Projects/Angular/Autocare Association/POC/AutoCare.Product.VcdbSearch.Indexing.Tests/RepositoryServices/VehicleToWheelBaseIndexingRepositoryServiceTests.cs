using System.Linq;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Microsoft.Azure.Search.Models;
using Xunit;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Application.Models.EntityModels;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToWheelBaseIndexingRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var indexSchemaRepo = new VehicleToWheelBaseIndexRepositoryService();
            indexSchemaRepo.CreateIndex();
        }


        [Fact()]
        public async void UpdateDocument_AllExistingVehicleToWheelBase_Test()
        {
            var vehicleToWheelBaseRepo = new VehicleToWheelBaseIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            context.Database.CommandTimeout = 0;
            bool isEndReached = false;
            int batch = 0;
            do
            {
                var vehicleToWheelBasesBatch = context.VehicleToWheelBases.OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = vehicleToWheelBasesBatch.Select(item => new VehicleToWheelBaseDocument
                {
                    VehicleToWheelBaseId = item.Id.ToString(),

                    BaseVehicleId = item.Vehicle.BaseVehicleId,
                    WheelBaseChangeRequestId = item.WheelBase.ChangeRequestId,
                    WheelBaseId = item.WheelBaseId,
                    WheelBaseName = item.WheelBase.Base,
                    WheelBaseMetric = item.WheelBase.WheelBaseMetric,
                    MakeId = item.Vehicle.BaseVehicle.MakeId,
                    MakeName = item.Vehicle.BaseVehicle.Make.Name,
                    ModelId = item.Vehicle.BaseVehicle.ModelId,
                    ModelName = item.Vehicle.BaseVehicle.Model.Name,
                    YearId = item.Vehicle.BaseVehicle.YearId,
                    RegionId = item.Vehicle.RegionId,
                    RegionName = item.Vehicle.Region.Name,
                    SubModelId = item.Vehicle.SubModelId,
                    SubModelName = item.Vehicle.SubModel.Name,
                    VehicleId = item.VehicleId,
                    VehicleToWheelBaseChangeRequestId = item.ChangeRequestId,
                    VehicleTypeGroupId = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = item.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = item.Vehicle.BaseVehicle.Model.VehicleType.Name,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToWheelBaseRepo.UpdateDocumentsAsync(documents);

                    Assert.NotNull(result);
                    Assert.True(result.Results.Count > 0);
                    Assert.True(result.Results.All(item => item.Succeeded == true));
                }
                else
                {
                    isEndReached = true;
                }

                batch++;
            }
            while (!isEndReached);

            
        }

    }
}
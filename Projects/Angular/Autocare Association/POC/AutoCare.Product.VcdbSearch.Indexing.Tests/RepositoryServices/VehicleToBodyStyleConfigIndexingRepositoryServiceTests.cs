using System.Linq;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Microsoft.Azure.Search.Models;
using Xunit;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Application.Models.EntityModels;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToBodyStyleConfigIndexingRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var indexSchemaRepo = new VehicleToBodyStyleConfigIndexRepositoryService();
            indexSchemaRepo.CreateIndex();
        }


        [Fact()]
        public async void UpdateDocument_AllExistingVehicleToBodyStyleConfigs_Test()
        {
            var vehicleToBodyStyleConfigRepo = new VehicleToBodyStyleConfigIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            context.Database.CommandTimeout = 0;
            bool isEndReached = false;
            int batch = 0;
            do
            {
                var vehicleToBodyStyleConfigsBatch = context.VehicleToBodyStyleConfigs.OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = vehicleToBodyStyleConfigsBatch.Select(item => new VehicleToBodyStyleConfigDocument
                {
                    VehicleToBodyStyleConfigId = item.Id.ToString(),

                    BaseVehicleId = item.Vehicle.BaseVehicleId,
                    BodyStyleConfigChangeRequestId = item.BodyStyleConfig.ChangeRequestId,
                    BodyStyleConfigId = item.BodyStyleConfigId,
                    BodyTypeId = item.BodyStyleConfig.BodyTypeId,
                    BodyTypeName = item.BodyStyleConfig.BodyType.Name,
                    BodyNumDoorsId = item.BodyStyleConfig.BodyNumDoorsId,
                    BodyNumDoors = item.BodyStyleConfig.BodyNumDoors.NumDoors,
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
                    VehicleToBodyStyleConfigChangeRequestId = item.ChangeRequestId,
                    VehicleTypeGroupId = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = item.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = item.Vehicle.BaseVehicle.Model.VehicleType.Name,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToBodyStyleConfigRepo.UpdateDocumentsAsync(documents);

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
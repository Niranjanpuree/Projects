using System.Linq;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Microsoft.Azure.Search.Models;
using Xunit;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Application.Models.EntityModels;
using System;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToBrakeConfigIndexingRepositoryServiceTests
    {
        [Fact()]
        public async void UpdateDocument_AllExistingVehicleToBrakeConfigs_Test()
        {
            var vehicleToBrakeConfigRepo = new VehicleToBrakeConfigIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            bool isEndReached = false;
            int batch = 0;
            context.Database.CommandTimeout = 180;
            do
            {
                var vehicleToBrakeConfigsBatch = context.VehicleToBrakeConfigs.OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = vehicleToBrakeConfigsBatch.Select(item => new VehicleToBrakeConfigDocument
                {
                    VehicleToBrakeConfigId = item.Id.ToString(),

                    BaseVehicleId = item.Vehicle.BaseVehicleId,
                    BrakeABSId = item.BrakeConfig.BrakeABSId,
                    BrakeABSName = item.BrakeConfig.BrakeABS.Name,
                    BrakeConfigChangeRequestId = item.BrakeConfig.ChangeRequestId,
                    BrakeConfigId = item.BrakeConfigId,
                    BrakeSystemId = item.BrakeConfig.BrakeSystemId,
                    BrakeSystemName = item.BrakeConfig.BrakeSystem.Name,
                    FrontBrakeTypeId = item.BrakeConfig.FrontBrakeTypeId,
                    FrontBrakeTypeName = item.BrakeConfig.FrontBrakeType.Name,
                    MakeId = item.Vehicle.BaseVehicle.MakeId,
                    MakeName = item.Vehicle.BaseVehicle.Make.Name,
                    ModelId = item.Vehicle.BaseVehicle.ModelId,
                    ModelName = item.Vehicle.BaseVehicle.Model.Name,
                    RearBrakeTypeId = item.BrakeConfig.RearBrakeTypeId,
                    RearBrakeTypeName = item.BrakeConfig.RearBrakeType.Name,
                    YearId = item.Vehicle.BaseVehicle.YearId,
                    RegionId = item.Vehicle.RegionId,
                    RegionName = item.Vehicle.Region.Name,
                    Source = null,
                    SubModelId = item.Vehicle.SubModelId,
                    SubModelName = item.Vehicle.SubModel.Name,
                    VehicleId = item.VehicleId,
                    VehicleToBrakeConfigChangeRequestId = item.ChangeRequestId,
                    VehicleTypeGroupId = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = item.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = item.Vehicle.BaseVehicle.Model.VehicleType.Name,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToBrakeConfigRepo.UpdateDocumentsAsync(documents);

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

            //upload brakeconfigs that do not have any vehicletobrakeconfigs
            isEndReached = false;
            batch = 0;
            do
            {
                var brakeConfigsBatch = context.BrakeConfigs.Where(item => item.VehicleToBrakeConfigs.Count == 0).OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = brakeConfigsBatch.Select(item => new VehicleToBrakeConfigDocument
                {
                    VehicleToBrakeConfigId = Guid.NewGuid().ToString(),
                    BrakeConfigId = item.Id,
                    FrontBrakeTypeId = item.FrontBrakeTypeId,
                    FrontBrakeTypeName = item.FrontBrakeType.Name,
                    RearBrakeTypeId = item.RearBrakeTypeId,
                    RearBrakeTypeName = item.RearBrakeType.Name,
                    BrakeABSId = item.BrakeABSId,
                    BrakeABSName = item.BrakeABS.Name,
                    BrakeSystemId = item.BrakeSystemId,
                    BrakeSystemName = item.BrakeSystem.Name,
                    BrakeConfigChangeRequestId = item.ChangeRequestId
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToBrakeConfigRepo.UpdateDocumentsAsync(documents);

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
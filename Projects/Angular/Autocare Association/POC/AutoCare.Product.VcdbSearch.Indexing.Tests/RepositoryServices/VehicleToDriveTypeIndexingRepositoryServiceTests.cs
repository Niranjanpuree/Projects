using System;
using System.Linq;
using AutoCare.Product.Application.Models.EntityModels;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToDriveTypeIndexingRepositoryServiceTests
    {
        [Fact()]
        public async void UpdateDocument_AllExistingCR_Test()
        {
            var driveTypeRepo = new VehicleToDriveTypeIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            bool isEndReached = false;
            int batch = 0;
            //Current count: 
            //int batch = 17;
            //Use this if requried: context.Database.CommandTimeout = 180;
            do
            {
                var vehicleToDriveTypeBatch = context.VehicleToDriveTypes
                                                        .Where(x=> (x.DriveTypeId.Equals(11) || x.DriveTypeId.Equals(12)))
                                                        .OrderBy(item => item.Id)
                                                        .Skip(batch * 1000).Take(1000);
                var documents = vehicleToDriveTypeBatch.Select(item => new VehicleToDriveTypeDocument
                {
                    VehicleToDriveTypeId = item.Id.ToString(),

                    VehicleId = item.Vehicle.Id,
                    BaseVehicleId = item.Vehicle.BaseVehicleId,
                    MakeId = item.Vehicle.BaseVehicle.MakeId,
                    MakeName = item.Vehicle.BaseVehicle.Make.Name,
                    ModelId = item.Vehicle.BaseVehicle.ModelId,
                    ModelName = item.Vehicle.BaseVehicle.Model.Name,
                    YearId = item.Vehicle.BaseVehicle.YearId,
                    SubModelId = item.Vehicle.SubModelId,
                    SubModelName = item.Vehicle.SubModel.Name,
                    RegionId = item.Vehicle.RegionId,
                    RegionName = item.Vehicle.Region.Name,
                    VehicleTypeId = item.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = item.Vehicle.BaseVehicle.Model.VehicleType.Name,
                    VehicleTypeGroupId = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    DriveTypeId = item.DriveTypeId,
                    DriveTypeName = item.DriveType.Name,

                    Source = "",
                    DriveTypeChangeRequestId = item.DriveType.ChangeRequestId,
                    VehicleToDriveTypeChangeRequestId = item.ChangeRequestId
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await driveTypeRepo.UpdateDocumentsAsync(documents);

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
                var vehicleToDriveTypeBatch = context.DriveTypes.Where(item => item.VehicleToDriveTypes.Count == 0).OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = vehicleToDriveTypeBatch.Select(item => new VehicleToDriveTypeDocument
                {
                    VehicleToDriveTypeId = Guid.NewGuid().ToString(),

                    DriveTypeId = item.Id,
                    DriveTypeName = item.Name,
                    DriveTypeChangeRequestId = item.ChangeRequestId
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await driveTypeRepo.UpdateDocumentsAsync(documents);

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

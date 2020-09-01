using System.Linq;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Microsoft.Azure.Search.Models;
using Xunit;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Application.Models.EntityModels;
using System;


namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToMfrBodyCodeIndexingRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var indexSchemaRepo = new VehicleToMfrBodyCodeIndexRepositoryService();
            indexSchemaRepo.CreateIndex();
        }
        [Fact()]
        public async void UpdateDocument_AllExistingVehicleToMfrBodyCodes_Test()
        {
            var vehicleToMfrBodyCodeRepo = new VehicleToMfrBodyCodeIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            bool isEndReached = false;
            int batch = 0;
            do
            {
                var vehicleToMfrBodyCodesBatch = context.VehicleToMfrBodyCodes.OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = vehicleToMfrBodyCodesBatch.Select(item => new VehicleToMfrBodyCodeDocument
                {
                    VehicleToMfrBodyCodeId = item.Id.ToString(),
                    BaseVehicleId = item.Vehicle.BaseVehicleId,
                    MfrBodyCodeChangeRequestId = item.MfrBodyCode.ChangeRequestId,
                    MfrBodyCodeId = item.MfrBodyCode.Id,
                    MfrBodyCodeName = item.MfrBodyCode.Name,
                    MakeId = item.Vehicle.BaseVehicle.MakeId,
                    MakeName = item.Vehicle.BaseVehicle.Make.Name,
                    ModelId = item.Vehicle.BaseVehicle.ModelId,
                    ModelName = item.Vehicle.BaseVehicle.Model.Name,
                    YearId = item.Vehicle.BaseVehicle.YearId,
                    RegionId = item.Vehicle.RegionId,
                    RegionName = item.Vehicle.Region.Name,
                    Source = null,
                    SubModelId = item.Vehicle.SubModelId,
                    SubModelName = item.Vehicle.SubModel.Name,
                    VehicleId = item.VehicleId,
                    VehicleToMfrBodyCodeChangeRequestId = item.ChangeRequestId,
                    VehicleTypeGroupId = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = item.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = item.Vehicle.BaseVehicle.Model.VehicleType.Name,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToMfrBodyCodeRepo.UpdateDocumentsAsync(documents);

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

            //upload MfrBodyCodes that do not have any vehicletoMfrBodyCodes
            isEndReached = false;
            batch = 0;
            do
            {
                var MfrBodyCodesBatch = context.MfrBodyCodes.Where(item => item.VehicleToMfrBodyCodes.Count == 0).OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = MfrBodyCodesBatch.Select(item => new VehicleToMfrBodyCodeDocument
                {
                    VehicleToMfrBodyCodeId = Guid.NewGuid().ToString(),
                    MfrBodyCodeId = item.Id,
                    MfrBodyCodeName = item.Name,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToMfrBodyCodeRepo.UpdateDocumentsAsync(documents);

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

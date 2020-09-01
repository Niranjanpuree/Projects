using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Microsoft.Azure.Search.Models;
using Xunit;
using System.Linq;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Application.Models.EntityModels;
using System;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleIndexingRepositoryServiceTests
    {
        //[Fact()]
        //public async void UploadDocumentsAsyncTest()
        //{
        //    var vehicleRepo = new VehicleIndexingRepositoryService();
        //    await vehicleRepo.UploadDocumentsAsync(null);
        //}

        [Fact()]
        public async void UpdateDocumentAsyncTest()
        {
            var vehicleRepo = new VehicleIndexingRepositoryService();
            VehicleDocument document = new VehicleDocument { VehicleId = "043aec55-9708-4629-9df8-a6a0d9a50843", BaseVehicleId = 138244 };
            DocumentIndexResult result = await vehicleRepo.UpdateDocumentAsync(document);

            Assert.NotNull(result);
            Assert.True(result.Results.Count > 0);
            Assert.True(result.Results.All(item => item.Succeeded));
        }

        [Fact()]
        public async void UpdateDocument_AllExistingVehicles_Test()
        {
            var vehicleRepo = new VehicleIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            bool isEndReached = false;
            int batch = 0;
            context.Database.CommandTimeout = 180;
            //Use this if requried: context.Database.CommandTimeout = 180;
            do
            {
                var vehiclesBatch = context.Vehicles.OrderBy(item=>item.Id).Skip(batch * 1000).Take(1000);
                var documents = vehiclesBatch.Select(item => new VehicleDocument
                {
                    VehicleId = item.Id.ToString(),

                    BaseVehicleId = item.BaseVehicleId,
                    MakeId = item.BaseVehicle.MakeId,
                    MakeName = item.BaseVehicle.Make.Name,
                    ModelId = item.BaseVehicle.ModelId,
                    ModelName = item.BaseVehicle.Model.Name,
                    YearId = item.BaseVehicle.YearId,
                    RegionId = item.RegionId,
                    RegionName = item.Region.Name,
                    Source = null,
                    SubModelId = item.SubModelId,
                    SubModelName = item.SubModel.Name,
                    VehicleTypeId = item.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = item.BaseVehicle.Model.VehicleType.Name,
                    VehicleTypeGroupId = item.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = item.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleChangeRequestId = item.ChangeRequestId,
                    BaseVehicleChangeRequestId = item.BaseVehicle.ChangeRequestId,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleRepo.UpdateDocumentsAsync(documents);

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


            //upload base vehicles that do not have any vehicles
            isEndReached = false;
            batch = 0;
            do
            {
                var baseVehiclesBatch = context.BaseVehicles.Where(item => item.Vehicles.Count == 0).OrderBy(item => item.Id).Skip(batch * 1000).Take(1000).ToList();
                var documents = baseVehiclesBatch.Select(item => new VehicleDocument
                {
                    VehicleId = Guid.NewGuid().ToString(),
                    BaseVehicleId = item.Id,
                    MakeId = item.MakeId,
                    MakeName = item.Make.Name,
                    ModelId = item.ModelId,
                    ModelName = item.Model.Name,
                    YearId = item.YearId,
                    BaseVehicleChangeRequestId = item.ChangeRequestId
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleRepo.UpdateDocumentsAsync(documents);

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

        //[Fact()]
        //public void TestExpressionTree()
        //{
        //    var vehicleRepo = new VehicleIndexingRepositoryService();
        //    var result = vehicleRepo.Get(x => x.MakeName == "Toyota");
        //    Assert.True(result == "makeName eq 'Toyota'");

        //    result = vehicleRepo.GetVechicleDocumentWhereAsync(x => x.MakeName == "Toyota" || (x.ModelName == "Camry" && x.Year == "2016"));
        //    Assert.True(result == "(makeName eq 'Toyota' or (modelName eq 'Camry' and year eq '2016'))");
        //}
    }
}
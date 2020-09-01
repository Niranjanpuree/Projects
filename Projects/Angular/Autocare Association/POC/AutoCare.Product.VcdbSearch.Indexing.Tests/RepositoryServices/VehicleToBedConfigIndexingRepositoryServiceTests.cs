using System.Linq;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Microsoft.Azure.Search.Models;
using Xunit;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Application.Models.EntityModels;
using System;
using AutoCare.Product.VcdbSearch.RepositoryService;
using AutoCare.Product.Search.Model;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToBedConfigIndexingRepositoryServiceTests
    {
        
        [Fact()]
        public async void UpdateDocument_AllExistingVehicleToBedConfigs_Test()
        {
            var vehicleToBedConfigRepo = new VehicleToBedConfigIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            context.Database.CommandTimeout = 180;

            bool isEndReached = false;
            int batch = 0;
            do
            {
                var vehicleToBedConfigsBatch = context.VehicleToBedConfigs.OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = vehicleToBedConfigsBatch.Select(item => new VehicleToBedConfigDocument
                {
                    VehicleToBedConfigId = item.Id.ToString(),

                    BaseVehicleId = item.Vehicle.BaseVehicleId,
                    BedConfigChangeRequestId = item.BedConfig.ChangeRequestId,
                    BedConfigId = item.BedConfigId,
                    BedLength = item.BedConfig.BedLength.Length,
                    BedLengthId = item.BedConfig.BedLengthId,
                    BedLengthMetric = item.BedConfig.BedLength.BedLengthMetric,
                    BedTypeId = item.BedConfig.BedTypeId,
                    BedTypeName = item.BedConfig.BedType.Name,
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
                    VehicleToBedConfigChangeRequestId = item.ChangeRequestId,
                    VehicleTypeGroupId = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroupId,
                    VehicleTypeGroupName = item.Vehicle.BaseVehicle.Model.VehicleType.VehicleTypeGroup.Name,
                    VehicleTypeId = item.Vehicle.BaseVehicle.Model.VehicleTypeId,
                    VehicleTypeName = item.Vehicle.BaseVehicle.Model.VehicleType.Name,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToBedConfigRepo.UpdateDocumentsAsync(documents);

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
                var brakeConfigsBatch = context.BedConfigs.Where(item => item.VehicleToBedConfigs.Count == 0).OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = brakeConfigsBatch.Select(item => new VehicleToBedConfigDocument
                {
                    VehicleToBedConfigId = Guid.NewGuid().ToString(),
                    BedConfigId = item.Id,
                    BedLength = item.BedLength.Length,
                    BedLengthId = item.BedLengthId,
                    BedLengthMetric = item.BedLength.BedLengthMetric,
                    BedTypeId = item.BedTypeId,
                    BedTypeName = item.BedType.Name,
                    BedConfigChangeRequestId = item.ChangeRequestId,
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await vehicleToBedConfigRepo.UpdateDocumentsAsync(documents);

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

        [Fact()]
        //Method to delete vehicle to bedconfig data with null bedConfigId
        public async void Delete_Dummy_Documents_Test()
        {
            var vehicleToBedConfigRepo = new VehicleToBedConfigIndexingRepositoryService();
            var  vehicleToBedConfigSearchRepositoryService = new VehicleToBedConfigSearchRepositoryService("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletobedconfigs");
            var vehicleToBedConfigSearchResult =
                                    await
                                        vehicleToBedConfigSearchRepositoryService.SearchAsync(null,
                                            $"bedConfigId eq null", new SearchOptions {RecordCount=1000 });

            var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
            foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
            {
                await vehicleToBedConfigRepo.DeleteDocumentByVehicleToBedConfigIdAsync(existingVehicleToBedConfigDocument.VehicleToBedConfigId);
                          
            }
               
        }

    }
}
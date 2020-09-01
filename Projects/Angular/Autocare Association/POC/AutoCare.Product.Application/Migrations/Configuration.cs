using System.Collections.Generic;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AutoCare.Product.Application.Models.EntityModels.VehicleConfigurationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AutoCare.Product.Application.Models.EntityModels.VehicleConfigurationContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Makes.Any())
            {
                var makesList = new List<Make>
                {
                    new Make() { Id = 1, Name = "Toyota"},
                    new Make() { Id = 2, Name = "Honda"},
                    new Make() { Id = 3, Name = "Acura"},
                    new Make() { Id = 4, Name = "Audi"},
                    new Make() { Id = 5, Name = "BMW"}
                };

                context.Makes.AddRange(makesList);
                context.SaveChanges();
            }

            if (!context.VehicleTypeGroups.Any())
            {
                var vehicleTypeGroups = new List<VehicleTypeGroup>
                {
                    new VehicleTypeGroup() { Id = 2, Name = "Light Duty", InsertDate = DateTime.Now},
                };

                context.VehicleTypeGroups.AddRange(vehicleTypeGroups);
                context.SaveChanges();
            }

            if (!context.VehicleTypes.Any())
            {
                var vehicleTypes = new List<VehicleType>
                {
                    new VehicleType() { Id = 5, Name = "Car", VehicleTypeGroupId = 2, InsertDate = DateTime.Now},
                    new VehicleType() { Id = 6, Name = "Truck", VehicleTypeGroupId = 2, InsertDate = DateTime.Now},
                    new VehicleType() { Id = 7, Name = "Van", VehicleTypeGroupId = 2, InsertDate = DateTime.Now},
                };

                context.VehicleTypes.AddRange(vehicleTypes);
                context.SaveChanges();
            }

            if (!context.Models.Any())
            {
                var models = new List<Model>
                {
                    new Model() { Id = 5567, Name = "Knight Model 70A", VehicleTypeId = 5, InsertDate = DateTime.Now},
                    new Model() { Id = 21257, Name = "FJ1 Pickup", VehicleTypeId = 5, InsertDate = DateTime.Now},
                    new Model() { Id = 3, Name = "Grand Vitara", VehicleTypeId = 6, InsertDate = DateTime.Now},
                };

                context.Models.AddRange(models);
                context.SaveChanges();
            }

            if (!context.SubModels.Any())
            {
                var submodels = new List<SubModel>
                {
                    new SubModel() {Id=200, Name="Gordini", InsertDate = DateTime.UtcNow },
                    new SubModel() {Id=201, Name="Sportwagon", InsertDate=DateTime.UtcNow },
                };

                context.SubModels.AddRange(submodels);
                context.SaveChanges();
            }

            if (!context.Regions.Any())
            {
                var regions = new List<Region>
                {
                    new Region() {Id = 1, Name="United States", RegionAbbr = "USA", InsertDate = DateTime.UtcNow },
                    new Region() {Id = 2, Name="Canada", RegionAbbr = "CAN", InsertDate = DateTime.UtcNow },
                    new Region() {Id = 3, Name="Mexico", RegionAbbr = "MEX", InsertDate = DateTime.UtcNow },
                };

                context.Regions.AddRange(regions);
                context.SaveChanges();
            }

            if (!context.Sources.Any())
            {
                var sources = new List<Source>
                {
                    new Source() {Id = 1, Name="OEM",  InsertDate = DateTime.UtcNow },
                    new Source() {Id = 2, Name="OEM+",  InsertDate = DateTime.UtcNow },
                };

                context.Sources.AddRange(sources);
                context.SaveChanges();
            }

            if (!context.PublicationStages.Any())
            {
                var publicationStages = new List<PublicationStage>
                {
                    new PublicationStage() {Id = 1, Name="1",  InsertDate = DateTime.UtcNow },
                    new PublicationStage() {Id = 2, Name="2",  InsertDate = DateTime.UtcNow },
                    new PublicationStage() {Id = 3, Name="3",  InsertDate = DateTime.UtcNow },
                    new PublicationStage() {Id = 4, Name="4",  InsertDate = DateTime.UtcNow },
                };

                context.PublicationStages.AddRange(publicationStages);
                context.SaveChanges();
            }

            if (!context.BaseVehicles.Any())
            {
                var baseVehicles = new List<BaseVehicle>
                {
                    new BaseVehicle() {Id = 1, MakeId = 1, ModelId = 5567, YearId = 2012, InsertDate = DateTime.UtcNow },
                };

                context.BaseVehicles.AddRange(baseVehicles);
                context.SaveChanges();
            }

            if (!context.Vehicles.Any())
            {
                var vehicles = new List<Vehicle>
                {
                    new Vehicle() {Id = 1, BaseVehicleId=1, SubModelId=200, SourceId=1, RegionId=1, PublicationStageId=1, PublicationStageSource="",PublicationStageDate=DateTime.UtcNow, PublicationEnvironment="Dev", InsertDate = DateTime.UtcNow },
                    new Vehicle() {Id = 2, BaseVehicleId=1, SubModelId=200, SourceId=1, RegionId=1, PublicationStageId=1, PublicationStageSource="",PublicationStageDate=DateTime.UtcNow, PublicationEnvironment="Dev", InsertDate = DateTime.UtcNow },
                };

                context.Vehicles.AddRange(vehicles);
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                var role = new List<Role>
                {
                    new Role() {Id = 1, Name="admin", Description = "admin", CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                    new Role() {Id = 2, Name="researcher",  Description = "researcher",CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                    new Role() {Id = 3, Name="user",  Description = "user",CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true}
                };

                context.Roles.AddRange(role);
                context.SaveChanges();
            }

            if (!context.User.Any())
            {
                var user = new List<User>
                {
                    new User() {Id = "admin@autocare.com", Name="admin@autocare.com",  CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true},
                    new User() {Id = "researcher@autocare.com", Name="researcher@autocare.com",  CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                    new User() {Id = "researcher2@autocare.com", Name="researcher2@autocare.com",  CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                    new User() {Id = "user@autocare.com", Name="user@autocare.com",  CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                    new User() {Id = "user2@autocare.com", Name="user2@autocare.com",  CreatedDateTime = DateTime.UtcNow,UpdatedDateTime = DateTime.UtcNow, IsActive = true }
                };

                context.User.AddRange(user);
                context.SaveChanges();
            }

            if (!context.UserRoleAssignments.Any())
            {
                var userRoles = new List<UserRoleAssignment>()
                {
                    new UserRoleAssignment() { Id = 1, RoleId = 1, UserId = "admin@autocare.com", CreatedDateTime = DateTime.UtcNow, UpdatedDateTime = null, IsActive = true },
                    new UserRoleAssignment() { Id = 2, RoleId = 2, UserId = "researcher@autocare.com", CreatedDateTime = DateTime.UtcNow, UpdatedDateTime = null, IsActive = true },
                    new UserRoleAssignment() { Id = 2, RoleId = 2, UserId = "researcher2@autocare.com", CreatedDateTime = DateTime.UtcNow, UpdatedDateTime = null, IsActive = true },
                    new UserRoleAssignment() { Id = 3, RoleId = 3, UserId = "user@autocare.com", CreatedDateTime = DateTime.UtcNow, UpdatedDateTime = null, IsActive = true },
                    new UserRoleAssignment() { Id = 3, RoleId = 3, UserId = "user2@autocare.com", CreatedDateTime = DateTime.UtcNow, UpdatedDateTime = null, IsActive = true }
                };

                context.UserRoleAssignments.AddRange(userRoles);
                context.SaveChanges();
            }
        }
    }
}

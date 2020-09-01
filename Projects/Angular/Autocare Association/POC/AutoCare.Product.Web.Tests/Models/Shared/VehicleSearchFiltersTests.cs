using AutoCare.Product.Web.Models.Shared;
using Xunit;

namespace AutoCare.Product.Web.Tests.Models.Shared
{
    public class VehicleSearchFiltersTests
    {
        [Fact()]
        public void Should_Return_Azure_Filter_String()
        {
            var vehicleSearchFilter = new VehicleSearchFilters()
            {
                StartYear = 2005,
                EndYear = 2010,
                Makes = new string[]
                {
                    "Toyota",
                    "Honda"
                },
                Models = new[]
                {
                    "Camry",
                    "Civic"
                }
            };

            var result = vehicleSearchFilter.ToAzureSearchFilter();
            Assert.True(result ==
                        "(((year ge '2005' and year ge '2010') and (makeName eq 'Toyota' or makeName eq 'Honda')) and (modelName eq 'Camry' or modelName eq 'Civic'))");
        }
    }
}
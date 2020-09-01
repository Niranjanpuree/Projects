using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoMapper;
using Moq;
using Xunit;

namespace AutoCare.Product.ApplicationTests.BusinessServices
{
    public class ChangeRequestBusinessServiceTests
    {
        [Fact()]
        public void Should_Add_ChangeRequest_Entity_With_ChangeType_Inserted_When_Id_Has_DefaultValue()
        {
            //Arrange

            var make = new Make()
            {
                Name = "Toyota"
            };

            var mockVcdbUnitOfWork = new Mock<IVcdbUnitOfWork>();
            var mockAutoMapper = new Mock<IMapper>();
            var mockTextSerializer = new Mock<ITextSerializer>();
            var mockChangeRequestRepositoryService = new Mock<IRepositoryService<ChangeRequestStaging>>();
            var mockChangeRequestIndexingService= new Mock<IChangeRequestIndexingService>(); 

            mockVcdbUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            mockVcdbUnitOfWork.Setup(x => x.GetRepositoryService<ChangeRequestStaging>())
                .Returns(mockChangeRequestRepositoryService.Object);

            var vcdbChangeRequestBusinessService = new VcdbChangeRequestBusinessService(mockVcdbUnitOfWork.Object,
                mockAutoMapper.Object, mockTextSerializer.Object,
                (new Mock<IVcdbChangeRequestItemBusinessService>()).Object,
                (new Mock<IVcdbChangeRequestCommentsBusinessService>()).Object,
                (new Mock<IVcdbChangeRequestAttachmentBusinessService>()).Object,
                mockChangeRequestIndexingService.Object,
                (new Mock<IVehicleIndexingService>()).Object,
                (new Mock<IVehicleToBrakeConfigIndexingService>()).Object,
                (new Mock<IVehicleSearchService>()).Object,
                (new Mock<IVehicleToBrakeConfigSearchService>()).Object,
                (new Mock<IAzureFileStorageRepositoryService>()).Object,
                (new Mock<IVcdbApproveChangeRequestProcessor>()).Object,
                (new Mock<IVcdbRejectChangeRequestProcessor>()).Object,
                (new Mock<IVcdbPreliminaryApproveChangeRequestProcessor>()).Object,
                (new Mock<IVcdbDeleteChangeRequestProcessor>()).Object);

            //Act
            var result = vcdbChangeRequestBusinessService.SubmitAsync(make, 0, "Test").Result;

            //Assert
            Assert.True(result > 0);
        }

        [Fact()]
        public async void Should_Throw_ChangeRequestExistException()
        {
            //Arrange

            var make = new Make()
            {
                Name = "Toyota"
            };

            var mockVcdbUnitOfWork = new Mock<IVcdbUnitOfWork>();
            var mockAutoMapper = new Mock<IMapper>();
            var mockTextSerializer = new Mock<ITextSerializer>();
            var mockChangeRequestRepositoryService = new Mock<IRepositoryService<ChangeRequestStaging>>();
            var mockChangeRequestIndexingService = new Mock<IChangeRequestIndexingService>();

            mockVcdbUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            mockVcdbUnitOfWork.Setup(x => x.GetRepositoryService<ChangeRequestStaging>())
                .Returns(mockChangeRequestRepositoryService.Object);

            mockChangeRequestRepositoryService.Setup(
                x => x.GetAsync(It.IsAny<Expression<Func<ChangeRequestStaging, bool>>>(),0))
                .ReturnsAsync(new List<ChangeRequestStaging>
                {
                    new ChangeRequestStaging
                    {
                        Id = 1,
                    }
                });

            var vcdbChangeRequestBusinessService = new VcdbChangeRequestBusinessService(mockVcdbUnitOfWork.Object,
                mockAutoMapper.Object, mockTextSerializer.Object,
                (new Mock<IVcdbChangeRequestItemBusinessService>()).Object,
                (new Mock<IVcdbChangeRequestCommentsBusinessService>()).Object,
                (new Mock<IVcdbChangeRequestAttachmentBusinessService>()).Object,
                mockChangeRequestIndexingService.Object,
                (new Mock<IVehicleIndexingService>()).Object,
                (new Mock<IVehicleToBrakeConfigIndexingService>()).Object,
                (new Mock<IVehicleSearchService>()).Object,
                (new Mock<IVehicleToBrakeConfigSearchService>()).Object,
                (new Mock<IAzureFileStorageRepositoryService>()).Object,
                (new Mock<IVcdbApproveChangeRequestProcessor>()).Object,
                (new Mock<IVcdbRejectChangeRequestProcessor>()).Object,
                (new Mock<IVcdbPreliminaryApproveChangeRequestProcessor>()).Object,
                (new Mock<IVcdbDeleteChangeRequestProcessor>()).Object);

            //Assert, Act
            await Assert.ThrowsAsync<ChangeRequestExistException>(() => vcdbChangeRequestBusinessService.SubmitAsync(make, 1, "Test"));
        }

        [Fact()]
        public void Should_Add_ChangeRequest_Entity_With_ChangeType_Deleted()
        {
            //Arrange
            var make = new Make()
            {
                Id = 1,
                Name = "Toyota"
            };

            var mockVcdbUnitOfWork = new Mock<IVcdbUnitOfWork>();
            var mockAutoMapper = new Mock<IMapper>();
            var mockTextSerializer = new Mock<ITextSerializer>();
            var mockChangeRequestRepositoryService = new Mock<IRepositoryService<ChangeRequestStaging>>();
            var mockChangeRequestIndexingService = new Mock<IChangeRequestIndexingService>();

            mockVcdbUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            mockVcdbUnitOfWork.Setup(x => x.GetRepositoryService<ChangeRequestStaging>())
                .Returns(mockChangeRequestRepositoryService.Object);

            var vcdbChangeRequestBusinessService = new VcdbChangeRequestBusinessService(mockVcdbUnitOfWork.Object,
                mockAutoMapper.Object, mockTextSerializer.Object,
                (new Mock<IVcdbChangeRequestItemBusinessService>()).Object,
                (new Mock<IVcdbChangeRequestCommentsBusinessService>()).Object,
                (new Mock<IVcdbChangeRequestAttachmentBusinessService>()).Object,
                mockChangeRequestIndexingService.Object,
                (new Mock<IVehicleIndexingService>()).Object,
                (new Mock<IVehicleToBrakeConfigIndexingService>()).Object,
                (new Mock<IVehicleSearchService>()).Object,
                (new Mock<IVehicleToBrakeConfigSearchService>()).Object,
                (new Mock<IAzureFileStorageRepositoryService>()).Object,
                (new Mock<IVcdbApproveChangeRequestProcessor>()).Object,
                (new Mock<IVcdbRejectChangeRequestProcessor>()).Object,
                (new Mock<IVcdbPreliminaryApproveChangeRequestProcessor>()).Object,
                (new Mock<IVcdbDeleteChangeRequestProcessor>()).Object);

            //Act
            var result = vcdbChangeRequestBusinessService.SubmitAsync(make, 1, "Test").Result;

            //Assert
            Assert.True(result > 0);
        }
    }
}
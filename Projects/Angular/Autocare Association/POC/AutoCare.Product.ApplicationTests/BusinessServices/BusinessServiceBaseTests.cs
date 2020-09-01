using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using Moq;
using System.Collections.Generic;
using AutoCare.Product.Application.Models.BusinessModels;
using Xunit;

namespace AutoCare.Product.ApplicationTests.BusinessServices
{
    public class BusinessServiceBaseTests
    {
        [Fact()]
        public void When_Add_Create_New_Change_Request()
        {
            //Arrange

            var make = new Make()
            {
                Id= 1,
                Name = "Nissan"
            };

            var mockVcdbUnitOfWork = new Mock<IVcdbUnitOfWork>();
            var mockVcdbChangeRequestBusinessService = new Mock<IVcdbChangeRequestBusinessService>();
            mockVcdbChangeRequestBusinessService.Setup(x => x.SubmitAsync(It.IsAny<Make>(), It.IsAny<int>(), It.IsAny<string>(), 
                ChangeType.None, It.IsAny<List<ChangeRequestItemStaging>>(), It.IsAny<CommentsStaging>(),
                It.IsAny<List<AttachmentsStaging>>(), It.IsAny<string>()))
                .ReturnsAsync(1);

            var mockTextSerializer = new Mock<ITextSerializer>();

            var makeBusinessService = new MakeBusinessService(mockVcdbUnitOfWork.Object, mockVcdbChangeRequestBusinessService.Object, mockTextSerializer.Object);

            //Act
            var result = makeBusinessService.SubmitAddChangeRequestAsync(make, "test", null, new CommentsStagingModel()).Result;

            //Assert
            Assert.True(result > 0);
        }
    }
}
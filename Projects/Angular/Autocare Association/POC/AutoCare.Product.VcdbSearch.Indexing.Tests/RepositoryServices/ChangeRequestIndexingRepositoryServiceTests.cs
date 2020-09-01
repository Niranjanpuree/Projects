using System.Linq;
using AutoCare.Product.Application.Models.EntityModels;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search.Models;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class ChangeRequestIndexingRepositoryServiceTests
    {
        [Fact()]
        public async void UpdateDocument_AllExistingCR_Test()
        {
            var changeRequestRepo = new ChangeRequestIndexingRepositoryService();
            var context = new VehicleConfigurationContext();
            bool isEndReached = false;
            int batch = 0;
            do
            {
                var changeRequestStaging = context.ChangeRequestStagings.OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = changeRequestStaging.Select(item => new ChangeRequestDocument
                {
                    ChangeRequestId = item.Id.ToString(),

                    //Assignee = (item.ChangeRequestAssignmentStagings != null && item.ChangeRequestAssignmentStagings.Any()) ? item.ChangeRequestAssignmentStagings.OrderByDescending(a => a.Id).First().AssignedByUserId : null,
                    ChangeRequestTypeId = item.ChangeRequestTypeId,
                    Entity = item.Entity,
                    ChangeType = item.ChangeType.ToString(),
                    Status = (short)item.Status,
                    StatusText = item.Status.ToString(),
                    RequestedBy = item.RequestedBy,
                    Likes = item.LikesStagings.Count,
                    SubmittedDate = item.CreatedDateTime,
                    Source = null,
                    CommentExists = (item.CommentsStagings.Count >0) ? true : false
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await changeRequestRepo.UpdateDocumentAsync(documents);

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


            //update data from change request store
            batch = 0;
            isEndReached = false;
            do
            {
                var changeRequestStore = context.ChangeRequestStores.OrderBy(item => item.Id).Skip(batch * 1000).Take(1000);
                var documents = changeRequestStore.Select(item => new ChangeRequestDocument
                {
                    ChangeRequestId = item.Id.ToString(),

                    //Assignee = (item.ChangeRequestAssignments != null && item.ChangeRequestAssignments.Any()) ? item.ChangeRequestAssignments.OrderByDescending(a => a.Id).First().AssignedByUserId : null,
                    ChangeRequestTypeId = item.ChangeRequestTypeId,
                    Entity = item.Entity,
                    ChangeType = item.ChangeType.ToString(),
                    Status = (short)item.Status,
                    StatusText = item.Status.ToString(),
                    RequestedBy = item.RequestedBy,
                    //Likes = item.Likes != null ? item.Likes.Count : 0,
                    Likes = item.Likes.Count,
                    SubmittedDate = item.RequestedDateTime,
                    UpdatedDate = item.CompletedDateTime,
                    Source = null,
                    CommentExists = (item.Comments.Count > 0) ? true : false
                }).ToList();

                if (documents != null && documents.Any())
                {
                    DocumentIndexResult result = await changeRequestRepo.UploadDocumentsAsync(documents);

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

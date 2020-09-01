using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Infrastructure.IdentityAuthentication;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using Microsoft.Practices.ObjectBuilder2;

namespace AutoCare.Product.Web.Controllers.Api
{
    public class ApiControllerBase : ApiController
    {
        public ApiControllerBase()
        {
            //authorize request in ctor
            AuthorizeRequest();
        }
        private void AuthorizeRequest()
        {
        }

        public CustomClaimsIdentity CurrentUser
        {
            get
            {
                var currentUser = User.Identity as CustomClaimsIdentity;
                if (currentUser == null)
                {
                    return new CustomClaimsIdentity();
                }
                return currentUser;
            }
        }

        public ReviewViewModel SetUpChangeRequestReview(string changeRequestStatus, string changeRequestSubmittedBy, ReviewViewModel reviewViewModel)
        {

            var claimsIdentity = User.Identity as CustomClaimsIdentity;
            // set review -> is complete.
            reviewViewModel.IsCompleted = changeRequestStatus.Equals(ChangeRequestStatus.Approved.ToString())
                                                            || changeRequestStatus.Equals(ChangeRequestStatus.Rejected.ToString())
                                                            || changeRequestStatus.Equals(ChangeRequestStatus.Deleted.ToString())
                                                            || (changeRequestStatus.Equals(ChangeRequestStatus.PreliminaryApproved.ToString())
                                                            && CurrentUser.Roles.Contains(CustomRoles.Researcher));
            // set review properties
            reviewViewModel.CanDelete = CurrentUser.CustomerId.Equals(changeRequestSubmittedBy)
                                                          && !reviewViewModel.IsCompleted;
            reviewViewModel.CanLike = !CurrentUser.Roles.Contains(CustomRoles.Admin) && !CurrentUser.Roles.Contains(CustomRoles.Researcher)
                && !CurrentUser.CustomerId.Equals(changeRequestSubmittedBy)
                && !reviewViewModel.IsCompleted;
            reviewViewModel.CanAssign = CurrentUser.Roles.Contains(CustomRoles.Admin) ||
                                                          CurrentUser.Roles.Contains(CustomRoles.Researcher);
            reviewViewModel.CanReview = (CurrentUser.Roles.Contains(CustomRoles.Admin) ||
                                                           CurrentUser.Roles.Contains(CustomRoles.Researcher)) &&
                                                          !reviewViewModel.IsCompleted;
            reviewViewModel.CanFinal = CurrentUser.Roles.Contains(CustomRoles.Admin);
            reviewViewModel.CanSubmit = (CurrentUser.Roles.Contains(CustomRoles.Admin) ||
                                                          CurrentUser.Roles.Contains(CustomRoles.Researcher) ||
                                                          CurrentUser.CustomerId.Equals(changeRequestSubmittedBy)) &&
                                                          !reviewViewModel.IsCompleted;

            //User
            reviewViewModel.CanAttach =
                (reviewViewModel.StagingItem.SubmittedBy.ToLower() ==
                 CurrentUser.Email.ToLower() &&
                 reviewViewModel.StagingItem.Status.ToLower() ==
                 ChangeRequestStatus.Submitted.ToString().ToLower()) ||

                //Researcher
                (CurrentUser.Roles.Contains(CustomRoles.Researcher) &&
                 (reviewViewModel.StagingItem.Status.ToLower() ==
                  ChangeRequestStatus.Submitted.ToString().ToLower() ||
                  reviewViewModel.StagingItem.Status.ToLower() ==
                  ChangeRequestStatus.PreliminaryApproved.ToString().ToLower())) ||

                //Admin
                CurrentUser.Roles.Contains(CustomRoles.Admin);

            reviewViewModel.Attachments.ForEach(attachment =>
            {
                if (claimsIdentity != null)
                {
                    attachment.CanDelete = attachment.AttachedBy == claimsIdentity.Email.ToLower()
                        && reviewViewModel.StagingItem.Status.ToLower() == ChangeRequestStatus.Submitted.ToString().ToLower();
                }
            });

            return reviewViewModel;
        }

        public List<AttachmentsModel> SetUpAttachmentsModels(List<AttachmentInputModel> attachments)
        {
            return attachments?.Select(a => new AttachmentsModel
            {
                AttachmentId = a.AttachmentId,
                FileName = a.FileName,
                FileExtension = a.FileExtension,
                FileSize = a.FileSize,
                ContentType = a.ContentType,
                ContainerName = a.ContainerName,
                ChunksIdList = a.ChunksIdList,
                FileStatus = a.FileStatus,
                AttachedBy = CurrentUser.CustomerId, // attachment.AttachedBy
                DirectoryPath = a.DirectoryPath
            }).ToList();
        }
    }
}

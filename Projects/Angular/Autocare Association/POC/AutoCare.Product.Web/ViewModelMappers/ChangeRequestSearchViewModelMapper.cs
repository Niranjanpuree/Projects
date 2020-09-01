using System;
using System.Linq;
using AutoCare.Product.Search.Model;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.Web.Models.Shared;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class ChangeRequestSearchViewModelMapper : IChangeRequestSearchViewModelMapper
    {
        public ChangeRequestSearchViewModel Map(ChangeRequestSearchResult source)
        {
            var changeRequestSearchViewModel = new ChangeRequestSearchViewModel()
            {
                Facets = new ChangeRequestSearchFacets()
                {
                    ChangeTypes = source.Facets.Any(cr => cr.Name.Equals("changeType")) ?
                        source.Facets.First(cr => cr.Name.Equals("changeType"))
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    ChangeEntities = source.Facets.Any(cr => cr.Name.Equals("entity")) ?
                            source.Facets.First(cr => cr.Name.Equals("entity"))
                                .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                                .ToArray() : default(string[]),
                    Statuses = source.Facets.Any(cr => cr.Name.Equals("statusText")) ?
                        source.Facets.First(cr => cr.Name.Equals("statusText"))
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    RequestsBy = source.Facets.Any(cr => cr.Name.Equals("requestedBy")) ?
                        source.Facets.First(cr => cr.Name.Equals("requestedBy"))
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    Assignees = source.Facets.Any(cr => cr.Name.Equals("assignee")) ?
                        source.Facets.First(cr => cr.Name.Equals("assignee"))
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[])
                },
                Result = new ChangeRequestSearchResultViewModel()
                {
                    ChangeRequests = source.Documents.Any() ?
                    source.Documents.Select(item => new ChangeRequestStagingViewModel()
                    {
                        Id = Convert.ToInt64(item.ChangeRequestId),
                        Assignee = item.Assignee,
                        ChangeRequestTypeId = item.ChangeRequestTypeId,
                        ChangeType = item.ChangeType + " "+ item.Entity,
                        Status = (ChangeRequestStatus)(item.Status ?? 0),
                        StatusText = item.StatusText,
                        RequestedBy = item.RequestedBy,
                        Likes = item.Likes,
                        CreatedDateTime = item.SubmittedDate,
                        UpdatedDateTime = item.UpdatedDate,
                        Entity = item.Entity,
                        CommentExists = item.CommentExists,
                        ChangeContent = item.ChangeContent
                    }).ToList() : null,
                }
            };
            return changeRequestSearchViewModel;
        }
    }
}
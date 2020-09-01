using System;
using System.Linq;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Ajax.Utilities;

namespace AutoCare.Product.Web.Models.Shared
{
    [Serializable]
    public class ChangeRequestSearchFilters
    {
        public string ChangeRequestId { get; set; }
        public string[] ChangeTypes { get; set; }
        public string[] ChangeEntities { get; set; }
        public string[] Statuses { get; set; }
        public string[] RequestsBy { get; set; }
        public string[] Assignees { get; set; }
        public DateTime? SubmittedDateFrom { get; set; }
        public DateTime? SubmittedDateTo { get; set; }


        public string ToAzureSearchFilter()
        {
            Expression<Func<ChangeRequestDocument, bool>> filterEx = null;

            filterEx = AddChangeRequestIdFilter(filterEx);

            filterEx = AddChangeRequestTypeIdFilter(filterEx);

            filterEx = AddEntityFilter(filterEx);

            filterEx = AddStatusFilter(filterEx);

            filterEx = AddRequestedByFilter(filterEx);

            filterEx = AddAssigneeFilter(filterEx);

            filterEx = AddSubmittedDateRangeFilter(filterEx);

            return filterEx?.ToAzureSearchFilter();
        }
        
        private Expression<Func<ChangeRequestDocument, bool>> AddAssigneeFilter(
            Expression<Func<ChangeRequestDocument, bool>> filterEx)
        {
            if (Assignees == null || !Assignees.Any())
            {
                return filterEx;
            }
            Expression<Func<ChangeRequestDocument, bool>> assigneeFilterEx = null;
            foreach (var assignee in Assignees)
            {
                assigneeFilterEx = assigneeFilterEx.OrElse(x => x.Assignee == assignee);
            }
            return filterEx.AndAlso(assigneeFilterEx);
        }

        private Expression<Func<ChangeRequestDocument, bool>> AddChangeRequestIdFilter(
            Expression<Func<ChangeRequestDocument, bool>> filterEx)
        {
            if (!ChangeRequestId.IsNullOrWhiteSpace())
            {
                filterEx = filterEx.OrElse(x =>x.ChangeRequestId == ChangeRequestId);
            }
            return filterEx;
        }

        private Expression<Func<ChangeRequestDocument, bool>> AddChangeRequestTypeIdFilter(
            Expression<Func<ChangeRequestDocument, bool>> filterEx)
        {
            if (ChangeTypes==null || !ChangeTypes.Any())
            {
                return filterEx;
            }
            Expression<Func<ChangeRequestDocument, bool>> changeRequestTypeFilterEx = null;
            foreach (var changeType in ChangeTypes)
            {
                changeRequestTypeFilterEx = changeRequestTypeFilterEx.OrElse(x => x.ChangeType == changeType);
            }
            return filterEx.AndAlso(changeRequestTypeFilterEx); 
        }

        private Expression<Func<ChangeRequestDocument, bool>> AddEntityFilter(
            Expression<Func<ChangeRequestDocument, bool>> filterEx)
        {
            if (ChangeEntities == null || !ChangeEntities.Any())
            {
                return filterEx;
            }
            Expression<Func<ChangeRequestDocument, bool>> changeRequestTypeFilterEx = null;
            foreach (var entity in ChangeEntities)
            {
                changeRequestTypeFilterEx = changeRequestTypeFilterEx.OrElse(x => x.Entity == entity);
            }
            return filterEx.AndAlso(changeRequestTypeFilterEx);
        }

        private Expression<Func<ChangeRequestDocument, bool>> AddStatusFilter(
            Expression<Func<ChangeRequestDocument, bool>> filterEx)
        {
            if (Statuses == null || !Statuses.Any())
            {
                return filterEx;
            }
            Expression<Func<ChangeRequestDocument, bool>> statusFilterEx = null;
            foreach (var status in Statuses)
            {
                statusFilterEx = statusFilterEx.OrElse(x => x.StatusText == status);
            }
            return filterEx.AndAlso(statusFilterEx);
        }

        private Expression<Func<ChangeRequestDocument, bool>> AddSubmittedDateRangeFilter(
            Expression<Func<ChangeRequestDocument, bool>> filterEx)
        {
            if (SubmittedDateFrom != null)
            {
                filterEx = filterEx.AndAlso(x => x.SubmittedDate >= SubmittedDateFrom);
            }
            
            if (SubmittedDateTo != null)
            {
                filterEx = filterEx.AndAlso(x => x.SubmittedDate <= SubmittedDateTo);
            }
            return filterEx;
        }

        private Expression<Func<ChangeRequestDocument, bool>> AddRequestedByFilter(
            Expression<Func<ChangeRequestDocument, bool>> filterEx)
        {
            if (RequestsBy == null || !RequestsBy.Any())
            {
                return filterEx;
            }
            Expression<Func<ChangeRequestDocument, bool>> requestedByFilterEx = null;
            foreach (var requestedBy in RequestsBy)
            {
                requestedByFilterEx = requestedByFilterEx.OrElse(x => x.RequestedBy == requestedBy);
            }
            return filterEx.AndAlso(requestedByFilterEx);
        }

    }
}
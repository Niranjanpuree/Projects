using System;
using Northwind.Core.Utilities;
using Northwind.Web.Models.ViewModels;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Models.ViewModels
{
    public class JobRequestViewModelForList : BaseViewModel
    {
        public Guid JobRequestGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public int Status { get; set; }
        public string TotalJobStatus
        {
            get
            {
                //return Status == (int)JobRequestStatus.Complete
                //    ? ProgressStatus.Done.ToString()
                //    : ProgressStatus.InProgress.ToString();
                return Status == (int)JobRequestStatus.Complete
                    ? ProgressStatus.Done.ToString()
                    : "In Progress";
            }
        }
        public bool IsIntercompanyWorkOrder { get; set; }
        public string Companies { get; set; }
        public string ContractReview { get; set; }
        public string ContractReviewStatus
        {
            get
            {
                //return (int)JobRequestStatus.ContractRepresentative < Status - 1
                //    ? ProgressStatus.Done.ToString()
                //    : ProgressStatus.InProgress.ToString();
                return (int)JobRequestStatus.ContractRepresentative < Status - 1
                    ? ProgressStatus.Done.ToString()
                    : "In Progress";
            }
        }
        public string ProjectControlReview { get; set; }
        public string ProjectControlReviewStatus
        {
            get
            {
                //return (int)JobRequestStatus.ProjectControl - 1 < Status - 1
                //    ? ProgressStatus.Done.ToString()
                //    : ProgressStatus.InProgress.ToString();
                return (int)JobRequestStatus.ProjectControl - 1 < Status - 1
                    ? ProgressStatus.Done.ToString()
                    : "In Progress";
            }
        }
        public string ProjectManagerReview { get; set; }
        public string ProjectManagerReviewStatus
        {
            get
            {
                //return (int)JobRequestStatus.ProjectManager - 1 < Status - 1
                //    ? ProgressStatus.Done.ToString()
                //    : ProgressStatus.InProgress.ToString();
                return (int)JobRequestStatus.ProjectManager - 1 < Status - 1
                    ? ProgressStatus.Done.ToString()
                    : "In Progress";
            }
        }
        public string AccountingReview { get; set; }
        public string AccountingReviewStatus
        {
            get
            {
                //return (int)JobRequestStatus.Accounting - 1 < Status - 1
                //    ? ProgressStatus.Done.ToString()
                //    : ProgressStatus.InProgress.ToString();
                return (int)JobRequestStatus.Accounting - 1 < Status - 1
                   ? ProgressStatus.Done.ToString()
                   : "In Progress";
            }
        }
        private String _initiatedBy;
        public string InitiatedBy
        {
            get
            {
                return FormatHelper.FormatJobRequestValue(_initiatedBy, CreatedOn.ToString("MM/dd/yyyy"));
            }
            set { this._initiatedBy = value; }
        }
        public string ContractOrTaskOrder { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractNumber { get; set; }
        public string ContractTitle { get; set; }
        public string TaskOrderNumber { get; set; }
        public string JobRequestTitle
        {
            get
            {
                return FormatHelper.FormatJobRequestTitle(ProjectNumber, ContractOrTaskOrder, ContractNumber,
                    ContractTitle,TaskOrderNumber);
            }
        }

    }
}

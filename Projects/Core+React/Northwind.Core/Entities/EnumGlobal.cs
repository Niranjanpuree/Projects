using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities
{
    public class EnumGlobal
    {
        public enum CrudType
        {
            Create = 0,
            Edit = 1,
            Delete = 2,
            Notify = 3,
            View = 4
        }

        public enum CrudTypeForAdditionalLogMessage
        {
            Added,
            Edited,
            Deleted,
            Enabled,
            Disabled,
            Notified,
            Uploaded,
            Downloaded
        }
        public enum ModuleType
        {
            Project = 1,
            Contract = 2
        }
        public enum UploadMethodName
        {
            EmployeeBillingRate = 1,
            WorkBreakDownStructure = 2,
            SubcontractorLaborBillingRates = 3
        }
        public enum ResourceType
        {
            Contract,
            Project,
            TaskOrder,
            Customer,
            Company,
            Office,
            Region,
            ContractModification,
            TaskOrderModification,
            ProjectModification,
            JobRequest,
            RevenueRecognition,
            EmployeeBillingRates,
            WorkBreakDownStructure,
            LaborCategoryRates,
            FarContractType,
            FarContract,
            ContractCloseOut,
            ContractNotice,
            ContractClauses,
            PreAward,
            RFP,
            Proposal,
            CertifiedCost,
            Discussions,
            Others,
            DocumentManager,
            Admin,
            Notification,
            Base
        }

        public enum ContractResourceFileKey
        {
            CloseOut,
            RFI,
            ChangeProposals,
            ContractModification
        }

        

        public enum ResourceActionPermission
        {
            Add,
            Edit,
            Delete,
            Details,
            List,
            ManageCompany,
            ManageRegion,
            ManageOffice,
            Manage,
            ManageFar,
            View,
            Export,
        }

        public enum ResourceAttributeName
        {
            Competition,
            PSCCode,
            NAICsCode,
            QualityLevel,
            RevenueFormula,
            BillingFrequency,
            ContractType,
            BillingFormula,
            PaymentTerms,
            QualityLevelRequirements,
            ApplicableWageDetemination,
            InvoiceSubmissionMethod,
            Currency,
            SetAside
        }

        public enum ResourceAction
        {
            Created,
            Updated,
            ContractReview,
            ContractCompleted,
            ContractCreate,
            ContractUpdate,
            ContractModCreate,
            ContractModUpdate,
            TaskOrderReview,
            TaskOrderUpdate,
            TaskOrderCreate,
            TaskOrderModCreate,
            TaskOrderModUpdate,
            TaskOrderCompleted,
            Deleted,
            Closeout
        }
        public enum JobRequestStatus
        {
            ContractRepresentative = 1,
            ProjectControl = 2,
            ProjectManager = 3,
            Accounting = 4,
            Complete = 5
        }
        public enum ProgressStatus
        {
            InProgress,
            Completed,
            Done,
        }

        public enum ActiveStatus
        {
            Active,
            Inactive
        }
        public enum NotificationType
        {
            EmailNotification,
            UserNotification
        }

        public enum CustomerType
        {
            Federal,
            Commercial
        }

        public enum UserAction
        {
            View,
            Favourite
        }

        public enum DaviesActType
        {
            DavisBaconAct,
            ServiceContractAct
        }

        public enum ActivityType
        {
            MyFavorite,
            MyContract,
            RecentlyViewed,
            All
        }

        public enum ArticleType
        {
            WhatYouNeedToKnow,
            ValuesCorner,
            MoreToKnow
        }

        public enum JobRequestFilterBy
        {
            All,
            MyPending,
            Pending
        }
    }

    public enum ValidationStatus
    {
        Success,
        Partial,
        Fail
    }

    public enum ValidationType
    {
        Strict,
        Relaxed,
        Ignore
    }

    public enum ImportStatus
    {
        Success,
        Fail,
        Deleted,
        Updated,
        PartialSuccess
    }

    public enum ImportAction
    {
        Add,
        Update,
        Enable,
        Disable,
        Delete
    }

    public enum ApprovalStatus
    {
        APPROVED,
        UNAPPROVED
    }

    public enum ContractStatus
    {
        Active,
        Inactive,
        Closed
    }

    public enum YesNoStatus
    {
        Yes,
        No
    }
}

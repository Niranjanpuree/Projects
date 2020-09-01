using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class EnumGlobal
    {
        public enum CrudType
        {
            Create = 0,
            Edit = 1,
            Delete = 2
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
            TaskOrder,
            Customer,
            Company,
            Office,
            Region,
            TaskOrderMod,
            ContractMod,
            JobRequest,
            RevenueRecognition,
            EmployeeBillingRates,
            WorkBreakDownStructure,
            SubLabourCategory,
            UserNotification
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
            Deleted
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
            On_Progress,
            Completed,
            Done,
        }

        public enum ActiveStatus
        {
            Active,
            Inactive
        }

        public enum YesNoStatus
        {
            Yes,
            No
        }
    }
}

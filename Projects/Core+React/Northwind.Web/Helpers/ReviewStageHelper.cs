using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Northwind.Web.Models.ViewModels.EnumGlobal;

namespace Northwind.Web.Helpers
{
    public class ReviewStageHelper
    {
        public static string ReviewStageByStatus(int status)
        {
            string stage = "";
            switch (status)
            {
                case (int)JobRequestStatus.ContractRepresentative:
                    stage = "Contract Representative Review";
                    break;
                case (int)JobRequestStatus.ProjectControl:
                    stage = "Project Controls Review";
                    break;
                case (int)JobRequestStatus.ProjectManager:
                    stage = "Project Manager Review";
                    break;
                case (int)JobRequestStatus.Accounting:
                    stage = "Accounting Review";
                    break;
                case (int)JobRequestStatus.Complete:
                    stage = "Completed";
                    break;
            }
            return stage;
        }
    }
}

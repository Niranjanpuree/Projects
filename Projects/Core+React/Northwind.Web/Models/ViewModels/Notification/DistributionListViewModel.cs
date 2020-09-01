using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Models.ViewModels;

namespace Northwind.Web.Models.ViewModels
{
    public class DistributionListViewModel : BaseViewModel
    {
        public Guid DistributionListGuid { get; set; }
        private string _name;
        public string Name
        {

            get { return string.Join("", Title.Split(' ')); }
            set { this._name = value; }
        }
        public string Title { get; set; }
        public bool IsPublic { get; set; }
        public string IsPublicStatus
        {
            get { return IsPublic == true ? EnumGlobal.YesNoStatus.Yes.ToString() : EnumGlobal.YesNoStatus.No.ToString(); }
        }
        public string CreatedByName { get; set; }
        public int MemberCount { get; set; }
        public string UpdatedOnFormatted { get; set; }
        public bool IsOwner { get; set; }

        public IEnumerable<UserViewModel> SelectedUsers { get; set; }
        public UserSelection UserSelection { get; set; }
    }

    public class UserSelection
    {
        public bool SelectedAll { get; set; }
        public IEnumerable<UserViewModel> ExcludeList { get; set; }
        public IEnumerable<UserViewModel> IncludeList { get; set; }
    }
    public class DistributionSelection
    {
        public bool SelectedAll { get; set; }
        public IEnumerable<DistributionListViewModel> ExcludeList { get; set; }
        public IEnumerable<DistributionListViewModel> IncludeList { get; set; }
    }
}

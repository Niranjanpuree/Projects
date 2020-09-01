using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;

namespace Northwind.Web.Models.ViewModels.Customer
{
    public class CustomerListViewModel : BaseViewModel
    {
        public Guid CustomerGuid { get; set; }
        public string CustomerName { get; set; }
        public string DisplayName { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CustomerTypeName { get; set; }
        public SearchVM SearchVm { get; set; }
        public string PrimaryEmail { get; set; }
        public string Agency { get; set; }
        public string Department { get; set; }
        public string CustomerCode { get; set; }

        private String _physicalAddress;
        public String Address
        {
            get
            {
                return FormatHelper.FormatAddress(_physicalAddress, AddressLine1, City, State,
                    ZipCode, Country);
            }
            set { this._physicalAddress = value; }
        }
        public string StatesName { get; set; }
        public string CountryName { get; set; }
        public string Abbreviations { get; set; }

    }
}

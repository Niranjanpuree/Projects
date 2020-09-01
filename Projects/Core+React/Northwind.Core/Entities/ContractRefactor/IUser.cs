using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.ContractRefactor
{
    public interface IUser
    {
        Guid UserGuid { get; set; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string GivenName { get; set; }
        string DisplayName { get; set; }
        string UserStatus { get; set; }
        string WorkEmail { get; set; }
        string PersonalEmail { get; set; }
        string WorkPhone { get; set; }
        string HomePhone { get; set; }
        string MobilePhone { get; set; }
        string JobStatus { get; set; }
        string JobTitle { get; set; }
        string Company { get; set; }
        string Department { get; set; }
        string Extension { get; set; }
    }
}

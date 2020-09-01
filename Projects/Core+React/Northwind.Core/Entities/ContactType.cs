using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{

    //This names must match names in database at all times.
    public enum ContactType
    {
        ContractRepresentative ,
        TechnicalContractRepresentative,
        ProjectRepresentative,
        TechnicalProjectRepresentative,
        Others
    }
}

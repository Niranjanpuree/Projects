using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class UserSyncResult
    {
        public string Username { get; set; }
        public UserSyncValidationResult UserSyncValidationResult { get; set; }
        public string SyncStatus { get; set; }

       
    }
}

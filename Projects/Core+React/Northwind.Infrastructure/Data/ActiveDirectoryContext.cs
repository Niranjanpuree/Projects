using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data
{
    public class ActiveDirectoryContext: IActiveDirectoryContext
    {
        public string LdapPath { get; set; }
        public string LdapDomainController { get; set; }
        public string LdapUsername { get; set; }
        public string LdapPassword { get; set; }
        public IActiveDirectoryUserRepository UserRepository { get; set; }
        public IActiveDirectoryGroupRepository GroupRepository { get; set; }

        public ActiveDirectoryContext(string ldapRoot, string ldapDomain, string ldapUsername, string ldapPassword)
        {
            LdapPath = ldapRoot;
            LdapDomainController = ldapDomain;
            LdapUsername = ldapUsername;
            LdapPassword = ldapPassword;
            UserRepository = new ActiveDirectoryUserRepository(this);
            GroupRepository = new ActiveDirectoryGroupRepository(this);
        }        

    }
}

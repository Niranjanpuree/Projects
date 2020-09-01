using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using AutoCare.Product.Web.Personify.ImsService;
using AutoCare.Product.Web.Personify.SsoService;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class PersonifyHelper : IPersonifyHelper
    {
        private readonly PersonifyConfiguration _personifyConfiguration;
        private readonly serviceSoap _personifySsoServiceProxy = null;
        private readonly IMServiceSoap _personifyImsServiceProxy;

        public PersonifyHelper(PersonifyConfiguration personifyConfiguration = null, 
            serviceSoap personifySsoServiceProxy = null,
            IMServiceSoap personifyImsServiceProxy = null)
        {
            _personifyConfiguration = personifyConfiguration ?? new PersonifyConfiguration();
            _personifySsoServiceProxy = personifySsoServiceProxy;
            _personifyImsServiceProxy = personifyImsServiceProxy;
        }


        private serviceSoap GetSsoServiceProxy()
        {
            var proxy = new serviceSoapClient("serviceSoap");
            ((ICommunicationObject)proxy).Open();
            return proxy;
        }

        private IMServiceSoap GetImsServiceProxy()
        {
            var proxy = new MServiceSoapClient("IMServiceSoap");
            ((ICommunicationObject)proxy).Open();
            return proxy;
        }

        private void CloseProxy(object proxy)
        {
            if (proxy == null)
            {
                return;
            }

            if (proxy is ICommunicationObject)
            {
                ((ICommunicationObject)proxy).Close();
            }

            if (proxy is IDisposable)
            {
                ((IDisposable)proxy).Dispose();
            }
            
        }

        public async Task<AutoCareUser> GetUserAsync(string username, string password)
        {
            serviceSoap proxy = null;
            AutoCareUser autoCareUser = null;

            try
            {
                proxy = _personifySsoServiceProxy ?? GetSsoServiceProxy();
                
                var customerAuth = proxy.CustomerAuthenticateAndGetId(_personifyConfiguration.VendorUserName, 
                    _personifyConfiguration.VendorPassword, username, password);

                autoCareUser = new AutoCareUser(customerAuth.CustomerId);

                var customerDetail = await proxy.SSOCustomerGetAsync(_personifyConfiguration.VendorUserName, _personifyConfiguration.VendorPassword,
                    autoCareUser.Id);

                autoCareUser.UserName = customerDetail.UserName;
                autoCareUser.Email = customerDetail.Email;

                autoCareUser.CustomerToken = await GetCustomerTokenAsync(username, password);
            }
            finally
            {
                CloseProxy(proxy);
            }

            return autoCareUser;
        }

        public async Task<string> GetCustomerTokenAsync(string username, string password)
        {
            serviceSoap proxy = null;
            string customerToken = null;

            try
            {
                proxy = _personifySsoServiceProxy ?? GetSsoServiceProxy();
                var url = HttpContext.Current.Request.Url;
                var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

                var callbackUrl = $"{url.Scheme}://{url.Host}{port}/";
                var redirectUrl =
                    SSOLoginManager.ConstructSSOURLForAutoLogin(_personifyConfiguration.SsoLoginUrl,
                        callbackUrl, username, password, true);
                //The url could be local host even for production environment because it doesn't matter which domain we use. 
                //All we do is to get CustomerToken from query string

                WebRequest request = WebRequest.Create(redirectUrl);
                request.Method = "GET";
                WebResponse response = await request.GetResponseAsync();
                if (!String.IsNullOrWhiteSpace(response.ResponseUri.Query))
                {
                    customerToken = HttpUtility.ParseQueryString(response.ResponseUri.Query)["ct"];
                }

                response.Close();
            }
            finally
            {
                CloseProxy(proxy);
            }

            return customerToken;
        }

        public async Task<List<string>> GetRolesAsync(string customerId)
        {
            IMServiceSoap proxy = null;
            List<string> roles = null;

            try
            {
                proxy = _personifyImsServiceProxy ?? GetImsServiceProxy();

                var result = await proxy.IMSCustomerRoleGetByTimssCustomerIdAsync(_personifyConfiguration.VendorUserName,
                    _personifyConfiguration.VendorPassword, customerId);


                roles = result.CustomerRoles.Select(x => x.Value).ToList();
            }
            finally
            {
                CloseProxy(proxy);
            }

            return roles;
        }

        public async Task<List<string>> GetCustomersByRoleAsync(string role)
        {
            IMServiceSoap proxy = null;
            List<string> customersId = null;

            try
            {
                proxy = _personifyImsServiceProxy ?? GetImsServiceProxy();

                var result = await proxy.IMSRoleCustomersGetAsync(_personifyConfiguration.VendorUserName,
                    _personifyConfiguration.VendorPassword, role);


                customersId = result.RoleCustomers.Select(x => x.TimssCustomerId).ToList();
            }
            finally
            {
                CloseProxy(proxy);
            }

            return customersId;
        }
    }
}
using System.Configuration;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class PersonifyConfiguration
    {
        public PersonifyConfiguration()
        {
            VendorUserName = ConfigurationManager.AppSettings.Get("VendorUsername");
            VendorPassword = ConfigurationManager.AppSettings.Get("VendorPassword");
            VendorBlock = ConfigurationManager.AppSettings.Get("VendorBlock");
            VendorIdentifier = ConfigurationManager.AppSettings.Get("VendorIdentifier");
            SsoLoginUrl = ConfigurationManager.AppSettings.Get("SsoLoginUrl");
        }
        public string VendorUserName { get; private set; }
        public string VendorPassword { get; private set; }
        public string VendorBlock { get; private set; }
        public string VendorIdentifier { get; private set; }
        public string SsoLoginUrl { get; private set; }
    }
}
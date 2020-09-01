using System;
using System.Configuration;

namespace AutoCare.Product.Application.Infrastructure
{
    public sealed class AppSettingConfiguration
    {
        private static volatile AppSettingConfiguration _instance;
        private static readonly object SyncRoot = new Object();

        public static AppSettingConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new AppSettingConfiguration();
                    }
                }

                return _instance;
            }
        }

        private AppSettingConfiguration()
        {
            AuthTokenUrl = ConfigurationManager.AppSettings.Get("AuthTokenUrl");
            DefaultChangeRequestRecordCount = Convert.ToInt32(
                        ConfigurationManager.AppSettings.Get("DefaultChangeRequestRecordCount"));
            StorageAccountConnectionStringName = ConfigurationManager.AppSettings.Get("StorageAccountConnectionString");

            DefaultTokenExpirationTimeInMinutes = Convert.ToInt32(
                ConfigurationManager.AppSettings.Get("DefaultTokenExpirationTimeInMinutes"));

            RememberMeTokenExpirationTimeInMinutes = Convert.ToInt32(
               ConfigurationManager.AppSettings.Get("RememberMeTokenExpirationTimeInMinutes"));
        }

        public string AuthTokenUrl { get; private set; }

        public int DefaultChangeRequestRecordCount { get; private set; }

        public string StorageAccountConnectionStringName { get; private set; }

        public int DefaultTokenExpirationTimeInMinutes { get; private set; }
        public int RememberMeTokenExpirationTimeInMinutes { get; private set; }
    }
}
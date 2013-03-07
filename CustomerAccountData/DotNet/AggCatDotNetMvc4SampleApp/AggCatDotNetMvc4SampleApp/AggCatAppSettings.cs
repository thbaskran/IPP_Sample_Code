using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace AggCatDotNetMvc4SampleApp.Mvc4
{
    public static class AggCatAppSettings
    {
        public static string ConsumerKey { get; set; }
        public static string ConsumerSecret { get; set; }
        public static string SamlIdentityProviderId { get; set; }
        public static X509Certificate2 Certificate { get; set; }
        public static string CustomerId { get; set; }

        public static void GetAppSettingsFromConfig()
        {
            ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            SamlIdentityProviderId = ConfigurationManager.AppSettings["SamlIdentityProviderId"];
            CustomerId = ConfigurationManager.AppSettings["CustomerID"];

            string certificateFile = System.Configuration.ConfigurationManager.AppSettings["PrivateKeyPath"];
            string password = System.Configuration.ConfigurationManager.AppSettings["PrivateKeyPassword"];
            Certificate = new X509Certificate2(certificateFile, password);
        }
    }
}
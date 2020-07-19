using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Security.Cryptography.Certificates;

namespace JapaneseDict.OnlineService
{
    public class AppConfig
    {
        private readonly IConfiguration _configuration;
        private static AppConfig _appConfig = null;
        public AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Package.Current.InstalledLocation.Path))
                .AddJsonFile("appsettings.development.json", optional: false)
                .AddJsonFile("appsettings.production.json", optional: true);
            _configuration = builder.Build();
        }
        public static AppConfig Configurations
        {
            get
            {
                if (_appConfig == null)
                    _appConfig = new AppConfig();
                return _appConfig;
            }
        }
        public string this[string key]
        {
            get { return _configuration.GetSection("data")[key]; }
        }
    }
}

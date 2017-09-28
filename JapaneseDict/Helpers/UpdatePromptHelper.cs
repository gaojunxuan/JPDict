using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace JapaneseDict.GUI.Helpers
{
    public static class UpdatePromptHelper
    {
        private const string LastVersionNumberKey = "LAST_VERSION_NUMBER";
        private static string lastVersionNumber = "1.9.11.0";
        public static bool Updated
        {
            get;set;
        }
        public static string LastVersionNumber
        {
            get { return lastVersionNumber; }
            internal set
            {
                lastVersionNumber = value;
            }
        }
        public static string GetAppVersion()
        {
            var ver = Windows.ApplicationModel.Package.Current.Id.Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
        }
        public static void LoadState()
        {
            try
            {
                LastVersionNumber = StorageHelper.GetSetting<string>(LastVersionNumberKey);

                if (lastVersionNumber != GetAppVersion())
                {
                    lastVersionNumber = GetAppVersion();
                    Updated = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Failed to load state, Exception: {0}", ex.ToString()));
            }
        }
        public static void StoreState()
        {
            try
            {
                StorageHelper.StoreSetting(LastVersionNumberKey, LastVersionNumber, true);
                StorageHelper.FlushToStorage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("FeedbackHelper.StoreState - Failed to store state, Exception: {0}", ex.ToString()));
            }

        }
    }
}

using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace JapaneseDict.OnlineService
{
    public class NoteSync
    {
        private StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private StorageFile _logFile;
        private StorageFile _onlineLog;
        private static readonly string[] scopes = new string[] { "onedrive.readwrite", "wl.offline_access", "wl.signin" };
        private static IOneDriveClient oneDriveClient = OneDriveClientExtensions.GetUniversalClient(scopes);
        private static AccountSession accountSession;
        static bool _isBusy = false;
        public async Task<NoteSync> Load()
        {
            _logFile = await _localFolder.GetFileAsync("synclog.log");
            _onlineLog = await _localFolder.GetFileAsync("onlinelog.log");
            return this;
        }
        public async void RequestAuth()
        {
            accountSession = await oneDriveClient.AuthenticateAsync();
        }
        public async void WriteToLog(DateTime time)
        {
            if (_logFile != null)
                await FileIO.WriteTextAsync(_logFile, time.ToString());
        }
        public async void UploadLog()
        {
            if (oneDriveClient.IsAuthenticated)
            {
                if (_logFile != null)
                {
                    using (Stream stream = await _logFile.OpenStreamForReadAsync())
                    {

                        await oneDriveClient
                                       .Drive
                                       .Root
                                       .ItemWithPath("JPDict/synclog.log")
                                       .Content
                                       .Request()
                                       .PutAsync<Item>(stream);

                    }
                }
            }
        }
        public async void DownloadOnlineLog()
        {
            if (oneDriveClient.IsAuthenticated)
            {
                Stream stream=await oneDriveClient
                                       .Drive
                                       .Root
                                       .ItemWithPath("JPDict/synclog.log")
                                       .Content
                                       .Request()
                                       .GetAsync();
                using (stream)
                {
                    using (var file = await _logFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await stream.CopyToAsync(file.GetOutputStreamAt(0).AsStreamForWrite());
                    }
                }
            }
        }
    }
}

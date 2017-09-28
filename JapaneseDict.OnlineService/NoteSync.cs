using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace JapaneseDict.OnlineService
{
    public static class NoteSync
    {
        public static StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private static GraphServiceClient _graphClient;
        public static async Task<bool> SignInCurrentUserAsync()
        {
            _graphClient = AuthenticationHelper.GetAuthenticatedClient();
            if (_graphClient != null)
            {
                var user = await _graphClient.Me.Request().GetAsync();
                string userId = user.Id;
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<DriveItem> UploadFileToOneDriveAsync()
        {
            DriveItem uploadedFile = null;
            try
            {
                StorageFile file = await _localFolder.GetFileAsync("note.db");
                if(file.IsAvailable)
                {
                    Stream stream = (await file.OpenReadAsync()).AsStreamForRead();
                    uploadedFile = await _graphClient.Me.Drive.Root.ItemWithPath("JPDict/cloudnote.db").Content.Request().PutAsync<DriveItem>(stream);
                }
            }
            catch (ServiceException)
            {
                return null;
            }
            return uploadedFile;
        }
        public static async Task<Stream> DownloadFileFromOneDriveAsync(string path)
        {
            try
            {
                return await _graphClient.Me.Drive.Root.ItemWithPath(path).Content.Request().GetAsync();
            }
            catch (ServiceException)
            {
                return null;
            }
        }
    }
}

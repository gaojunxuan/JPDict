using Microsoft.OneDrive.Sdk;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using JapaneseDict.Models;

namespace JapaneseDict.QueryEngine
{
    internal static class OnedriveHelper
    {
        private static readonly string[] scopes = new string[] { "onedrive.readwrite", "wl.offline_access", "wl.signin" };
        private static IOneDriveClient oneDriveClient = OneDriveClientExtensions.GetUniversalClient(scopes);
        private static AccountSession accountSession;
        static bool _isBusy = false;       
        public static async void RequestAuth()
        {
            
            accountSession = await oneDriveClient.AuthenticateAsync();
            
        }
        public static async void UploadNotebook()
        {
            try
            {
                _isBusy = true;
                StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db"));
                var tempfile = await file.CopyAsync(ApplicationData.Current.LocalFolder, "note.temp.db", NameCollisionOption.ReplaceExisting);
                using (Stream stream = await tempfile.OpenStreamForReadAsync())
                {

                    await oneDriveClient
                                   .Drive
                                   .Root
                                   .ItemWithPath("sjnote.db")
                                   .Content
                                   .Request()
                                   .PutAsync<Item>(stream);


                }
                _isBusy = false;
            }
            catch
            {
                _isBusy = false;
            }
           
        }
        public static async void UploadSpecificFile(string filepath,string targetpath)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(filepath);
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                await oneDriveClient.Drive
                                    .Root
                                    .ItemWithPath(targetpath)
                                    .Content
                                    .Request()
                                    .PutAsync<Item>(stream);
            }
        }
        public static async void DownloadNotebook()
        {
            try
            {
                _isBusy = true;
                var stream = await oneDriveClient
                     .Drive
                     .Root
                     .ItemWithPath("sjnote.db")
                     .Content
                     .Request()
                     .GetAsync();
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("note-sync.db", CreationCollisionOption.ReplaceExisting);
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                var fs = await file.OpenStreamForWriteAsync();
                fs.Write(buffer, 0, buffer.Length);
                fs.Dispose();
                stream.Dispose();
                _isBusy = false;
            }
            catch
            {
                _isBusy = false;
            }
            

        }
        public static async void DownloadSpecificFile(string filepath,string targetname)
        {
            var stream = await oneDriveClient
                     .Drive
                     .Root
                     .ItemWithPath(filepath) 
                     .Content 
                     .Request()
                     .GetAsync();
            var file=await ApplicationData.Current.LocalFolder.CreateFileAsync(targetname, CreationCollisionOption.ReplaceExisting);
            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);   
            var fs = await file.OpenStreamForWriteAsync();
            fs.Write(buffer, 0, buffer.Length);
            fs.Dispose();
            stream.Dispose();
        }
        public async static void SyncNotebook()
        {
            DownloadNotebook();
            while(_isBusy)
            {
                await Task.Delay(500);
            }
            QueryEngine.UserDefDictQueryEngine.MergeDb("note-sync.db");
            while (_isBusy)
            {
                await Task.Delay(500);
            }
            UploadNotebook();
            while (_isBusy)
            {
                await Task.Delay(500);
            }
        }
       
        public static bool IsAuthenticated
        {
            get
            {
                return oneDriveClient.IsAuthenticated;
            }
        }
        public static async void Logout()
        {
            await oneDriveClient.SignOutAsync();
            
        }
    }
}

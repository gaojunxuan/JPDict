using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using JapaneseDict.OnlineService;
using Windows.UI.Xaml.Controls;
using JapaneseDict.QueryEngine;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using System.IO;
using Windows.UI.Core;
using Windows.Storage;
using JapaneseDict.GUI;
using JapaneseDict.GUI.Helpers;
using Microsoft.Identity.Client;

namespace JapaneseDict.GUI.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    { 
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property

        PackageVersion pv = Package.Current.Id.Version;
        public string ApplicationVersion
        {
            get
            {
                return $"v{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}";
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged();
            }
        }


        private RelayCommand _updateDictCommand;
        /// <summary>
        /// Gets the UpdateDictCommand.
        /// </summary>
        public RelayCommand UpdateDictCommand
        {
            get
            {
                return _updateDictCommand
                    ?? (_updateDictCommand = new RelayCommand(
                    async() =>
                    {
                        IsBusy = true;
                        try
                        {
                            await Task.Run(async () =>
                            {
                                var updates = await OnlineService.OnlineUpdate.GetAllUpdates();
                                await OnlineUpdate.ApplyUpdate(updates);
                            });
                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async() => 
                            {
                                await new MessageDialog("已经成功更新了你的词库", "成功").ShowAsync();
                            });
                        }
                        catch
                        {
                            await new MessageDialog("更新失败，请检查网络连接", "失败").ShowAsync();
                        }
                        IsBusy = false;
                    }));
            }
        }

        private RelayCommand _uploadNotebookCommand;
        /// <summary>
        /// Gets the UpdateNotebookCommand.
        /// </summary>
        public RelayCommand UploadNotebookCommand
        {
            get
            {
                return _uploadNotebookCommand
                    ?? (_uploadNotebookCommand = new RelayCommand(
                    async() =>
                    {
                        try
                        {
                            IsBusy = true;
                            await Task.Run(async () =>
                            { 
                                if (await NoteSync.SignInCurrentUserAsync())
                                {
                                    await NoteSync.UploadFileToOneDriveAsync();
                                }
                            });
                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await new MessageDialog("已经成功备份了你的生词本", "成功").ShowAsync();
                                IsBusy = false;
                            });
                        }
                        catch (MsalServiceException msalEx)
                        {
                            if (msalEx.ErrorCode == "authentication_canceled")
                                await new MessageDialog("备份操作被取消", "出现错误").ShowAsync();
                            else
                                await new MessageDialog($"其他错误\n{msalEx.ErrorCode}", "出现错误").ShowAsync();
                        }
                        catch (MsalClientException clientEx)
                        {
                            if (clientEx.ErrorCode == "network_not_available")
                                await new MessageDialog("请检查网络连接", "出现错误").ShowAsync();
                            if (clientEx.ErrorCode == "access_denied")
                                await new MessageDialog("权限不足，可能是由于你拒绝了 Skylark JPDict 访问 OneDrive 文件的请求。", "出现错误").ShowAsync();
                            else
                                await new MessageDialog($"其他错误\n{clientEx.ErrorCode}", "出现错误").ShowAsync();

                        }
                        catch (Exception ex)
                        {
                            await new MessageDialog("备份过程中出现错误，请稍后重试。\n\n" + ex.Message, "出现错误").ShowAsync();
                            NoteSync.Logout();
                        }
                        IsBusy = false;
                    }));
            }
        }

        private RelayCommand _downloadNotebookCommand;
        /// <summary>
        /// Gets the DownloadNotebookCommand.
        /// </summary>
        public RelayCommand DownloadNotebookCommand
        {
            get
            {
                return _downloadNotebookCommand
                    ?? (_downloadNotebookCommand = new RelayCommand(
                    async() =>
                    {
                        IsBusy = true;
                        try
                        {
                            await Task.Run(async () =>
                            {       
                                if (await NoteSync.SignInCurrentUserAsync())
                                {
                                    var filestream = await NoteSync.DownloadFileFromOneDriveAsync("JPDict/cloudnote.db");
                                    var file = await NoteSync._localFolder.TryGetItemAsync("cloudnote.db");
                                    if (file == null)
                                    {
                                        file = await NoteSync._localFolder.CreateFileAsync("cloudnote.db", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                                    }
                                    var writestream = await (file as StorageFile).OpenStreamForWriteAsync();
                                    await filestream.CopyToAsync(writestream);
                                    filestream.Dispose();
                                    writestream.Dispose();
                                    QueryEngine.QueryEngine.NotebookQueryEngine.CopyFromBackup();
                                    NotebookPage.NeedRefresh = true;
                                }
                            });
                        }
                        catch(MsalServiceException msalEx)
                        {
                            if(msalEx.ErrorCode== "authentication_canceled")
                                await new MessageDialog("恢复操作被取消", "出现错误").ShowAsync();
                            else
                                await new MessageDialog($"其他错误\n{msalEx.ErrorCode}", "出现错误").ShowAsync();
                        }
                        catch (MsalClientException clientEx)
                        {
                            if(clientEx.ErrorCode=="network_not_available")
                                await new MessageDialog("请检查网络连接", "出现错误").ShowAsync();
                            if (clientEx.ErrorCode== "access_denied")
                                await new MessageDialog("权限不足，可能是由于你拒绝了 Skylark JPDict 访问 OneDrive 文件的请求。", "出现错误").ShowAsync();
                            else
                                await new MessageDialog($"其他错误\n{clientEx.ErrorCode}", "出现错误").ShowAsync();

                        }
                        catch (Exception ex)
                        {
                            await new MessageDialog("恢复过程中出现错误，请稍后重试。\n\n" + ex.Message, "出现错误").ShowAsync();
                            NoteSync.Logout();
                        }
                        IsBusy = false;
                    }));
            }
        }

        public bool UseTexTra
        {
            get
            {
                return StorageHelper.GetSetting<bool>("UseTexTra");
            }
            set
            {
                StorageHelper.StoreSetting("UseTexTra", value, true);
                StorageHelper.FlushToStorage();
                RaisePropertyChanged();
            }
        }

        public bool UseNHKEasyNews
        {
            get
            {
                return StorageHelper.GetSetting<bool>("UseNHKEasyNews");
            }
            set
            {
                StorageHelper.StoreSetting("UseNHKEasyNews", value, true);
                StorageHelper.FlushToStorage();
                RaisePropertyChanged();
            }
        }
        
        public int LemmatizerMode
        {
            get
            {
                return StorageHelper.GetSetting<int>("LemmatizerMode");
            }
            set
            {
                StorageHelper.StoreSetting("LemmatizerMode", value, true);
                StorageHelper.FlushToStorage();
                RaisePropertyChanged();
            }
        }
    }
}


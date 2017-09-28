using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
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

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class SettingsPage_Model : ViewModelBase<SettingsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property
        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<BindableBase, String> _TitleDefaultValueFactory = m => m.GetType().Name;
        #endregion


        PackageVersion pv = Package.Current.Id.Version;
        public string ApplicationVersion
        {
            get { return _ApplicationVersionLocator(this).Value; }
            set { _ApplicationVersionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string ApplicationVersion Setup        
        protected Property<string> _ApplicationVersion = new Property<string> { LocatorFunc = _ApplicationVersionLocator };
        static Func<BindableBase, ValueContainer<string>> _ApplicationVersionLocator = RegisterContainerLocator<string>(nameof(ApplicationVersion), model => model.Initialize(nameof(ApplicationVersion), ref model._ApplicationVersion, ref _ApplicationVersionLocator, _ApplicationVersionDefaultValueFactory));
        static Func<BindableBase, string> _ApplicationVersionDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                //return default(string);
                
                return $"v{vm.pv.Major}.{vm.pv.Minor}.{vm.pv.Build}.{vm.pv.Revision}";
            };
        #endregion

        public CommandModel<ReactiveCommand, String> CommandUpdateDict
        {
            get { return _CommandUpdateDictLocator(this).Value; }
            set { _CommandUpdateDictLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandUpdateDict Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandUpdateDict = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandUpdateDictLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandUpdateDictLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandUpdateDict), model => model.Initialize(nameof(CommandUpdateDict), ref model._CommandUpdateDict, ref _CommandUpdateDictLocator, _CommandUpdateDictDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandUpdateDictDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandUpdateDict);           // Command resource  
                var commandId = nameof(CommandUpdateDict);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            try
                            {
                                await Task.Run(async () => 
                                {
                                    var updates = await OnlineService.OnlineUpdate.GetAllUpdates();
                                    await OnlineUpdate.ApplyUpdate(updates);
                                });
                                
                                await new MessageDialog("已经成功升级了您的词库", "升级成功").ShowAsync();
                            }
                            catch
                            {
                                await new MessageDialog("升级失败，请检查您的网络连接", "升级失败").ShowAsync();

                            }
                            //Todo: Add UpdateDict logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion


        public CommandModel<ReactiveCommand, String> CommandUploadNotebook
        {
            get { return _CommandUploadNotebookLocator(this).Value; }
            set { _CommandUploadNotebookLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandUploadNotebook Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandUploadNotebook = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandUploadNotebookLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandUploadNotebookLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandUploadNotebook), model => model.Initialize(nameof(CommandUploadNotebook), ref model._CommandUploadNotebook, ref _CommandUploadNotebookLocator, _CommandUploadNotebookDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandUploadNotebookDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandUploadNotebook);           // Command state  
                var commandId = nameof(CommandUploadNotebook);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            try
                            {
                                await Task.Run(async () =>
                                {
                                    if (await NoteSync.SignInCurrentUserAsync())
                                    {
                                        await NoteSync.UploadFileToOneDriveAsync();
                                    }
                                });
                            }
                            catch(Exception ex)
                            {
                                await new MessageDialog("备份过程中出现错误，请稍后重试。\n\n" + ex.ToString(), "出现错误").ShowAsync();
                            }
                            //Todo: Add UploadNotebook logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(state);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion

        public CommandModel<ReactiveCommand, String> CommandDownloadNotebook
        {
            get { return _CommandDownloadNotebookLocator(this).Value; }
            set { _CommandDownloadNotebookLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandDownloadNotebook Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandDownloadNotebook = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandDownloadNotebookLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandDownloadNotebookLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandDownloadNotebook), model => model.Initialize(nameof(CommandDownloadNotebook), ref model._CommandDownloadNotebook, ref _CommandDownloadNotebookLocator, _CommandDownloadNotebookDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandDownloadNotebookDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandDownloadNotebook);           // Command state  
                var commandId = nameof(CommandDownloadNotebook);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
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
                                        QueryEngine.QueryEngine.UserDefDictQueryEngine.CopyFromBackup();
                                        NotebookPage._needRefresh = true;
                                    }
                                });
                                
                            }
                            catch(Exception ex)
                            {
                                await new MessageDialog("恢复过程中出现错误，请稍后重试。\n\n" + ex.Message, "出现错误").ShowAsync();
                            }
                            //Todo: Add DownloadNotebook logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(state);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion

        #region Life Time Event Handling

        ///// <summary>
        ///// This will be invoked by view when this viewmodel instance is set to view's ViewModel property. 
        ///// </summary>
        ///// <param name="view">Set target</param>
        ///// <param name="oldValue">Value before set.</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        //{
        //    return base.OnBindedToView(view, oldValue);
        //}

        ///// <summary>
        ///// This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten.
        ///// </summary>
        ///// <param name="view">Overwrite target view.</param>
        ///// <param name="newValue">The value replacing </param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
        //{
        //    return base.OnUnbindedFromView(view, newValue);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}

        ///// <summary>
        ///// <para>If dispose actions got exceptions, will handled here. </para>
        ///// </summary>
        ///// <param name="exceptions">
        ///// <para>The exception and dispose infomation</para>
        ///// </param>
        //protected override async void OnDisposeExceptions(IList<DisposeInfo> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await TaskExHelper.Yield();
        //}

        #endregion



    }

}


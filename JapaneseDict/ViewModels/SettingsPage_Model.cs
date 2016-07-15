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

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class SettingsPage_Model : ViewModelBase<SettingsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
        private readonly string[] scopes = new string[] { "onedrive.readwrite", "wl.offline_access", "wl.signin" };
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


        public CommandModel<ReactiveCommand, String> CommandSyncNotebook
        {
            get { return _CommandSyncNotebookLocator(this).Value; }
            set { _CommandSyncNotebookLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandSyncNotebook Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandSyncNotebook = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSyncNotebookLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSyncNotebookLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandSyncNotebook), model => model.Initialize(nameof(CommandSyncNotebook), ref model._CommandSyncNotebook, ref _CommandSyncNotebookLocator, _CommandSyncNotebookDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSyncNotebookDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandSyncNotebook);           // Command resource  
                var commandId = nameof(CommandSyncNotebook);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            QueryEngine.QueryEngine.UserDefDictQueryEngine.SyncDb();
                            await Task.Delay(500);
                            //Todo: Add SyncNotebook logic here, or
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


        public CommandModel<ReactiveCommand, String> CommandNavToFeedbackPage
        {
            get { return _CommandNavToFeedbackPageLocator(this).Value; }
            set { _CommandNavToFeedbackPageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNavToFeedbackPage Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandNavToFeedbackPage = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavToFeedbackPageLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavToFeedbackPageLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandNavToFeedbackPage), model => model.Initialize(nameof(CommandNavToFeedbackPage), ref model._CommandNavToFeedbackPage, ref _CommandNavToFeedbackPageLocator, _CommandNavToFeedbackPageDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavToFeedbackPageDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandNavToFeedbackPage);           // Command resource  
                var commandId = nameof(CommandNavToFeedbackPage);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            (Window.Current.Content as Frame).Navigate(typeof(FeedbackPage));
                            //Todo: Add NavToFeedbackPage logic here, or
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


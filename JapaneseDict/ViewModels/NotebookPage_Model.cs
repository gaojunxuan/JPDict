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
using JapaneseDict.Models;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using JapaneseDict.GUI.Models;
using Windows.Foundation.Collections;

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class NotebookPage_Model : ViewModelBase<NotebookPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

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
        private async void LoadData()
        {
            this.IsBusy = true;
            try
            {
                Func<ObservableCollection<GroupedNoteItem>> func = (() => { return new ObservableCollection<GroupedNoteItem>((from item in QueryEngine.QueryEngine.UserDefDictQueryEngine.Get() orderby item.GroupingKey group item by item.GroupingKey into newItems select new GroupedNoteItem { Key = newItems.Key, ItemContent = newItems.ToList() }).ToList()); });
                this.GroupedNoteList = await Task.Run(func);
                if (this.GroupedNoteList.Count == 0)
                {
                    this.IsNotebookEmpty = true;
                }
                this.IsBusy = false;
            }
            catch
            {
                this.IsBusy = false;
            }
           
        }
        public ObservableCollection<GroupedNoteItem> GroupedNoteList
        {
            get { return _GroupedNoteListLocator(this).Value; }
            set { _GroupedNoteListLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<GroupedNoteItem> GroupedNoteList Setup        
        protected Property<ObservableCollection<GroupedNoteItem>> _GroupedNoteList = new Property<ObservableCollection<GroupedNoteItem>> { LocatorFunc = _GroupedNoteListLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<GroupedNoteItem>>> _GroupedNoteListLocator = RegisterContainerLocator<ObservableCollection<GroupedNoteItem>>(nameof(GroupedNoteList), model => model.Initialize(nameof(GroupedNoteList), ref model._GroupedNoteList, ref _GroupedNoteListLocator, _GroupedNoteListDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<GroupedNoteItem>> _GroupedNoteListDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                return default(ObservableCollection<GroupedNoteItem>);
                //TODO: Add the logic that produce default value from vm current status.
                //return new ObservableCollection<GroupedNoteItem>((from item in QueryEngine.QueryEngine.UserDefDictQueryEngine.Get() orderby item.GroupingKey group item by item.GroupingKey into newItems select new GroupedNoteItem { Key = newItems.Key, ItemContent = newItems.ToList() }).ToList());
            };
        #endregion
        public CommandModel<ReactiveCommand, String> CommandRemoveFromNotebook
        {
            get { return _CommandRemoveFromNotebookLocator(this).Value; }
            set { _CommandRemoveFromNotebookLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandRemoveFromNotebook Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandRemoveFromNotebook = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandRemoveFromNotebookLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandRemoveFromNotebookLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandRemoveFromNotebook), model => model.Initialize(nameof(CommandRemoveFromNotebook), ref model._CommandRemoveFromNotebook, ref _CommandRemoveFromNotebookLocator, _CommandRemoveFromNotebookDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandRemoveFromNotebookDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandRemoveFromNotebook);           // Command resource  
                var commandId = nameof(CommandRemoveFromNotebook);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            QueryEngine.QueryEngine.UserDefDictQueryEngine.Remove(Convert.ToInt32(e.EventArgs.Parameter));
                            vm.LoadData();
                            //Todo: Add RemoveFromNotebook logic here, or
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

        public bool IsBusy
        {
            get { return _IsBusyLocator(this).Value; }
            set { _IsBusyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsBusy Setup        
        protected Property<bool> _IsBusy = new Property<bool> { LocatorFunc = _IsBusyLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsBusyLocator = RegisterContainerLocator<bool>(nameof(IsBusy), model => model.Initialize(nameof(IsBusy), ref model._IsBusy, ref _IsBusyLocator, _IsBusyDefaultValueFactory));
        static Func<BindableBase, bool> _IsBusyDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(bool);
            };
        #endregion

        public bool IsNotebookEmpty
        {
            get { return _IsNotebookEmptyLocator(this).Value; }
            set { _IsNotebookEmptyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsNotebookEmpty Setup        
        protected Property<bool> _IsNotebookEmpty = new Property<bool> { LocatorFunc = _IsNotebookEmptyLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsNotebookEmptyLocator = RegisterContainerLocator<bool>(nameof(IsNotebookEmpty), model => model.Initialize(nameof(IsNotebookEmpty), ref model._IsNotebookEmpty, ref _IsNotebookEmptyLocator, _IsNotebookEmptyDefaultValueFactory));
        static Func<BindableBase, bool> _IsNotebookEmptyDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(bool);
            };
        #endregion

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
        protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        {
            this.IsBusy = false;
            this.GroupedNoteList = new ObservableCollection<GroupedNoteItem>();
            this.LoadData();
            return base.OnBindedViewLoad(view);
        }

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


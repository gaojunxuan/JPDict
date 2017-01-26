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
using JapaneseDict.GUI.Extensions;

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class KanjiFlashcardPage_Model : ViewModelBase<KanjiFlashcardPage_Model>
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



        public ObservableCollection<Kanjidict> Kanji
        {
            get { return _KanjiLocator(this).Value; }
            set { _KanjiLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<Kanjidict> Kanji Setup        
        protected Property<ObservableCollection<Kanjidict>> _Kanji = new Property<ObservableCollection<Kanjidict>> { LocatorFunc = _KanjiLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<Kanjidict>>> _KanjiLocator = RegisterContainerLocator<ObservableCollection<Kanjidict>>(nameof(Kanji), model => model.Initialize(nameof(Kanji), ref model._Kanji, ref _KanjiLocator, _KanjiDefaultValueFactory));
        static Func<ObservableCollection<Kanjidict>> _KanjiDefaultValueFactory = () => default(ObservableCollection<Kanjidict>);
        #endregion


        public CommandModel<ReactiveCommand, String> CommandReplay
        {
            get { return _CommandReplayLocator(this).Value; }
            set { _CommandReplayLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandReplay Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandReplay = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandReplayLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandReplayLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandReplay), model => model.Initialize(nameof(CommandReplay), ref model._CommandReplay, ref _CommandReplayLocator, _CommandReplayDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandReplayDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandReplay);           // Command resource  
                var commandId = nameof(CommandReplay);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            if(vm.Kanji.Count!=0)
                            {
                                var res = vm.Kanji.ToList();
                                res.Shuffle();
                                vm.Kanji = new ObservableCollection<Kanjidict>(res);
                            }
                            //Todo: Add Replay logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandShowReading
        {
            get { return _CommandShowReadingLocator(this).Value; }
            set { _CommandShowReadingLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowReading Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowReading = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowReadingLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowReadingLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowReading), model => model.Initialize(nameof(CommandShowReading), ref model._CommandShowReading, ref _CommandShowReadingLocator, _CommandShowReadingDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowReadingDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandShowReading);           // Command resource  
                var commandId = nameof(CommandShowReading);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var res = new ObservableCollection<Kanjidict>(vm.Kanji);
                            foreach (var i in res)
                            {
                                i.ShowReading = Windows.UI.Xaml.Visibility.Visible;
                            }
                            vm.Kanji = res;
                            //Todo: Add ShowReading logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandHideReading
        {
            get { return _CommandHideReadingLocator(this).Value; }
            set { _CommandHideReadingLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandHideReading Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandHideReading = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandHideReadingLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandHideReadingLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandHideReading), model => model.Initialize(nameof(CommandHideReading), ref model._CommandHideReading, ref _CommandHideReadingLocator, _CommandHideReadingDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandHideReadingDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandHideReading);           // Command resource  
                var commandId = nameof(CommandHideReading);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var res = new ObservableCollection<Kanjidict>(vm.Kanji);
                            foreach (var i in res)
                            {
                                i.ShowReading = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                            vm.Kanji = res;
                            //Todo: Add HideReading logic here, or
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


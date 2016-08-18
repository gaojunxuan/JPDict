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

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class HiraganaRecitePage_Model : ViewModelBase<HiraganaRecitePage_Model>
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


        public List<string> Hiraganas
        {
            get { return _HiraganasLocator(this).Value; }
            set { _HiraganasLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property List<string> Hiraganas Setup        
        protected Property<List<string>> _Hiraganas = new Property<List<string>> { LocatorFunc = _HiraganasLocator };
        static Func<BindableBase, ValueContainer<List<string>>> _HiraganasLocator = RegisterContainerLocator<List<string>>(nameof(Hiraganas), model => model.Initialize(nameof(Hiraganas), ref model._Hiraganas, ref _HiraganasLocator, _HiraganasDefaultValueFactory));
        static Func<BindableBase, List<string>> _HiraganasDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(List<string>);
            };
        #endregion


        public int Current
        {
            get { return _CurrentLocator(this).Value; }
            set { _CurrentLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int Current Setup        
        protected Property<int> _Current = new Property<int> { LocatorFunc = _CurrentLocator };
        static Func<BindableBase, ValueContainer<int>> _CurrentLocator = RegisterContainerLocator<int>(nameof(Current), model => model.Initialize(nameof(Current), ref model._Current, ref _CurrentLocator, _CurrentDefaultValueFactory));
        static Func<BindableBase, int> _CurrentDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(int);
            };
        #endregion


        public List<string> Romajis
        {
            get { return _RomajisLocator(this).Value; }
            set { _RomajisLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property List<string> Romajis Setup        
        protected Property<List<string>> _Romajis = new Property<List<string>> { LocatorFunc = _RomajisLocator };
        static Func<BindableBase, ValueContainer<List<string>>> _RomajisLocator = RegisterContainerLocator<List<string>>(nameof(Romajis), model => model.Initialize(nameof(Romajis), ref model._Romajis, ref _RomajisLocator, _RomajisDefaultValueFactory));
        static Func<BindableBase, List<string>> _RomajisDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(List<string>);
            };
        #endregion


        public string CurrentHiragana
        {
            get { return _CurrentHiraganaLocator(this).Value; }
            set { _CurrentHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string CurrentHiragana Setup        
        protected Property<string> _CurrentHiragana = new Property<string> { LocatorFunc = _CurrentHiraganaLocator };
        static Func<BindableBase, ValueContainer<string>> _CurrentHiraganaLocator = RegisterContainerLocator<string>(nameof(CurrentHiragana), model => model.Initialize(nameof(CurrentHiragana), ref model._CurrentHiragana, ref _CurrentHiraganaLocator, _CurrentHiraganaDefaultValueFactory));
        static Func<BindableBase, string> _CurrentHiraganaDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(string);
            };
        #endregion


        public CommandModel<ReactiveCommand, String> CommandNext
        {
            get { return _CommandNextLocator(this).Value; }
            set { _CommandNextLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNext Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandNext = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNextLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNextLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandNext), model => model.Initialize(nameof(CommandNext), ref model._CommandNext, ref _CommandNextLocator, _CommandNextDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNextDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandNext);           // Command resource  
                var commandId = nameof(CommandNext);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            vm.Current += 1;
                            vm.CurrentHiragana = vm.Hiraganas[vm.Current];
                            //Todo: Add Next logic here, or
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


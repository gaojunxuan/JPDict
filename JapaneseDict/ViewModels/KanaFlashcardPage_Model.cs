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
using static JapaneseDict.Util.KanaFlashcardHelper;

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class KanaFlashcardPage_Model : ViewModelBase<KanaFlashcardPage_Model>
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


        public ObservableCollection<Kana> Hiragana
        {
            get { return _HiraganaLocator(this).Value; }
            set { _HiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<Kana> Hiragana Setup        
        protected Property<ObservableCollection<Kana>> _Hiragana = new Property<ObservableCollection<Kana>> { LocatorFunc = _HiraganaLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<Kana>>> _HiraganaLocator = RegisterContainerLocator<ObservableCollection<Kana>>(nameof(Hiragana), model => model.Initialize(nameof(Hiragana), ref model._Hiragana, ref _HiraganaLocator, _HiraganaDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<Kana>> _HiraganaDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<Kana>);
            };
        #endregion


        public ObservableCollection<Kana> Katakana
        {
            get { return _KatakanaLocator(this).Value; }
            set { _KatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<Kana> Katakana Setup        
        protected Property<ObservableCollection<Kana>> _Katakana = new Property<ObservableCollection<Kana>> { LocatorFunc = _KatakanaLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<Kana>>> _KatakanaLocator = RegisterContainerLocator<ObservableCollection<Kana>>(nameof(Katakana), model => model.Initialize(nameof(Katakana), ref model._Katakana, ref _KatakanaLocator, _KatakanaDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<Kana>> _KatakanaDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<Kana>);
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


        public CommandModel<ReactiveCommand, String> CommandShowVoicedHiragana
        {
            get { return _CommandShowVoicedHiraganaLocator(this).Value; }
            set { _CommandShowVoicedHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowVoicedHiragana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowVoicedHiragana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowVoicedHiraganaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowVoicedHiraganaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowVoicedHiragana), model => model.Initialize(nameof(CommandShowVoicedHiragana), ref model._CommandShowVoicedHiragana, ref _CommandShowVoicedHiraganaLocator, _CommandShowVoicedHiraganaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowVoicedHiraganaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandShowVoicedHiragana);           // Command state  
                var commandId = nameof(CommandShowVoicedHiragana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            List<Kana> res = GetRandomHiraganaWithVoicedConsonants().ToList();

                            if (vm.Hiragana.Where(k => k.IsHistory == true).Count() == 0)
                            {
                                res.Remove(res.Where(k => k.Content == "ゐ").First());
                                res.Remove(res.Where(k => k.Content == "ゑ").First());
                            }
                            vm.Hiragana = new ObservableCollection<Kana>(res);
                            //Todo: Add ShowVoicedHiragana logic here, or
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
        public CommandModel<ReactiveCommand, String> CommandHideVoicedHiragana
        {
            get { return _CommandHideVoicedHiraganaLocator(this).Value; }
            set { _CommandHideVoicedHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandHideVoicedHiragana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandHideVoicedHiragana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandHideVoicedHiraganaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandHideVoicedHiraganaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandHideVoicedHiragana), model => model.Initialize(nameof(CommandHideVoicedHiragana), ref model._CommandHideVoicedHiragana, ref _CommandHideVoicedHiraganaLocator, _CommandHideVoicedHiraganaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandHideVoicedHiraganaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandHideVoicedHiragana);           // Command state  
                var commandId = nameof(CommandHideVoicedHiragana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            List<Kana> res = GetRandomHiragana().ToList();
                            
                            if (vm.Hiragana.Where(k => k.IsHistory==true).Count() == 0)
                            {
                                res.Remove(res.Where(k => k.Content == "ゐ").First());
                                res.Remove(res.Where(k => k.Content == "ゑ").First());
                            }
                            vm.Hiragana = new ObservableCollection<Kana>(res);
                            //Todo: Add HideVoicedHiragana logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandShowHistoryHiragana
        {
            get { return _CommandShowHistoryHiraganaLocator(this).Value; }
            set { _CommandShowHistoryHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowHistoryHiragana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowHistoryHiragana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowHistoryHiraganaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowHistoryHiraganaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowHistoryHiragana), model => model.Initialize(nameof(CommandShowHistoryHiragana), ref model._CommandShowHistoryHiragana, ref _CommandShowHistoryHiraganaLocator, _CommandShowHistoryHiraganaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowHistoryHiraganaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandShowHistoryHiragana);           // Command state  
                var commandId = nameof(CommandShowHistoryHiragana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            Random rand = new Random(DateTime.Now.Millisecond);
                            var wyi = new Kana() { Content = "ゐ", Romaji = "wyi", IsHistory = true };
                            var wye = new Kana() { Content = "ゑ", Romaji = "wye", IsHistory = true };
                            if (!(vm.Hiragana.Contains(wyi)&&vm.Hiragana.Contains(wye)))
                            {
                                vm.Hiragana.Insert(rand.Next(0, vm.Hiragana.Count - 1), wyi);
                                vm.Hiragana.Insert(rand.Next(0, vm.Hiragana.Count - 1), wye);
                            }
                                                      
                            //Todo: Add ShowHistoryHiragana logic here, or
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


        public CommandModel<ReactiveCommand, String> CommandHideHistoryHiragana
        {
            get { return _CommandHideHistoryHiraganaLocator(this).Value; }
            set { _CommandHideHistoryHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandHideHistoryHiragana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandHideHistoryHiragana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandHideHistoryHiraganaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandHideHistoryHiraganaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandHideHistoryHiragana), model => model.Initialize(nameof(CommandHideHistoryHiragana), ref model._CommandHideHistoryHiragana, ref _CommandHideHistoryHiraganaLocator, _CommandHideHistoryHiraganaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandHideHistoryHiraganaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandHideHistoryHiragana);           // Command state  
                var commandId = nameof(CommandHideHistoryHiragana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            try
                            {
                                vm.Hiragana.Remove(vm.Hiragana.Where(k => k.Content == "ゐ").First());
                                vm.Hiragana.Remove(vm.Hiragana.Where(k => k.Content == "ゑ").First());
                            }
                            catch
                            {
                                var wyi = new Kana() { Content = "ゐ", Romaji = "wyi", IsHistory = true };
                                var wye = new Kana() { Content = "ゑ", Romaji = "wye", IsHistory = true };
                                vm.Hiragana.Add(wyi);
                                vm.Hiragana.Add(wye);
                            }
                            //Todo: Add HideHistoryHiragana logic here, or
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

    }

}


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

                            if (vm.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
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
                            if (vm.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
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
                            if (vm.Hiragana.Where(k => k.IsHistory == true).Count() == 0)
                            {
                                vm.Hiragana.Insert(rand.Next(0, vm.Hiragana.Count - 1), wyi);
                                vm.Hiragana.Insert(rand.Next(0, vm.Hiragana.Count - 1), wye);
                            }
                            var res = new ObservableCollection<Kana>(vm.Hiragana);
                            if (vm.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Hiragana = res;
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
                            if (vm.Hiragana.Where(k => k.IsHistory == true).Count() != 0)
                            {
                                vm.Hiragana.Remove(vm.Hiragana.Where(k => k.Content == "ゐ").First());
                                vm.Hiragana.Remove(vm.Hiragana.Where(k => k.Content == "ゑ").First());
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

        public CommandModel<ReactiveCommand, String> CommandReplayHiragana
        {
            get { return _CommandReplayHiraganaLocator(this).Value; }
            set { _CommandReplayHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandReplayHiragana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandReplayHiragana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandReplayHiraganaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandReplayHiraganaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandReplayHiragana), model => model.Initialize(nameof(CommandReplayHiragana), ref model._CommandReplayHiragana, ref _CommandReplayHiraganaLocator, _CommandReplayHiraganaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandReplayHiraganaDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandReplayHiragana);           // Command resource  
                var commandId = nameof(CommandReplayHiragana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res;
                            if (vm.Hiragana.Where(k => k.Content == "ば").Count() != 0)
                            {
                                res = new ObservableCollection<Kana>(GetRandomHiraganaWithVoicedConsonants());
                            }
                            else
                            {
                                res = new ObservableCollection<Kana>(GetRandomHiragana());
                            }

                            if (vm.Hiragana.Where(k => k.IsHistory == true).Count() == 0)
                            {
                                res.Remove(res.Where(k => k.Content == "ゐ").First());
                                res.Remove(res.Where(k => k.Content == "ゑ").First());
                            }
                            if (vm.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Hiragana = res;
                            //Todo: Add ReplayHiragana logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandHideHiraganaRomaji
        {
            get { return _CommandHideHiraganaRomajiLocator(this).Value; }
            set { _CommandHideHiraganaRomajiLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandHideHiraganaRomaji Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandHideHiraganaRomaji = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandHideHiraganaRomajiLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandHideHiraganaRomajiLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandHideHiraganaRomaji), model => model.Initialize(nameof(CommandHideHiraganaRomaji), ref model._CommandHideHiraganaRomaji, ref _CommandHideHiraganaRomajiLocator, _CommandHideHiraganaRomajiDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandHideHiraganaRomajiDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandHideHiraganaRomaji);           // Command resource  
                var commandId = nameof(CommandHideHiraganaRomaji);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res = new ObservableCollection<Kana>(vm.Hiragana);
                            foreach(var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                            vm.Hiragana = res;
                            //Todo: Add HideHiraganaRomaji logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandShowHiraganaRomaji
        {
            get { return _CommandShowHiraganaRomajiLocator(this).Value; }
            set { _CommandShowHiraganaRomajiLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowHiraganaRomaji Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowHiraganaRomaji = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowHiraganaRomajiLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowHiraganaRomajiLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowHiraganaRomaji), model => model.Initialize(nameof(CommandShowHiraganaRomaji), ref model._CommandShowHiraganaRomaji, ref _CommandShowHiraganaRomajiLocator, _CommandShowHiraganaRomajiDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowHiraganaRomajiDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandShowHiraganaRomaji);           // Command resource  
                var commandId = nameof(CommandShowHiraganaRomaji);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res = new ObservableCollection<Kana>(vm.Hiragana);
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                            vm.Hiragana = res;
                            //Todo: Add ShowHiraganaRomaji logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandGetOrderHiragana
        {
            get { return _CommandGetOrderHiraganaLocator(this).Value; }
            set { _CommandGetOrderHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandGetOrderHiragana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandGetOrderHiragana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandGetOrderHiraganaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandGetOrderHiraganaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandGetOrderHiragana), model => model.Initialize(nameof(CommandGetOrderHiragana), ref model._CommandGetOrderHiragana, ref _CommandGetOrderHiraganaLocator, _CommandGetOrderHiraganaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandGetOrderHiraganaDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandGetOrderHiragana);           // Command resource  
                var commandId = nameof(CommandGetOrderHiragana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res;
                            res = new ObservableCollection<Kana>(GetOrderHiraganaWithVoicedConsonants());
                            res.Remove(res.Where(k => k.Content == "ゐ").First());
                            res.Remove(res.Where(k => k.Content == "ゑ").First());
                            
                            if (vm.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Hiragana = res;
                            //Todo: Add GetOrderHiragana logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandGetDisorderHiragana
        {
            get { return _CommandGetDisorderHiraganaLocator(this).Value; }
            set { _CommandGetDisorderHiraganaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandGetDisorderHiragana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandGetDisorderHiragana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandGetDisorderHiraganaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandGetDisorderHiraganaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandGetDisorderHiragana), model => model.Initialize(nameof(CommandGetDisorderHiragana), ref model._CommandGetDisorderHiragana, ref _CommandGetDisorderHiraganaLocator, _CommandGetDisorderHiraganaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandGetDisorderHiraganaDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandGetDisorderHiragana);           // Command resource  
                var commandId = nameof(CommandGetDisorderHiragana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var res=new ObservableCollection<Kana>(GetRandomHiragana());
                            if (vm.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Hiragana = res;
                            //Todo: Add GetDisorderHiragana logic here, or
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


        public CommandModel<ReactiveCommand, String> CommandShowVoicedKatakana
        {
            get { return _CommandShowVoicedKatakanaLocator(this).Value; }
            set { _CommandShowVoicedKatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowVoicedKatakana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowVoicedKatakana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowVoicedKatakanaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowVoicedKatakanaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowVoicedKatakana), model => model.Initialize(nameof(CommandShowVoicedKatakana), ref model._CommandShowVoicedKatakana, ref _CommandShowVoicedKatakanaLocator, _CommandShowVoicedKatakanaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowVoicedKatakanaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandShowVoicedKatakana);           // Command state  
                var commandId = nameof(CommandShowVoicedKatakana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            List<Kana> res = GetRandomKatakanaWithVoicedConsonants().ToList();

                            if (vm.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                            {
                                res.Remove(res.Where(k => k.Content == "ヰ").First());
                                res.Remove(res.Where(k => k.Content == "ヱ").First());
                            }

                            if (vm.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Katakana = new ObservableCollection<Kana>(res);
                            //Todo: Add ShowVoicedKatakana logic here, or
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
        public CommandModel<ReactiveCommand, String> CommandHideVoicedKatakana
        {
            get { return _CommandHideVoicedKatakanaLocator(this).Value; }
            set { _CommandHideVoicedKatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandHideVoicedKatakana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandHideVoicedKatakana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandHideVoicedKatakanaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandHideVoicedKatakanaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandHideVoicedKatakana), model => model.Initialize(nameof(CommandHideVoicedKatakana), ref model._CommandHideVoicedKatakana, ref _CommandHideVoicedKatakanaLocator, _CommandHideVoicedKatakanaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandHideVoicedKatakanaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandHideVoicedKatakana);           // Command state  
                var commandId = nameof(CommandHideVoicedKatakana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            List<Kana> res = GetRandomKatakana().ToList();

                            if (vm.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                            {
                                res.Remove(res.Where(k => k.Content == "ヰ").First());
                                res.Remove(res.Where(k => k.Content == "ヱ").First());
                            }
                            if (vm.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Katakana = new ObservableCollection<Kana>(res);
                            //Todo: Add HideVoicedKatakana logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandShowHistoryKatakana
        {
            get { return _CommandShowHistoryKatakanaLocator(this).Value; }
            set { _CommandShowHistoryKatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowHistoryKatakana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowHistoryKatakana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowHistoryKatakanaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowHistoryKatakanaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowHistoryKatakana), model => model.Initialize(nameof(CommandShowHistoryKatakana), ref model._CommandShowHistoryKatakana, ref _CommandShowHistoryKatakanaLocator, _CommandShowHistoryKatakanaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowHistoryKatakanaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandShowHistoryKatakana);           // Command state  
                var commandId = nameof(CommandShowHistoryKatakana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            Random rand = new Random(DateTime.Now.Millisecond);
                            var wyi = new Kana() { Content = "ヰ", Romaji = "wyi", IsHistory = true };
                            var wye = new Kana() { Content = "ヱ", Romaji = "wye", IsHistory = true };
                            if (vm.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                            {
                                vm.Katakana.Insert(rand.Next(0, vm.Katakana.Count - 1), wyi);
                                vm.Katakana.Insert(rand.Next(0, vm.Katakana.Count - 1), wye);
                            }
                            var res = new ObservableCollection<Kana>(vm.Katakana);
                            if (vm.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Katakana = res;
                            //Todo: Add ShowHistoryKatakana logic here, or
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


        public CommandModel<ReactiveCommand, String> CommandHideHistoryKatakana
        {
            get { return _CommandHideHistoryKatakanaLocator(this).Value; }
            set { _CommandHideHistoryKatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandHideHistoryKatakana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandHideHistoryKatakana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandHideHistoryKatakanaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandHideHistoryKatakanaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandHideHistoryKatakana), model => model.Initialize(nameof(CommandHideHistoryKatakana), ref model._CommandHideHistoryKatakana, ref _CommandHideHistoryKatakanaLocator, _CommandHideHistoryKatakanaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandHideHistoryKatakanaDefaultValueFactory =
            model =>
            {
                var state = nameof(CommandHideHistoryKatakana);           // Command state  
                var commandId = nameof(CommandHideHistoryKatakana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            if (vm.Katakana.Where(k => k.IsHistory == true).Count() != 0)
                            {
                                vm.Katakana.Remove(vm.Katakana.Where(k => k.Content == "ヰ").First());
                                vm.Katakana.Remove(vm.Katakana.Where(k => k.Content == "ヱ").First());
                            }
                            //Todo: Add HideHistoryKatakana logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandReplayKatakana
        {
            get { return _CommandReplayKatakanaLocator(this).Value; }
            set { _CommandReplayKatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandReplayKatakana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandReplayKatakana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandReplayKatakanaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandReplayKatakanaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandReplayKatakana), model => model.Initialize(nameof(CommandReplayKatakana), ref model._CommandReplayKatakana, ref _CommandReplayKatakanaLocator, _CommandReplayKatakanaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandReplayKatakanaDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandReplayKatakana);           // Command resource  
                var commandId = nameof(CommandReplayKatakana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res;
                            if (vm.Katakana.Where(k => k.Content == "バ").Count() != 0)
                            {
                                res = new ObservableCollection<Kana>(GetRandomKatakanaWithVoicedConsonants());
                            }
                            else
                            {
                                res = new ObservableCollection<Kana>(GetRandomKatakana());
                            }

                            if (vm.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                            {
                                res.Remove(res.Where(k => k.Content == "ヰ").First());
                                res.Remove(res.Where(k => k.Content == "ヱ").First());
                            }
                            if (vm.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Katakana = res;
                            //Todo: Add ReplayKatakana logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandHideKatakanaRomaji
        {
            get { return _CommandHideKatakanaRomajiLocator(this).Value; }
            set { _CommandHideKatakanaRomajiLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandHideKatakanaRomaji Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandHideKatakanaRomaji = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandHideKatakanaRomajiLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandHideKatakanaRomajiLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandHideKatakanaRomaji), model => model.Initialize(nameof(CommandHideKatakanaRomaji), ref model._CommandHideKatakanaRomaji, ref _CommandHideKatakanaRomajiLocator, _CommandHideKatakanaRomajiDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandHideKatakanaRomajiDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandHideKatakanaRomaji);           // Command resource  
                var commandId = nameof(CommandHideKatakanaRomaji);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res = new ObservableCollection<Kana>(vm.Katakana);
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                            vm.Katakana = res;
                            //Todo: Add HideKatakanaRomaji logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandShowKatakanaRomaji
        {
            get { return _CommandShowKatakanaRomajiLocator(this).Value; }
            set { _CommandShowKatakanaRomajiLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowKatakanaRomaji Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowKatakanaRomaji = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowKatakanaRomajiLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowKatakanaRomajiLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowKatakanaRomaji), model => model.Initialize(nameof(CommandShowKatakanaRomaji), ref model._CommandShowKatakanaRomaji, ref _CommandShowKatakanaRomajiLocator, _CommandShowKatakanaRomajiDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowKatakanaRomajiDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandShowKatakanaRomaji);           // Command resource  
                var commandId = nameof(CommandShowKatakanaRomaji);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res = new ObservableCollection<Kana>(vm.Katakana);
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                            vm.Katakana = res;
                            //Todo: Add ShowKatakanaRomaji logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandGetOrderKatakana
        {
            get { return _CommandGetOrderKatakanaLocator(this).Value; }
            set { _CommandGetOrderKatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandGetOrderKatakana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandGetOrderKatakana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandGetOrderKatakanaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandGetOrderKatakanaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandGetOrderKatakana), model => model.Initialize(nameof(CommandGetOrderKatakana), ref model._CommandGetOrderKatakana, ref _CommandGetOrderKatakanaLocator, _CommandGetOrderKatakanaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandGetOrderKatakanaDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandGetOrderKatakana);           // Command resource  
                var commandId = nameof(CommandGetOrderKatakana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            ObservableCollection<Kana> res;
                            res = new ObservableCollection<Kana>(GetOrderKatakanaWithVoicedConsonants());
                            res.Remove(res.Where(k => k.Content == "ヰ").First());
                            res.Remove(res.Where(k => k.Content == "ヱ").First());

                            if (vm.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Katakana = res;
                            //Todo: Add GetOrderKatakana logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandGetDisorderKatakana
        {
            get { return _CommandGetDisorderKatakanaLocator(this).Value; }
            set { _CommandGetDisorderKatakanaLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandGetDisorderKatakana Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandGetDisorderKatakana = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandGetDisorderKatakanaLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandGetDisorderKatakanaLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandGetDisorderKatakana), model => model.Initialize(nameof(CommandGetDisorderKatakana), ref model._CommandGetDisorderKatakana, ref _CommandGetDisorderKatakanaLocator, _CommandGetDisorderKatakanaDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandGetDisorderKatakanaDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandGetDisorderKatakana);           // Command resource  
                var commandId = nameof(CommandGetDisorderKatakana);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var res = new ObservableCollection<Kana>(GetRandomKatakana());
                            if (vm.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                foreach (var i in res)
                                {
                                    i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                                }
                            }
                            vm.Katakana = res;
                            //Todo: Add GetDisorderKatakana logic here, or
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
    }

}


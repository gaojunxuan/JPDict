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
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Xaml;
using System.Threading;
using JapaneseDict.Models;
using JapaneseDict.OnlineService;
using Windows.UI.Popups;
using System.Net.Http;
using Windows.Storage.Streams;

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class ResultPage_Model : ViewModelBase<ResultPage_Model>,IDisposable
    {
        
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
        ObservableCollection<MainDict> results;
        string _keyword;
        public ResultPage_Model(string keyword)
        {
            _keyword = keyword;
            QueryWord();
            EnableBackButtonOnTitleBar((sender, args) =>
            {
                BindableBase model=this;

                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();

                }
                DisableBackButtonOnTitleBar();


            });
            
            if(IsInDesignMode)
            {
                this._keyword = "あ";
            }
            
        }
        public ResultPage_Model(int id)
        {
            
            QueryWord(id);

            EnableBackButtonOnTitleBar((sender, args) =>
            {
                BindableBase model = this;

                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();

                }
                DisableBackButtonOnTitleBar();


            });

            if (IsInDesignMode)
            {
                this._keyword = "あ";
            }

        }
        private async void QueryWord()
        {

            this.results = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(_keyword);
            this.Results = results;
            //this.Cn2JpResult = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryCn2JpForUIAsync(_keyword);
            //this.OnlineResult = await QueryEngine.QueryEngine.OnlineQueryEngine.Query(_keyword);
        }
        private async void QueryWord(int id)
        {

            this.results = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(id);
            this.Results = results;
            //this.OnlineResult = await QueryEngine.QueryEngine.OnlineQueryEngine.Query(results.First().JpChar);
        }


        public ObservableCollection<MainDict> Results
        {
            get { return _ResultsLocator(this).Value; }
            set { _ResultsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<MainDict> Results Setup        
        protected Property<ObservableCollection<MainDict>> _Results = new Property<ObservableCollection<MainDict>> { LocatorFunc = _ResultsLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<MainDict>>> _ResultsLocator = RegisterContainerLocator<ObservableCollection<MainDict>>(nameof(Results), model => model.Initialize(nameof(Results), ref model._Results, ref _ResultsLocator, _ResultsDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<MainDict>> _ResultsDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return vm.results;
            };
        #endregion

        private void EnableBackButtonOnTitleBar(EventHandler<BackRequestedEventArgs> onBackRequested)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += onBackRequested;
        }
        private void DisableBackButtonOnTitleBar()
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        public ResultPage_Model()
        {
            
            EnableBackButtonOnTitleBar((sender, args) =>
            {
                if (this.StageManager.DefaultStage.Frame != null && this.StageManager.DefaultStage.Frame.CanGoBack)
                {
                    if (!this.StageManager.DefaultStage.Frame.CanGoBack)
                    {
                        DisableBackButtonOnTitleBar();
                    }
                    this.StageManager.DefaultStage.Frame.GoBack(); 
                }
                    
            });
        }
        public CommandModel<ReactiveCommand, String> CommandQueryWords
        {
            get { return _CommandQueryWordsLocator(this).Value; }
            set { _CommandQueryWordsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandQueryWords Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandQueryWords = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandQueryWordsLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandQueryWordsLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandQueryWords), model => model.Initialize(nameof(CommandQueryWords), ref model._CommandQueryWords, ref _CommandQueryWordsLocator, _CommandQueryWordsDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandQueryWordsDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandQueryWords);           // Command resource  
                var commandId = nameof(CommandQueryWords);
                var vm = CastToCurrentType(model);
                ObservableCollection<MainDict> results = new ObservableCollection<MainDict>();
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e => {
                            if ((!string.IsNullOrWhiteSpace(e.EventArgs.Parameter.ToString())) && (e.EventArgs.Parameter.ToString() != "Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs"))
                            {
                                Frame rootFrame = Window.Current.Content as Frame;

                                if (rootFrame.CanGoBack)
                                {
                                    rootFrame.GoBack();
                                    rootFrame.Navigate(typeof(ResultPage), e.EventArgs.Parameter.ToString());
                                    //GC.Collect();
                                }
                            }
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

        public CommandModel<ReactiveCommand, String> CommandQueryCn2JpResult
        {
            get { return _CommandQueryCn2JpResultLocator(this).Value; }
            set { _CommandQueryCn2JpResultLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandQueryCn2JpResult Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandQueryCn2JpResult = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandQueryCn2JpResultLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandQueryCn2JpResultLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandQueryCn2JpResult), model => model.Initialize(nameof(CommandQueryCn2JpResult), ref model._CommandQueryCn2JpResult, ref _CommandQueryCn2JpResultLocator, _CommandQueryCn2JpResultDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandQueryCn2JpResultDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandQueryCn2JpResult);           // Command resource  
                var commandId = nameof(CommandQueryCn2JpResult);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            vm.IsLocalQueryBusy = true;
                            vm.Cn2JpResult = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryCn2JpForUIAsync(vm.results.First().JpChar);
                            //Todo: Add QueryCn2JpResult logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                            vm.IsLocalQueryBusy = false;
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

        public CommandModel<ReactiveCommand, String> CommandQueryOnline
        {
            get { return _CommandQueryOnlineLocator(this).Value; }
            set { _CommandQueryOnlineLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandQueryOnline Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandQueryOnline = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandQueryOnlineLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandQueryOnlineLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandQueryOnline), model => model.Initialize(nameof(CommandQueryOnline), ref model._CommandQueryOnline, ref _CommandQueryOnlineLocator, _CommandQueryOnlineDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandQueryOnlineDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandQueryOnline);           // Command resource  
                var commandId = nameof(CommandQueryOnline);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            vm.IsOnlineQueryBusy = true;
                            vm.OnlineResult = await QueryEngine.QueryEngine.OnlineQueryEngine.Query(vm.results.First().JpChar);
                            //Todo: Add QueryOnline logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                            vm.IsOnlineQueryBusy = false;
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

        public bool IsOnlineQueryBusy
        {
            get { return _IsOnlineQueryBusyLocator(this).Value; }
            set { _IsOnlineQueryBusyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsOnlineQueryBusy Setup        
        protected Property<bool> _IsOnlineQueryBusy = new Property<bool> { LocatorFunc = _IsOnlineQueryBusyLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsOnlineQueryBusyLocator = RegisterContainerLocator<bool>(nameof(IsOnlineQueryBusy), model => model.Initialize(nameof(IsOnlineQueryBusy), ref model._IsOnlineQueryBusy, ref _IsOnlineQueryBusyLocator, _IsOnlineQueryBusyDefaultValueFactory));
        static Func<BindableBase, bool> _IsOnlineQueryBusyDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(bool);
            };
        #endregion

        public bool IsLocalQueryBusy
        {
            get { return _IsLocalQueryBusyLocator(this).Value; }
            set { _IsLocalQueryBusyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsLocalQueryBusy Setup        
        protected Property<bool> _IsLocalQueryBusy = new Property<bool> { LocatorFunc = _IsLocalQueryBusyLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsLocalQueryBusyLocator = RegisterContainerLocator<bool>(nameof(IsLocalQueryBusy), model => model.Initialize(nameof(IsLocalQueryBusy), ref model._IsLocalQueryBusy, ref _IsLocalQueryBusyLocator, _IsLocalQueryBusyDefaultValueFactory));
        static Func<BindableBase, bool> _IsLocalQueryBusyDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(bool);
            };
        #endregion


        #region Obsoleted Code
        //public string JpChar
        //{
        //    get { return _JpCharLocator(this).Value; }
        //    set { _JpCharLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property string JpChar Setup        
        //protected Property<string> _JpChar = new Property<string> { LocatorFunc = _JpCharLocator };
        //static Func<BindableBase, ValueContainer<string>> _JpCharLocator = RegisterContainerLocator<string>(nameof(JpChar), model => model.Initialize(nameof(JpChar), ref model._JpChar, ref _JpCharLocator, _JpCharDefaultValueFactory));
        //static Func<BindableBase, string> _JpCharDefaultValueFactory =
        //    model =>
        //    {
        //        var vm = CastToCurrentType(model);
        //        //TODO: Add the logic that produce default value from vm current status.
        //        return default(string);
        //    };
        //#endregion


        //public string Kana
        //{
        //    get { return _KanaLocator(this).Value; }
        //    set { _KanaLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property string Kana Setup        
        //protected Property<string> _Kana = new Property<string> { LocatorFunc = _KanaLocator };
        //static Func<BindableBase, ValueContainer<string>> _KanaLocator = RegisterContainerLocator<string>(nameof(Kana), model => model.Initialize(nameof(Kana), ref model._Kana, ref _KanaLocator, _KanaDefaultValueFactory));
        //static Func<BindableBase, string> _KanaDefaultValueFactory =
        //    model =>
        //    {
        //        var vm = CastToCurrentType(model);
        //        //TODO: Add the logic that produce default value from vm current status.
        //        return default(string);
        //    };
        //#endregion


        //public string Explanation
        //{
        //    get { return _ExplanationLocator(this).Value; }
        //    set { _ExplanationLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property string Explanation Setup        
        //protected Property<string> _Explanation = new Property<string> { LocatorFunc = _ExplanationLocator };
        //static Func<BindableBase, ValueContainer<string>> _ExplanationLocator = RegisterContainerLocator<string>(nameof(Explanation), model => model.Initialize(nameof(Explanation), ref model._Explanation, ref _ExplanationLocator, _ExplanationDefaultValueFactory));
        //static Func<BindableBase, string> _ExplanationDefaultValueFactory =
        //    model =>
        //    {
        //        var vm = CastToCurrentType(model);
        //        //TODO: Add the logic that produce default value from vm current status.
        //        return default(string);
        //    };
        //#endregion
        #endregion
        public CommandModel<ReactiveCommand, String> CommandSpeak
        {
            get { return _CommandSpeakLocator(this).Value; }
            set { _CommandSpeakLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandSpeak Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandSpeak = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSpeakLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSpeakLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandSpeak), model => model.Initialize(nameof(CommandSpeak), ref model._CommandSpeak, ref _CommandSpeakLocator, _CommandSpeakDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSpeakDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandSpeak);           // Command resource  
                var commandId = nameof(CommandSpeak);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            const string CLIENT_ID = "skylark_jpdict";
                            const string CLIENT_SECRET = "uzHa5qUm4+GehYnL2pMIw8XtNox8sbqGNq7S+UiM6bk=";
                            try
                            {
                                SpeechSynthesizer speech = new SpeechSynthesizer(CLIENT_ID, CLIENT_SECRET);
                                string text = e.EventArgs.Parameter.ToString();
                                string language = "ja";
                                // Gets the audio stream.
                                var stream = await speech.GetSpeakStreamAsync(text, language);
                                MediaElement mediaEle = new MediaElement();
                                // Reproduces the audio stream using a MediaElement.
                                mediaEle.SetSource(stream, speech.MimeContentType);
                                mediaEle.Play();

                            }
                            catch(HttpRequestException)
                            {
                                await new MessageDialog("请检查您的网络连接", "出现错误").ShowAsync();
                            }
                            
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

        public CommandModel<ReactiveCommand, String> CommandAddToNotebook
        {
            get { return _CommandAddToNotebookLocator(this).Value; }
            set { _CommandAddToNotebookLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandAddToNotebook Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandAddToNotebook = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandAddToNotebookLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandAddToNotebookLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandAddToNotebook), model => model.Initialize(nameof(CommandAddToNotebook), ref model._CommandAddToNotebook, ref _CommandAddToNotebookLocator, _CommandAddToNotebookDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandAddToNotebookDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandAddToNotebook);           // Command resource  
                var commandId = nameof(CommandAddToNotebook);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            QueryEngine.QueryEngine.UserDefDictQueryEngine.Add(Convert.ToInt32(e.EventArgs.Parameter));
                            //Todo: Add AddToNotebook logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandSendFeedback
        {
            get { return _CommandSendFeedbackLocator(this).Value; }
            set { _CommandSendFeedbackLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandSendFeedback Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandSendFeedback = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSendFeedbackLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSendFeedbackLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandSendFeedback), model => model.Initialize(nameof(CommandSendFeedback), ref model._CommandSendFeedback, ref _CommandSendFeedbackLocator, _CommandSendFeedbackDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSendFeedbackDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandSendFeedback);           // Command resource  
                var commandId = nameof(CommandSendFeedback);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            if(!string.IsNullOrWhiteSpace(e.EventArgs.Parameter.ToString()))
                            {
                                (Window.Current.Content as Frame).Navigate(typeof(FeedbackPage), e.EventArgs.Parameter.ToString());
                            }
                            //Todo: Add SendFeedback logic here, or
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

        public ObservableCollection<OnlineDict> OnlineResult
        {
            get { return _OnlineResultLocator(this).Value; }
            set { _OnlineResultLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<OnlineDict> OnlineResult Setup        
        protected Property<ObservableCollection<OnlineDict>> _OnlineResult = new Property<ObservableCollection<OnlineDict>> { LocatorFunc = _OnlineResultLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<OnlineDict>>> _OnlineResultLocator = RegisterContainerLocator<ObservableCollection<OnlineDict>>(nameof(OnlineResult), model => model.Initialize(nameof(OnlineResult), ref model._OnlineResult, ref _OnlineResultLocator, _OnlineResultDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<OnlineDict>> _OnlineResultDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<OnlineDict>);
            };
        #endregion


        public ObservableCollection<MainDict> Cn2JpResult
        {
            get { return _Cn2JpResultLocator(this).Value; }
            set { _Cn2JpResultLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<MainDict> Cn2JpResult Setup        
        protected Property<ObservableCollection<MainDict>> _Cn2JpResult = new Property<ObservableCollection<MainDict>> { LocatorFunc = _Cn2JpResultLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<MainDict>>> _Cn2JpResultLocator = RegisterContainerLocator<ObservableCollection<MainDict>>(nameof(Cn2JpResult), model => model.Initialize(nameof(Cn2JpResult), ref model._Cn2JpResult, ref _Cn2JpResultLocator, _Cn2JpResultDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<MainDict>> _Cn2JpResultDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<MainDict>);
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
        protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        {
            this.Dispose();
            return base.OnBindedViewUnload(view);
        }

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


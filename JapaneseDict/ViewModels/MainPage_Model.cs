using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JapaneseDict.Models;
using JapaneseDict.OnlineService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class MainPage_Model : ViewModelBase<MainPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        public MainPage_Model()
        {
            LoadData();
        }
        void LoadData()
        {
            GetEverydaySentence();
            GetNHKNews();
            GetListening();
        }
        async void GetEverydaySentence()
        {
            this.EverdaySentenceList = new ObservableCollection<EverydaySentence>();
            for (int i = 0; i < 3; i++)
            {
                this.EverdaySentenceList.Add((await JapaneseDict.OnlineService.JsonHelper.GetEverydaySentence(i)));
                await Task.Delay(100);
            }
        }
        async void GetNHKNews()
        {
            var res = await JapaneseDict.OnlineService.JsonHelper.GetNHKNews();
            this.NHKNews = new ObservableCollection<NHKNews>(res.Take(4));
        }
        async void GetListening()
        {
            this.NHKListeningSlow = new ObservableCollection<NHKRadios>();
            this.NHKListeningNormal = new ObservableCollection<NHKRadios>();
            this.NHKListeningFast = new ObservableCollection<NHKRadios>();
            for (int i = 0; i < await OnlineService.JsonHelper.GetNHKRadiosItemsCount()-1; i++)
            {
                this.NHKListeningSlow.Add(await JsonHelper.GetNHKRadios(i, "slow"));
                this.NHKListeningNormal.Add(await JsonHelper.GetNHKRadios(i, "normal"));
                this.NHKListeningFast.Add(await JsonHelper.GetNHKRadios(i, "fast"));
            }
        }
        const string CLIENT_ID = "skylark_jpdict";
        const string CLIENT_SECRET = "uzHa5qUm4+GehYnL2pMIw8XtNox8sbqGNq7S+UiM6bk=";
        SpeechSynthesizer transServ = new SpeechSynthesizer(CLIENT_ID, CLIENT_SECRET);
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
                //SQLiteConnection conn;
                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e => {
                            if ((!string.IsNullOrWhiteSpace(e.EventArgs.Parameter.ToString()))&&(e.EventArgs.Parameter.ToString()!= "Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs"))
                            {
                                Frame rootFrame = Window.Current.Content as Frame;
                                rootFrame.Navigate(typeof(ResultPage),Util.StringHelper.ResolveReplicator(e.EventArgs.Parameter.ToString().Replace(" ", "").Replace(" ", "")));
                                //GC.Collect();
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

        public CommandModel<ReactiveCommand, String> CommandNavToKanjiFlashcardPage
        {
            get { return _CommandNavToKanjiFlashcardPageLocator(this).Value; }
            set { _CommandNavToKanjiFlashcardPageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNavToKanjiFlashcardPage Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandNavToKanjiFlashcardPage = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavToKanjiFlashcardPageLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavToKanjiFlashcardPageLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandNavToKanjiFlashcardPage), model => model.Initialize(nameof(CommandNavToKanjiFlashcardPage), ref model._CommandNavToKanjiFlashcardPage, ref _CommandNavToKanjiFlashcardPageLocator, _CommandNavToKanjiFlashcardPageDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavToKanjiFlashcardPageDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandNavToKanjiFlashcardPage);           // Command resource  
                var commandId = nameof(CommandNavToKanjiFlashcardPage);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var parm = e.EventArgs.Parameter.ToString();
                            bool result = Int32.TryParse(parm, out int jlpt);
                            if (result)
                            {
                                (Window.Current.Content as Frame).Navigate(typeof(KanjiFlashcardPage),jlpt);
                            }
                            //Todo: Add NavToKanjiFlashcardPage logic here, or
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

        public CommandModel<ReactiveCommand, String> CommandNavToKanaFlashcardPage
        {
            get { return _CommandNavToKanaFlashcardPageLocator(this).Value; }
            set { _CommandNavToKanaFlashcardPageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNavToKanaFlashcardPage Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandNavToKanaFlashcardPage = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavToKanaFlashcardPageLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavToKanaFlashcardPageLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandNavToKanaFlashcardPage), model => model.Initialize(nameof(CommandNavToKanaFlashcardPage), ref model._CommandNavToKanaFlashcardPage, ref _CommandNavToKanaFlashcardPageLocator, _CommandNavToKanaFlashcardPageDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavToKanaFlashcardPageDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandNavToKanaFlashcardPage);           // Command resource  
                var commandId = nameof(CommandNavToKanaFlashcardPage);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var parm = e.EventArgs.Parameter.ToString();
                            bool result = Int32.TryParse(parm, out int index);
                            if (result)
                            {
                                (Window.Current.Content as Frame).Navigate(typeof(KanaFlashcardPage), index);
                            }
                            //Todo: Add NavToKanaFlashcardPage logic here, or
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

        public ObservableCollection<EverydaySentence> EverdaySentenceList
        {
            get { return _EverdaySentenceListLocator(this).Value; }
            set { _EverdaySentenceListLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<EverydaySentence> EverdaySentenceList Setup        
        protected Property<ObservableCollection<EverydaySentence>> _EverdaySentenceList = new Property<ObservableCollection<EverydaySentence>> { LocatorFunc = _EverdaySentenceListLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<EverydaySentence>>> _EverdaySentenceListLocator = RegisterContainerLocator<ObservableCollection<EverydaySentence>>(nameof(EverdaySentenceList), model => model.Initialize(nameof(EverdaySentenceList), ref model._EverdaySentenceList, ref _EverdaySentenceListLocator, _EverdaySentenceListDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<EverydaySentence>> _EverdaySentenceListDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<EverydaySentence>);
            };
        #endregion

        public ObservableCollection<NHKNews> NHKNews
        {
            get { return _NHKNewsLocator(this).Value; }
            set { _NHKNewsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<NHKNews> NHKNews Setup        
        protected Property<ObservableCollection<NHKNews>> _NHKNews = new Property<ObservableCollection<NHKNews>> { LocatorFunc = _NHKNewsLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<NHKNews>>> _NHKNewsLocator = RegisterContainerLocator<ObservableCollection<NHKNews>>(nameof(NHKNews), model => model.Initialize(nameof(NHKNews), ref model._NHKNews, ref _NHKNewsLocator, _NHKNewsDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<NHKNews>> _NHKNewsDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<NHKNews>);
            };
        #endregion

        public ObservableCollection<NHKRadios> NHKListeningSlow
        {
            get { return _NHKListeningSlowLocator(this).Value; }
            set { _NHKListeningSlowLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<NHKRadios> NHKListeningSlow Setup        
        protected Property<ObservableCollection<NHKRadios>> _NHKListeningSlow = new Property<ObservableCollection<NHKRadios>> { LocatorFunc = _NHKListeningSlowLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<NHKRadios>>> _NHKListeningSlowLocator = RegisterContainerLocator<ObservableCollection<NHKRadios>>(nameof(NHKListeningSlow), model => model.Initialize(nameof(NHKListeningSlow), ref model._NHKListeningSlow, ref _NHKListeningSlowLocator, _NHKListeningSlowDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<NHKRadios>> _NHKListeningSlowDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<NHKRadios>);
            };
        #endregion

        public ObservableCollection<NHKRadios> NHKListeningNormal
        {
            get { return _NHKListeningNormalLocator(this).Value; }
            set { _NHKListeningNormalLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<NHKRadios> NHKListeningNormal Setup        
        protected Property<ObservableCollection<NHKRadios>> _NHKListeningNormal = new Property<ObservableCollection<NHKRadios>> { LocatorFunc = _NHKListeningNormalLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<NHKRadios>>> _NHKListeningNormalLocator = RegisterContainerLocator<ObservableCollection<NHKRadios>>(nameof(NHKListeningNormal), model => model.Initialize(nameof(NHKListeningNormal), ref model._NHKListeningNormal, ref _NHKListeningNormalLocator, _NHKListeningNormalDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<NHKRadios>> _NHKListeningNormalDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<NHKRadios>);
            };
        #endregion

        public ObservableCollection<NHKRadios> NHKListeningFast
        {
            get { return _NHKListeningFastLocator(this).Value; }
            set { _NHKListeningFastLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<NHKRadios> NHKListeningFast Setup        
        protected Property<ObservableCollection<NHKRadios>> _NHKListeningFast = new Property<ObservableCollection<NHKRadios>> { LocatorFunc = _NHKListeningFastLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<NHKRadios>>> _NHKListeningFastLocator = RegisterContainerLocator<ObservableCollection<NHKRadios>>(nameof(NHKListeningFast), model => model.Initialize(nameof(NHKListeningFast), ref model._NHKListeningFast, ref _NHKListeningFastLocator, _NHKListeningFastDefaultValueFactory));
        static Func<BindableBase, ObservableCollection<NHKRadios>> _NHKListeningFastDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(ObservableCollection<NHKRadios>);
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


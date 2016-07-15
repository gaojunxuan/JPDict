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
using System.Diagnostics;
using Windows.UI.Popups;

namespace JapaneseDict.GUI.ViewModels
{

    [DataContract]
    public class TranslationPage_Model : ViewModelBase<TranslationPage_Model>
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
        public string TranslationResult
        {
            get { return _TranslationResultLocator(this).Value; }
            set { _TranslationResultLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string TranslationResult Setup        
        protected Property<string> _TranslationResult = new Property<string> { LocatorFunc = _TranslationResultLocator };
        static Func<BindableBase, ValueContainer<string>> _TranslationResultLocator = RegisterContainerLocator<string>("TranslationResult", model => model.Initialize("TranslationResult", ref model._TranslationResult, ref _TranslationResultLocator, _TranslationResultDefaultValueFactory));
        static Func<BindableBase, string> _TranslationResultDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(string);
            };
        #endregion

        public CommandModel<ReactiveCommand, String> CommandTranslate
        {
            get { return _CommandTranslateLocator(this).Value; }
            set { _CommandTranslateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandTranslate Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandTranslate = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandTranslateLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandTranslateLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandTranslate", model => model.Initialize("CommandTranslate", ref model._CommandTranslate, ref _CommandTranslateLocator, _CommandTranslateDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandTranslateDefaultValueFactory =
            model =>
            {
                var resource = "CommandTranslate";           // Command resource  
                var commandId = "CommandTranslate";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            string sourcelang = vm.SourceLang;
                            string targetlang;
                            if(sourcelang=="jp")
                            {
                                targetlang = "zh";
                            }
                            else
                            {
                                targetlang = "jp";
                            }
                            if (e.EventArgs.Parameter != null)
                            {
                                try
                                {
                                    if (e.EventArgs.Parameter.ToString().Count() > 2000)
                                    {
                                        await new MessageDialog("最多只能翻译 2000 字符的内容，超出部分将被自动去除。", "提示").ShowAsync();
                                        vm.TranslationResult = await OnlineService.JsonHelper.GetTranslateResult(e.EventArgs.Parameter.ToString().Substring(0, 2000), sourcelang, targetlang);
                                    }
                                    else
                                    {
                                        vm.TranslationResult = await OnlineService.JsonHelper.GetTranslateResult(e.EventArgs.Parameter.ToString(), sourcelang, targetlang);
                                    }

                                }
                                catch
                                {
                                    Debug.WriteLine("error");
                                }

                            }
                            //Todo: Add Translate logic here, or
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

        public string SourceLang
        {
            get { return _SourceLangLocator(this).Value; }
            set { _SourceLangLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string SourceLang Setup        
        protected Property<string> _SourceLang = new Property<string> { LocatorFunc = _SourceLangLocator };
        static Func<BindableBase, ValueContainer<string>> _SourceLangLocator = RegisterContainerLocator<string>(nameof(SourceLang), model => model.Initialize(nameof(SourceLang), ref model._SourceLang, ref _SourceLangLocator, _SourceLangDefaultValueFactory));
        static Func<BindableBase, string> _SourceLangDefaultValueFactory =
            model =>
            {
                var vm = CastToCurrentType(model);
                //TODO: Add the logic that produce default value from vm current status.
                return default(string);
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
        protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        {
            this.SourceLang = "jp";
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


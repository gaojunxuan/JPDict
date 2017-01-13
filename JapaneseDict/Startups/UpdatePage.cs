using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using JapaneseDict.GUI;
using JapaneseDict.GUI.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action UpdatePageConfig =
           CreateAndAddToAllConfig(ConfigUpdatePage);

        public static void ConfigUpdatePage()
        {
            ViewModelLocator<UpdatePage_Model>
                .Instance
                .Register(context =>
                    new UpdatePage_Model())
                .GetViewMapper()
                .MapToDefault<UpdatePage>();

        }
    }
}

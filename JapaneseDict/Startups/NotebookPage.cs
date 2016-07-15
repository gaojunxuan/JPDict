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
        static Action NotebookPageConfig =
           CreateAndAddToAllConfig(ConfigNotebookPage);

        public static void ConfigNotebookPage()
        {
            ViewModelLocator<NotebookPage_Model>
                .Instance
                .Register(context =>
                    new NotebookPage_Model())
                .GetViewMapper()
                .MapToDefault<NotebookPage>();

        }
    }
}

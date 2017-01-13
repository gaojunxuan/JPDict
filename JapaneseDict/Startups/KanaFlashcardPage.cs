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
        static Action KanaFlashcardPageConfig =
           CreateAndAddToAllConfig(ConfigKanaFlashcardPage);

        public static void ConfigKanaFlashcardPage()
        {
            ViewModelLocator<KanaFlashcardPage_Model>
                .Instance
                .Register(context =>
                    new KanaFlashcardPage_Model())
                .GetViewMapper()
                .MapToDefault<KanaFlashcardPage>();

        }
    }
}

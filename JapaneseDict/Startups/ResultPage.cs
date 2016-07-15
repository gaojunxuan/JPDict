using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using JapaneseDict;
using JapaneseDict.GUI.ViewModels;
using System;
using System.Net;
using System.Windows;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JapaneseDict.GUI;

namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action ResultPageConfig =
           CreateAndAddToAllConfig(ConfigResultPage);

        public static void ConfigResultPage()
        {
            
                ViewModelLocator<ResultPage_Model>
                .Instance
                .Register(context =>
                    new ResultPage_Model())
                .GetViewMapper()
                .MapToDefault<ResultPage>();
            

        }


       
    }
}

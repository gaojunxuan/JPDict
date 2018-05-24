using System;
using GalaSoft.MvvmLight.Ioc;
using JapaneseDict.GUI.Services;
using JapaneseDict.GUI;
using CommonServiceLocator;

namespace JapaneseDict.GUI.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<MainViewModel, MainPage>();
            Register<NotebookViewModel, NotebookPage>();
            Register<ResultViewModel, ResultPage>();
            Register<TranslationViewModel, TranslationPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<UpdateViewModel, UpdatePage>();
            Register<KanaFlashcardViewModel, KanaFlashcardPage>();
            Register<KanjiFlashcardViewModel, KanjiFlashcardPage>();

        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public NotebookViewModel NotebookViewModel => ServiceLocator.Current.GetInstance<NotebookViewModel>();
        public ResultViewModel ResultViewModel => ServiceLocator.Current.GetInstance<ResultViewModel>();
        public TranslationViewModel TranslationViewModel => ServiceLocator.Current.GetInstance<TranslationViewModel>();
        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public UpdateViewModel UpdateViewModel => ServiceLocator.Current.GetInstance<UpdateViewModel>();
        public KanaFlashcardViewModel KanaFlashcardViewModel => ServiceLocator.Current.GetInstance<KanaFlashcardViewModel>();
        public KanjiFlashcardViewModel KanjiFlashcardViewModel => ServiceLocator.Current.GetInstance<KanjiFlashcardViewModel>();
        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}

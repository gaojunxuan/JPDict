using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Diagnostics;
using Windows.UI.Popups;
using JapaneseDict.GUI.Helpers;

namespace JapaneseDict.GUI.ViewModels
{
    public class TranslationViewModel : ViewModelBase
    {
        public TranslationViewModel()
        {
            this.SourceLang = "jp";
        }

        private string translationResult;

        public string TranslationResult
        {
            get { return translationResult; }
            set
            {
                translationResult = value;
                RaisePropertyChanged();
            }
        }
        private RelayCommand<string> _translateCommand;

        /// <summary>
        /// Gets the TranslateCommand.
        /// </summary>
        public RelayCommand<string> TranslateCommand
        {
            get
            {
                return _translateCommand
                    ?? (_translateCommand = new RelayCommand<string>(
                    async(x) =>
                    {
                        string sourcelang = this.SourceLang;
                        if (x != null)
                        {
                            try
                            {
                                bool useTexTra = StorageHelper.GetSetting<bool>("UseTexTra");
                                if (x.ToString().Count() > 2000)
                                {
                                    await new MessageDialog("最多只能翻译 2000 字符的内容，超出部分将被自动去除。", "提示").ShowAsync();
                                    if (sourcelang == "jp")
                                    {
                                        if (useTexTra)
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetJpToCnTranslationResult(x.ToString().Substring(0,2000));
                                        }
                                        else
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetTranslateResult(x.ToString().Substring(0,2000), "jp", "zh");
                                        }
                                    }
                                    else
                                    {
                                        if (useTexTra)
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetCnToJpTranslationResult(x.ToString().Substring(0, 2000));
                                        }
                                        else
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetTranslateResult(x.ToString().Substring(0, 2000), "zh", "jp");
                                        }
                                    }
                                }
                                else
                                {
                                    if (sourcelang == "jp")
                                    {
                                        if (useTexTra)
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetJpToCnTranslationResult(x.ToString());
                                        }
                                        else
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetTranslateResult(x.ToString(), "jp", "zh");
                                        }
                                    }
                                    else
                                    {
                                        if (useTexTra)
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetCnToJpTranslationResult(x.ToString());
                                        }
                                        else
                                        {
                                            this.TranslationResult = await OnlineService.JsonHelper.GetTranslateResult(x.ToString(), "zh", "jp");
                                        }
                                    }
                                }

                            }
                            catch
                            {
                                Debug.WriteLine("error");
                            }
                        }
                        
                    }));
            }
        }
        private string sourceLang;
        public string SourceLang
        {
            get { return sourceLang; }
            set
            {
                sourceLang = value;
                RaisePropertyChanged();
            }
        }
              
    }

}


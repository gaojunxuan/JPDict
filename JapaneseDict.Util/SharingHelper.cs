using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace JapaneseDict.Util
{
    public static class SharingHelper
    {
        private static string Title { get; set; }
        private static string Content { get; set; }
        private static DataTransferManager dataTransferManager;
        private static void SetText(string content,string title)
        {
            Title = title;
            Content = content;
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }
        private static void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
           
            if (GetShareContent(e.Request))
            {
                if (String.IsNullOrEmpty(e.Request.Data.Properties.Title))
                {
                    return;
                }
            }
        }
        private static bool GetShareContent(DataRequest request)
        {
            bool succeeded = false;

            string dataPackageText = Content;
            if (!String.IsNullOrEmpty(dataPackageText))
            {
                DataPackage requestData = request.Data;
                requestData.Properties.Title = Title;
                requestData.SetText(dataPackageText);
                succeeded = true;
            }
            else
            {
                request.FailWithDisplayText("出现错误，请稍后重试");
            }
            return succeeded;
        }
        public static void ShowShareUI(string title,string content)
        {
            SetText(content, title);
            DataTransferManager.ShowShareUI();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JapaneseDict.GUI.Helpers
{
    public class ApplicationViewHelper
    {
        static Dictionary<int, CoreApplicationView> viewsDict = new Dictionary<int, CoreApplicationView>();
        static Dictionary<string, int> nameDict = new Dictionary<string, int>();
        static Dictionary<int, string> idDict = new Dictionary<int, string>();

        public static int MainViewId { get; set; }

        static async void Prepare()
        {
            var views = CoreApplication.Views;
            if (viewsDict.Count != views.Count)
            {
                foreach (var v in views)
                {
                    int id = -1;
                    await v.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => id = ApplicationView.GetForCurrentView().Id);
                    if (!viewsDict.ContainsKey(id))
                        viewsDict.Add(id, v);
                }
            }
        }
        /// <summary>
        /// Get the view with specified id, return null if not exist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CoreApplicationView GetViewFromId(int id)
        {
            Prepare();
            if (viewsDict.ContainsKey(id))
                return viewsDict[id];
            return null;
        }
        /// <summary>
        /// Get the view with specified name, return null if not exist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CoreApplicationView GetViewFromName(string name)
        {
            Prepare();
            if (nameDict.ContainsKey(name))
                if (viewsDict.ContainsKey(nameDict[name]))
                    return viewsDict[nameDict[name]];
            return null;
        }
        /// <summary>
        /// Create a new CoreApplicationView and return its id.
        /// </summary>
        /// <param name="sourcePageType"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static async Task<int> CreateNewViewAsync(Type sourcePageType, object navParameter = null, string title = "")
        {
            Prepare();
            var newView = CoreApplication.CreateNewView();
            int viewId = -1;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                viewId = ApplicationView.GetForCurrentView().Id;
                frame.Navigate(sourcePageType, navParameter);
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = title;
                ApplicationView.GetForCurrentView().Consolidated += ApplicationViewHelper_Consolidated;
            });
            viewsDict.Add(viewId, newView);
            return viewId;
        }
        /// <summary>
        /// Create a new CoreApplicationView and return its id.
        /// </summary>
        /// <param name="sourcePageType"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static async Task<int> CreateNewViewAsync(string name, Type sourcePageType, object navParameter = null, string title = "JPDict")
        {
            Prepare();
            var newView = CoreApplication.CreateNewView();
            int viewId = -1;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                viewId = ApplicationView.GetForCurrentView().Id;
                frame.Navigate(sourcePageType, navParameter);
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = title;
                ApplicationView.GetForCurrentView().Consolidated += ApplicationViewHelper_Consolidated;
            });
            nameDict.Add(name, viewId);
            idDict.Add(viewId, name);
            viewsDict.Add(viewId, newView);
            return viewId;
        }

        private static void ApplicationViewHelper_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            CoreApplicationView view = GetViewFromId(sender.Id);
            if (view != null)
            {
                if (!view.CoreWindow.Visible)
                {
                    RemoveView(sender.Id);
                    Window.Current.Content = null;
                }
            }
        }
        /// <summary>
        /// Remove a view from dictionary.
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveView(int id)
        {
            if (viewsDict.ContainsKey(id))
                viewsDict.Remove(id);
            if (idDict.ContainsKey(id))
            {
                nameDict.Remove(idDict[id]);
                idDict.Remove(id);
            }
        }
        /// <summary>
        /// Remove a view from dictionary.
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveView(string name)
        {
            if (nameDict.ContainsKey(name))
            {
                int id = nameDict[name];
                nameDict.Remove(name);
                if (idDict.ContainsKey(id))
                    idDict.Remove(id);
                if (viewsDict.ContainsKey(id))
                    viewsDict.Remove(id);
            }
        }
        public static bool Contains(int id)
        {
            return viewsDict.ContainsKey(id);
        }
        public static bool Contains(string name)
        {
            return nameDict.ContainsKey(name);
        }
        public static int GetId(string name)
        {
            if (nameDict.ContainsKey(name))
                return nameDict[name];
            return -1;
        }
    }
}

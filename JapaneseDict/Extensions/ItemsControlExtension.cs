using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace JapaneseDict.GUI.Extensions
{
    public static class ItemsControlExtension
    {
        /// <summary>
        /// 为 ItemsControl 异步添加项目
        /// </summary>
        /// <param name="items">需要添加的项目</param>
        /// <returns></returns>
        public static async Task AddItemsAsync(this ItemsControl control, IEnumerable<Object> items)
        {
            foreach (var item in items)
            {
                await control.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    control.Items.Add(item);
                });
            }
        }

        /// <summary>
        /// 为 ItemsControl 异步添加项目,每次添加一个项目后等待指定毫秒
        /// </summary>
        /// <param name="items">需要添加的项目</param>
        /// <param name="milliseconds">每次等待的毫秒数</param>
        /// <returns></returns>
        public static async Task AddItemsAsync(this ItemsControl control, IEnumerable<Object> items, int milliseconds)
        {
            foreach (var item in items)
            {
                await control.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    control.Items.Add(item);
                });
                await Task.Delay(milliseconds);
            }
        }
    }
}

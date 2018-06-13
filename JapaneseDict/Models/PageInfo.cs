using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.GUI.Models
{
    public class PageInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public Type TargetPageType { get; set; }
        public object Parameter { get; set; }
        public (Type,object) NavigationInfo
        {
            get
            {
                return (TargetPageType, Parameter);
            }
        }
    }
}

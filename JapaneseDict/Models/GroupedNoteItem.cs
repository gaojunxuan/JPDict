using JapaneseDict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.GUI.Models
{
    public class GroupedNoteItem
    {
        public string Key { get; set; }
        public List<UserDefDict> ItemContent { get; set; }
    }
}

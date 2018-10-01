using JapaneseDict.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.GUI.Models
{
    public class GroupedDictItem : IGrouping<int, Dict>
    {
        List<Dict> elements;
        public ObservableCollection<Dict> Definitions { get; set; }
        public GroupedDictItem(IGrouping<int,Dict> g)
        {
            if (g == null)
                throw new ArgumentNullException("g");
            Key = g.Key;
            elements = g.ToList();
            Definitions = new ObservableCollection<Dict>(elements);
        }
        public int Key { get; private set; }

        public IEnumerator<Dict> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public string Reading => elements.First().Reading;
        public string Keyword => elements.First().Keyword;
        public string Kanji => elements.First().Kanji;
        public string LoanWord => elements.First().LoanWord;
        public string SeeAlso => elements.First().SeeAlso;
        public bool IsInNotebook => elements.First().IsInNotebook;
        public bool IsControlBtnVisible => elements.First().IsControlBtnVisible;
        public bool ShowKanjiPanel => !string.IsNullOrEmpty(Kanji);
        public bool ShowLoanWordPanel => !string.IsNullOrEmpty(LoanWord);
        public bool ShowSeeAlsoPanel => !string.IsNullOrEmpty(SeeAlso);
    }
}

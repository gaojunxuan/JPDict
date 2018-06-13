using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.GUI.Models
{
    public class Verb
    {
        public string OriginalForm { get; set; }
        public string MasuForm { get; set; }
        public string MasuNegative { get; set; }
        public string Causative { get; set; }
        public string NegativeCausative { get; set; }
        public string EbaForm { get; set; }
        public string Imperative { get; set; }
        public string NegativeImperative { get; set; }
        public string NegativeForm { get; set; }
        public string PastNegative { get; set; }
        public string Passive { get; set; }
        public string NegativePassive { get; set; }
        public string Potential { get; set; }
        public string NegativePotential { get; set; }
        public string TaForm { get; set; }
        public string TeForm { get; set; }
        public string Volitional { get; set; }
    }
}

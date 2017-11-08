using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Util
{
    public class StringHelper
    {
        public static string ResolveReplicator(string input)
        {            
            if (input.Contains("々"))
            {
                string result = "";
                if (input.IndexOf('々') != 0)
                {
                    result = input.Replace('々', input[input.IndexOf('々') - 1]);
                    return result;
                }
            }
            return input;
        }
        public static string PrepareVerbs(string input)
        {
            if(VerbConjugationHelper.IsNegative(input))
            {
                return VerbConjugationHelper.FromNegativeToOriginal(input);
            }
            else if(VerbConjugationHelper.IsCausative(input))
            {
                return VerbConjugationHelper.FromCausativeToOriginal(input);
            }
            else if(VerbConjugationHelper.IsPassive(input))
            {
                return VerbConjugationHelper.FromPassiveToOriginal(input);
            }
            else if (VerbConjugationHelper.IsMasu(input))
            {
                return VerbConjugationHelper.FromMasuToOriginal(input);
            }
            return input.Length>=4?input.Substring(0,2):input;
        }
    }
}

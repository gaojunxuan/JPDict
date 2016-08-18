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
    }
}

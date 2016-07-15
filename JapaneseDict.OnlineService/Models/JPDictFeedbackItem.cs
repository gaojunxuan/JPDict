using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class JPDictFeedbackItem
    {
        public string SuggestJpChar { get; set; }
        public string SuggestExplanation { get; set; }
        public string SuggestKana { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public string Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public byte[] Version { get; set; }

    }
}

using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.OnlineService.Models
{
    public class OnedriveNote
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int OriginID { get; set; }
    }
}

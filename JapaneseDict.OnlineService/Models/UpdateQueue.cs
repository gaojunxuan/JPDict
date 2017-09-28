using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.OnlineService.Models
{
    public class UpdateQueue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int OriginId { get; set; }
        public string Action { get; set; }
    }
}

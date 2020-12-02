using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KihBot.Data
{
    public class FunData
    {
        public string[] BaseAnswers { get; set; }
        public Dictionary<ulong, List<string>> CustomAnswers { get; set; }
        public FunData() { }
        
    }
}

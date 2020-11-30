using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KihBot.Data
{
    class FunData : IData
    {
        public List<string> MagicBallAnswers { get; set; }

        [JsonIgnore]
        public string FileName { get; init; }
        public FunData()
        {
            MagicBallAnswers = new List<string>();
            this.FileName = "fundata.json";
        }
    }
}

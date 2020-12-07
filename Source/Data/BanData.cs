using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KihBot.Data
{
    [Serializable]
    public class BanData : IData
    {
        [JsonIgnore]
        public string FileName { get; init; }
        public BanData() => FileName = "bandata.json";

        public void LoadData(IData data)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KihBot.Data
{
    public interface IData
    {
        [JsonIgnore]
        string FileName { get; init; }
        public void LoadData(IData data);
    }
}

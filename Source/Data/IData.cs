using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace KihBot.Data
{
    public interface IData
    {
        [JsonIgnore]
        string FileName { get; init; }
    }
}

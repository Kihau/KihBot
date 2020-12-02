using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KihBot.Data
{
    [Serializable]
    public class ConfigData : IData
    {
        /// <summary>
        /// Serializble data
        /// </summary>
        public string Token { get; set; }
        public string Version { get; set; }
        public int Color { get; set; }
        public string DefaultPrefix { get; set; }
        public string DefaultCommand { get; set; }

        public List<ulong> WhitelistedUsers { get; set; }
        public Dictionary<ulong, string> Prefixes { get; set; }

        [JsonIgnore]
        public string FileName { get; init; }
        public ConfigData() => FileName = "config.json";

        public void LoadData(IData data)
        {
            var properties = this.GetType().GetProperties();
            foreach (var prop in properties)
                prop.SetValue(this, prop.GetValue(data));
        }
    }
}

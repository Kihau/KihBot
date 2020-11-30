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
        public string Token { get; init; }
        public string Version { get; init; }
        public string DefaultPrefix { get; init; }
        public string DefaultCommand { get; init; }

        public List<ulong> WhitelistedUsers { get; set; }
        public Dictionary<ulong, string> Prefixes { get; set; }

        [JsonIgnore]
        public string FileName { get; init; }
        public ConfigData()
        {
            Prefixes = new Dictionary<ulong, string>();
            this.FileName = "config.json";
        }

        void ClearConfig()
        {

        }
    }
}

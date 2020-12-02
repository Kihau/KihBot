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
    public class ConfigData
    {
        /// <summary>
        /// Serializble data
        /// </summary>
        public string Token { get; init; }
        public string Version { get; init; }
        public int Color { get; init; }
        public string DefaultPrefix { get; init; }
        public string DefaultCommand { get; init; }

        public List<ulong> WhitelistedUsers { get; set; }
        public Dictionary<ulong, string> Prefixes { get; set; }

        public ConfigData() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KihBot.Database
{
    [Serializable]
    public class Configuration
    {
        public string Token { get; init; }
        public string Version { get; init; }

        public string DefaultPrefix { get; init; }
        public string DefaultCommand { get; init; }

        public Dictionary<ulong, string> Prefixes { get; set; }

        public Configuration() { }
    }
}

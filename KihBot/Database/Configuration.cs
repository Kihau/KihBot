using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Database
{
    [Serializable]
    public class Configuration
    {
        [NonSerialized]
        public readonly string DefaultPrefix = "/";

        [NonSerialized]
        public readonly string DefaultCommand = "/help";

        public string Token { get; init; }
        public Dictionary<ulong, string> Prefixes { get; set; }

        public Configuration() { }
    }
}

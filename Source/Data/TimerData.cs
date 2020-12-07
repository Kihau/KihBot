using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace KihBot.Data
{
    [Serializable]
    public class TimerData : IData
    {
        // <guild ids, all guild timers>
        public Dictionary<ulong, GuildTimer> GuildTimers { get; set; }

        [JsonIgnore]
        public string FileName { get; init; }
        public TimerData() => FileName = "timerdata.json";

        public void LoadData(IData data)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class GuildTimer
    {
        // <user ids, user timers>
        public Dictionary<ulong, UserTimer> UserTimers { get; set; };

        public GuildTimer() { }
    }

    [Serializable]
    public class UserTimer
    {
        public bool Global { get; set; }
        public string Reminder { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan RefreshTime { get; set; }

        public UserTimer() { }
    }

}

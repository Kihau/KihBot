using DSharpPlus;
using DSharpPlus.Entities;
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
        public Dictionary<ulong, GuildTimer> GuildTimers { get; set; } = new();

        [JsonIgnore]
        public string FileName { get; init; }
        public TimerData() => FileName = "timerdata.json";

        public GuildTimer GetGuildTimer(ulong id)
        {
            if (GuildTimers.TryGetValue(id, out var value))
                return value;
            else
            {
                GuildTimer guildTimer = new GuildTimer();
                GuildTimers.Add(id, guildTimer);
                return guildTimer;
            }
        }

        public void LoadData(IData data)
        {
            GuildTimers = (data as TimerData).GuildTimers;

            // clear
            //foreach (var guild in GuildTimers.Where(x => x.Value.UserTimers.Count == 0).Select(x => x.Key))
            //    GuildTimers.Remove(guild);

            //foreach (var guild in GuildTimers.Where(x => x.Value.UserTimers.Count == 0).Select(x => x.Key))
            //    GuildTimers.Remove(guild);

        }

        public void Start(DiscordClient client)
        {
            foreach (var guildTimer in GuildTimers.Values)
            {
                foreach (var userTimers in guildTimer.UserTimers.Values)
                {
                    foreach (var uTimer in userTimers.Where(x => x.timer == null))
                    {
                        Task.Run(async () =>
                        {
                            AutoResetEvent autoReset = new AutoResetEvent(false);
                            await uTimer.StartTimer(client, autoReset);
                            autoReset.WaitOne();
                            uTimer.timer.Dispose();
                            userTimers.Remove(uTimer);
                        });
                    }
                }
            }
        }
    }

    public class GuildTimer
    {
        // <user ids, user timers>
        public Dictionary<ulong, List<UserTimer>> UserTimers { get; set; }

        public GuildTimer() => UserTimers = new Dictionary<ulong, List<UserTimer>>();
        public List<UserTimer> GetUserTimers(ulong id)
        {
            if (UserTimers.TryGetValue(id, out var value))
                return value.Where(x => x.totalTime != TimeSpan.Zero).ToList(); 
            else
            {
                var userTimers = new List<UserTimer>();
                UserTimers.Add(id, userTimers);
                return userTimers;
            }
        }
        
        public List<UserTimer> GetUserReminders(ulong id)
        {
            if (UserTimers.TryGetValue(id, out var value))
                return value.Where(x => x.totalTime == TimeSpan.Zero).ToList();
            else
            {
                var userTimers = new List<UserTimer>();
                UserTimers.Add(id, userTimers);
                return userTimers;
            }
        }
    }

    public class UserTimer
    {
        public bool Global { get; set; }
        public string Reminder { get; set; }
        public string TotalTimeString { get; set; }
        public string RefreshTimeString { get; set; }
        public DateTime StopTime { get; set; }
        public ulong ChannelId { get; set; }

        public TimeSpan totalTime;
        public TimeSpan refreshTime;
        [JsonIgnore]
        public TimeSpan remainingTime { get => TimeSpan.FromSeconds(Math.Round((StopTime - DateTime.Now).TotalSeconds)); }
        public DiscordChannel channel;
        public DiscordMessage message;
        public Timer timer;

        public UserTimer() { }
        public UserTimer(ulong channelid, TimeSpan totaltime, TimeSpan refreshtime, string reminder = null, bool global = false)
        {
            ChannelId = channelid;
            TotalTimeString = totaltime.ToString();
            RefreshTimeString = refreshtime.ToString();
            StopTime = DateTime.Now + totaltime;
            Reminder = reminder;
            Global = global;
        }

        public string GetInfo() { throw new NotImplementedException(); }

        public async Task StartTimer(DiscordClient client, AutoResetEvent autoReset)
        {
            if (DateTime.Now > StopTime)
            {
                autoReset.Set();
                return;
            }

            totalTime = TimeSpan.Parse(TotalTimeString);
            refreshTime = TimeSpan.Parse(RefreshTimeString);

            var totalref = (StopTime - DateTime.Now).Ticks / refreshTime.Ticks;
            var time = refreshTime.Ticks * totalref;
            var delay = TimeSpan.FromTicks((StopTime - DateTime.Now).Ticks - time);

            channel = await client.GetChannelAsync(ChannelId);
            timer = new Timer(async stateInfo =>
            {
                var remaining = TimeSpan.FromSeconds(Math.Round((StopTime - DateTime.Now).TotalSeconds));
                if (DateTime.Now >= StopTime)
                {
                    await channel.SendMessageAsync(Reminder ?? $"Czas dobiegł końca! Timer był aktywny przez {totalTime}");
                    (stateInfo as AutoResetEvent).Set();
                }
                else if (message == null)
                    message = await channel.SendMessageAsync($"Pozostało: `{remaining}`");
                else await message.ModifyAsync($"Pozostało: `{remaining}`");
            }, autoReset, delay, refreshTime);
        }
    }
}
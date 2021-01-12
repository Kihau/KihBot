using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using KihBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    public partial class MainCommandModule : BaseCommandModule
    {

        [Group("timer"), Description("Komendy związane z timerami")]
        public class TimerCommandModule : BaseCommandModule
        {
            public TimerData Data { get; set; }

            [GroupCommand, Description("Wyświetla wybrany timer")]
            public async Task LastTimerCommand(CommandContext context, int index = 1)
            {
                index--;
                var userTimers = Data.GetGuildTimer(context.Guild.Id).GetUserTimers(context.User.Id);
                if (index >= 0 && userTimers.Count > index)
                {
                    var uTimer = userTimers[index];
                    var remaining = TimeSpan.FromSeconds(Math.Round((uTimer.StopTime - DateTime.Now).TotalSeconds));
                    if (uTimer.refreshTime > TimeSpan.Zero)
                    {
                        await uTimer.message.DeleteAsync();
                        if (uTimer.message.Reference.Message != null)
                        uTimer.message = await context.RespondAsync($"Pozostały czas: `{remaining}`");
                    }
                    else await context.RespondAsync($"Pozostały czas: `{remaining}`");
                }
                else await context.RespondAsync("Nie posiadasz żadnych timerów :c");
            }

            [Command("start"), Aliases("new", "add")]
            public async Task TimerCreateCommand(CommandContext context, TimeSpan totaltime, TimeSpan refreshtime, bool global = false, [RemainingText] string reminder = null)
            {
                if (totaltime == TimeSpan.Zero || 
                    (refreshtime != TimeSpan.FromSeconds(10) && refreshtime < TimeSpan.FromSeconds(10)))
                    return;

                var userTimers = Data.GetGuildTimer(context.Guild.Id).GetUserTimers(context.User.Id);
                var uTimer = new UserTimer(context.Channel.Id, totaltime, refreshtime, reminder, global);
                userTimers.Add(uTimer);

                await context.RespondAsync($"Ustawiono timer na {totaltime}");

                await Task.Run(async () =>
                {
                    AutoResetEvent autoReset = new AutoResetEvent(false);
                    await uTimer.StartTimer(context.Client, autoReset);

                    autoReset.WaitOne();
                    uTimer.timer.Dispose();
                    userTimers.Remove(uTimer);
                });
            }

            [Command("list")]
            public async Task TimerListCommand(CommandContext context, bool global = false)
            {
                var userTimers = Data.GetGuildTimer(context.Guild.Id).GetUserTimers(context.User.Id);
                if (userTimers.Count > 0)
                    for (int i = 0; i < userTimers.Count; i++)
                        await context.RespondAsync($"{i + 1}. Pozostały czas: {userTimers[i].remainingTime}");
                else await context.RespondAsync("Nie posiadasz żadnych timerów :c");
            }

            [Command("stop")]
            public async Task TimerRemoveCommand(CommandContext context, int number)
            {
                number--;
                var userTimers = Data.GetGuildTimer(context.Guild.Id).GetUserTimers(context.User.Id);
                if (number >= 0 & number < userTimers.Count)
                {
                    var uTimer = userTimers[number];
                    userTimers.Remove(uTimer);
                    await uTimer.timer.DisposeAsync();

                    await context.RespondAsync("Usunięto timer");
                }
            }

            [Command("stopall")]
            public async Task TimerRemoveAllCommand(CommandContext context)
            {
                var userTimers = Data.GetGuildTimer(context.Guild.Id).GetUserTimers(context.User.Id);
                foreach (var uTimer in userTimers)
                {
                    if (uTimer.timer != null)
                        await uTimer.timer.DisposeAsync();
                    userTimers.Remove(uTimer);
                }
                await context.RespondAsync("Usunięto wszystkie timery");
            }
        }

        [Group("reminder"), Description("Przypominacze wysyłające wiadomość po określonym czasie")]
        public class ReminderCommandModule : BaseCommandModule
        {

        }
    }
}

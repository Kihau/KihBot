using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using KihBot.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    [Description("Module containing fun and test commands")]
    public class FunCommandModule : BaseCommandModule
    {
        [Command("nice"), Description("NICE")]
        public async Task NiceCommand(CommandContext context) 
            => await context.RespondAsync("**NICE** :)");

        [Group("8ball"), Description("Magiczna kula przepowie ci przyszłość")]
        public class MagicBallCommandModule : BaseCommandModule
        {
            public FunData Data { get; set; }
            public ConfigData Config { get; set; }

            [GroupCommand]
            public async Task MagicBallCommand(CommandContext context, [RemainingText] string text = null)
            {
                Random rng = new Random();
                var embed = new DiscordEmbedBuilder().WithColor(Config.Color);

                if (Data.CustomAnswers.TryGetValue(context.Guild.Id, out var stringList))
                    embed.AddField("Magiczna kula mówi:", stringList[rng.Next(0, stringList.Count)]);
                else embed.AddField("O NIE!", "Nie posiadasz żadnych odpowiedzi :c");

                await context.RespondAsync(embed: embed.Build());
            }

            [Command("add"), Description("Dodaje nową odpowiedź magiczej kuli")]
            public async Task MagicBallAddCommand(CommandContext context, [Description("Treść nowej odpowiedzi"), RemainingText] string text)
            {
                if (Data.CustomAnswers.TryGetValue(context.Guild.Id, out var stringList)) 
                    stringList.Add(text);
                else Data.CustomAnswers.Add(context.Guild.Id, new List<string>() { text });

                await context.RespondAsync("Dodano odpowiedź");
            }

            [Command("removeall"), Description("Usuwa wszytski opdowiedzi magiczej kuli")]
            public async Task MagicBallRemoveAllCommand(CommandContext context)
            {
                if (Data.CustomAnswers.ContainsKey(context.Guild.Id))
                {
                    Data.CustomAnswers.Remove(context.Guild.Id);
                    await context.RespondAsync("Usunięto wszystkie odpowiedzi");
                } 
            }

            [Command("remove"), Description("Usuwa wybraną odpowiedź z listy (patrz: `8ball list`)")]
            public async Task MagicBallRemoveCommand(CommandContext context, [Description("Pozycja odpowiedzi z listy")] uint number)
            {
                var index = (int)--number;
                if (Data.CustomAnswers.TryGetValue(context.Guild.Id, out var stringList)
                     && index < stringList.Count && stringList.Count > 0)
                {
                    if (stringList.Count == 1)
                        Data.CustomAnswers.Remove(context.Guild.Id);
                    else stringList.RemoveAt(index);
                    await context.RespondAsync("Usunięto odpowiedź.");
                }
            }

            [Command("edit"), Description("Edytuje wybraną odpowiedź z listy (patrz: `8ball list`)")]
            public async Task MagicBallEditCommand(CommandContext context, [Description("Pozycja odpowiedzi z listy")] uint number, [Description("Treść nowej odpowiedzi"), RemainingText] string text)
            {
                var index = (int)--number;
                if (Data.CustomAnswers.TryGetValue(context.Guild.Id, out var stringList)
                     && index < stringList.Count && stringList.Count > 0)
                {
                    stringList[index] = text;
                    await context.RespondAsync("Edytowano odpowiedź.");
                }
            }

            [Command("list"), Description("Wyświtla listę odpowiedzi magicznej kuli")]
            public async Task MagicBallEditCommand(CommandContext context)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                string answers = "";

                if (Data.CustomAnswers.TryGetValue(context.Guild.Id, out var stringList))
                    for (int i = 0; i < stringList.Count; i++)
                        answers += $"{i + 1}. {stringList[i]}\n";
                else answers = "Nie masz żadnych odpowiedzi :c";

                embed.AddField("Twoje odpowiedzi", answers)
                    .WithColor(Config.Color);
                await context.RespondAsync(embed: embed.Build());
            }
        }

        [Command("test")]
        public async Task TestCommand(CommandContext context)
            => await context.RespondAsync("||~~__***TO JEST TEST***__~~||");     

        [Command("999")]
        public async Task AmbulanceCallComamnd(CommandContext context) 
            => await context.RespondAsync("**WEEEEEEUUUUUUU WEEEEEEUUUUU  KARETKA JUŻ JEDZIE**");      

        [Command("997")]
        public async Task PoliceCallComamnd(CommandContext context, DiscordUser user = null)
        {
            string name = user == null ? "" : user.Username;
            await context.RespondAsync($"**ZARAZ ZGARNIĘTY ZOSTANIE DELIKWENT {name} PRZEZ BAGIETY!**");
        }

        [Command("psy")]
        public async Task CopsCallComamnd(CommandContext context, DiscordUser user = null)
        {
            string name = user == null ? "" : user.Username ;
            await context.RespondAsync($"**PSY JUŻ JADO PO FRAJERA {name}**");
        }

        [Command("time")]
        public async Task TimerTestComamnd(CommandContext context, TimeSpan span)
        {
            var now = DateTime.Now - span;
            await context.RespondAsync(now.ToString());
        }

        [Command("string")]
        public async Task StringTestComamnd(CommandContext context, string span)
        {
            var now = DateTime.Now - TimeSpan.Parse(span);
            await context.RespondAsync(now.ToString());
        }
    }
}

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using KihBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    [Description("Module containing fun commads")]
    public class FunCommandModule : BaseCommandModule
    {
        [Command("nice")]
        public async Task NiceCommand(CommandContext context) 
            => await context.RespondAsync("**NICE** :)");

        [Group("8ball")]
        public class MagicBallCommandModule : BaseCommandModule
        {
            public FunData Data { get; set; }
            public ConfigData Config { get; set; }

            [GroupCommand]
            public async Task MagicBallCommand(CommandContext context, [RemainingText] string text)
            {
                Random rng = new Random();
                var embed = new DiscordEmbedBuilder()
                    .AddField("Magiczna kula mówi:", Data.BaseAnswers[rng.Next(0, Data.BaseAnswers.Length)])
                    .WithColor(Config.Color)
                    .Build();

                await context.RespondAsync(embed: embed);
            }

            [Command("add")]
            public async Task MagicBallAddCommand(CommandContext context, [RemainingText] string text)
            {
                if (Data.CustomAnswers.TryGetValue(context.Guild.Id, out var strings)) 
                    strings.Add(text);
                else Data.CustomAnswers.Add(context.Guild.Id, new List<string>() { text });
            }
        }

        [Command("test")]
        public async Task TestCommand(CommandContext context)
        {
            await context.RespondAsync("||~~__***TO JEST TEST***__~~||");
        }

        [Command("info")]
        public async Task GetGuildCommand(CommandContext context)
        {
            await context.RespondAsync($"ID serwera: {context.Guild.Id}");
        }
    }
}

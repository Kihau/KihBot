using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
            
        [Command("8ball")]
        public async Task MagicBallCommand(CommandContext context, [RemainingText]string text)
        {
            Random rng = new Random();
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

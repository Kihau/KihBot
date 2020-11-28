using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using KihBot.Database;

namespace KihBot.Modules
{
    public class TestModule : BaseCommandModule
    {
        public Configuration config { get; set; }

        [Command("test")]
        public async Task TestCommand(CommandContext context)
        {
            await context.RespondAsync("D#+ COMMAND EXECUTED");
        }

        [Command("info")]
        public async Task GetGuildCommand(CommandContext context)
        {
            await context.RespondAsync($"ID serwera: {context.Guild.Id}");
        }

        [Command("prefix")]
        public async Task SetPrefixCommand(CommandContext context, string prefix)
        {
            if (config.Prefixes.ContainsKey(context.Guild.Id))
                config.Prefixes[context.Guild.Id] = prefix;
            else config.Prefixes.Add(context.Guild.Id, prefix);

            await context.RespondAsync($"Ustawiono prefix na: `{prefix}`");
        }
    }
}

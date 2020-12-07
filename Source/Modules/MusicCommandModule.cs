using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    [Description("Module containing music commands")]
    public class MusicCommandModule : BaseCommandModule
    {
        [Command("join")]
        public async Task JoinChannelCommand(CommandContext context, DiscordChannel channel)
        {

        }
    }
}

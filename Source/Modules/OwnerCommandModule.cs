using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using KihBot.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    [RequireOwner, Hidden]
    public class OwnerCommandModule : BaseCommandModule
    {
        public DataSavingService Data { get; set; }

        [Command("stop")]
        public async Task StopBotCommand(CommandContext context)
        {
            await context.RespondAsync("Wyłącznie bota. . .");

            // Serialize as async
            Data.Serialize();

            await context.Client.DisconnectAsync();
            context.Client.Dispose();
        }

        [Command("wladd")]
        public async Task AddUserCommand(CommandContext context, DiscordUser user) { }


        [Command("wlremove")]
        public async Task RemoveUserCommand(CommandContext context, DiscordUser user) { }

        [Command("config"), RequireOwner, Hidden]
        public async Task DisplayBotConfigCommand(CommandContext context)
        {

        }
    }
}

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    [Description("Module contaning main commands")]
    public partial class MainCommandModule : BaseCommandModule
    {
        [Command("avatar")]
        public async Task DisplayAvatarCommand(CommandContext context, DiscordUser req_user = null, ushort? req_size = null)
        {
            var user = req_user ?? context.User;
            var size = req_size ?? 2048;
            await context.RespondAsync($"Awatar {user.Username}");
            await context.RespondAsync(user.GetAvatarUrl(DSharpPlus.ImageFormat.Auto, size));
        }

        [Command("emote")]
        public async Task DisplayEmoteCommand(CommandContext context, DiscordEmoji req_emoji) 
            => await context.RespondAsync(req_emoji.Url);
        
    }
}

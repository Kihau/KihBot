using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using KihBot.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DSharpPlus;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    [Description("Module contaning main commands")]
    public partial class MainCommandModule : BaseCommandModule
    {
        public ConfigData Config { get; set; }

        [Command("avatar")]
        public async Task DisplayAvatarCommand(CommandContext context, DiscordUser req_user = null, ushort? req_size = null)
        {
            var user = req_user ?? context.User;
            var size = req_size ?? 2048;
            var embed = new DiscordEmbedBuilder()
                .WithTitle($"Awatar {user.Username}")
                .WithImageUrl(user.GetAvatarUrl(ImageFormat.Auto, size))
                .WithFooter(Config.Version)
                .WithTimestamp(DateTime.UtcNow)
                .WithColor(Config.Color);
            await context.RespondAsync(embed: embed);
        }

        [Command("emote")]
        public async Task DisplayEmoteCommand(CommandContext context, DiscordEmoji req_emoji)
            => await context.RespondAsync(req_emoji.Url);
    }
}

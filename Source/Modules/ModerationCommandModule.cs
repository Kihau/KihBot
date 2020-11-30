using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using KihBot.Database;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace KihBot.Modules
{
    [Description("Module containing moderator commands"), RequirePermissions(Permissions.Administrator)]
    public class ModerationCommandModule : BaseCommandModule
    {
        public Configuration Config { get; set; }

        [Command("clear")]
        public async Task ClearMessagesCommand(CommandContext context, int amount)
        {
            var messages = (await context.Channel.GetMessagesAsync(amount + 1)).Where(x => (DateTime.Now - x.CreationTimestamp).TotalDays < 14);

            DiscordMessage message;
            var totaldeleted = messages.ToList().Count - 1;

            if (totaldeleted < 0)
            {
                await context.Channel.DeleteMessagesAsync(messages);
                message = await context.RespondAsync($"Usunięto `{totaldeleted}` wiadomości");
            }
            else message = await context.RespondAsync($"Nie mogę usuwać wiadomości starszych niż 14 dni :c");

            await Task.Delay(3000);
            await message.DeleteAsync();
        }

        [Command("prefix")]
        public async Task SetPrefixCommand(CommandContext context, string prefix)
        {
            if (Config.Prefixes.ContainsKey(context.Guild.Id))
            {
                if (prefix != Config.DefaultPrefix)
                    Config.Prefixes[context.Guild.Id] = prefix;
                else Config.Prefixes.Remove(context.Guild.Id);   // Remove unnecessary field
            }
            // Don't add default prefixes to the the dictionary to keep the config file clean
            else if (prefix != Config.DefaultPrefix) Config.Prefixes.Add(context.Guild.Id, prefix);

            await context.RespondAsync($"Ustawiono prefix na: `{prefix}`");
        }

        [Command("nuke")]
        public async Task NukeChannelAsync(CommandContext context, DiscordChannel req_channel = null)
        {
            var channel = req_channel ?? context.Channel;

            if (channel.Type == ChannelType.Text)
            {
                var confirm = new DiscordEmbedBuilder()
                    .WithTitle($"__***DETONACJA KANAŁU:***__ `{channel.Name}`")
                    .AddField($"Czy jesteś pewny, że chcesz nieodwracalnie wyczyścić ten kanał?",
                    ":white_check_mark: - TAK, chcę wyczyścić ten kanał\n :x: - NIE, rozmyśliłem się")
                    .WithColor(DiscordColor.Violet).Build();
                var message = await context.RespondAsync(embed: confirm);

                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":white_check_mark:"));
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":x:"));

                var result = await message.WaitForReactionAsync(context.User, TimeSpan.FromSeconds(10));

                if (!result.TimedOut && result.Result.Emoji == DiscordEmoji.FromName(context.Client, ":white_check_mark:"))
                {
                    List<DiscordOverwriteBuilder> overrites = new List<DiscordOverwriteBuilder>();
                    foreach (var overwrite in channel.PermissionOverwrites.AsEnumerable())
                        overrites.Add(await new DiscordOverwriteBuilder().FromAsync(overwrite));

                    await context.Guild.CreateChannelAsync(channel.Name, channel.Type, channel.Parent, channel.Topic, null, null, overrites, channel.IsNSFW, channel.PerUserRateLimit);
                    await channel.DeleteAsync();
                }
                await message.DeleteAsync();
            }
        }

        [Command("clone")]
        public async Task CloneChannelAsync(CommandContext context, string name, DiscordChannel req_channel)
        {
            var channel = req_channel ?? context.Channel;

            if (channel.Type == ChannelType.Text)
            {
                List<DiscordOverwriteBuilder> overrites = new List<DiscordOverwriteBuilder>();
                foreach (var overwrite in channel.PermissionOverwrites.AsEnumerable())
                    overrites.Add(await new DiscordOverwriteBuilder().FromAsync(overwrite));

                await context.Guild.CreateChannelAsync(name, channel.Type, channel.Parent, channel.Topic, null, null, overrites, channel.IsNSFW, channel.PerUserRateLimit);
            }
        }

        [Command("kick")]
        public async Task KickUserCommand(CommandContext context, DiscordMember req_user, [RemainingText] string reason = null)
            => await req_user.RemoveAsync(reason);


        [Command("ban")]
        public async Task BanUserCommand(CommandContext context, DiscordMember req_user, DateTime time, [RemainingText] string reason = null)
        {

        }

    }
}

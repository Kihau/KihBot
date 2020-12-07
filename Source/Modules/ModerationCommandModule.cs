using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using KihBot.Data;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace KihBot.Modules
{
    [Description("Module containing moderator commands"), RequirePermissions(Permissions.Administrator)]
    public class ModerationCommandModule : BaseCommandModule
    {
        public ConfigData Config { get; set; }

        [Command("clear"), Description("Czyści wybraną ilość wiadomości")]
        public async Task ClearMessagesCommand(CommandContext context, [Description("ilość waidomości do wyczyszczenia")] int amount)
        {
            var messages = (await context.Channel.GetMessagesAsync(amount + 1)).Where(x => (DateTime.Now - x.CreationTimestamp).TotalDays < 14);

            DiscordMessage message;
            var totaldeleted = messages.ToList().Count - 1;

            if (totaldeleted > 0)
            {
                await context.Channel.DeleteMessagesAsync(messages);
                message = await context.RespondAsync($"Usunięto `{totaldeleted}` wiadomości");
            }
            else message = await context.RespondAsync($"Nie mogę usuwać wiadomości starszych niż 14 dni :c");

            await Task.Delay(3000);
            await message.DeleteAsync();
        }

        [Command("prefix"), Description("Ustawia nowy prefix bota")]
        public async Task SetPrefixCommand(CommandContext context, [Description("Nowy prefix (np. \"`/`\"")] string prefix)
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

        [Command("nuke"), Description("Zupełnie czyści wybrany kanał")]
        public async Task NukeChannelAsync(CommandContext context, [Description("Nazwa kanłu (np. `#channel`)")] DiscordChannel req_channel = null)
        {
            var channel = req_channel ?? context.Channel;

            if (channel.Type == ChannelType.Text)
            {
                // Create comfirmation embed
                var confirm = new DiscordEmbedBuilder()
                    .WithTitle($"__***DETONACJA KANAŁU:***__ `{channel.Name}`")
                    .AddField($"Czy jesteś pewny, że chcesz nieodwracalnie wyczyścić ten kanał?",
                    ":white_check_mark: - TAK, chcę wyczyścić ten kanał\n :x: - NIE, rozmyśliłem się")
                    .WithColor(DiscordColor.Violet).Build();
                var message = await context.RespondAsync(embed: confirm);

                // Add yes and no reactions
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":white_check_mark:"));
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":x:"));

                var result = await message.WaitForReactionAsync(context.User, TimeSpan.FromSeconds(10));
                await message.DeleteAsync();

                if (!result.TimedOut && result.Result.Emoji == DiscordEmoji.FromName(context.Client, ":white_check_mark:"))
                {
                    await context.RespondAsync("__**TACTICAL NUKE INCOMING!**__\nhttps://tenor.com/view/explosion-mushroom-cloud-atomic-bomb-bomb-boom-gif-4464831");

                    await Task.Delay(1000);

                    await channel.CloneAsync();
                    await channel.DeleteAsync();
                }
            }
        }

        [Command("clone"), Description("Klonuje wybrany kanał")]
        public async Task CloneChannelAsync(CommandContext context, [Description("Nazwa nowego kanału")] string name, [Description("Kanał do sklonowania (np. `#channel`)")] DiscordChannel req_channel = null)
        {
            var channel = req_channel ?? context.Channel;
            var clone = await channel.CloneAsync();
            await clone.ModifyAsync(x => x.Name = name);
        }

        [Command("kick"), Description("Wyrzuca użytkowanika z serwera")]
        public async Task KickUserCommand(CommandContext context, [Description("Użytkownik do wyrzucenia (np. `@Kihau` lub `Kihau#3428`)")] DiscordMember req_user, [Description("Powód wyrzucenia"), RemainingText] string reason = null)
            => await req_user.RemoveAsync(reason);


        [Command("ban"), Description("Banuje użytkowanika z serwera")]
        public async Task BanUserCommand(CommandContext context, [Description("Użytkownik do zbanowania (np. `@Kihau` lub `Kihau#3428`)")] DiscordMember req_user, [Description("Czas zbanowania (np. `10h`, `10d5h`, `21:37`)")] DateTime time, [Description("Powód zbanowania"), RemainingText] string reason = null)
        {

        }

    }
}

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

        [Command("yo")]
        public async Task YoCommand(CommandContext context)
        {
            var message = (await context.Channel.GetMessagesAsync(2)).First();
            var time = DateTime.Now - message.CreationTimestamp;
            await context.RespondAsync(time.TotalSeconds.ToString());
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
            // Don't add default prefixes to the the dictionary to keep the config clean
            else if (prefix != Config.DefaultPrefix) Config.Prefixes.Add(context.Guild.Id, prefix);

            await context.RespondAsync($"Ustawiono prefix na: `{prefix}`");
        }
    }
}

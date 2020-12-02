using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using KihBot.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    [RequireOwner, Hidden]
    public class OwnerCommandModule : BaseCommandModule
    {
        public DataService Data { get; set; }
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


        [Group("data")]
        public class DataCommandModule : BaseCommandModule
        {
            public DataService Data { get; set; }
            [GroupCommand]
            public async Task GetDataCommand(CommandContext context)
            {
                var files = new Dictionary<string, Stream>();

                Data.Serialize();
                files.Add("config.json", File.OpenRead("config.json"));
                files.Add("fundata.json", File.OpenRead("fundata.json"));

                var message = await context.RespondWithFilesAsync(files);
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":x:"));

                await message.WaitForReactionAsync(context.User, DiscordEmoji.FromName(context.Client, ":x:"));
                foreach (var file in files)
                    file.Value.Close();
                await message.DeleteAsync();
            }

            [Command("load")]
            public async Task LoadDataCommand(CommandContext context)
            {
                var attached = context.Message.Attachments.ToList();
                using (var webclient = new WebClient())
                    foreach (var file in attached)
                        webclient.DownloadFile(file.Url, file.FileName);
                await context.Message.DeleteAsync();
                await context.RespondAsync("Załadowano dane");
            }

            [Command("save")]
            public async Task SaveDataCommand(CommandContext context)
            {

            }


            [Command("reload")]
            public async Task ReloadDataCommand(CommandContext context)
            {

            }
        }
    }
}

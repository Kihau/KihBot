using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using KihBot.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            Data.SerializeData();
            await context.Client.DisconnectAsync();
        }

        [Command("wladd")]
        public async Task AddUserCommand(CommandContext context, DiscordUser user) { }


        [Command("wlremove")]
        public async Task RemoveUserCommand(CommandContext context, DiscordUser user) { }


        [Group("data")]
        public class DataCommandModule : BaseCommandModule
        {
            public DataService Data { get; set; }
            [Command("get"), Description("Sendas all data files")]
            public async Task GetDataCommand(CommandContext context, string name = null)
            {
                var files = new Dictionary<string, Stream>();

                Data.SerializeData();
                // check if name is not null and get single file
                Data.DataList.ForEach(x => files.Add(x.FileName, File.OpenRead(x.FileName)));

                var message = await context.RespondWithFilesAsync(files);
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":x:"));

                await message.WaitForReactionAsync(context.User, DiscordEmoji.FromName(context.Client, ":x:"));
                foreach (var file in files)
                    file.Value.Close();
                await message.DeleteAsync();
            }

            [Command("set"), Description("Downloads and sets new data files")]
            public async Task SetDataCommand(CommandContext context)
            {
                var attached = context.Message.Attachments.ToList();
                if (attached.Count > 0)
                {
                    using (var webclient = new WebClient())
                        foreach (var file in attached)
                            webclient.DownloadFile(file.Url, file.FileName);

                    await context.Message.DeleteAsync();
                    await context.RespondAsync("Pobrano pliki z danymi");
                }
                else await context.RespondAsync("Nie załączono plików z danymi");
            }

            [Command("load"), Description("Deserializes and loads the data")]
            public async Task LoadDataCommand(CommandContext context)
            {
                Data.DeserializeData();
                await context.RespondAsync("Załadowano dane");
            }

            [Command("save"), Description("Serializes data")]
            public async Task SaveDataCommand(CommandContext context)
            {
                Data.SerializeData();
                await context.RespondAsync("Zapisano dane");
            }
        }
    }
}

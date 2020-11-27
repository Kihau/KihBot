using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KihBot
{
    public class DiscordBot
    {
        private DiscordClient Clinet { get; set; }

        public DiscordBot()
        {
            ConfigureClient();
            var services = ConfigureServices();
            ConfigureCommands(services);
        }

        public async Task RunAsync()
        {
            DiscordActivity activity = new DiscordActivity("Hollow Knight Silksong Closed Beta", ActivityType.Playing);
            await Clinet.ConnectAsync(activity, UserStatus.DoNotDisturb, DateTimeOffset.Now);
            await Task.Delay(Timeout.Infinite);
        }

        private void ConfigureClient()
        {
            var config = new DiscordConfiguration()
            {
                AutoReconnect = true,
                Intents = DiscordIntents.All,
                Token = File.ReadAllText("thisisaverylongtextfilenamethatcontainsdiscordbottokenthatisusedtologin.txt"),
                TokenType = TokenType.Bot,           
            };
            Clinet = new DiscordClient(config);
        }

        private void ConfigureCommands(IServiceProvider services)
        {
            var config = new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "/" },
                DmHelp = false,
                EnableDefaultHelp = true,
                EnableMentionPrefix = false,
                EnableDms = false,
                Services = services
            };

            var commands = Clinet.UseCommandsNext(config);
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
        }

        private IServiceProvider ConfigureServices()
        {
            return null;
        }
    }
}

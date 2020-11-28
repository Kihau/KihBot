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
        private Dictionary<ulong, string> prefixes;  // Get config class/service/something here (BotConfiguration /w dependency injection)

        public DiscordBot()
        {
            prefixes = new Dictionary<ulong, string>   // Delete
            {
                // Casement
                { 519857141494841344, "/" },
                // Schowek
                { 539093667365912576, "hey" }
            };

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
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                Token = File.ReadAllText("thisisaverylongtextfilenamethatcontainsdiscordbottokenthatisusedtologin.txt"),
                TokenType = TokenType.Bot,           
            };
            Clinet = new DiscordClient(config);
        }

        private void ConfigureCommands(IServiceProvider services)
        {
            var resolver = new PrefixResolverDelegate(async(message) =>
            {             
                var Id = message.Channel.GuildId;                                     // Get the Id of the server
                if (prefixes.TryGetValue(Id, out string prefix))                      // Check if dictionary contains Id 
                    return message.Content.StartsWith(prefix) ? prefix.Length : -1;   // Check if message starts with the prefix 
                else if (message.Content.StartsWith("/")) return 1;                   // Check if message starts with a default prefix "/"
                else return message.Content == "/help" ? 1 : -1;                      // Check if message is a default help command
            });

            var config = new CommandsNextConfiguration()
            {
                //StringPrefixes = new[] { "/" },
                PrefixResolver = resolver,
                DmHelp = false,
                EnableDefaultHelp = false,
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

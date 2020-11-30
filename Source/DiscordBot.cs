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
using KihBot.Data;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using KihBot.Services;

namespace KihBot
{
    public class DiscordBot
    {
        private DiscordClient Client { get; set; }
        private ConfigData Config { get; set; }
        private DataSavingService Data { get; set; }

        public DiscordBot()
        {
            var services = ConfigureServices();
            LoadData(services);

            ConfigureClient();
            ConfigureCommands(services); 
        }

        public async Task RunAsync()
        {
            DiscordActivity activity = new DiscordActivity("Hollow Knight Silksong Closed Beta", ActivityType.Playing);
            await Client.ConnectAsync(activity, UserStatus.DoNotDisturb, DateTimeOffset.Now);
            await Task.Delay(Timeout.Infinite);
        }

        private void LoadData(IServiceProvider services)
        {
            Data = services.GetService<DataSavingService>();
            Data.Deserialize();
            Config = (ConfigData)services.GetService<List<IData>>().Single(x => x.GetType() == typeof(ConfigData));

            // Clear out config from unnecessary stuff
            Config.Prefixes.Where(x => x.Value == Config.DefaultPrefix)
                .Select(x=>x.Key).ToList().ForEach(x => Config.Prefixes.Remove(x));
        }

        private void ConfigureClient()
        {
            var clientconfig = new DiscordConfiguration()
            {
                AutoReconnect = true,
                Intents = DiscordIntents.All,
                LogTimestampFormat = "HH:mm:ss",
                Token = Config.Token,
                TokenType = TokenType.Bot,           
            };

            Client = new DiscordClient(clientconfig);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromSeconds(30)
            });

        }

        private void ConfigureCommands(IServiceProvider services)
        {
            var resolver = new PrefixResolverDelegate(async (message) =>
            {
                var guildId = message.Channel.GuildId;                              // Get the Id of the server
                if (Config.Prefixes.TryGetValue(guildId, out string prefix))        // Check if dictionary contains Id 
                    return message.Content.StartsWith(prefix) ? prefix.Length : -1; // Check if message starts with the prefix 
                else if (message.Content.StartsWith(Config.DefaultPrefix))          // Check if message starts with a default prefix "/"
                    return Config.DefaultPrefix.Length;                             // Return length to skip fist characters of a message
                else return message.Content == Config.DefaultCommand ? 1 : -1;      // Check if message is a default help command
            });

            var commandconfig = new CommandsNextConfiguration()
            {
                //StringPrefixes = new[] { "/" },               
                PrefixResolver = resolver,                
                DmHelp = false,
                EnableDefaultHelp = true,
                EnableMentionPrefix = false,
                EnableDms = false,
                // Create custom event handler
                Services = services
            };

            var commands = Client.UseCommandsNext(commandconfig);
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
        }

        private IServiceProvider ConfigureServices()
        {
            List<IData> DataList = new List<IData>
            {
                new ConfigData(),
                new FunData()
            };

            return new ServiceCollection()
                .AddSingleton<List<IData>>(DataList)
                .AddSingleton<DataSavingService>()
                .BuildServiceProvider();
        }
    }
}

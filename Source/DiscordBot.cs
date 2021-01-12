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
using System.Text.Json;

namespace KihBot
{
    public class DiscordBot
    {
        private DiscordClient Client { get; set; }
        private ConfigData Config { get; set; }
        private DataService Data { get; set; }

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
            await Data.StartTimers(Client);

            string exit = null;
            do
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    Console.Write("Do you want to close the bot? [y/n]: ");
                    exit = Console.ReadLine();
                }
            } while (exit != "y");

            await Client.DisconnectAsync();
            Client.Dispose();

            Console.Write("Do you want to save the data? [y/n]: ");
            if (Console.ReadLine() == "y")
                Data.SerializeData();
        }

        private void LoadData(IServiceProvider services)
        {
            Data = services.GetRequiredService<DataService>();
            Data.DeserializeData();
            Config = services.GetRequiredService<ConfigData>();
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
                //EnableDefaultHelp = false,             
                EnableMentionPrefix = false,
                EnableDms = false,
                // Create custom command handler
                Services = services
            };

            var commands = Client.UseCommandsNext(commandconfig);
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
            commands.SetHelpFormatter<KihbotHelpFormatter>();
        }

        private IServiceProvider ConfigureServices()
        {
            var collection = new ServiceCollection()
                .AddSingleton<ConfigData>()
                .AddSingleton<FunData>()
                .AddSingleton<TimerData>()
                .AddSingleton<DataService>();
            
            return collection.BuildServiceProvider();
        }
    }
}
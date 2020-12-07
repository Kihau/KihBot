using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using KihBot.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Services
{
    public class KihbotHelpFormatter : BaseHelpFormatter
    {
        private DiscordEmbedBuilder embed;
        private ConfigData Config { get; set; }
        private string Prefix { get; set; }

        public KihbotHelpFormatter(CommandContext context, ConfigData config) : base(context)
        {
            Config = config;

            if (Config.Prefixes.TryGetValue(context.Guild.Id, out string prefix))
                this.Prefix = prefix;
            else this.Prefix = Config.DefaultPrefix;
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            string arg_string = null;

            foreach (var arg in command.Overloads.First().Arguments)
            {
                if (arg.IsOptional) arg_string += $"`[{arg.Name}]`";
                else arg_string += $"`({arg.Name})`";

                arg_string += $" - {arg.Description ?? "Nie podano opisu argumentu"}\n";
            }

            embed = new DiscordEmbedBuilder()
                .WithTitle($"__**Help**__ `{Prefix}{command.QualifiedName}`")
                .AddField("Jak używać?", "`(arg)` - argument wymagany\n`[arg]` - argument opcjonaly\n")
                .AddField("Opis komendy", command.Description ?? "Brak opisu komendy")
                .AddField("Argumenty", arg_string ?? "Komenda nie posiada argumentow")
                .WithColor(Config.Color);

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
        {
            embed = new DiscordEmbedBuilder()
                .WithTitle(Config.Version)
                .WithThumbnail("https://cdn.discordapp.com/avatars/773682218538106903/31c3083da9324c8933ac023e6179aee8.png?size=64")
                .AddField("Informacje", $"Autor bota: `Kihau#3428`\nAktualny prefix: \"`{Prefix}`\"")
                .AddField("Jak używać?", $"Informacje o komendzie: \"`{Prefix}help (nazwa komendy)`\"\n" +
                $"Domyślna komenda help: \"`{Config.DefaultPrefix}help`\"")
                .WithTimestamp(DateTime.UtcNow)
                .WithFooter("Help")
                .WithColor(Config.Color);

            string commands = "";
            foreach (var cmd in cmds)
                commands += $"`{cmd.QualifiedName}`, ";
            embed.AddField("Dostępne komendy", commands);

            return this;
        }

        public override CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: embed.Build());
        }
    }
}

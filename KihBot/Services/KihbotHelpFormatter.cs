using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Services
{
    class KihbotHelpFormatter : BaseHelpFormatter
    {
        // protected DiscordEmbedBuilder _embed;
        // protected StringBuilder _strBuilder;

        public KihbotHelpFormatter(CommandContext ctx) : base(ctx)
        {
            // _embed = new DiscordEmbedBuilder();
            // _strBuilder = new StringBuilder();

            // Help formatters do support dependency injection.
            // Any required services can be specified by declaring constructor parameters. 

            // Other required initialization here ...
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            // _embed.AddField(command.Name, command.Description);            
            // _strBuilder.AppendLine($"{command.Name} - {command.Description}");

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
        {
            foreach (var cmd in cmds)
            {
                // _embed.AddField(cmd.Name, cmd.Description);            
                // _strBuilder.AppendLine($"{cmd.Name} - {cmd.Description}");
            }

            return this;
        }

        public override CommandHelpMessage Build()
        {
            throw new NotImplementedException();
            // return new CommandHelpMessage(embed: _embed);
            // return new CommandHelpMessage(content: _strBuilder.ToString());
        }
    }
}

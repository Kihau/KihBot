using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Services
{
    public class KihbotHelpFormatter : BaseHelpFormatter
    {
        private DiscordEmbed embed;

        public KihbotHelpFormatter(CommandContext context) : base(context)
        {

            // Help formatters do support dependency injection.
            // Any required services can be specified by declaring constructor parameters. 

            // Other required initialization here ...
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            embed = new DiscordEmbedBuilder().AddField(command.Name, command.Description).Build();            

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
        {
            foreach (var cmd in cmds)
            {
                         
            }

            return this;
        }

        public override CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: embed);
        }
    }
}

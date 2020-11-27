using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;

namespace KihBot.Modules
{
    class TestModule : BaseCommandModule
    {
        [Command("test")]
        public async Task TestCommand(CommandContext context)
        {
            await context.RespondAsync("D#+ COMMAND EXECUTED");
        }
    }
}

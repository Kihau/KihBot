using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihBot.Modules
{
    public partial class MainCommandModule : BaseCommandModule
    {
        [Group("timer"), Description("Module containing timer commands")]
        public class TimerCommandModule : BaseCommandModule
        {
            [Command("new")]
            public async Task TimerCreateCommand(TimeSpan time, TimeSpan refreshtime, string reminder = null, bool global = false)
            {

            }

            [Command("list")]
            public async Task TimerListCommand(bool global = false)
            {

            }

            [Command("remove")]
            public async Task TimerRemoveCommand(int number)
            {

            }

            [Command("removeall")]
            public async Task TimerRemoveAllCommand()
            {

            }
        }
    }
}

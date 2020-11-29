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
        [Aliases("timer")]
        [Description("Module containing timer commands")]
        public class TimerCommandModule : BaseCommandModule
        {

        }
    }
}

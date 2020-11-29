using System;

namespace KihBot
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordBot kihBot = new DiscordBot();
            kihBot.RunAsync().GetAwaiter().GetResult();
        }
    }
}

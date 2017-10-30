using System;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Extensions.Client;
using TwitchLib.Extensions.API.v5;
namespace TwitchBotAlpha
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Contributers : jamesdudley1129@gmail.com , saidmetiche2000@gmail.com");
            Console.ForegroundColor = ConsoleColor.Green;
            TwitchBot bot = new TwitchBot();
            bot.Connect();
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    
                    BitRouletteGame.automatedGame.Abort();
                    bot.EventDocumentationThread.Abort();
                    BitRouletteGame.automatedReminders.Abort();
                    break;
                }
            }
            bot.Disconnect();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchBotAlpha
{
    static class BitRouletteGame
    {
        private static bool gameRunning = false;
        public static bool gameActiveReadonly = gameRunning;
        public static long pool = 0;
        public static List<UserProfile> contestants = new List<UserProfile>();
        public static Thread automatedGame = new Thread(new ThreadStart(AutoEnd));
        public static Thread automatedReminders = new Thread(new ThreadStart(Reminder));
        public static void NewGame(bool AutoEND)
        {
            if (gameRunning == false) {
                gameRunning = true;
                gameActiveReadonly = gameRunning;
                string msg = "New game started type Cheer<ammount> to enter BitRoulette Giveaways {Disclaimer-IS FOR TESTING ONLY-There is no rewards atm!}";
                TwitchBot.twitchClient.SendMessage(msg);
                automatedGame.Start();
                automatedReminders.Start();
            }
            else
            {
                string msg = "Game Already running! Type '!bitroulette /e' to force the game to end early!";
                TwitchBot.twitchClient.SendMessage(msg);
            }
            //create a event like >> ChatProssesor.Cheer += OnCheerProsses;
        }
        public static void AutoEnd()
        {
            while (true)
            {
                Thread.Sleep(5000);
                if (pool >= 1000)
                {
                    
                    GameOver();
                    break;
                }
            }
        }
        public static void Reminder()
        {
            while (true)
            {
                Thread.Sleep(60000 * 20);
                TwitchBot.twitchClient.SendMessage("A game is running. Type Cheer<ammount> to enter BitRoulette Giveaways {Disclaimer-IS FOR TESTING ONLY-There is no rewards atm!");
            }
        }

        public static void NewEntry(UserProfile user, long ammount)
        {
            string msg;
            if (gameRunning == true)//can enter givaway twice
            {
                if (contestants.Count != 0)
                {


                    if (!contestants.Contains(user))
                    {

                        contestants.Add(user);
                        pool += ammount;
                        msg = "You have been entered into a giveaway! Thanks for the cheer of :" + ammount + " {Disclaimer-IS FOR TESTING ONLY-There is no rewards atm!} @" + user.follower.User.DisplayName;
                    }
                    else
                    {
                        pool += ammount;
                        msg = "Thanks for the bits its been added to the pool! @" + user.follower.User.DisplayName;
                    }
                }
                else
                {

                    contestants.Add(user);
                    pool += ammount;
                    msg = "You have been entered into a giveaway! Thanks for the cheer of :" + ammount + " {Disclaimer-IS FOR TESTING ONLY-There is no rewards atm!} @" + user.follower.User.DisplayName;
                }
            }
            else
            {
                msg = "How did you get this error please report it via wisper to @james_doesgames.";
            }
            TwitchBot.twitchClient.SendMessage(msg);
        }

        public static void GameOver()
        {
            string msg ="";
            UserProfile winnerName;
            PrizeType prize;
            Random x = new Random();
            int winnerIndex = x.Next(0, contestants.Count);

            if (pool > 1000)
            {
                //want to do based on chances in direct relationshop to ammount donated
                winnerName = contestants[winnerIndex];
                prize = PrizeType.Giveway;
                long xpWon = contestants.Count * 100;
                Winner winner = new Winner(winnerName, ContestType.BitRouletteGame, prize, xpWon);
            }
            else
            {
                try
                {
                    prize = PrizeType.XP;
                    winnerName = contestants[winnerIndex];
                    long xpWon = contestants.Count / 2 * 100;
                    msg = "Congratulations YOU WIN! @" + winnerName.follower.User.DisplayName + ".You won: " + prize.ToString() + "!";
                    TwitchBot.contestEventDocumentation.NewWinnerOfNewContest(new Winner(winnerName, ContestType.BitRouletteGame, prize, xpWon));
                }
                catch (ArgumentOutOfRangeException)
                { 
                    msg = "GameOver There were not entrys!";
                }
            }
            gameRunning = false;
            gameActiveReadonly = gameRunning;
            TwitchBot.twitchClient.SendMessage(msg);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("*BitRoulette GAME OVER*");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}

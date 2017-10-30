using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace TwitchBotAlpha
{
     public class EventFile
    {
        public EventFile()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Load();
        }
        string FolderName = APIResources.channel.DisplayName + "Channel";
        string filepath;
        public void FileSave()
        {
            filepath = @"C:\Users\james dudley\Documents\Visual Studio 2017\Projects\TwitchBotAlpha\TwitchBotAlpha\BotData\ChannelData\";
            string fileContestEventDocumentation = @"\ContestEventDocumentation.txt";
            string eventString = "";
            while (true)
            {
                Thread.Sleep(60000);
                Console.WriteLine("Saveing to '" + filepath + FolderName + fileContestEventDocumentation + "'");
                StreamWriter EventFile = new StreamWriter(filepath + FolderName + fileContestEventDocumentation);

                foreach (Winner winner in TwitchBot.contestEventDocumentation.winners)
                {
                    eventString += " ";
                    eventString += winner.thisContest.contestType;
                    eventString += " ";
                    eventString += winner.prize;
                    eventString += " ";
                    eventString += winner.xpWon;
                    eventString += " ";
                    eventString += winner.thisContest.dateTime;
                    eventString += " ";
                    eventString += winner.thisContest.payedOut;
                    eventString += " ";
                    eventString += winner.profile.follower.User.DisplayName;
                    eventString += " ";
                    eventString += winner.profile.xp;
                    eventString += " ";
                    eventString += winner.profile.contestWon[0]/*foreach contest won print*/;
                    eventString += "/";
                }
                EventFile.Write(eventString);

                Console.ForegroundColor = ConsoleColor.Green;
                EventFile.Close();
            }
        }
        public void Load()
        {
            filepath = @"C:\Users\james dudley\Documents\Visual Studio 2017\Projects\TwitchBotAlpha\TwitchBotAlpha\BotData\ChannelData\";
            string fileContestEventDocumentation = @"\ContestEventDocumentation.txt";
            List<string> eventComponents = new List<string>();
            List<List<string>> Events = new List<List<string>>();
            StreamReader EventFile = new StreamReader(filepath + FolderName + fileContestEventDocumentation);

            foreach (string x in EventFile.ReadToEnd().Split(' '))
            {
                eventComponents.Add(x);
                foreach (string y in EventFile.ReadToEnd().Split('/'))
                {
                    Events.Add(eventComponents);
                    eventComponents.Clear();
                }
            }

            Winner winner;
            UserProfile userProfile;
            ContestType contestType;
            PrizeType prizeType;
            long xp;
            foreach (List<string> x in Events)
            {

                foreach (string y in x)
                {
                    switch (eventComponents.IndexOf(y))
                    {

                        case 0:
                            if (PrizeType.Giveway.ToString() == y)
                            {
                                prizeType = PrizeType.Giveway;
                            }
                            if (PrizeType.XP.ToString() == y)
                            {
                                prizeType = PrizeType.XP;
                            }
                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:

                            break;
                        case 5:

                            break;
                        case 6:

                            break;
                        case 7:

                            break;
                    }
                }



            }
        }

    }

}


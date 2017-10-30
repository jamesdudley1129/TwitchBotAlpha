using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using TwitchLib.Models.API.v5.Channels;

namespace TwitchBotAlpha
{
    public class ChannalResources
    {
        public List<UserProfile> profiles = new List<UserProfile>();
        public List<ChannelFollow> followers = new List<ChannelFollow>();
        public List<UserProfile> moderators = new List<UserProfile>();
        //#VARIABLES#//
        
        //#CONSTRUCTORS#//
        public ChannalResources()
        {
            CreateAllUserProfiles();


        }
        //#METHODS#//

        public void GetEventDocumentation(string requester)//retrives documentation
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            string msg = "Error retriveing doc.";
            try
            {
                foreach (Winner winner in TwitchBot.contestEventDocumentation.winners)
                {
                    msg += "dateTime :" + winner.thisContest.dateTime + " :: profile :" + winner.profile.follower.User.DisplayName + " :: Contest :" + winner.thisContest.contestType + ":: prize :" + winner.prize + ":: payed out :" + winner.thisContest.payedOut + ":: xp Gain:" + winner.xpWon + "\n";
                    Console.WriteLine("dateTime :" + winner.thisContest.dateTime + " :: profile :" + winner.profile.follower.User.DisplayName + " :: Contest :" + winner.thisContest.contestType + ":: prize :" + winner.prize + ":: payed out :" + winner.thisContest.payedOut + ":: xp Gain:" + winner.xpWon);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }
            TwitchBot.twitchClient.SendWhisper(requester, msg);
            TwitchBot.twitchClient.SendMessage(msg);
            Console.WriteLine("Request finnished!");
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public void ClearEventDocumentation(List<Winner> winners)
        {
            winners.Clear();
        }

        public void CreateAllUserProfiles()
        {
            APIResources.FollowersAsync().Wait();
            followers = APIResources.followersList;
            foreach (ChannelFollow follower in followers)
            {
                UserProfile user = new UserProfile(follower);
                profiles.Add(user);
            }
        }
        public void GetAllUserProfiles()
        {
            string msg = "";
            profiles = profiles.OrderBy(o => o.follower.User.DisplayName).ToList();
            foreach (UserProfile follower in profiles)
            {
                msg += "dateTime :" + follower.follower.CreatedAt + " :: profile :" + follower.follower.User.DisplayName;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("dateTime :" + follower.follower.CreatedAt + " :: profile :" + follower.follower.User.DisplayName);
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
        //write to file

        //creat new file

        //create file dir

        //read from file

        //get winners

    }



}

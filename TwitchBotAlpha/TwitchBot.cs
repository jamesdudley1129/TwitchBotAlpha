using System;
//using System.Net.Sockets;
using System.Threading;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Models.API.v5;
//using TwitchLib.Events.Client;
//using TwitchLib.Extensions.Client;
//using TwitchLib.Extensions.API.v5;
//using TwitchLib.Internal.TwitchAPI;
//using TwitchLib.Models.API.Undocumented.Chatters;
//using System.Collections.Generic;
using TwitchLib.Models.API.v5.Channels;
using System.Collections.Generic;
using TwitchLib.Models.API.v5.Subscriptions;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TwitchLib.Models.API.v5.Streams;
using TwitchLib.Models.API.Undocumented.Chatters;

namespace TwitchBotAlpha
{

    public class TwitchBot
    {
        //private channel resourses
        private ConnectionCredentials connectionCredentials = new ConnectionCredentials(Credentials.Bot_Username, Credentials.BotToken, "irc-ws.chat.twitch.tv", 80);

        //other parts of the programs libary
        public ChannalResources channalResources;
        public static ContestEventDocumentation contestEventDocumentation = new ContestEventDocumentation();
        public static TwitchClient twitchClient;
        public EventFile eventFile;
        public Thread EventDocumentationThread;

        ChannelAuthed channel;
        public void Connect()
        {
            //Text to explain the status of the client 
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Connecting...");
            Console.ForegroundColor = ConsoleColor.Green;
            
            //creates a new TwitchClient
            twitchClient = new TwitchClient(connectionCredentials, Credentials.Channel_Name, logging: true);
            
            //set settings for twitch client
            twitchClient.OverrideBeingHostedCheck = true;
            TwitchAPI.Settings.Validators.SkipAccessTokenValidation = true;
            TwitchAPI.Settings.Validators.SkipClientIdValidation = true;
            TwitchAPI.Settings.Validators.SkipDynamicScopeValidation = true;
            TwitchAPI.Settings.ClientId = Credentials.ClientID;
            TwitchAPI.Settings.AccessToken = Credentials.AccessToken_OfChannel;
            TwitchAPI.Settings.Scopes.Add(TwitchLib.Enums.AuthScopes.Channel_Read);

            //Events and triggers NEEDS TO CHANGE TO SEPRATE FILE FOR TWITCHLIB NATIVE EVENTS AND TRIGGERS
            #region EventLinkage
            twitchClient.OnLog += TwitchClient_OnLog;
            twitchClient.OnBeingHosted += TwitchClient_OnBeingHosted;
            twitchClient.OnChannelStateChanged += TwitchClient_OnChannelStateChanged;
            twitchClient.OnChatCleared += TwitchClient_OnChatCleared;
            twitchClient.OnChatColorChanged += TwitchClient_OnChatColorChanged;
            twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
            twitchClient.OnConnected += TwitchClient_OnConnected;
            twitchClient.OnConnectionError += TwitchClient_OnConnectionError;
            twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
            twitchClient.OnExistingUsersDetected += TwitchClient_OnExistingUsersDetected;
            twitchClient.OnHostingStarted += TwitchClient_OnHostingStarted;
            twitchClient.OnHostingStopped += TwitchClient_OnHostingStopped;
            twitchClient.OnHostLeft += TwitchClient_OnHostLeft;
            twitchClient.OnIncorrectLogin += TwitchClient_OnIncorrectLogin;
            twitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
            twitchClient.OnLeftChannel += TwitchClient_OnLeftChannel;
            twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            twitchClient.OnMessageSent += TwitchClient_OnMessageSent;
            twitchClient.OnModeratorJoined += TwitchClient_OnModeratorJoined;
            twitchClient.OnModeratorLeft += TwitchClient_OnModeratorLeft;
            twitchClient.OnModeratorsReceived += TwitchClient_OnModeratorsReceived;
            twitchClient.OnNewSubscriber += TwitchClient_OnNewSubscriber;
            twitchClient.OnNowHosting += TwitchClient_OnNowHosting;
            twitchClient.OnReSubscriber += TwitchClient_OnReSubscriber;
            twitchClient.OnSendReceiveData += TwitchClient_OnSendReceiveData;
            twitchClient.OnUserBanned += TwitchClient_OnUserBanned;
            twitchClient.OnUserJoined += TwitchClient_OnUserJoined;
            twitchClient.OnUserLeft += TwitchClient_OnUserLeft;
            twitchClient.OnUserStateChanged += TwitchClient_OnUserStateChanged;
            twitchClient.OnUserTimedout += TwitchClient_OnUserTimedout;
            twitchClient.OnWhisperCommandReceived += TwitchClient_OnWhisperCommandReceived;
            twitchClient.OnWhisperReceived += TwitchClient_OnWhisperReceived;
            twitchClient.OnWhisperSent += TwitchClient_OnWhisperSent;
            #endregion

            //Connects the TwitchClient
            twitchClient.Connect();
            
            
            //text to make it easyier to read the Console
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("______________________________________________________________\n______________________________________________________________\n______________________________________________________________");
            Console.WriteLine(twitchClient.IsConnected);
            Console.ForegroundColor = ConsoleColor.Green;
            
            //Sets Local Values
            channalResources = new ChannalResources();
            APIResources.GetChannelID().Wait();            
            APIResources.Stream().Wait();
            

            
            
            
            channel = APIResources.channel;
            Console.WriteLine("You are connected to :" + channel.DisplayName);
            eventFile = new EventFile();
            //EventDocumentationThread.Start();
            //EventDocumentationThread = new Thread(new ThreadStart(eventFile.FileSave));
        
        }

        private void TwitchClient_OnWhisperSent(object sender, TwitchLib.Events.Client.OnWhisperSentArgs e)
        {

        }

        private void TwitchClient_OnWhisperReceived(object sender, TwitchLib.Events.Client.OnWhisperReceivedArgs e)
        {
            bool IsMod = false;
            APIResources.Chatters().Wait();
            foreach (var x in APIResources.chatters)
            {
                if (x.Username == e.WhisperMessage.Username)
                {
                    if (x.UserType == TwitchLib.Enums.UserType.Moderator ||x.UserType == TwitchLib.Enums.UserType.Broadcaster)
                    {
                        IsMod = true;
                        break;
                    }

                }
            }
            Console.WriteLine("is mod :" + IsMod);
            if (IsMod == true)
            {
                Console.WriteLine("Private incomeing transmision..." + e.WhisperMessage.Username + " : " + e.WhisperMessage.Message);
                if (e.WhisperMessage.Message.StartsWith("!"))
                {
                    Console.WriteLine(" IScommand = true");

                    if (e.WhisperMessage.Message.ToLower() == "!geteventdocumentation")
                    {
                        channalResources.GetEventDocumentation(e.WhisperMessage.DisplayName);
                    }
                    if (e.WhisperMessage.Message.ToLower() == "!geteventdocumentation /c")
                    {
                        channalResources.ClearEventDocumentation(contestEventDocumentation.winners);
                    }
                }
            }
            if (e.WhisperMessage.Message.ToLower() == "!help")
            {
                if (e.WhisperMessage.Message.ToLower().Contains("/stats"))
                {
                    //spacific coommands
                }
                else
                {
                    //basic help commands
                }
            }
            if (e.WhisperMessage.Message.ToLower() == "!stats")
            {
                DateTime created = new DateTime();
                long xp = new long();
                long contestWon = new long();
                foreach (UserProfile x in channalResources.profiles)
                {
                    if (e.WhisperMessage.DisplayName.Contains(x.follower.User.DisplayName))
                    {
                        xp = x.xp;
                        contestWon = x.contestWon.Count;
                        created = x.follower.CreatedAt;
                        break;
                    }
                }
                twitchClient.SendWhisper(e.WhisperMessage.DisplayName, "CreatedAT : " + created + " :: XP :" + xp + " :: contestWOn :" + contestWon);
            }

        }
    
        private void TwitchClient_OnWhisperCommandReceived(object sender, TwitchLib.Events.Client.OnWhisperCommandReceivedArgs e)
        {

        }

        private void TwitchClient_OnUserTimedout(object sender, TwitchLib.Events.Client.OnUserTimedoutArgs e)
        {

        }

        private void TwitchClient_OnUserStateChanged(object sender, TwitchLib.Events.Client.OnUserStateChangedArgs e)
        {

        }

        private void TwitchClient_OnUserLeft(object sender, TwitchLib.Events.Client.OnUserLeftArgs e)
        {

        }

        private void TwitchClient_OnUserJoined(object sender, TwitchLib.Events.Client.OnUserJoinedArgs e)
        {

        }

        private void TwitchClient_OnUserBanned(object sender, TwitchLib.Events.Client.OnUserBannedArgs e)
        {

        }

        private void TwitchClient_OnSendReceiveData(object sender, TwitchLib.Events.Client.OnSendReceiveDataArgs e)
        {

        }

        private void TwitchClient_OnReSubscriber(object sender, TwitchLib.Events.Client.OnReSubscriberArgs e)
        {

        }

        private void TwitchClient_OnNowHosting(object sender, TwitchLib.Events.Client.OnNowHostingArgs e)
        {

        }

        private void TwitchClient_OnNewSubscriber(object sender, TwitchLib.Events.Client.OnNewSubscriberArgs e)
        {

        }

        private void TwitchClient_OnModeratorsReceived(object sender, TwitchLib.Events.Client.OnModeratorsReceivedArgs e)
        {

        }

        private void TwitchClient_OnModeratorLeft(object sender, TwitchLib.Events.Client.OnModeratorLeftArgs e)
        {

        }

        private void TwitchClient_OnModeratorJoined(object sender, TwitchLib.Events.Client.OnModeratorJoinedArgs e)
        {

        }

        private void TwitchClient_OnMessageSent(object sender, TwitchLib.Events.Client.OnMessageSentArgs e)
        {

        }

        private void TwitchClient_OnMessageReceived(object sender, TwitchLib.Events.Client.OnMessageReceivedArgs e)
        {
            Console.WriteLine("incomeing transmision..." + e.ChatMessage.Username + " : " + e.ChatMessage.Message);
            string msg = e.ChatMessage.Message.ToLower();
            Console.ForegroundColor = ConsoleColor.Blue;
            if (msg.StartsWith("!"))
            {
                if (e.ChatMessage.IsModerator || e.ChatMessage.IsBroadcaster)
                {
                    if (msg == "!getprofiles")
                    {
                        channalResources.GetAllUserProfiles();
                    }
                    if (msg == "!getchannel")
                    {
                        APIResources.GetChannelID().Wait();
                        channel = APIResources.channel;
                    }
                    if (msg == "!followers")
                    {
                        APIResources.FollowersAsync().Wait();
                        foreach(var follower in APIResources.followersList)
                        {
                            Console.WriteLine(follower.CreatedAt + " :: Name :" + follower.User.DisplayName);
                        }
                    }
                    if (msg == "!subscribers")
                    {

                        foreach (var subscriber in APIResources.subscribers)
                        {
                            Console.WriteLine(subscriber.CreatedAt + " :: Name :" + subscriber.User.DisplayName);
                        }
                    }
                    if (msg == "!bitroulette /s" || msg == "!bitroulette /s /a")
                    {
                        BitRouletteGame.NewGame(msg.EndsWith("/a"));
                        Console.WriteLine("*BitRoulette Game started!*");
                    }

                    if (msg == "!bitroulette /e")
                    {
                        BitRouletteGame.GameOver();
                    }

                }
            }
            if (msg.StartsWith("cheer"))
            {
                Console.WriteLine("cheer incomeing");
                ChatProssesor.Cheer(msg, out bool cheer, out long ammount);
                if (cheer == true) {
                    if (BitRouletteGame.gameActiveReadonly == true)
                    {
                        // e.ChatMessage.Username
                        foreach (UserProfile currentUser in channalResources.profiles)
                        {
                            if (currentUser.follower.User.DisplayName.ToLower() == e.ChatMessage.Username.ToLower())
                            {
                                BitRouletteGame.NewEntry(currentUser, ammount);
                                break;
                            }
                        }

                    }
                    else
                    {
                        twitchClient.SendMessage("thanks for the cheer of :" + ammount + " bits it really helps!!");
                    }
                }
                else
                {
                    Console.WriteLine("false cheer format");
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
    
        private void TwitchClient_OnLeftChannel(object sender, TwitchLib.Events.Client.OnLeftChannelArgs e)
        {

        }

        private void TwitchClient_OnJoinedChannel(object sender, TwitchLib.Events.Client.OnJoinedChannelArgs e)
        {
            
        }

        private void TwitchClient_OnIncorrectLogin(object sender, TwitchLib.Events.Client.OnIncorrectLoginArgs e)
        {

        }

        private void TwitchClient_OnHostLeft(object sender, EventArgs e)
        {

        }

        private void TwitchClient_OnHostingStopped(object sender, TwitchLib.Events.Client.OnHostingStoppedArgs e)
        {

        }

        private void TwitchClient_OnHostingStarted(object sender, TwitchLib.Events.Client.OnHostingStartedArgs e)
        {

        }

        private void TwitchClient_OnExistingUsersDetected(object sender, TwitchLib.Events.Client.OnExistingUsersDetectedArgs e)
        {

        }

        private void TwitchClient_OnDisconnected(object sender, TwitchLib.Events.Client.OnDisconnectedArgs e)
        {

        }

        private void TwitchClient_OnConnectionError(object sender, TwitchLib.Events.Client.OnConnectionErrorArgs e)
        {

        }

        private void TwitchClient_OnConnected(object sender, TwitchLib.Events.Client.OnConnectedArgs e)
        {
            
        }

        private void TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e)
        {

        }

        private void TwitchClient_OnChatColorChanged(object sender, TwitchLib.Events.Client.OnChatColorChangedArgs e)
        {

        }

        private void TwitchClient_OnChatCleared(object sender, TwitchLib.Events.Client.OnChatClearedArgs e)
        {

        }

        private void TwitchClient_OnChannelStateChanged(object sender, TwitchLib.Events.Client.OnChannelStateChangedArgs e)
        {

        }

        private void TwitchClient_OnBeingHosted(object sender, TwitchLib.Events.Client.OnBeingHostedArgs e)
        {

        }

        private void TwitchClient_OnLog(object sender, TwitchLib.Events.Client.OnLogArgs e)
        {
            Console.WriteLine(e.Data);
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnecting...");
            twitchClient.Disconnect();
        }
    }
}
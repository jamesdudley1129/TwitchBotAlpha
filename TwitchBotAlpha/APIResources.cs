using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.API.v5.Channels;
using TwitchLib.Models.API.v5.Subscriptions;
using TwitchLib.Models.API.v5.Streams;
using TwitchLib.Models.API.Undocumented.Chatters;

namespace TwitchBotAlpha
{
    public static class APIResources
    {
        public static List<ChannelFollow> followersList;
        public static List<Subscription> subscribers = new List<Subscription>();
        public static ChannelAuthed channel = new ChannelAuthed();
        public static StreamByUser stream;
        public static List<ChatterFormatted> chatters;
        
        public static async Task GetChannelID()
        {
            channel = await TwitchAPI.Channels.v5.GetChannelAsync();
        }

        public static async Task FollowersAsync()
        {

            //followers = await TwitchAPI.Channels.v5.GetChannelFollowersAsync(channel.Id.ToString());
            followersList = await TwitchAPI.Channels.v5.GetAllFollowersAsync(channel.Id.ToString());
        }

        public static async Task SubscribersAsync()
        {

            subscribers = await TwitchAPI.Channels.v5.GetAllSubscribersAsync(channel.Id.ToString());
        }
        public static async Task Stream()
        {
            stream = await TwitchAPI.Streams.v5.GetStreamByUserAsync(channel.Id.ToString());

        }
        public static async Task Chatters()
        {
            chatters= await TwitchAPI.Undocumented.GetChattersAsync(channel.Name);
           
        }
    }
}
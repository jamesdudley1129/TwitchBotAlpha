using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Models.API.v5.Channels;
namespace TwitchBotAlpha
{
    public class UserProfile//model for a UserProfileObject
    {
        public List<ContestWon> contestWon = new List<ContestWon>();
        public long xp = 0;
        public ChannelFollow follower;

        //sprite customizablitys 
        //bio 
        //social media
        public UserProfile(ChannelFollow user)
        {

            follower = user;
        }

    }
}

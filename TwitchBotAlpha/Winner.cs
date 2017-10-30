using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TwitchBotAlpha
{
    public class Winner//constructed A-1st
    {
        public PrizeType prize;
        public long xpWon;

        public UserProfile profile;
        public ContestWon thisContest;

        public Winner(UserProfile user, ContestType contestType, PrizeType prizeType, long xpGiveaway)
        {
            thisContest = new ContestWon(contestType);
            prize = prizeType;
            xpWon = xpGiveaway;
            profile = user;
            thisContest.dateTime = new DateTime();
            thisContest.payedOut = false;
            ApplyUpdates();
        }

        public void ApplyUpdates()
        {
            profile.xp += xpWon;
            if (prize != PrizeType.XP)
            {
                profile.contestWon.Add(thisContest);
            }
        }
    }
}

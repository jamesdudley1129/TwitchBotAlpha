using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TwitchBotAlpha
{
    public class ContestWon//model for a ContestWonObject
    {
        public ContestType contestType;
        public DateTime dateTime;
        public bool payedOut;
        public ContestWon(ContestType contestType)
        {
            this.contestType = contestType;
            dateTime = new DateTime();
            payedOut = false;
        }
    }
}

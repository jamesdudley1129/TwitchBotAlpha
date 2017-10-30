using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TwitchBotAlpha
{
    public class ContestEventDocumentation//Constructed A-2nd
    {
        public List<Winner> winners = new List<Winner>();
        public ContestEventDocumentation()
        {
            //checks for existing documentation
            //and adds existing documentation
        }
        public void NewWinnerOfNewContest(Winner winner)//called A-3rd
        {
            winners.Add(winner);
        }
    }
}

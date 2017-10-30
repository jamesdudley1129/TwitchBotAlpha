using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBotAlpha
{
    static public class ChatProssesor
    {
        static public void Cheer(string incommingMSG, out bool cheer, out long ammount)
        {
            cheer = false;
            ammount = 0;

            if (incommingMSG.StartsWith("cheer"))
            {
                char[] msgArray = incommingMSG.ToArray();

                if (int.TryParse(msgArray[5].ToString(),out int result))
                {
                    Console.WriteLine("cheer is recived");
                    cheer = true;
                }
            }

            //split the cheers into separate strings
            //add the cheers into a cheersInMsg
            string[] cheersInMsg = incommingMSG.Split(' ');

            foreach(string cheers in cheersInMsg)
            {
                //split bits from cheers
                long.TryParse(cheers.Substring(cheers.IndexOf('r') + 1),out long tempAmmount);
                ammount += tempAmmount;
            }

            
            

            //add bits


            /* foreach (string msg_section in CheersInMsg)
             {
                 long tempAmount = 0;
                 if (msg_section.StartsWith("cheer"))
                 {
                     Console.WriteLine(incommingMSG);
                     string tempAmmountString = incommingMSG.Substring(incommingMSG.IndexOf('r') + 1);
                     Console.WriteLine(tempAmmountString);
                     string[] cheer_Msg_split = tempAmmountString.Split(' ');
                     tempAmmountString = cheer_Msg_split[0];
                     //maybe foreach cheer after repeat ^

                     Console.WriteLine(tempAmmountString);
                     long.TryParse(tempAmmountString, out tempAmount);

                 }
                 ammount += tempAmount;
             }
             */
        }

    }
}

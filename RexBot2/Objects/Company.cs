using System;
using System.Collections.Generic;
using System.Text;

namespace RexBot2.Objects
{
    public class Company
    {
        public int Index;
        public string Name;
        public int Risk; // 0 - 5
        public int Ethicality; // 0 - 5
        public int MaxReward;
        public int AvailableShare;
        public int Duration;


        public Company(int index, string name, int risk, int ethicality, int availableshare, int maxreward, int duration)
        {
            Index = index;
            Name = name;
            Risk = risk;
            Ethicality = ethicality;
            MaxReward = maxreward;
            AvailableShare = availableshare;
            Duration = duration;
        }

        public string getRiskString()
        {
            string riskstring = "";
            switch (Risk)
            {
                case 0: riskstring = "Safe AF"; break;
                case 1: riskstring = "Low"; break;
                case 2: riskstring = "Medium"; break;
                case 3: riskstring = "High"; break;
                case 4: riskstring = "Dangerous AF"; break;
                case 5: riskstring = "CRAZY"; break;
                default: riskstring = "ERROR!"; break;
            }

            return riskstring;
        }

        public string getEthicalityString()
        {
            string ethicalitystring = "";
            switch (Risk)
            {
                case 0: ethicalitystring = "NICK"; break;
                case 1: ethicalitystring = "Evil"; break;
                case 2: ethicalitystring = "Bad"; break;
                case 3: ethicalitystring = "Meh"; break;
                case 4: ethicalitystring = "Good"; break;
                case 5: ethicalitystring = "Angel"; break;
                default: ethicalitystring = "ERROR!"; break;
            }
            return ethicalitystring;
        }

        public string getDurationString()
        {
            string durationstring = "";
            int hours = Duration / 60;
            int minutes = Duration % 60;
            durationstring += hours.ToString() + "h " + minutes.ToString() + "m";
            return durationstring;
        }

        public override string ToString()
        {
            return Index.ToString() + " - **" + Name + "** / " + getRiskString() + " / " + MaxReward.ToString() + "% / " + getEthicalityString() + " / " + AvailableShare.ToString()  + " / " + getDurationString();
        }
    }

}

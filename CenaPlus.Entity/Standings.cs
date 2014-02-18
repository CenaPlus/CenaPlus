using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class StandingItem
    {
        public int Rank { get; set; }
        public Entity.ContestType Type = new Entity.ContestType();
        public string Competitor { get; set; }
        public int UserID { get; set; }
        public int MainKey
        {
            get
            {
                int mainkey = 0;
                foreach (var t in Details)
                {
                    if (Type == Entity.ContestType.OI || Type == Entity.ContestType.Codeforces || Type == Entity.ContestType.TopCoder)
                    {
                        mainkey += t.FirstScore;
                    }
                    else
                    {
                        if (t.SecondScore != 0) mainkey++;
                    }
                }
                return mainkey;
            }
        }//OI为分数，ACM为AC数，CF、TC为总分数
        public int SecKey
        {
            get
            {
                if (Type == Entity.ContestType.Codeforces || Type == Entity.ContestType.TopCoder) return 0;
                int seckey = 0;
                foreach (var t in Details)
                {
                    seckey += t.SecondScore;
                }
                return seckey;
            }
        }//OI为时间消耗，ACM为总罚时
        public string SecDisplay
        {
            get
            {
                if (Type == Entity.ContestType.Codeforces || Type == Entity.ContestType.TopCoder)
                {
                    return HackStr;
                }
                if (Type == Entity.ContestType.OI)
                {
                    return SecKey + " ms";
                }
                else
                {
                    return new TimeSpan(0, 0, SecKey).ToString();
                }
            }
        }
        public string HackStr
        {
            get
            {
                string str = "";
                if (SuccessfulHack > 0)
                {
                    str += "+" + SuccessfulHack;
                }
                if (UnsuccessfulHack > 0)
                {
                    if (SuccessfulHack != 0)
                    {
                        str += " : ";
                    }
                    str += "-" + UnsuccessfulHack;
                }
                return str;
            }
        }
        public int? SuccessfulHack { get; set; }
        public int? UnsuccessfulHack { get; set; }
        public StandingDetail[] Details;
        public string[] Display//这里必须另开一个数组
        {
            get
            {
                string[] t = new string[Details.Length];
                for (int i = 0; i < Details.Length; i++)
                {
                    t[i] = Details[i].Display;
                }
                return t;
            }
        }
    }
    public class StandingDetail
    {
        public Entity.ContestType DisplayFormat { get; set; }
        public int FirstScore { get; set; }//OI为分数，ACM为尝试失败次数，CF、TC为分数
        public int SecondScore { get; set; }//ACM为该题目罚时(未通过罚时为0)，CF，TC为解题时间，均以秒为单位/oi耗时 毫秒
        public int ThirdScore { get; set; }//CF TC尝试失败次数
        public int RecordID { get; set; }
        public string Display
        {
            get
            {
                string Content = "";
                switch (DisplayFormat)
                {
                    case Entity.ContestType.OI:
                        {
                            Content = FirstScore.ToString();
                            break;
                        }
                    case Entity.ContestType.ACM:
                        {
                            if (SecondScore != 0)
                            {
                                Content = new TimeSpan(0, 0, SecondScore).ToString();
                                if (FirstScore != 0)
                                {
                                    Content += "\r\n";
                                }
                            }
                            if (FirstScore != 0)
                            {
                                Content += String.Format("(-{0})", FirstScore - 1);
                            }
                            else
                            { 
                                if(SecondScore!=0)
                                    Content = String.Format("(-{0})", FirstScore);
                            }
                            break;
                        }
                    case Entity.ContestType.Codeforces:
                    case Entity.ContestType.TopCoder:
                        {
                            if (FirstScore != 0)
                            {
                                Content = String.Format("{0}\r\n{1}", FirstScore, new TimeSpan(0, SecondScore / 60, 0).ToString());
                            }
                            else
                            {
                                if (ThirdScore != 0)
                                    Content = String.Format("(-{0})", ThirdScore);
                            }
                            break;
                        }
                    default: Content = "N/A"; break;
                }
                return Content;
            }
        }
    }
}

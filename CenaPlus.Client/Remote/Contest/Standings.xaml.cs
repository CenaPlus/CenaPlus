using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Standings.xaml
    /// </summary>
    public partial class Standings : UserControl
    {
        public List<StandingItem> StandingItems = new List<StandingItem>();
        public Standings()
        {
            InitializeComponent();
            for (int i = 'A'; i <= 'D'; i++)
            {
                DataGridTextColumn dgtc = new DataGridTextColumn();
                dgtc.Header = (char)i + "";
                dgtc.Binding = new Binding(String.Format("Display.[{0}]", ((int)(i - 'A'))).ToString());
                dgtc.SortMemberPath = String.Format("Display.[{0}]", ((int)(i - 'A'))).ToString();
                dgStandings.Columns.Add(dgtc);
            }
            
            for (int i = 0; i < 10; i++)
            {
                StandingItem t = new StandingItem();
                t.Competitor = "user" + (i + 1);
                t.MainKey = 100 + i * 10;
                t.SecKey = 123 - i;
                t.Details = new StandingDetail[4];//题目个数
                t.Details[0] = (new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 50, SecondScore = 123 });
                t.Details[1]=(new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 100, SecondScore = 421 });
                t.Details[2]=(new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 80, SecondScore = 46 });
                t.Details[3]=(new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 20, SecondScore = 22 });
                StandingItems.Add(t);
            }
            dgStandings.ItemsSource = StandingItems;
        }
    }
    public class StandingItem
    {
        //public int Rank { }
        public string Competitor { get; set; }
        public int MainKey { get; set; }//OI为分数，ACM为AC数，CF、TC为总分数
        public int SecKey { get; set; }//OI为时间消耗，ACM为总罚时
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
        public string Number { get; set; }
        public int FirstScore { get; set; }//OI为分数，ACM为尝试失败次数，CF、TC为分数
        public int SecondScore { get; set; }//ACM为该题目罚时，CF，TC为解题时间，均以秒为单位/oi耗时 毫秒
        public int ThirdScore { get; set; }//CF TC尝试失败次数
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
                                Content += String.Format("(-{0})", FirstScore);
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

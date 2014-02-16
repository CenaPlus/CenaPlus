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
        public void Sort()//排名变化了执行这个更新，最好排名数据是一个静态的，客户端收到了服务器推送的排名更新，后台就更新了，打开到这个页面自动就能看到，就是全部排名只加载一次，之后推送变化值。
        {
            StandingItems.Sort((x, y) => x.MainKey == y.MainKey ? x.SecKey - y.SecKey : y.MainKey - x.MainKey);
            dgStandings.Items.Refresh();
        }
        public Standings()
        {
            InitializeComponent();
            for (int i = 'A'; i <= 'D'; i++)//动态加载列
            {
                DataGridTextColumn dgtc = new DataGridTextColumn();
                dgtc.Header = "    "+(char)i;//不加空格表头似乎没居中
                dgtc.Binding = new Binding(String.Format("Display.[{0}]", ((int)(i - 'A'))).ToString());//这个string[] Display成员是自动根据Details更新的，我不知道如何直接绑定到Details
                dgtc.ElementStyle = Resources["dgCell"] as Style;//这里是让单元格内文字居中用的
                dgStandings.Columns.Add(dgtc);
            }
            for (int i = 0; i < 10; i++)
            {
                StandingItem t = new StandingItem();//这个实体只需要给Competitor Type赋值就行，其他的都是根据Details计算出来的
                t.Type = Entity.ContestType.ACM;
                t.Competitor = "user" + (i + 1);
                t.Details = new StandingDetail[4];//题目个数
                t.Details[0] = (new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 50, SecondScore = 123 });
                t.Details[1]=(new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 100, SecondScore = 421 });
                t.Details[2]=(new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 80, SecondScore = 46 });
                t.Details[3]=(new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 20, SecondScore = 22 });
                StandingItems.Add(t);
            }
            dgStandings.ItemsSource = StandingItems;
            Sort();
            Entity.ContestType Type = Entity.ContestType.ACM;
            //这里是针对不同赛制更新不同表头
            if (Type == Entity.ContestType.OI)
            {
                dgtcMainKey.Header = "Score";
                dgtcSecKey.Header = "Time";
            }
            else if (Type == Entity.ContestType.ACM)
            {
                dgtcMainKey.Header = "AC";
                dgtcSecKey.Header = "Penalty";
            }
            else
            {
                dgtcMainKey.Header = "Score";
                dgtcSecKey.Header = "Hack";
            }
        }
    }
    public class StandingItem
    {
        public Entity.ContestType Type = new Entity.ContestType();
        public string Competitor { get; set; }
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
        public string Number { get; set; }
        public int FirstScore { get; set; }//OI为分数，ACM为尝试失败次数，CF、TC为分数
        public int SecondScore { get; set; }//ACM为该题目罚时(未通过罚时为0)，CF，TC为解题时间，均以秒为单位/oi耗时 毫秒
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

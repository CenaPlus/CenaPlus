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
using CenaPlus.Entity;

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
                t.Details[0] = (new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 50, SecondScore = 123, RecordID = 10000 + i * 10 });
                t.Details[1] = (new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 100, SecondScore = 421, RecordID = 10001 + i * 10 });
                t.Details[2] = (new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 80, SecondScore = 46, RecordID = 10002 + i * 10 });
                t.Details[3] = (new StandingDetail() { DisplayFormat = Entity.ContestType.ACM, FirstScore = 20, SecondScore = 22, RecordID = 10003 + i * 10 });
                StandingItems.Add(t);
            }
            dgStandings.ItemsSource = StandingItems;
            Sort();
            Entity.ContestType Type = Entity.ContestType.ACM;
            //这里是针对不同赛制更新不同表头
            if (Type == Entity.ContestType.OI)
            {
                dgtcMainKey.Header = "Score";
                dgtcMainKey.Width = 80;
                dgtcSecKey.Header = "Time";
            }
            else if (Type == Entity.ContestType.ACM)
            {
                dgtcMainKey.Header = "AC";
                dgtcMainKey.Width = 50;
                dgtcSecKey.Header = "Penalty";
            }
            else
            {
                dgtcMainKey.Header = "Score";
                dgtcMainKey.Width = 80;
                dgtcSecKey.Header = "Hack";
            }
        }

        private void dgStandings_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            IList<DataGridCellInfo> selectedcells = e.AddedCells;
            if (selectedcells.Count == 1 && selectedcells[0].Column.Header.ToString().Trim(' ').Length == 1 && selectedcells[0].Column.Header.ToString().Trim(' ')[0] >= 'A' && selectedcells[0].Column.Header.ToString().Trim(' ')[0] <= 'Z')
                btnHack.IsEnabled = true;
            else
                btnHack.IsEnabled = false;
        }

        private void btnHack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((dgStandings.SelectedCells[0].Item as StandingItem).Details[(int)(dgStandings.SelectedCells[0].Column.Header.ToString().Trim(' ')[0] - 'A')].RecordID + "");
        }
    }

}

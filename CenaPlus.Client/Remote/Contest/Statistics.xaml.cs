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
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : UserControl
    {
        public List<StatisticsViewItem> StatisticsItems = new List<StatisticsViewItem>();
        public Statistics()
        {
            InitializeComponent();
            for (char i = 'A'; i <= 'I'; i++)
            {
                StatisticsViewItem t = new StatisticsViewItem();
                t.Problem = i + "";
                t.AC = 123;
                t.CE = 23;
                t.WA = i;
                StatisticsItems.Add(t);
            }
            StatisticsListView.ItemsSource = StatisticsItems;
        }
    }
    public class StatisticsViewItem
    {
        public string Problem { get; set; }
        public int AC { get; set; }
        public int CE { get; set; }
        public int SE { get; set; }
        public int VE { get; set; }
        public int WA { get; set; }
        public int TLE { get; set; }
        public int MLE { get; set; }
        public int RE { get; set; }
    }
}

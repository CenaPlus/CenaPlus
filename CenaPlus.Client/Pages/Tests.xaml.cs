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
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;

namespace CenaPlus.Client.Pages
{
    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : UserControl
    {
        protected List<ContestList> ContestList = new List<ContestList>();
        public Tests()
        {
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                ContestList t = new ContestList();
                t.ID = i;
                t.StartTime = Convert.ToDateTime("2014-1-24 12:00");
                t.EndTime = Convert.ToDateTime("2014-1-24 14:00");
                t.Type = ContestType.TopCoder;
                t.Title = "Cena Plus Beta Round #" + i;
                ContestList.Add(t);
            }
            ContestListBox.ItemsSource = ContestList;
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // ModernDialog.ShowMessage(e.Source.ToString(), "Message", MessageBoxButton.OK);
        }

        private void ContestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModernDialog.ShowMessage(e.Source.ToString(), "Message", MessageBoxButton.OK);
        }
    }
    public class ContestList : CenaPlus.Entity.Contest
    {
        private const string DetailTemplate = "{0} UTC / {1} hrs / {2} Format";
        public string Detail
        {
            get 
            {
                return String.Format(DetailTemplate, StartTime, (Duration.TotalSeconds / 60 / 60).ToString("0.0"), Type);
            }
        }
    }
}

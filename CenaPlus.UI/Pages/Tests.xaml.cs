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

namespace CenaPlus.UI.Pages
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
                t.Begin = Convert.ToDateTime("2014-1-24 12:00");
                t.End = Convert.ToDateTime("2014-1-24 14:00");
                t.Format = i%4;
                t.Host = "Official";
                t.Rating = 10;
                t.Title = "Cena Plus Beta Round #" + i;
                ContestList.Add(t);
            }
            ContestListBox.ItemsSource = ContestList;
        }
    }
    public class ContestList : CenaPlus.Entity.Contests
    {
        private const string DetailTemplate = "{0} UTC / {1} hrs / {2} Format / Rating {3}";
        public string Detail
        {
            get 
            {
                return String.Format(DetailTemplate, Begin, (Length / 60 / 60).ToString("0.0"), FormatStr[Format], Rating);
            }
        }
    }
}

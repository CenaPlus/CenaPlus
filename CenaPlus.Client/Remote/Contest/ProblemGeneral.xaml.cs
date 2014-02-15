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
    /// Interaction logic for ProblemGeneral.xaml
    /// </summary>
    public partial class ProblemGeneral : UserControl
    {
        public List<ProblemListBoxItem> ProblemListBoxItems = new List<ProblemListBoxItem>();
        public ProblemGeneral()
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                ProblemListBoxItem t = new ProblemListBoxItem();
                t.Title = (char)(i + 'A') + ": A+B Problem";
                if (i % 2 == 0)
                    t.Details = "Solved @2014-2-15 18:07";
                else
                    t.Details = "Pending / 1s per test case / 64MB memory limit";
                //Hacked by Gasai Yuno @2014-2-15 18:07
                ProblemListBoxItems.Add(t);
            }
            ProblemListBox.ItemsSource = ProblemListBoxItems;
        }
    }
    public class ProblemListBoxItem
    {
        public string Title { get; set; }
        public string Details { get; set; }
    }
}

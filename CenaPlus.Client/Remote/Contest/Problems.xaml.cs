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
using FirstFloor.ModernUI.Presentation;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Contest_Problems.xaml
    /// </summary>
    public partial class Problems : UserControl
    {
        public Problems()
        {
            InitializeComponent();
            for (char c = 'A'; c <= 'J'; c++)
            {
                Link a = new Link();
                a.DisplayName = c + "";
                a.Source = new Uri("/Remote/Contest/Problem.xaml#" + c, UriKind.Relative);
                ProblemTab.Links.Add(a);
            }
            ProblemTab.SelectedSource = new Uri("/Remote/Contest/Problem.xaml#A", UriKind.Relative);
        }
    }
}

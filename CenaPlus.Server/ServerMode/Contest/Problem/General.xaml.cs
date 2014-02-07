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
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;

namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : UserControl, IContent
    {
        private int id;

        public General()
        {
            InitializeComponent();
            lbLanguageForbidden.ItemsSource = Enum.GetNames(typeof(ProgrammingLanguage));
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            double memoryKB;
            if (!double.TryParse(txtMemoryLimit.Text, out memoryKB))
            {
                ModernDialog.ShowMessage("Memory limit should be float point number", "Error", MessageBoxButton.OK);
                return;
            }

            long memoryLimit = (long)(memoryKB * 1024);
            int timeLimit;
            if (!int.TryParse(txtTimeLimit.Text, out timeLimit))
            {
                ModernDialog.ShowMessage("Time limit should be integer", "Error", MessageBoxButton.OK);
                return;
            }
            int score;
            if (!int.TryParse(txtScore.Text, out score))
            {
                ModernDialog.ShowMessage("Score should be integer", "Error", MessageBoxButton.OK);
                return;
            }
            App.Server.UpdateProblem(id, txtTitle.Text, null, score, timeLimit, memoryLimit, null, null, null, null, null, null);
            ModernDialog.ShowMessage("Saved", "Message", MessageBoxButton.OK);
        }
        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            id = int.Parse(e.Fragment);
            var problem = App.Server.GetProblem(id);
            txtTitle.Text = problem.Title;
            txtTimeLimit.Text = problem.TimeLimit.ToString();
            txtMemoryLimit.Text = (problem.MemoryLimit / 1024.0).ToString();
            txtScore.Text = problem.Score.ToString();
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }


    }
}

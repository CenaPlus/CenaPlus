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

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : UserControl,IContent
    {
        private int id;

        public General()
        {
            InitializeComponent();
            foreach (var type in Enum.GetNames(typeof(ContestType)))
            {
                cbbType.Items.Add(new ListBoxItem { Content = type });
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            id = int.Parse(e.Fragment);
            var contest = App.Server.GetContest(id);
            txtTitle.Text = contest.Title;
            txtBeginTime.Text = contest.StartTime.ToString("hh:mm:ss");
            txtEndTime.Text = contest.EndTime.ToString("hh:mm:ss");
            dateBeginDate.SelectedDate = contest.StartTime;
            dateEndDate.SelectedDate = contest.EndTime;
            cbbType.SelectedIndex = (int)contest.Type;
        }
        
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!dateBeginDate.SelectedDate.HasValue)
            {
                ModernDialog.ShowMessage("Please select start date", "Error", MessageBoxButton.OK);
                return;
            }

            if (!dateEndDate.SelectedDate.HasValue)
            {
                ModernDialog.ShowMessage("Please select end date", "Error", MessageBoxButton.OK);
                return;
            }

            TimeSpan startTime, endTime;

            if (!TimeSpan.TryParse(txtBeginTime.Text, out startTime))
            {
                ModernDialog.ShowMessage("Invalid start time", "Error", MessageBoxButton.OK);
                return;
            }
            if (!TimeSpan.TryParse(txtEndTime.Text, out endTime))
            {
                ModernDialog.ShowMessage("Invalid end time", "Error", MessageBoxButton.OK);
                return;
            }

            var start = dateBeginDate.SelectedDate.Value.Add(startTime);
            var end = dateEndDate.SelectedDate.Value.Add(endTime);
            if (startTime > endTime)
            {
                ModernDialog.ShowMessage("Start time should be less than end time", "Error", MessageBoxButton.OK);
                return;
            }
            
            App.Server.UpdateContest(id, txtTitle.Text, null, start, end, (ContestType)cbbType.SelectedIndex);
            ModernDialog.ShowMessage("Saved", "Message", MessageBoxButton.OK);
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

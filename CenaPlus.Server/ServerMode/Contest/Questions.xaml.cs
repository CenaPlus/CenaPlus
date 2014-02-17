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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Questions.xaml
    /// </summary>
    public partial class Questions : UserControl, IContent
    {
        private List<QuestionListItem> QuestionListItems = new List<QuestionListItem>();
        public Questions()
        {
            InitializeComponent();
        }

        private void lstQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstQuestion.SelectedItem == null)
            {
                spDetails.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                spDetails.Visibility = System.Windows.Visibility.Visible;
                QuestionTextBlock.Text = (lstQuestion.SelectedItem as QuestionListItem).Description;
                txtAnswer.Text = (lstQuestion.SelectedItem as QuestionListItem).Answer;
                if ((lstQuestion.SelectedItem as QuestionListItem).Status != Entity.QuestionStatus.Pending)
                {
                    spDetails.IsEnabled = false;
                }
                else
                {
                    spDetails.IsEnabled = true;
                }
            }
        }

        class QuestionListItem : Entity.Question
        {
            public string Details
            {
                get
                {
                    return String.Format("Asker: {0} / {1} @{2}", AskerNickName, Status, Time.ToShortTimeString());
                }
            }
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            QuestionListItems.Clear();
            var contest_id = int.Parse(e.Fragment);
            var list = from id in App.Server.GetQuestionList(contest_id)
                       let q = App.Server.GetQuestion(id)
                       select new QuestionListItem
                       {
                           ID = q.ID,
                           AskerID = q.AskerID,
                           Answer = q.Answer,
                           Asker =q.Asker,
                           Contest = q.Contest,
                           ContestID = q.ContestID,
                           ContestName = q.ContestName,
                           Description = q.Description,
                           Status = q.Status,
                           StatusAsInt = q.StatusAsInt,
                           Time = q.Time,
                           AskerNickName = q.AskerNickName
                       };
            foreach (var item in list) QuestionListItems.Add(item);
            lstQuestion.ItemsSource = QuestionListItems;
            lstQuestion.Items.Refresh();
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //TODO: 更新记录并推送
            var q = lstQuestion.SelectedItem as QuestionListItem;
            if (rdbtnPrivate.IsChecked == true)
                q.Status = Entity.QuestionStatus.Private;
            else if (rdbtnPublic.IsChecked == true)
                q.Status = Entity.QuestionStatus.Public;
            else
                q.Status = Entity.QuestionStatus.Rejected;
            q.Answer = txtAnswer.Text;
            if (q.Status == Entity.QuestionStatus.Public)
            {
                //TODO: 推送此条提问，使得客户端弹出MsgBox
            }
            else
            { 
                //TODO: 推送给提问者
            }
        }
    }
}

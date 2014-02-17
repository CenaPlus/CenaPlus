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
using CenaPlus.Entity;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for FAQ.xaml
    /// </summary>
    public partial class Questions : UserControl, IContent
    {
        private List<QuestionListItem> questionList = new List<QuestionListItem>();
        private int contestID;

        public Questions()
        {
            InitializeComponent();
            lstQuestion.ItemsSource = questionList;
        }

        private void lstQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstQuestion.SelectedIndex != -1)
            {
                QuestionTextBlock.Text = (lstQuestion.SelectedItem as QuestionListItem).Description;
                AnswerTextBlock.Text = (lstQuestion.SelectedItem as QuestionListItem).Answer;
            }
            else
            {
                QuestionTextBlock.Text = "";
                AnswerTextBlock.Text = "";
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            contestID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetQuestionList(contestID)
                       let q = App.Server.GetQuestion(id)
                       select new QuestionListItem
                       {
                           ID = q.ID,
                           AskerNickName = q.AskerNickName,
                           Status = q.Status,
                           Time = q.Time,
                           Answer = q.Answer,
                           Description = q.Description
                       };
            questionList.Clear();
            foreach (var item in list) questionList.Add(item);

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int id = App.Server.AskQuestion(contestID, txtQuestion.Text);
            txtQuestion.Text = "";
            var question = App.Server.GetQuestion(id);
            questionList.Add(new QuestionListItem
            {
                ID = question.ID,
                Answer = question.Answer,
                AskerNickName = question.AskerNickName,
                Description = question.Description,
                Status = question.Status,
                Time = question.Time
            });
            lstQuestion.Items.Refresh();
            ModernDialog.ShowMessage("Done", "Message", MessageBoxButton.OK);
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

        class QuestionListItem : Question
        {
            public string Details
            {
                get
                {
                    return String.Format("Asker: {0} / {1} @{2}", AskerNickName, Status, Time.ToShortTimeString());
                }
            }
        }
    }
}

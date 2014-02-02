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
    /// Interaction logic for FAQ.xaml
    /// </summary>
    public partial class Questions : UserControl
    {
        public List<QuestionListItem> FAQListItems = new List<QuestionListItem>();
        public Questions()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                QuestionListItem t = new QuestionListItem();
                t.Description = "int64 is %lld or %I64d? ";
                t.Answer = "%lld test test test test test test test test test test test test test test test test test test test test test test test test test test";
                t.Time = DateTime.Now;
                t.Status = QuestionStatus.Public;
                t.AskerNickName = "Gasai Yuno";
                FAQListItems.Add(t);
            }
            FAQListBox.ItemsSource = FAQListItems;
        }

        private void FAQListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FAQListBox.SelectedItem != null)
            {
                QuestionTextBlock.Text = (FAQListBox.SelectedItem as QuestionListItem).Question;
                AnswerTextBlock.Text = (FAQListBox.SelectedItem as QuestionListItem).Answer;
            }
            else
            {
                QuestionTextBlock.Text = "";
                AnswerTextBlock.Text = "";
            }
        }
    }

    public class QuestionListItem : Question
    {
        public string Details
        {
            get
            {
                if (AskerNickName != String.Empty)
                    return String.Format("Asker: {0} / {1} @{2}", Asker, Status, Time.ToShortTimeString());
                else
                    return String.Format("{0} @{1}", Status, Time.ToShortTimeString());
            }
        }
    }
}

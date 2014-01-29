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
    /// Interaction logic for FAQ.xaml
    /// </summary>
    public partial class FAQ : UserControl
    {
        public List<FAQListItem> FAQListItems = new List<FAQListItem>();
        public FAQ()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                FAQListItem t = new FAQListItem();
                t.Question = "int64 is %lld or %I64d? ";
                t.Answer = "%lld test test test test test test test test test test test test test test test test test test test test test test test test test test";
                t.Time = DateTime.Now;
                t.Status = FAQStatus.Public;
                t.Asker = "Gasai Yuno";
                FAQListItems.Add(t);
            }
            FAQListBox.ItemsSource = FAQListItems;
        }

        private void FAQListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FAQListBox.SelectedItem != null)
            {
                QuestionTextBlock.Text = (FAQListBox.SelectedItem as FAQListItem).Question;
                AnswerTextBlock.Text = (FAQListBox.SelectedItem as FAQListItem).Answer;
            }
            else
            {
                QuestionTextBlock.Text = "";
                AnswerTextBlock.Text = "";
            }
        }
    }
    //TODO: Move the entity into the entity layer.
    public class FAQListItem
    {
        public string Asker { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public FAQStatus Status { get; set; }
        public DateTime Time { get; set; }
        public string Details
        {
            get
            {
                if (Asker != String.Empty)
                    return String.Format("Asker: {0} / {1} @{2}", Asker, Status, Time.ToShortTimeString());
                else
                    return String.Format("{0} @{1}", Status, Time.ToShortTimeString());
            }
        }
    }
    public enum FAQStatus { Pending, Whisper, Public, Rejected };
}

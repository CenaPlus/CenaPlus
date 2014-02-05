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

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Status.xaml
    /// </summary>
    public partial class Status : UserControl
    {
        public List<StatusListViewItem> StatusListViewItems = new List<StatusListViewItem>();
        public Status()
        {
            InitializeComponent();
            for (int i = 1; i < 10; i++)
            {
                StatusListViewItem t = new StatusListViewItem();
                t.ID = i;
                t.Language = Entity.ProgrammingLanguage.CXX;
                t.ProblemTitle = "A: A+B Problem";
                t.Status = Entity.RecordStatus.Pending;
                t.Competitor = "GasaiYuno";
                StatusListViewItems.Add(t);
            }
            StatusListView.ItemsSource = StatusListViewItems;
        }

        private void txtSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextEditor.HighLightEdit.HighLight(txtSource);
        }

        private void StatusListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusListView.SelectedItem == null)
            {
                DetailsPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                DetailsPanel.Visibility = Visibility.Visible;
                txtSource.AppendText("int a, b;");//
            }
        }
    }
    public class StatusListViewItem
    {
        public int ID { get; set; }
        public string ProblemTitle { get; set; }
        public Entity.RecordStatus Status { get; set; }
        public string Competitor { get; set; }
        public Entity.ProgrammingLanguage Language { get; set; }
    }
}

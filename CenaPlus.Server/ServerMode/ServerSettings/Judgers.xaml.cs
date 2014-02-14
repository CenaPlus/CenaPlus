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

namespace CenaPlus.Server.ServerMode.ServerSettings
{
    /// <summary>
    /// Interaction logic for Judgers.xaml
    /// </summary>
    public partial class Judgers : UserControl
    {
        public List<JudgeNodeListBoxItem> JudgeNodeListBoxItems = new List<JudgeNodeListBoxItem>();
        public Judgers()
        {
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                JudgeNodeListBoxItem t = new JudgeNodeListBoxItem();
                t.Connected = Convert.ToBoolean(i % 2);
                t.Name = "Cena+ Judge Node #" + (i - 1);
                t.IP = "192.168.0." + i;
                JudgeNodeListBoxItems.Add(t);
            }
            JudgeNodeListBox.ItemsSource = JudgeNodeListBoxItems;
        }

        private void JudgeNodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (JudgeNodeListBox.SelectedItem != null)
            {
                btnConnect.IsEnabled = !(JudgeNodeListBox.SelectedItem as JudgeNodeListBoxItem).Connected;
                btnDisconnect.IsEnabled = (JudgeNodeListBox.SelectedItem as JudgeNodeListBoxItem).Connected;
            }
        }
    }
    public class JudgeNodeListBoxItem
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public bool Connected { get; set; }
        public string Details
        {
            get 
            {
                return String.Format("{0}{1}", Connected ? "Connected " : "", IP);
            }
        }
    }
}

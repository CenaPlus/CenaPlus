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
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for ResultPush.xaml
    /// </summary>
    public partial class ResultPush : UserControl
    {
        public ResultPush(Entity.Record record)
        {
            InitializeComponent();
            tbStatusID.Text = record.ID.ToString();
            tbTime.Text = record.TimeUsage != null ? record.TimeUsage.ToString() + " ms" : "??";
            tbMemory.Text = record.MemoryUsage != null ? (Convert.ToInt32(record.MemoryUsage / 1024)).ToString() + " KiB" : "??";
            tbStatus.Text = record.Status.ToString();
            if (record.Status == Entity.RecordStatus.Accepted)
            {
                tbStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (record.Status == Entity.RecordStatus.SystemError || record.Status == Entity.RecordStatus.ValidatorError || record.Status == Entity.RecordStatus.CompileError)
            {
                tbStatus.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                tbStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            tbDetail.Text = record.Detail.Trim('\r').Trim('\n');
        }
    }
}

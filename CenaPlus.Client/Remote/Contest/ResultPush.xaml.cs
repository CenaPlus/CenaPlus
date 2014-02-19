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
        public ResultPush(Entity.Result result)
        {
            InitializeComponent();
            tbStatusID.Text = result.StatusID.ToString();
            tbLanguage.Text = result.Language.ToString();
            tbSubmittion.Text = result.SubmissionTime.ToString();
            tbTime.Text = result.TimeUsage != null ? result.TimeUsage.ToString() + " ms" : "??";
            tbMemory.Text = result.MemoryUsage != null ? (Convert.ToInt32(result.MemoryUsage / 1024)).ToString() + " KiB" : "??";
            tbStatus.Text = result.Status.ToString();
            if (result.Status == Entity.RecordStatus.Accepted)
            {
                tbStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (result.Status == Entity.RecordStatus.SystemError || result.Status == Entity.RecordStatus.ValidatorError || result.Status == Entity.RecordStatus.CompileError)
            {
                tbStatus.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                tbStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            tbDetail.Text = result.Detail.Trim('\r').Trim('\n');
        }
    }
}

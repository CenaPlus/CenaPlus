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
using System.IO;
using CenaPlus.Client.Bll;
namespace CenaPlus.Client.Content
{
    /// <summary>
    /// Interaction logic for Contest_Description.xaml
    /// </summary>
    public partial class Contest_Description : UserControl
    {
        public Contest_Description()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Pages.Contest.ContestID;
            Entity.Contest contest = Foobar.Server.GetContest(id);
            var wholeRange = new TextRange(txtDescription.Document.ContentStart, txtDescription.Document.ContentEnd);
            using (MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(contest.Description)))
            {
                wholeRange.Load(mem, DataFormats.Rtf);
            }
        }
    }
}

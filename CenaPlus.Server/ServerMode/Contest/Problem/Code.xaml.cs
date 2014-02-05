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

namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for Code.xaml
    /// </summary>
    public partial class Code : UserControl
    {
        public Code()
        {
            InitializeComponent();
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextEditor.HighLightEdit.HighLight(txtCode);
        }
    }
}

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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using CenaPlus.Entity;
using System.IO;

namespace CenaPlus.Client.Remote
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl, IContent
    {
        public Home()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(App.Server.GetCircular()));
            new TextRange(txtCircular.Document.ContentStart, txtCircular.Document.ContentEnd).Load(mem, DataFormats.Rtf);
            User me = App.Server.GetProfile();
            txtID.Text = "ID: " + me.ID;
            txtName.Text = "Account: " + me.Name;
            txtNickName.Text = me.NickName;
            txtRole.Text = me.Role.ToString();
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }
    }
}

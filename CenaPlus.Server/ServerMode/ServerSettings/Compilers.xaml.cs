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
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Server.Bll;
namespace CenaPlus.Server.ServerMode.ServerSettings
{
    /// <summary>
    /// Interaction logic for Compilers.xaml
    /// </summary>
    public partial class Compilers : UserControl,IContent
    {
        public Compilers()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ConfigHelper.C = TxtC.Text;
            ConfigHelper.CXX = TxtCXX.Text;
            ConfigHelper.CXX11 = TxtCXX11.Text;
            ConfigHelper.Javac = TxtJavac.Text;
            ConfigHelper.Java = TxtJava.Text;
            ConfigHelper.Pascal = TxtPascal.Text;
            ConfigHelper.Python27 = TxtPython27.Text;
            ConfigHelper.Python33 = TxtPython33.Text;
            ConfigHelper.Ruby = TxtRuby.Text;
            ConfigHelper.Dir_fpc = TxtDirFPC.Text;
            ConfigHelper.Dir_gcc = TxtDirGcc.Text;
            ConfigHelper.Dir_gccinc = TxtDirGccInc.Text;
            ConfigHelper.Dir_gcclib = TxtDirGccLib.Text;
            ConfigHelper.Dir_jdk = TxtDirJDK.Text;
            ConfigHelper.Dir_py27 = TxtDirPy27.Text;
            ConfigHelper.Dir_py33 = TxtDirPy33.Text;
            ConfigHelper.Dir_rb = TxtDirRb.Text;

            /*
            App.Server.SetConfig(ConfigKey.Compiler.C, TxtC.Text);
            App.Server.SetConfig(ConfigKey.Compiler.CXX, TxtCXX.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Javac, TxtJavac.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Java, TxtJava.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Pascal, TxtPascal.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Python27, TxtPython27.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Python33, TxtPython33.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Ruby, TxtRuby.Text);
            ModernDialog.ShowMessage("Saved", "Message", MessageBoxButton.OK);*/
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            TxtC.Text = ConfigHelper.C;
            TxtCXX.Text = ConfigHelper.CXX;
            TxtCXX11.Text = ConfigHelper.CXX11;
            TxtJavac.Text = ConfigHelper.Javac;
            TxtJava.Text = ConfigHelper.Java;
            TxtPascal.Text = ConfigHelper.Pascal;
            TxtPython27.Text = ConfigHelper.Python27;
            TxtPython33.Text = ConfigHelper.Python33;
            TxtRuby.Text = ConfigHelper.Ruby;
            TxtDirFPC.Text = ConfigHelper.Dir_fpc;
            TxtDirGcc.Text = ConfigHelper.Dir_gcc;
            TxtDirGccInc.Text = ConfigHelper.Dir_gccinc;
            TxtDirGccLib.Text = ConfigHelper.Dir_gcclib;
            TxtDirJDK.Text = ConfigHelper.Dir_jdk;
            TxtDirPy27.Text = ConfigHelper.Dir_py27;
            TxtDirPy33.Text = ConfigHelper.Dir_py33;
            TxtDirRb.Text = ConfigHelper.Dir_rb;
            /*
            TxtC.Text = App.Server.GetConfig(ConfigKey.Compiler.C) ?? ConfigKey.Compiler.DefaultC;
            TxtCXX.Text = App.Server.GetConfig(ConfigKey.Compiler.CXX) ?? ConfigKey.Compiler.DefaultCXX;
            TxtJavac.Text = App.Server.GetConfig(ConfigKey.Compiler.Javac) ?? ConfigKey.Compiler.DefaultJavac;
            TxtJava.Text = App.Server.GetConfig(ConfigKey.Compiler.Java) ?? ConfigKey.Compiler.DefaultJava;
            TxtPascal.Text = App.Server.GetConfig(ConfigKey.Compiler.Pascal) ?? ConfigKey.Compiler.DefaultPascal;
            TxtPython27.Text = App.Server.GetConfig(ConfigKey.Compiler.Python27) ?? ConfigKey.Compiler.DefaultPython27;
            TxtPython33.Text = App.Server.GetConfig(ConfigKey.Compiler.Python33) ?? ConfigKey.Compiler.DefaultPython33;
            TxtRuby.Text = App.Server.GetConfig(ConfigKey.Compiler.Ruby) ?? ConfigKey.Compiler.DefaultRuby;
             * */
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

    }
}

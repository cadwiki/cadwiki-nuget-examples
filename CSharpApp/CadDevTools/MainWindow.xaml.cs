using cadwiki.NetUtils;
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

namespace CadDevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Hide();
            string solutionDir = Paths.TryGetSolutionDirectoryPath();
            string wildCardFileName = "*" + "MainApp.dll";
            string mainAppDll = Paths.GetNewestDllInVsubfoldersOfSolutionDirectory(solutionDir, wildCardFileName);
            cadwiki.CadDevTools.MainWindow.Dependencies dependencies = new cadwiki.CadDevTools.MainWindow.Dependencies();
            dependencies.DllFilePathsToNetloadCommaDelimited = mainAppDll;
            dependencies.AutoCADExePath = @"C:\Program Files\Autodesk\AutoCAD 2024\acad.exe";
            dependencies.AutoCADStartupSwitches = "/p VANILLA";
            dependencies.CustomDirectoryToSearchForDllsToLoadFrom = solutionDir;
            dependencies.DllWildCardSearchPattern = wildCardFileName;

            Window window = new cadwiki.CadDevTools.MainWindow(dependencies);
            window.ShowDialog();
            this.Close();
        }
    }
}

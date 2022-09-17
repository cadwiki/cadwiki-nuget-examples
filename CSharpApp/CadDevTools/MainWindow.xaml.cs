using cadwiki.DllReloader.AutoCAD;
using cadwiki.NetUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace CadDevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AutoCADAppDomainDllReloader dllReloader = new AutoCADAppDomainDllReloader();
            dllReloader.ClearIni();
            previousAutoCADLocationValue = noneValue;

            acadLocations = new List<string>() { @"C:\Program Files\Autodesk\AutoCAD 2021\acad.exe", @"C:\Program Files\Autodesk\AutoCAD 2022\acad.exe", @"E:\Program Files\Autodesk\AutoCAD 2021\acad.exe", @"E:\Program Files\Autodesk\AutoCAD 2022\acad.exe" };
            // This call is required by the designer.
            this.InitializeComponent();
            ReadCadDevToolsIniFromTemp();
            if (!previousAutoCADLocationValue.Equals(noneValue) & File.Exists(previousAutoCADLocationValue))
            {
                this.ButtonLoadNewest.IsEnabled = true;
                this.ButtonSelectLoad.IsEnabled = true;
                EditRichTextBoxWithAutoCADLocation();
            }
            else
            {
                this.ButtonLoadNewest.IsEnabled = false;
                this.ButtonSelectLoad.IsEnabled = false;
            }
        }

        private string iniFileName = "CadDevToolsSettings.ini";
        private string iniSubFolder = "CadDevTools";
        private string noneValue = "(none)";
        private string previousAutoCADLocationKey = "PREVIOUS-AUTOCAD-LOCATION";
        private string previousAutoCADLocationValue;

        private void ReadCadDevToolsIniFromTemp()
        {
            string iniFilePath = GetCadDevToolsIniFilePath();
            var objIniFile = new IniFile(iniFilePath);
            previousAutoCADLocationValue = objIniFile.GetString("Settings", previousAutoCADLocationKey, noneValue);
            acadLocation = previousAutoCADLocationValue;
        }

        private string GetCadDevToolsIniFilePath()
        {
            string windowTempFolder = System.IO.Path.GetTempPath();
            string iniFolder = windowTempFolder + @"\" + iniSubFolder;
            if (!Directory.Exists(iniFolder))
            {
                Directory.CreateDirectory(iniFolder);
            }
            string iniFilePath = iniFolder + @"\" + iniFileName;
            return iniFilePath;
        }

        private readonly List<string> acadLocations;

        private string acadLocation = "";

        private void ButtonSelectAutoCADYear_Click(object sender, RoutedEventArgs e)
        {
            var window = new cadwiki.WpfUi.WindowGetFilePath(acadLocations);
            window.ShowDialog();
            bool wasOkClicked = window.WasOkayClicked;
            if (wasOkClicked)
            {
                acadLocation = window.SelectedFolder;
                if (File.Exists(acadLocation))
                {
                    this.ButtonLoadNewest.IsEnabled = true;
                    this.ButtonSelectLoad.IsEnabled = true;
                    EditRichTextBoxWithAutoCADLocation();
                    string iniFilePath = GetCadDevToolsIniFilePath();
                    var objIniFile = new IniFile(iniFilePath);
                    objIniFile.WriteString("Settings", previousAutoCADLocationKey, acadLocation);
                }
                else
                {
                    cadwiki.WpfUi.Utils.SetErrorStatus(this.TextBlockStatus, this.TextBlockMessage, "Unable to start AutoCAD because file does not exist: " + acadLocation);
                }
            }

            else
            {
                cadwiki.WpfUi.Utils.SetErrorStatus(this.TextBlockStatus, this.TextBlockMessage, "Unable to start AutoCAD because File selection window was canceled.");
            }
        }

        private void EditRichTextBoxWithAutoCADLocation()
        {
            var flowDoc = new FlowDocument();
            var paragraph1 = new Paragraph();
            paragraph1.Inlines.Add(new Run("Selected program: "));
            paragraph1.Inlines.Add(new Bold(new Run(acadLocation)));
            flowDoc.Blocks.Add(paragraph1);
            var paragraph2 = new Paragraph();
            paragraph2.Inlines.Add(new Run("You can now use the other buttons."));
            flowDoc.Blocks.Add(paragraph2);
            this.RichTextBoxSelectedAutoCAD.Document = flowDoc;
        }

        private void ButtonLoadNewest_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            cadwiki.WpfUi.Utils.SetProcessingStatus(this.TextBlockStatus, this.TextBlockMessage, "Please wait until CAD launches and netloads the most recently built CadApp.dll in your solution directory.");
            string solutionDir = GetSolutionDirectory();
            string wildCardFileName = "*MainApp.dll";
            var cadApps = Paths.GetAllWildcardFilesInVSubfolder(solutionDir, wildCardFileName);
            string cadAppDll = cadApps.FirstOrDefault();
            if (!File.Exists(cadAppDll))
            {
                throw new Exception("Dll does not exist, try building or rebuilding the CadApp.");
            }
            NetLoadDll(cadAppDll);
        }

        private string GetSolutionDirectory()
        {
            string folder = Directory.GetCurrentDirectory();
            string parent = System.IO.Path.GetDirectoryName(folder);
            parent = System.IO.Path.GetDirectoryName(parent);
            parent = System.IO.Path.GetDirectoryName(parent);
            parent = System.IO.Path.GetDirectoryName(parent);
            return parent;
        }

        private void ButtonSelectLoad_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            cadwiki.WpfUi.Utils.SetProcessingStatus(this.TextBlockStatus, this.TextBlockMessage, "Please wait until CAD launches and netloads your selected dll into AutoCAD.");
            string folder = Directory.GetCurrentDirectory();
            string solutionDir = GetSolutionDirectory();
            string wildCardFileName = "*MainApp.dll";
            var cadApps = Paths.GetAllWildcardFilesInVSubfolder(solutionDir, wildCardFileName);

            var window = new cadwiki.WpfUi.WindowGetFilePath(cadApps);
            window.Width = 1200d;
            window.Height = 300d;
            window.ShowDialog();
            bool wasOkClicked = window.WasOkayClicked;
            if (wasOkClicked)
            {
                string filePath = window.SelectedFolder;
                NetLoadDll(filePath);
            }
            else
            {
                cadwiki.WpfUi.Utils.SetErrorStatus(this.TextBlockStatus, this.TextBlockMessage, "User closed dll load menu.");
            }
        }


        private void NetLoadDll(string cadAppDll)
        {

            cadwiki.WpfUi.Utils.SetProcessingStatus(this.TextBlockStatus, this.TextBlockMessage, "Please wait until CAD launches netloads the dll.");
            if (acadLocation.Contains("2021"))
            {
                var interop = new AcadInterop2021.Utils();
                bool isAutoCADRunning = interop.IsAutoCADRunning();
                if (isAutoCADRunning == false)
                {
                    System.Windows.Forms.Application.DoEvents();
                    var processInfo = new ProcessStartInfo() { FileName = acadLocation };
                    interop.StartAutoCADApp(processInfo);
                }
                interop.ConfigureRunningAutoCADForUsage();
                // interop.OpenDrawingTemplate(dwtFilePath, True)
                interop.NetloadDll(cadAppDll);
            }
            else
            {
                cadwiki.WpfUi.Utils.SetErrorStatus(this.TextBlockStatus, this.TextBlockMessage, "Invalid AutoCAD location: " + acadLocation);
            }

            cadwiki.WpfUi.Utils.SetSuccessStatus(this.TextBlockStatus, this.TextBlockMessage, "Dll netload complete: " + cadAppDll);
            System.Windows.Forms.Application.DoEvents();
        }


        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


using System.IO;
using System.Reflection;
using Autodesk.Windows;
using cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons;

namespace MainApp.UiRibbon.Panels
{
    public class Info
    {
        public static RibbonPanel CreateInfoPanel(RibbonButton blankButton)
        {
            var row1 = new RibbonRowPanel();
            var row2 = new RibbonRowPanel();
            var row3 = new RibbonRowPanel();
            var row4 = new RibbonRowPanel();
            var currentIExtensionAppAssembly = Assembly.GetExecutingAssembly();
            string dllName = App.AcadAppDomainDllReloader.GetReloadedAssemblyNameSafely(currentIExtensionAppAssembly);
            string versionNumberStr = "";
            string exeName = "";
            if (App.AcadAppDomainDllReloader.GetReloadCount() >= 1)
            {
                exeName = Path.GetFileName(App.AcadAppDomainDllReloader.GetDllPath());
                versionNumberStr = GenericClickCommandHandler.GetAssemblyVersionFromFullName(dllName);
            }
            else
            {
                string filePath = Assembly.GetExecutingAssembly().Location;
                exeName = Path.GetFileName(filePath);
                versionNumberStr = GenericClickCommandHandler.GetAssemblyVersionFromFullName(currentIExtensionAppAssembly.FullName);
            }
            var versionNumber = CreateVersionNumberButton(versionNumberStr);
            var assemblyName = CreateAssemblyNameButton(exeName);
            var reloadCount = CreateReloadCountButton(exeName);
            var ribbonPanelSource = new RibbonPanelSource();
            ribbonPanelSource.Title = "Info";
            ribbonPanelSource.Items.Add(row1);
            ribbonPanelSource.Items.Add(new RibbonRowBreak());
            ribbonPanelSource.Items.Add(row2);
            ribbonPanelSource.Items.Add(new RibbonRowBreak());
            ribbonPanelSource.Items.Add(row3);
            ribbonPanelSource.Items.Add(new RibbonRowBreak());
            ribbonPanelSource.Items.Add(row4);
            var ribbonPanel = new RibbonPanel();
            ribbonPanel.Source = ribbonPanelSource;
            row1.Items.Add(versionNumber);
            row2.Items.Add(assemblyName);
            row3.Items.Add(reloadCount);
            row4.Items.Add(blankButton);
            row4.Items.Add(blankButton);
            return ribbonPanel;
        }

        private static RibbonButton CreateVersionNumberButton(string versionNumberStr)
        {
            var versionNumber = new RibbonButton();
            versionNumber.Name = "Version";
            versionNumber.ShowText = true;
            versionNumber.Text = " v" + versionNumberStr + " ";
            versionNumber.Size = RibbonItemSize.Standard;
            versionNumber.IsEnabled = false;
            return versionNumber;
        }

        private static RibbonButton CreateReloadCountButton(string exeName)
        {
            var button = new RibbonButton();
            button.Name = "ReloadCount";
            button.ShowText = true;
            button.Text = " Reload Count: " + App.AcadAppDomainDllReloader.GetReloadCount().ToString();
            button.Size = RibbonItemSize.Standard;
            button.CommandHandler = new DllReloadClickCommandHandler();
            button.ToolTip = "Reload the " + exeName + " dll into AutoCAD";
            object[] parameters = new object[] { };

            var uiRouter = new UiRouter(
                "BusinessLogic",
                "BusinessLogic.Commands.HelloFromCadWiki", 
                "Run", 
                parameters, 
                App.AcadAppDomainDllReloader, 
                Assembly.GetExecutingAssembly()
            );
            button.CommandParameter = uiRouter;
            return button;
        }

        private static RibbonButton CreateAssemblyNameButton(string exeName)
        {
            var assemblyName = new RibbonButton();
            assemblyName.Name = "ReleaseStatus";
            assemblyName.ShowText = true;
            assemblyName.Text = " Dll: " + exeName;
            assemblyName.Size = RibbonItemSize.Standard;
            assemblyName.IsEnabled = false;
            return assemblyName;
        }
    }
}
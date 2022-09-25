
using System.Reflection;
using Autodesk.Windows;
using cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons;

namespace MainApp.UiRibbon.Panels
{
    public class Example
    {
        public static string Title = "Example";
        public static string Id = "cadwiki.ExamplePanel";

        public static RibbonPanel CreateExamplePanel(RibbonButton blankButton)
        {
            var ribbonPanelSource = new RibbonPanelSource();
            ribbonPanelSource.Title = Title;
            ribbonPanelSource.Id = Id;
            var exampleButton = ExampleButtons.CreateExampleButton();
            var helloButton = ExampleButtons.CreatHelloButton();
            var pluginButton = ExampleButtons.CreatePluginButton();
            var row1 = new RibbonRowPanel();
            row1.IsTopJustified = true;
            row1.Items.Add(exampleButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(helloButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(pluginButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(blankButton);
            ribbonPanelSource.Items.Add(row1);
            var exportersPanel = new RibbonPanel();
            exportersPanel.Source = ribbonPanelSource;
            return exportersPanel;
        }
    }

    public partial class ExampleButtons
    {
        public static RibbonButton CreateExampleButton()
        {
            var ribbonButton = new RibbonButton();
            ribbonButton.Name = "Example - Run MyCommand";
            ribbonButton.ShowText = true;
            ribbonButton.Text = "Example - Run MyCommand";
            ribbonButton.Size = RibbonItemSize.Standard;
            var uiRouter = new UiRouter(
                "BusinessLogic",
                "BusinessLogic.Commands.Example", 
                "Run", 
                null, 
                App.AcadAppDomainDllReloader, 
                Assembly.GetExecutingAssembly());
            ribbonButton.CommandParameter = uiRouter;
            ribbonButton.CommandHandler = new GenericClickCommandHandler();
            ribbonButton.ToolTip = "Click to run MyCommand using the CommandHandler And Command Parameter bound to this UiRibbon button";
            return ribbonButton;
        }

        public static string HelloButtonId = "Hello";

        public static RibbonButton CreatHelloButton()
        {
            var ribbonButton = new RibbonButton();
            ribbonButton.Name = "Hello";
            ribbonButton.Id = HelloButtonId;
            ribbonButton.ShowText = true;
            ribbonButton.Text = "Hello";
            ribbonButton.Size = RibbonItemSize.Standard;
            //start here 5 - UiRouter
            //The UiRouter contains all the information necessary for the AutoCADAppDomainDllReloader to
            //parse a dll in the current app domain, and call the method you want to call
            //the AutoCADAppDomainDllReloader will call your method from the most recently reloaded dll
            var uiRouter = new UiRouter(
                "BusinessLogic",
                "BusinessLogic.Commands.HelloFromCadWiki", 
                "Run", 
                null, 
                App.AcadAppDomainDllReloader, 
                Assembly.GetExecutingAssembly());
            //start here 6 - RibbonButton CommandParameter = uiRouter
            //the UiRouter is stored on the ribbonButton.CommandParameter
            ribbonButton.CommandParameter = uiRouter;
            //start here 7 - GenericClickCommandHandler
            //the GenericClickCommandHandler handles all Execute calls by utilizing the CommandParameter above
            ribbonButton.CommandHandler = new GenericClickCommandHandler();
            ribbonButton.ToolTip = "Click to run HelloFromCadWiki";
            return ribbonButton;
        }

        public static RibbonButton CreatePluginButton()
        {
            var ribbonButton = new RibbonButton();
            ribbonButton.Name = "Example - Run Plugin Method";
            ribbonButton.ShowText = true;
            ribbonButton.Text = "Example - Run Plugin Method";
            ribbonButton.Size = RibbonItemSize.Standard;
            var uiRouter = new UiRouter(
                "Plugin",
                "Plugin.MyCommands",
                "MyCommand2",
                null,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly());
            ribbonButton.CommandParameter = uiRouter;
            ribbonButton.CommandHandler = new GenericClickCommandHandler();
            ribbonButton.ToolTip = "Click to run MyCommand using the CommandHandler And Command Parameter bound to this UiRibbon button";
            return ribbonButton;
        }

    }
}
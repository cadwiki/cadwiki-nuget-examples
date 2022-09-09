
using System.Reflection;
using Autodesk.Windows;
using cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons;

namespace MainApp.UiRibbon.Panels
{
    public class Example
    {
        public static RibbonPanel CreateExamplePanel(RibbonButton blankButton)
        {
            var ribbonPanelSource = new RibbonPanelSource();
            ribbonPanelSource.Title = "Example";
            var exampleButton = ExampleButtons.CreateExampleButton();
            var helloButton = ExampleButtons.CreatHelloButton();
            var row1 = new RibbonRowPanel();
            row1.IsTopJustified = true;
            row1.Items.Add(exampleButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(helloButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(blankButton);
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
            var uiRouter = new UiRouter("BusinessLogic.Commands.Example", "Run", null, App.AcadAppDomainDllReloader, Assembly.GetExecutingAssembly());
            ribbonButton.CommandParameter = uiRouter;
            ribbonButton.CommandHandler = new GenericClickCommandHandler();
            ribbonButton.ToolTip = "Click to run MyCommand using the CommandHandler And Command Parameter bound to this UiRibbon button";
            return ribbonButton;
        }

        public static RibbonButton CreatHelloButton()
        {
            var ribbonButton = new RibbonButton();
            ribbonButton.Name = "Hello";
            ribbonButton.ShowText = true;
            ribbonButton.Text = "Hello";
            ribbonButton.Size = RibbonItemSize.Standard;
            var uiRouter = new UiRouter("BusinessLogic.Commands.HelloFromCadWiki", "Run", null, App.AcadAppDomainDllReloader, Assembly.GetExecutingAssembly());
            ribbonButton.CommandParameter = uiRouter;
            ribbonButton.CommandHandler = new GenericClickCommandHandler();
            ribbonButton.ToolTip = "Click to run HelloFromCadWiki";
            return ribbonButton;
        }

    }
}
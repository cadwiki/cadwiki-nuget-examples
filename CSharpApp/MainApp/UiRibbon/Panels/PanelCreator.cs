
using Autodesk.Windows;

namespace MainApp.UiRibbon.Panels
{
    public class PanelCreator
    {
        public static void AddAllPanels(RibbonTab ribbonTab)
        {
            var blankButton = new RibbonButton();
            blankButton.Name = "BlankButton1";
            blankButton.Size = RibbonItemSize.Standard;
            blankButton.IsEnabled = false;
            var infoRibbonPanel = Info.CreateInfoPanel(blankButton);
            ribbonTab.Panels.Add(infoRibbonPanel);
            var examplePanel = Example.CreateExamplePanel(blankButton);
            ribbonTab.Panels.Add(examplePanel);
            var testPanel = Test.CreateTestsPanel(blankButton);
            ribbonTab.Panels.Add(testPanel);
        }
    }
}

using Autodesk.Windows;

namespace MainApp.UiRibbon.Tabs
{
    public class AppTab
    {
        private static readonly string tabName = "AppTab";

        public static RibbonTab CreateAppTab()
        {
            var ribbonTab = new RibbonTab();
            ribbonTab.Title = tabName;
            ribbonTab.Id = tabName;
            ribbonTab.Name = tabName;
            return ribbonTab;
        }
    }
}
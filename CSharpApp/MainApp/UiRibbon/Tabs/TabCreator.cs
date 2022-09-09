using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using MainApp.UiRibbon.Panels;
using Microsoft.VisualBasic;

namespace MainApp.UiRibbon.Tabs
{
    public class TabCreator
    {
        public static void AddTabs(Document doc)
        {
            var appRibbonTab = AppTab.CreateAppTab();
            PanelCreator.AddAllPanels(appRibbonTab);
            var allRibbonTabs = new List<RibbonTab>(new RibbonTab[] { appRibbonTab });
            AddTabs(doc, allRibbonTabs);
        }

        private static void AddTabs(Document doc, List<RibbonTab> ribbonTabs)
        {
            doc.Editor.WriteMessage(Constants.vbLf + "Adding tabs...");
            foreach (RibbonTab ribbonTab in ribbonTabs)
                AddTab(doc, ribbonTab);
        }

        private static void AddTab(Document doc, RibbonTab ribbonTab)
        {
            doc.Editor.WriteMessage(Constants.vbLf + "Add tab...");
            var ribbonControl = ComponentManager.Ribbon;
            if (ribbonControl != null & ribbonTab != null)
            {
                if (ribbonTab.Name == null)
                {
                    throw new Exception("Ribbon Tab does not have a name. Please add a name to the ribbon tab.");
                }
                string tabName = ribbonTab.Name;
                doc.Editor.WriteMessage(Constants.vbLf + tabName);
                var doesTabAlreadyExist = ribbonControl.FindTab(tabName);
                if (doesTabAlreadyExist != null)
                {
                    ribbonControl.Tabs.Remove(doesTabAlreadyExist);
                }
                ribbonControl.Tabs.Add(ribbonTab);
                ribbonTab.IsActive = true;
            }
        }
    }
}
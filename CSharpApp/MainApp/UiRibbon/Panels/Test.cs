﻿
using System.Reflection;
using Autodesk.Windows;
using cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons;

namespace MainApp.UiRibbon.Panels
{
    public class Test
    {

        public static RibbonPanel CreateTestsPanel(RibbonButton blankButton)
        {
            var integrationTestsButton = CreateRegressionTestsButton();
            var sampleButton = CreateSampleButton();
            var ribbonPanelSource = new RibbonPanelSource();
            ribbonPanelSource.Title = "Tests";
            var row1 = new RibbonRowPanel();
            row1.IsTopJustified = true;
            row1.Items.Add(integrationTestsButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(sampleButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(blankButton);
            row1.Items.Add(new RibbonRowBreak());
            row1.Items.Add(blankButton);
            ribbonPanelSource.Items.Add(row1);
            var ribbonPanel = new RibbonPanel();
            ribbonPanel.Source = ribbonPanelSource;
            return ribbonPanel;
        }

        public static RibbonButton CreateRegressionTestsButton()
        {
            var ribbonButton = new RibbonButton();
            ribbonButton.Name = "Regression Tests";
            ribbonButton.ShowText = true;
            ribbonButton.Text = "Regression Tests";

            var allRegressionTests = typeof(RegressionTests.RegressionTests);
            var allIntegrationTests = typeof(IntegrationTests.Tests);
            var allRegressionTestTypes = new[] { allRegressionTests, allIntegrationTests };
            var uiRouter = new UiRouter(
                "MainApp",    
                "MainApp.Workflows.NUnitTestRunner", 
                "Run", 
                new[] { (allRegressionTestTypes) }, 
                App.AcadAppDomainDllReloader, 
                Assembly.GetExecutingAssembly()
            );
            ribbonButton.CommandParameter = uiRouter;
            ribbonButton.CommandHandler = new GenericClickCommandHandler();
            ribbonButton.ToolTip = "Runs regression tests from the current .dll";
            return ribbonButton;
        }


        public static RibbonButton CreateSampleButton()
        {
            var ribbonButton = new RibbonButton();
            ribbonButton.Name = "Sample";
            ribbonButton.ShowText = true;
            ribbonButton.Text = "Sample";

            var uiRouter = new UiRouter(
                "YourDllAssemblyName",
                "YourClassName",
                "YourPublicStaticRunMethod",
                null,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly()
            );
            ribbonButton.CommandParameter = uiRouter;
            ribbonButton.CommandHandler = new GenericClickCommandHandler();
            ribbonButton.ToolTip = "Sample command";
            return ribbonButton;
        }
    }
}
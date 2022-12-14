using Autodesk.Windows;
using Microsoft.VisualBasic;

using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using static System.Windows.Automation.AutomationElement;
using Application = System.Windows.Forms.Application;
using Microsoft.Test.Input;
using System.Drawing;
using System.Windows;
using System.Drawing.Imaging;
using cadwiki.NUnitTestRunner.TestEvidence;
using cadwiki.NUnitTestRunner.Creators;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;

namespace MainApp.IntegrationTests
{
    [TestFixture]
    public class Tests
    {

        [SetUp]
        public void Init()
        {
            cadwiki.AutoCAD2021.Base.Utilities.Commands.SendLispCommandStartUndoMark();
        }

        [TearDown]
        public void TearDown()
        {
            cadwiki.AutoCAD2021.Base.Utilities.Commands.SendLispCommandEndUndoMark();
            cadwiki.AutoCAD2021.Base.Utilities.Commands.SendLispCommandUndoBack();
        }


        [Test]
        public void Test_Is1EqualTo1_ShouldPass()
        {
            Assert.AreEqual(1, 1, "Test failed");
        }




        [Test]
        public void Test_ClickUiRibbonHelloWorld_ShouldAddScreenshotToPdf()
        {
            var ribbonControl = ComponentManager.Ribbon;
            RibbonTab appTab = ribbonControl.FindTab("AppTab");
            RibbonPanel examplePanel = appTab.FindPanel(UiRibbon.Panels.Example.Id);
            RibbonItem item = examplePanel.FindItem(UiRibbon.Panels.ExampleButtons.HelloButtonId);
            RibbonButton ribbonButton = (RibbonButton)item;
            //simulate a Ui click by calling Execute on the Ribbon button command handler
            ribbonButton.CommandHandler.Execute(ribbonButton);
            Application.DoEvents();

            var testEvidenceCreator = new TestEvidenceCreator();
            IntPtr windowIntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("Hello from Cadwiki v53");
            var evidence = new Evidence();
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "Title");
            String controlName = "ButtonOk";
            testEvidenceCreator.MicrosoftTestClickUiControl(windowIntPtr, controlName);

            evidence = testEvidenceCreator.GetEvidenceForCurrentTest();
            Assert.IsTrue(System.IO.File.Exists(evidence.Images[0].FilePath), "jpeg was not created.");

            Assert.AreEqual(1, 1, "Test failed");
        }

        [Test]
        public void Test_ClickUiRibbonHelloWorld_ShouldAddSecondScreenShotToPdf()
        {
            var ribbonControl = ComponentManager.Ribbon;
            RibbonTab appTab = ribbonControl.FindTab("AppTab");
            RibbonPanel examplePanel = appTab.FindPanel(UiRibbon.Panels.Example.Id);
            RibbonItem item = examplePanel.FindItem(UiRibbon.Panels.ExampleButtons.HelloButtonId);
            RibbonButton ribbonButton = (RibbonButton)item;
            //simulate a Ui click by calling Execute on the Ribbon button command handler
            ribbonButton.CommandHandler.Execute(ribbonButton);
            Application.DoEvents();

            var testEvidenceCreator = new TestEvidenceCreator();
            IntPtr windowIntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("Hello from Cadwiki v53");
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "Title");
            String controlName = "ButtonOk"; ;
            testEvidenceCreator.MicrosoftTestClickUiControl(windowIntPtr, controlName);
            var evidence = testEvidenceCreator.GetEvidenceForCurrentTest();
            Assert.IsTrue(System.IO.File.Exists(evidence.Images[0].FilePath), "jpeg was not created.");
        }

        [Test]
        public async Task<Object> Test_LongRunningHorizontalLineDraw_ShouldAddScreenShotToPdf()
        {
            await DelayedWork();
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            List<object> parameters = new List<object>()
            {
                "_.LINE",
                "0,0",
                "1,0",
                ""
            };

            await DrawLine(doc, parameters);
            await ZoomExtents();

            var testEvidenceCreator = new TestEvidenceCreator();
            IntPtr windowIntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("Autodesk AutoCAD");
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "After draw line async");
            var evidence = testEvidenceCreator.GetEvidenceForCurrentTest();
            Assert.IsTrue(System.IO.File.Exists(evidence.Images[0].FilePath), "jpeg was not created.");
            return null;
        }

        [Test]
        public async Task<Object> Test_LongRunningVerticalLineDraw_ShouldAddScreenShotToPdf()
        {
            await DelayedWork();
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            List<object> parameters = new List<object>()
            {
                "_.LINE",
                "0,0",
                "0,1",
                ""
            };

            await DrawLine(doc, parameters);
            await ZoomExtents();

            var testEvidenceCreator = new TestEvidenceCreator();
            IntPtr windowIntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("Autodesk AutoCAD");
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "After draw line async");
            var evidence = testEvidenceCreator.GetEvidenceForCurrentTest();
            Assert.IsTrue(System.IO.File.Exists(evidence.Images[0].FilePath), "jpeg was not created.");
            return null;
        }

        private static async Task DrawLine(Document doc, List<object> parameters)
        {
            await Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.ExecuteInCommandContextAsync(
                async (obj) =>
                {
                    await doc.Editor.CommandAsync(parameters.ToArray());
                },
            null);
        }

        private static async Task ZoomExtents()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            List<object> parameters = new List<object>()
            {
                "_.ZOOM",
                "EXTENTS"
            };
            await Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.ExecuteInCommandContextAsync(
                async (obj) =>
                {
                    await doc.Editor.CommandAsync(parameters.ToArray());
                },
            null);
        }

        private async Task DelayedWork()
        {
            await Task.Delay(5000);
        }
    }






}




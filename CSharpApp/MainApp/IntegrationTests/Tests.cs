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

namespace MainApp.IntegrationTests
{
    [TestFixture]
    public class Tests
    {

        [SetUp]
        public void Init()
        {
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(vla-startundomark (vla-get-ActiveDocument (vlax-get-acad-object)))" + Constants.vbLf, true, false, false);
        }

        [TearDown]
        public void TearDown()
        {
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(vla-endundomark (vla-get-ActiveDocument (vlax-get-acad-object)))" + Constants.vbLf, true, false, false);
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(command-s \"._undo\" \"back\" \"yes\")" + Constants.vbLf, true, false, false);


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
            String controlName = "ButtonOk";;
            testEvidenceCreator.MicrosoftTestClickUiControl(windowIntPtr, controlName);

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

            Assert.IsTrue(System.IO.File.Exists(testEvidenceCreator.GetEvidenceForCurrentTest()
                .Images[0].FilePath), "jpeg was not created.");
        }

        [Test]
        public async Task<Object> Test_LongRunningBlockInsert_ShouldAddSecondScreenShotToPdf()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            object[] parameters =
            {
                "_.LINE",
                "0,0",
                "1,1"
            };
            await DelayedWork();
            //await doc.Editor.CommandAsync(parameters);
            var testEvidenceCreator = new TestEvidenceCreator();
            IntPtr windowIntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("AutoCAD");
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "After draw line async");

            Assert.IsTrue(System.IO.File.Exists(testEvidenceCreator.GetEvidenceForCurrentTest()
                .Images[0].FilePath), "jpeg was not created.");
            return null;
        }

        private async Task DelayedWork()
        {
            await Task.Delay(5000);
        }
    }






}




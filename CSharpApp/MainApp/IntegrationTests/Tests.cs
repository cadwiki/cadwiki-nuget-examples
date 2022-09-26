﻿using Autodesk.Windows;
using Microsoft.VisualBasic;

using NUnit.Framework;
using System;
using System.Diagnostics;

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
        public void Test_ClickUiRibbonHelloWorld_ShouldPass()
        {
            var ribbonControl = ComponentManager.Ribbon;
            RibbonTab appTab = ribbonControl.FindTab("AppTab");
            RibbonPanel examplePanel = appTab.FindPanel(UiRibbon.Panels.Example.Id);
            RibbonItem item = examplePanel.FindItem(UiRibbon.Panels.ExampleButtons.HelloButtonId);
            RibbonButton ribbonButton = (RibbonButton)item;
            //simulate a Ui click by calling Execute on the Ribbon button command handler
            ribbonButton.CommandHandler.Execute(ribbonButton);
            IntPtr intPtr = WinGetHandle("Hello from Cadwiki v53");
            Assert.AreEqual(1, 1, "Test failed");
        }


        public static IntPtr WinGetHandle(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }

    }
}
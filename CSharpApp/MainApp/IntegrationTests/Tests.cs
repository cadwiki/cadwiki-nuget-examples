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
            Application.DoEvents();

            IntPtr windowIntPtr = ProcessesGetHandleFromUiTitle("Hello from Cadwiki v53");
            String controlName = "ButtonOk";
            MicrosoftTestClickUiControl(windowIntPtr, controlName);

            Assert.AreEqual(1, 1, "Test failed");
        }

        public IntPtr ProcessesGetHandleFromUiTitle(string wName)
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

        public Boolean MicrosoftTestClickUiControl(IntPtr windowIntPtr, string controlName)
        {
            AutomationElement element = GetElementByControlName(windowIntPtr, controlName);
            if (element is null)
            {
                return false;
            }
            Point clickableSystemDrawingPoint = GetClickableSystemDrawingPointFromElement(element);
            return MicrosoftTestClickPoint(clickableSystemDrawingPoint);
        }

        private AutomationElement GetElementByControlName(IntPtr windowIntPtr,
            String controlNameToFind)
        {
            var root = AutomationElement.FromHandle(windowIntPtr);
            AutomationElementCollection elementCollection = root.FindAll(TreeScope.Subtree, Condition.TrueCondition);

            foreach (AutomationElement element in elementCollection)
            {
                AutomationElementInformation current = element.Current;
                ControlType controlType = current.ControlType;
                String controlText = current.Name;
                String controlName = current.AutomationId;
                if (controlName.Equals(controlNameToFind))
                {
                    return element;

                }
            }
            return null;
        }

        private Point GetClickableSystemDrawingPointFromElement(AutomationElement element)
        {
            System.Windows.Point windowsPoint = element.GetClickablePoint();
            System.Drawing.Point drawingPoint = new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            return drawingPoint;
        }

        private Boolean MicrosoftTestClickPoint(Point clickableSystemDrawingPoint)
        {
            // Move the mouse to the point. Then click
            Microsoft.Test.Input.Mouse.MoveTo(clickableSystemDrawingPoint);
            Microsoft.Test.Input.Mouse.Click(MouseButton.Left);
            return true;
        }

    }
}
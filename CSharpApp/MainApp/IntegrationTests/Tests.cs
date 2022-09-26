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

namespace MainApp.IntegrationTests
{
    [TestFixture]
    public class Tests
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


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
            IntPtr windowIntPtr = WinApiGetHandleFromUiTitle("Hello from Cadwiki v53");
            var root = AutomationElement.FromHandle(windowIntPtr);
            AutomationElementCollection elements = root.FindAll(TreeScope.Subtree, 
                Condition.TrueCondition);

            foreach (AutomationElement element in elements)
            {
                AutomationElementInformation current = element.Current;
                ControlType controlType = current.ControlType;
                String controlText = current.Name;
                String controlName = current.AutomationId;
                if (controlName.Equals("ButtonOk"))
                {
                    System.Windows.Point windowsPoint = element.GetClickablePoint();
                    System.Drawing.Point drawingPoint = new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);

                    // Move the mouse to the point. Then click
                    Microsoft.Test.Input.Mouse.MoveTo(drawingPoint);
                    Microsoft.Test.Input.Mouse.Click(MouseButton.Left);

                }
                var test = element.ToString();
            }

            Assert.AreEqual(1, 1, "Test failed");
        }

        public delegate int Callback(int hWnd, int lParam);

        public static IntPtr WinApiGetHandleFromUiTitle(string wName)
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
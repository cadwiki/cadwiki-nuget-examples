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
            String screenshotPath = "C:\\Temp\\test.jpg";
            ImageFormat format = ImageFormat.Jpeg;

            PrintWindowToImage(windowIntPtr, screenshotPath, format);
            MicrosoftTestClickUiControl(windowIntPtr, controlName);

            Assert.AreEqual(1, 1, "Test failed");
        }

        public static void PrintWindowToImage(IntPtr windowIntPtr, string screenshotPath, ImageFormat format)
        {
            Bitmap screenshot = PrintWindow(windowIntPtr);
            screenshot.Save(screenshotPath, format);
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public static Bitmap PrintWindow(IntPtr hwnd)
        {
            RECT rc;
            GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
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
            System.Drawing.Point clickableSystemDrawingPoint = GetClickableSystemDrawingPointFromElement(element);
            return MicrosoftTestClickPoint(clickableSystemDrawingPoint);
        }

        private AutomationElement GetElementByControlName(IntPtr windowIntPtr,
            String controlNameToFind)
        {
            var root = AutomationElement.FromHandle(windowIntPtr);
            AutomationElementCollection elementCollection = root.FindAll(TreeScope.Subtree, 
                System.Windows.Automation.Condition.TrueCondition);

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

        private System.Drawing.Point GetClickableSystemDrawingPointFromElement(AutomationElement element)
        {
            System.Windows.Point windowsPoint = element.GetClickablePoint();
            System.Drawing.Point drawingPoint = new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            return drawingPoint;
        }

        private Boolean MicrosoftTestClickPoint(System.Drawing.Point clickableSystemDrawingPoint)
        {
            // Move the mouse to the point. Then click
            Microsoft.Test.Input.Mouse.MoveTo(clickableSystemDrawingPoint);
            Microsoft.Test.Input.Mouse.Click(MouseButton.Left);
            return true;
        }

    }








    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        private int _Left;
        private int _Top;
        private int _Right;
        private int _Bottom;

        public RECT(RECT Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        {
        }
        public RECT(int Left, int Top, int Right, int Bottom)
        {
            _Left = Left;
            _Top = Top;
            _Right = Right;
            _Bottom = Bottom;
        }

        public int X
        {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Y
        {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Left
        {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Top
        {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Right
        {
            get { return _Right; }
            set { _Right = value; }
        }
        public int Bottom
        {
            get { return _Bottom; }
            set { _Bottom = value; }
        }
        public int Height
        {
            get { return _Bottom - _Top; }
            set { _Bottom = value + _Top; }
        }
        public int Width
        {
            get { return _Right - _Left; }
            set { _Right = value + _Left; }
        }
        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set
            {
                _Left = value.X;
                _Top = value.Y;
            }
        }
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set
            {
                _Right = value.Width + _Left;
                _Bottom = value.Height + _Top;
            }
        }

        public static implicit operator Rectangle(RECT Rectangle)
        {
            return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
        }
        public static implicit operator RECT(Rectangle Rectangle)
        {
            return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
        }
        public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
        {
            return Rectangle1.Equals(Rectangle2);
        }
        public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
        {
            return !Rectangle1.Equals(Rectangle2);
        }

        public override string ToString()
        {
            return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public bool Equals(RECT Rectangle)
        {
            return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
        }

        public override bool Equals(object Object)
        {
            if (Object is RECT)
            {
                return Equals((RECT)Object);
            }
            else if (Object is Rectangle)
            {
                return Equals(new RECT((Rectangle)Object));
            }

            return false;
        }
    }
}




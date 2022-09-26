
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.UiRibbon.Panels
{

    //https://p2p.wrox.com/c/37119-simulating-button-click-using-win32-api-net.html
    //class3



    //https://stackoverflow.com/questions/16196272/get-all-controls-by-findwindowex

    public class Class2
    {
        public const int BM_CLICK = 0x00F5;
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;

        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            list.Add(handle);
            return true;
        }

        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                Win32Callback childProc = new Win32Callback(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static string GetWinClass(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return null;
            StringBuilder classname = new StringBuilder(100);
            IntPtr result = GetClassName(hwnd, classname, classname.Capacity);
            if (result != IntPtr.Zero)
                return classname.ToString();
            return null;
        }

        public static IEnumerable<IntPtr> EnumAllWindows(IntPtr hwnd)
        {
            List<IntPtr> children = GetChildWindows(hwnd);
            if (children == null)
                yield break;
            foreach (IntPtr child in children)
            {
                yield return child;
            }
        }
    }


    //https://social.msdn.microsoft.com/Forums/en-US/89135a11-d324-4a07-8ffd-5a081ac7f23d/how-to-get-name-of-button-by-api-vbnet?forum=vbgeneral
    public class Class1
    {
        [DllImport("user32.dll", EntryPoint = "FindWindowW")]
        private static extern IntPtr FindWindowW([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int EnumCallBackDelegate(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "EnumChildWindows")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumCallBackDelegate lpEnumFunc, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern int SendMessageW(IntPtr hWnd, uint Msg, int wParam, [MarshalAs(UnmanagedType.LPTStr)] string lParam);

        [DllImport("user32.dll", EntryPoint = "GetClassNameW")]
        public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpClassName, int nMaxCount);



        private const int WM_GETTEXT = 0xD;
        private const int WM_GETTEXTLENGTH = 0xE;
        private List<ChildWindow> ChildWindows = new List<ChildWindow>(); // this List will contain the text of all the controls in the 3rd party program

        // this function is used by the EnumChildWindows function to iterate through all the child windows of the window you passed to the function
        private int EnumChildWindowProc(IntPtr hwnd, IntPtr lParam)
        {

            // get the Class Name of the child window
            System.Text.StringBuilder classname = new System.Text.StringBuilder(256);
            GetClassNameW(hwnd, classname, 256);

            // get the length of the text of the child window
            int txtlen = SendMessageW(hwnd, WM_GETTEXTLENGTH, 0, null);

            // get the text of the child window
            string txt = Strings.Space(txtlen + 1); // create a string that is the length of the text + 1
            SendMessageW(hwnd, WM_GETTEXT, txtlen + 1, txt); // get the text of the window

            // create a new instance of the ChildWindow class and set the Handle, Text, and Class Name of the current child window
            ChildWindow cw = new ChildWindow();
            cw.cwHandle = hwnd;
            cw.cwText = txt.Remove(txt.IndexOf(Strings.Chr(0)));
            cw.cwClassName = classname.ToString();
            ChildWindows.Add(cw); // add the ChildWindow class to the ChildWindows List

            return 1; // must return 1 to keep iterating through the child windows.
        }
    }

    public class ChildWindow
    {
        public IntPtr cwHandle;
        public string cwText;
        public string cwClassName;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenAppsProject.Windows
{
    internal class ShowDesktop
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWMINIMIZED = 2;

        public static void MinimizeAll()
        {
            IntPtr handle = GetForegroundWindow();
            ShowWindowAsync(handle, SW_SHOWMINIMIZED);
            SendKeys.SendWait("^{ESC}"); // Simulate Ctrl+Esc to show the start menu (alternative to Win key)
            SendKeys.SendWait("^D"); // Ctrl+D to show desktop
        }
    }
}


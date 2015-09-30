using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace GuoKun.Configuration
{
    public class SingletonApplication : Application
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Process current = Process.GetCurrentProcess();
            bool createdNew = true;
            _mutex = new Mutex(true, current.ProcessName, out createdNew);
            if (!createdNew)
            {
                foreach (Process p in Process.GetProcessesByName(current.ProcessName))
                {
                    if (p.Id != current.Id)
                    {
                        SetForegroundWindow(p.MainWindowHandle);
                        ShowWindowAsync(p.MainWindowHandle, 9);
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}

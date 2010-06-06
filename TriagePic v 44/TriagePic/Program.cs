using System;
using System.Threading; // for Mutex
using System.Collections.Generic;
using System.Windows.Forms;

namespace TriagePicNamespace
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool instantiated = false;
            // Local means in per-session namespace; alternative is Global
            string mutexName = "Local\\" +  System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            Mutex mutex = new Mutex (false, mutexName, out instantiated);
            if (instantiated)
            {
                // If instantiated is true, this is the first instance 
                // of the application; else, another instance is running. 
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TriagePic());
            }
            else
            {
                MessageBox.Show("Sorry, only one instance of TriagePic can run at a time.");
                // If instead we wanted to quietly use the existing instance, could move focus to it:
                // Process current = Process.GetCurrentProcess();
                // foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                // {
                //      if (process.Id != current.Id)
                //      {
                //          SetForegroundWindow(process.MainWindowHandle);
                //          break;
                //      }
                // }
                Application.Exit();
            }
            GC.KeepAlive(mutex); // Needed since mutex itself isn't static
        }
    }
}
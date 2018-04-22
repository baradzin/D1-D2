using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Module4_BCL
{
    class Program
    {
        private static bool keepRunning = true;

        static void Main(string[] args)
        {
            InitSystemEvents();
            Console.WriteLine("Enter Ctrl+C or Ctrl+Break to exit");

            FileSystemWatcherManager FSWM = new FileSystemWatcherManager();
            FSWM.RunSystemWatchers();

            while (keepRunning) { }
            Console.WriteLine("Exited successfully");
        }

        private static void InitSystemEvents()
        {
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                keepRunning = false;
            };
        }
    }
}

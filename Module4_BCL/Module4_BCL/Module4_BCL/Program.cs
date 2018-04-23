using System;
using System.Configuration;
using System.Globalization;

namespace Module4_BCL
{
    class Program
    {
        private static bool keepRunning = true;

        static void Main(string[] args)
        {
            InitSystemEvents();
            Console.WriteLine(Resources.ExitHotKeysMessage);

            FileSystemWatcherManager FSWM = new FileSystemWatcherManager();
            FSWM.RunSystemWatchers();

            while (keepRunning) { }
            Console.WriteLine(Resources.ExitMessage);
        }

        private static void InitSystemEvents()
        {
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                keepRunning = false;
            };

            var cultureInfo = new CultureInfo(ConfigurationManager.AppSettings["Localisation"]);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}

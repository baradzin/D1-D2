using System;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace Module4_BCL
{
    class Program
    {
        private static bool keepRunning = true;
        public static TimeZoneInfo TIMEZONE = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["TimeZone"]);

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
            Console.OutputEncoding = Encoding.UTF8;
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

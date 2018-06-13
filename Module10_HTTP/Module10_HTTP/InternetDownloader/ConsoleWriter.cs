using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetDownloaderLib
{
    public class ConsoleWriter : ILogger
    {
        public bool IsVerbose { get; set; } = true;

        public void Error(string str)
        {
            ConsoleWriteLine("ERROR: " +str);
        }

        public void Write(string str)
        {
            ConsoleWriteLine(str);
        }

        public void WriteLine(string str)
        {
            ConsoleWriteLine(str);
        }

        private void ConsoleWriteLine(string str)
        {
            if (IsVerbose) {
                Console.WriteLine(str);
            }            
        }
    }
}

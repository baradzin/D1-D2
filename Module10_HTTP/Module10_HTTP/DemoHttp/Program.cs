using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InternetDownloaderLib;

namespace DemoHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleWriter();
            var downloader = new InternetDownloader(@"https://www.google.com/",
                @"E:\Sites", 1, true,
                CrossingOption.OnlyInternalRecources, new []{"png"}, logger);
            downloader.DownloadContent().Wait();
            
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetDownloaderLib;

namespace DemoHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloader = new InternetDownloader(@"https://www.google.com/",
                @"D:\D1-D2\Module10_HTTP\Module10_HTTP\Sites", 1, true,
                CrossingOption.AllRecources, null);
            downloader.DownloadContent();
        }
    }
}

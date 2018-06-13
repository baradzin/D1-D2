using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetDownloaderLib
{
    public class FileSaver
    {
        private ILogger _logger;
        public FileSaver(ILogger logger)
        {
            this._logger = logger;
        }

        public void SaveSourceHtml(string source, string urlName, string rootFolder)
        {
            _logger.WriteLine($"Source from {urlName} found");
            try
            {
                var fullName = Path.Combine(rootFolder, "_" + urlName + ".html");

                using (FileStream fs = File.Create(fullName))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(source);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    _logger.WriteLine($"File {fullName} was created");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error while downloading");
            }
        }

        //public void SaveSourceImage()
    }
}

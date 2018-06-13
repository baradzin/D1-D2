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
        public FileSaver()
        {

        }

        public void SaveSourceHtml(string source, string urlName, string rootFolder)
        {
            Console.WriteLine("Source found");
            try
            {
                var fullName = Path.Combine(rootFolder, "_" + urlName);

                using (FileStream fs = File.Create(fullName + ".html"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(source);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while downloading");
            }
        }

        //public void SaveSourceImage()
    }
}

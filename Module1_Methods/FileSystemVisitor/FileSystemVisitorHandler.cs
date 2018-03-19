using System;
using System.Collections.Generic;
using System.Text;

namespace FileSystemVisitor
{
    public class FileSystemVisitorHandler
    {
        public void Start_Progress()
        {
            Console.WriteLine("Start searching files");
        }

        public void Finished_Progress()
        {
            Console.WriteLine("Search finished");
        }

        public bool FileFinded_ActionRequired(string fileName)
        {
            Console.WriteLine("Searching file was found, if you want to finish searching press Y");
            string key = Console.ReadLine();
            if (key.ToUpper().Equals("Y"))
            {
                return true;
            }
            return false;
        }

        public bool DirectoryFinded_ActionRequired(string directoryName)
        {
            Console.WriteLine("Searching directory was found, if you want to finish searching press Y");
            string key = Console.ReadLine();
            if (key.ToUpper().Equals("Y"))
            {
                return true;
            }
            return false;
        }
    }
}

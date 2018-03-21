using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace FileSystemVisitor
{
    public class FileSystemVisitorEventArgs
    {
        public string Message { get; set; }
        public File File { get; set; }

        public FileSystemVisitorEventArgs() { }

        public FileSystemVisitorEventArgs(File file)
        {
            this.File = file;
        }

        public FileSystemVisitorEventArgs(string message)
        {
            this.Message = message;
        }

        public void ProgressWriter(object sender, FileSystemVisitorEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public void FilePathWriter(object sender, FileSystemVisitorEventArgs e)
        {
            Console.WriteLine(e.File.Path);
        }
    }
}

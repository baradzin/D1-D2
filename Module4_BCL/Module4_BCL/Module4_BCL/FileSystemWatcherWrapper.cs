using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Module4_BCL
{
    internal class FileSystemWatcherWrapper : FileSystemWatcher
    {
        /// <summary>
        /// Template with filename pattern = key, Target folder = value
        /// </summary>
        public Dictionary<string, string> TemplateFilePattern_TargetPath { get; set; }

        /// <summary>
        /// Default Key for files without needed extension
        /// </summary>
        private string defaultKey = "defaultFolder";

        public bool NumberFiles { get; set; }
        private static int fileNumber = 1;

        public bool MarkDateForFileMoving { get; set; }

        public FileSystemWatcherWrapper(Dictionary<string, string> template, string path, bool numberFiles, bool markDateForFiles) : base(path)
        {
            this.TemplateFilePattern_TargetPath = template;
            this.NumberFiles = numberFiles;
            this.MarkDateForFileMoving = markDateForFiles;
            this.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.FileName;
            this.Created += OnCreated;
            this.EnableRaisingEvents = true;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File {e.Name} was created {new FileInfo(e.FullPath).CreationTime} and finded in {e.FullPath}.");
            MoveFile(e.Name, e.FullPath);
        }

        private bool MoveFile(string fileName, string fullPath)
        {
            try {
                string destinationFullPath;
                string targetFileName;
                if(SearchTemplatePath(fileName, out destinationFullPath)) {
                    UpdateFileNameIfRequired(fileName, out targetFileName);
                    destinationFullPath = System.IO.Path.Combine(destinationFullPath, targetFileName);
                    File.Move(fullPath, destinationFullPath);
                    Console.WriteLine($"File {fileName} was successfull moved to {destinationFullPath}");
                    return true;
                }
            } catch { }
            Console.WriteLine($"File {fileName} wasn't moved");
            return false;
        }

        private void UpdateFileNameIfRequired(string fileName, out string targetFileName) 
        {
            try {
                object locker = new object();
                lock (locker) {
                    targetFileName = NumberFiles ? $"({fileNumber++}){fileName}" : fileName;
                }
                if (MarkDateForFileMoving)
                {
                    targetFileName = $"({DateTime.Now.ToString("dd_MM_yyyy")}){targetFileName}";
                }
            } catch {
                Console.WriteLine("Unable to rename file");
                targetFileName = fileName;
            }
            
        }

        private bool SearchTemplatePath(string fileName, out string destinationFolderPath)
        {
            try
            {
                foreach (var extPattern in TemplateFilePattern_TargetPath.Keys.ToList())
                {
                    if (Regex.IsMatch(fileName, extPattern, RegexOptions.IgnoreCase))
                    {
                        destinationFolderPath = TemplateFilePattern_TargetPath[extPattern];
                        Console.WriteLine($"Pattern for {fileName} was found: Pattern {extPattern}, Destination Folder {destinationFolderPath}");
                        return true;
                    }
                }

                destinationFolderPath = TemplateFilePattern_TargetPath[defaultKey];
                Console.WriteLine($"Pattern for {fileName} wasn't found: Default destination folder {destinationFolderPath}");
                return true;
            }
            catch { }
            destinationFolderPath = Path;
            Console.WriteLine($"Pattern for {fileName} wasn't find");
            return false;
        }
    }
}

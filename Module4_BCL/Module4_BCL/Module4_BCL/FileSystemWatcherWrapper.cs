using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine(string.Format(Resources.FileFindedMessage, e.Name,
                TimeZoneInfo.ConvertTime(new FileInfo(e.FullPath).CreationTime, Program.TIMEZONE), e.FullPath));
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
                    Console.WriteLine(string.Format(Resources.FileSuccessfullMovedMessage, fileName, destinationFullPath));
                    return true;
                }
            } catch { }
            Console.WriteLine(Resources.UnableToMoveFile, fileName);
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
                    targetFileName = $"({TimeZoneInfo.ConvertTime(DateTime.Now, Program.TIMEZONE).ToString("dd_MM_yyyy")}){targetFileName}";
                }
            } catch {
                Console.WriteLine(Resources.UnableToRenameFile);
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
                        Console.WriteLine(string.Format(Resources.PatternFoundMessage, fileName, extPattern, destinationFolderPath));
                        return true;
                    }
                }

                destinationFolderPath = TemplateFilePattern_TargetPath[defaultKey];
                Console.WriteLine(string.Format(Resources.DefaultPatternFound, fileName, destinationFolderPath));
                return true;
            }
            catch { }
            destinationFolderPath = Path;
            Console.WriteLine(Resources.NotDefinedDefaultFolder);
            return false;
        }
    }
}

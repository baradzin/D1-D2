using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemVisitor
{
    public class FileSystemVisitor
    {
        public delegate bool Filtration(string filtrationString);
        public delegate void FileVisitorEventHandler(object sender, FileSystemVisitorEventArgs e);

        public event FileVisitorEventHandler ProgressStartEvent;
        public event FileVisitorEventHandler ProgressFinishedEvent;
        public event FileVisitorEventHandler FileFindedEvent;
        public event FileVisitorEventHandler DirectoryFindedEvent;
        public event FileVisitorEventHandler FilteredFileFindedEvent;
        public event FileVisitorEventHandler FilteredDirectoryFindedEvent;

        public DirectoryInfo Root { get; }
        public List<File> FileList { get;}
        public List<File> ExcludedFileList { get; }
        private bool _cancelSearchFlag = false;
        private bool _excludeFilteredFiles = false;
        private Filtration _filtrationFunc = null;

        public FileSystemVisitor(string root, Filtration filtrationFunc = null)
        {
            if (Directory.Exists(root)) {
                this.Root = new DirectoryInfo(root);
                this.FileList = new List<File>();
                this.ExcludedFileList = new List<File>();
                this._filtrationFunc = filtrationFunc;
            } else {
                throw new DirectoryNotFoundException($"Directory {root} not found");
            }
        }

        public void SubscribeEvents(bool stopSearchFilteredFlag, bool excludeFilesFilteredFlag)
        {
            FileSystemVisitorEventArgs handler = new FileSystemVisitorEventArgs();

            ProgressStartEvent += handler.ProgressWriter;
            ProgressFinishedEvent += handler.ProgressWriter;
            FileFindedEvent += handler.FilePathWriter;
            DirectoryFindedEvent += handler.FilePathWriter;
            FilteredFileFindedEvent += handler.FilePathWriter;
            FilteredDirectoryFindedEvent += handler.FilePathWriter;

            if (stopSearchFilteredFlag)
            {
                FilteredFileFindedEvent += this.CancelSearch;
                FilteredDirectoryFindedEvent += this.CancelSearch;
            }

            if (excludeFilesFilteredFlag)
            {
                FilteredFileFindedEvent += this.ExcludeFile;
                FilteredDirectoryFindedEvent += this.ExcludeFile;
            }
        }

        public void StartWalkingOnDirectory()
        {
            ProgressStartEvent(this, new FileSystemVisitorEventArgs("Start traversing..."));

            bool isFiltered = _filtrationFunc != null;

            foreach (var file in WalkDirectoryTree(Root))
            {
                this.FileList.Add(file);
                if (isFiltered && _filtrationFunc(file.Name)) {
                    if (file.IsDirectory) {
                        FilteredDirectoryFindedEvent(this, new FileSystemVisitorEventArgs(file));
                    } else {
                        FilteredFileFindedEvent(this, new FileSystemVisitorEventArgs(file));
                    }
                } else {
                    if (file.IsDirectory) {
                        DirectoryFindedEvent(this, new FileSystemVisitorEventArgs(file));
                    } else {
                        FileFindedEvent(this, new FileSystemVisitorEventArgs(file));
                    }
                }

                if (_cancelSearchFlag) {
                    break;
                }              
            }

            ProgressFinishedEvent(this, new FileSystemVisitorEventArgs("Search finished"));

            if (_excludeFilteredFiles) {
                PrintFilteredFileList();
            }          
        }

        private IEnumerable<File> WalkDirectoryTree(DirectoryInfo root)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = root.GetFiles();
                subDirs = root.GetDirectories();
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }

            if (files != null || subDirs != null)
            {
                foreach (FileInfo file in files)
                {
                    yield return new File(file.FullName, isDirectory: false);
                }

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    yield return new File(dirInfo.FullName, isDirectory: true);

                    foreach (var file in WalkDirectoryTree(dirInfo))
                    {
                        yield return file;
                    }
                }
            }
            yield break;
        }

        private void CancelSearch(object sender, FileSystemVisitorEventArgs e)
        {
            Console.WriteLine("Search stopped");
            this._cancelSearchFlag = true;
        }

        private void ExcludeFile(object sender, FileSystemVisitorEventArgs e)
        {
            this.ExcludedFileList.Add(e.File);
            this._excludeFilteredFiles = true;
        }

        public void PrintFilteredFileList()
        {
            Console.WriteLine("\n\n------------------------------------------List of Files with excluded files---------------------------------------------\n");
            foreach (var file in FileList.Except(ExcludedFileList).ToList())
            {
                Console.WriteLine(file.Path);
            }
        }  
    }
}

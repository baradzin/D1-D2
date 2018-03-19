using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitor
{
    public class FileSystemVisitor
    {
        public delegate bool Filtration(string filtrationString);
        public delegate void StartProgress();
        public delegate void FinishedProgress();
        public delegate bool FileFinded(string fileName);
        public delegate bool DirectoryFinded(string directoryName);

        public event StartProgress ProgressStart;
        public event FinishedProgress ProgressFinished;
        public event FileFinded FileFinded_ActionRequired;
        public event DirectoryFinded DirectoryFinded_ActionRequired;


        public DirectoryInfo Root { get; }
        public List<string> listFiles { get; set; }
        Filtration _filtrationFunc;

        public FileSystemVisitor(string root, Filtration filtrationFunc)
        {
            if (Directory.Exists(root))
            {
                this.Root = new DirectoryInfo(root);
                this.listFiles = new List<string>();
                this._filtrationFunc = filtrationFunc;
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory {root} not found");
            }
        }

        public void StartWalkingOnDirectory()
        {
            //Start
            ProgressStart();

            foreach (var file in WalkDirectoryTree(Root))
            {
                Console.WriteLine(file.Path);
            }

            //Finished
            ProgressFinished();
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
    }
}

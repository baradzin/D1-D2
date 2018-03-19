namespace FileSystemVisitor
{
    public class File
    {
        public string Path { get; }
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }
        public bool IsDirectory { get; set; }

        public File(string path, bool isDirectory)
        {
            this.Path = path;
            this.IsDirectory = isDirectory;
        }
    }
}
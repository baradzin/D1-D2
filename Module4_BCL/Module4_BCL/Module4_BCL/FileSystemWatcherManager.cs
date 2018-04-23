using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Module4_BCL.ConfigurationItems;

namespace Module4_BCL
{
    public class FileSystemWatcherManager
    {
        private List<FileSystemWatcherWrapper> FileWatchersList;

        private Dictionary<string, string> Template { get; }

        private List<string> PathList { get; }

        private bool NumberFiles { get; }

        private bool MarkDateForFileMoving { get; }

        private string configSection = "FSWSection";

        public FileSystemWatcherManager()
        {
            var configItems = (FileSystemWatcherConfigurationSection)ConfigurationManager.GetSection(configSection);
            this.NumberFiles = configItems.NumberFiles;
            this.MarkDateForFileMoving = configItems.MartDateForFiles;
            this.Template = configItems.TemplateCollection.Cast<TemplateElement>()
                 .ToDictionary(n => n.Pattern, n => n.Path.ToString());
            this.PathList = configItems.PathList.Cast<PathElement>().Select(x => x.Path).ToList();
            this.FileWatchersList = new List<FileSystemWatcherWrapper>();
        }

        public void RunSystemWatchers()
        {
            foreach(var path in PathList)
            {
                this.FileWatchersList.Add(new FileSystemWatcherWrapper(Template, path, NumberFiles, MarkDateForFileMoving));
            }
        }
    }
}

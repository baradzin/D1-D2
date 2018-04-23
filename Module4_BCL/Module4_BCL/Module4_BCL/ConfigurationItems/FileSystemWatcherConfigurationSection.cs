using System.Configuration;

namespace Module4_BCL.ConfigurationItems
{
    public class FileSystemWatcherConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("numberFiles", DefaultValue = "false", IsRequired = false)]
        public bool NumberFiles
        {
            get
            {
                return (bool)this["numberFiles"];
            }
            set
            {
                this["numberFiles"] = value;
            }
        }

        [ConfigurationProperty("markDateForFiles", DefaultValue = "false", IsRequired = false)]
        public bool MartDateForFiles
        {
            get
            {
                return (bool)this["markDateForFiles"];
            }
            set
            {
                this["markDateForFiles"] = value;
            }
        }

        [ConfigurationProperty("templateItems")]
        public TemplateDictionaryCollection TemplateCollection
        {
            get { return (TemplateDictionaryCollection)this["templateItems"]; }
        }

        [ConfigurationProperty("pathItems")]
        public PathCollection PathList
        {
            get { return (PathCollection)this["pathItems"]; }
        }
    }
}

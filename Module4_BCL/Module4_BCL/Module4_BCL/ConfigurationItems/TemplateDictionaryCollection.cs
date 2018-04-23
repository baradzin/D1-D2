using System.Configuration;

namespace Module4_BCL.ConfigurationItems
{
    public class TemplateDictionaryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TemplateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TemplateElement)element).Pattern;
        }
    }

    public class TemplateElement : ConfigurationElement
    {
        [ConfigurationProperty("pattern", IsKey = true)]
        public string Pattern
        {
            get { return (string)base["pattern"]; }
        }

        [ConfigurationProperty("targetPath")]
        public string Path
        {
            get { return (string)base["targetPath"]; }
        }
    }
}

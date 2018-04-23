using System.Configuration;

namespace Module4_BCL.ConfigurationItems
{
    public class PathCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PathElement)element).Path;
        }
    }

    public class PathElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true)]
        public string Path
        {
            get { return (string)base["path"]; }
        }
    }
}

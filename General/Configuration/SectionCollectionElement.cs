using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Config
{
    public class SectionCollectionElement : ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "", IsRequired = false)]
        public String key
        {
            get
            {
                return (String)this["key"];
            }
        }
        [ConfigurationProperty("value", DefaultValue = "", IsRequired = false)]
        public String value
        {
            get
            {
                return (String)this["value"];
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public AddCollection AddCollectionMembers
        {
            get
            {
                return (AddCollection)this[""];
            }
        }
    }
}

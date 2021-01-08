using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Config
{
    public class Add : ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "", IsRequired = true)]
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
        [ConfigurationProperty("connectionString", DefaultValue = "", IsRequired = false)]
        public String connectionString
        {
            get
            {
                return (String)this["connectionString"];
            }
        }
        [ConfigurationProperty("providerName", DefaultValue = "", IsRequired = false)]
        public String providerName
        {
            get
            {
                return (String)this["providerName"];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Config
{
    public class Section : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
                public SectionCollection SectionCollectionMembers
                {
                    get
                    {
                        return (SectionCollection)this[""];
                    }
                }

    }
}

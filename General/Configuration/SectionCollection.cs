using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace General.Config
{
    public class SectionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SectionCollectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SectionCollectionElement)element).key;
        }

        protected override string ElementName
        {
            get
            {
                return "SectionCollectionElement";
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == "SectionCollectionElement";
        }



        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
    }

}

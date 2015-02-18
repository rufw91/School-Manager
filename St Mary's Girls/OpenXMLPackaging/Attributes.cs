using System;

namespace OpenXmlPackaging {
    
    public class ElementAttribute : Attribute {
        public string Name { get; set; }

        public ElementAttribute(string name) {
            Name = name;
        }
    }
}

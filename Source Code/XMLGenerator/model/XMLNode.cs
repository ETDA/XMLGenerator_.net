using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace XMLGenerator.model
{
    class XMLNode
    {
        public string name { get; set; }
        public string type { get; set; }
        public string minOccur { get; set; }
        public string maxOccur { get; set; }
        public string parentName { get; set; }
        public bool isRoot { get; set; }
        public bool isMandatory { get; set; }
        public bool isOptional { get; set; }
        public bool isRepeatable { get; set; }
        public bool isArray { get; set; }
        public bool isComplexType { get; set; }
    }
}

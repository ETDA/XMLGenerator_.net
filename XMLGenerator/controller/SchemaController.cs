using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using XMLGenerator.model;

namespace XMLGenerator.controller
{
    class SchemaController
    {
        private List<XMLNode> xmlNodeList;
        private Dictionary<string, string> nameSpaceList;
        //private List<string> schemaFileList;

        public SchemaController(string messagepackage, string templateFile) 
        {


            xmlNodeList = new List<XMLNode>();
            //schemaFileList = new List<string>();
            nameSpaceList = new Dictionary<string, string>();
            
            GetNameSpaceList(templateFile);
            GetSchema(messagepackage);
        }

        private void GetNameSpaceList(string templateFile)
        {
            string xml = File.ReadAllText(templateFile);
            XNamespace xmlns = XNamespace.Get("http://www.w3.org/2000/xmlns");
            XDocument document = XDocument.Parse(xml);
            var documentElement = document.Root;
            //Console.WriteLine("Root is " + documentElement.Name);
            foreach (XAttribute attribute in documentElement.Attributes())
            {
                //Console.WriteLine("Name: " + attribute.Name.LocalName + " / Value: " + attribute.Value);
                nameSpaceList.Add(attribute.Value, attribute.Name.LocalName);

            }


            //Console.WriteLine();
            // if you have a file: var doc = XDocument.Load(<path to xml file>)
            /*
            foreach (var element in doc.Descendants(xs + "element"))
            {
                string name = element.Attribute("name") != null ? element.Attribute("name").Value : "";
                //string maxOccurs = element.Attribute("maxOccurs") != null ? element.Attribute("maxOccurs").Value : "";
                //string minOccurs = element.Attribute("minOccurs") != null ? element.Attribute("minOccurs").Value : "";
                //string type = element.Attribute("type") != null ? element.Attribute("type").Value : "";
                //Console.WriteLine("Name: " + element.Attribute("name").Value + " / Type: " + type);
            }
            */
        }

        private void GetSchema(string messagePackage)
        {
            List<string> schemaFileList = ListSchemaFile(messagePackage);

            foreach (string schemaFile in schemaFileList)
            {
                string xsdFile = File.ReadAllText(schemaFile);
                XNamespace xmlns = XNamespace.Get("http://www.w3.org/2000/xmlns");
                XDocument document = XDocument.Parse(xsdFile);
                XElement documentElement = document.Root;
                string targetNameSpace = documentElement.Attribute("targetNamespace") != null ? documentElement.Attribute("targetNamespace").Value : "";
                string prefix = "";
                if (nameSpaceList.TryGetValue(targetNameSpace, out prefix)) {
                    if (!prefix.Equals(""))
                    {
                        //Console.WriteLine(prefix + " " + schemaFile);
                        GetComplexNode(prefix, schemaFile);
                    }
                }
            }

        }

        private List<string> ListSchemaFile(string messagePackage)
        {
            List<string> schemaFileList = new List<string>();

            foreach (string file in Directory.GetFiles(messagePackage, "*.xsd", SearchOption.AllDirectories))
            {
                //Console.WriteLine(file);
                schemaFileList.Add(file);
            }

            return schemaFileList;
        }

        private void GetComplexNode(string prefix, string schemaFile)
        {
            //string xsdFile = File.ReadAllText(schemaFile);
            var xs = XNamespace.Get("http://www.w3.org/2001/XMLSchema");
            var doc = XDocument.Load(schemaFile);
            // if you have a file: var doc = XDocument.Load(<path to xml file>)
            foreach (var element in doc.Descendants(xs + "complexType"))
            {
                getSequenceNode(element, prefix);
            }

            for (int i = 0; i < xmlNodeList.Count(); i++)
            {
                string xmlNodeName = xmlNodeList[i].type;
                for (int j = 0; j < xmlNodeList.Count(); j++)
                {
                    if (xmlNodeList[j].parentName.Equals(xmlNodeName))
                    {
                        //System.out.println(xmlNodeList.get(i).getName() + " is a complex type");
                        xmlNodeList[i].isComplexType = true;
                        if (!xmlNodeList[i].maxOccur.Equals("") && !xmlNodeList[i].maxOccur.Equals("1"))
                        {
                            xmlNodeList[i].isArray = true;
                        }
                        break;
                    }
                }
            }
        }

        private void getSequenceNode(XElement complexElement, string prefix)
        {
            var xs = XNamespace.Get("http://www.w3.org/2001/XMLSchema");
            foreach (var sequenceElement in complexElement.Descendants(xs + "sequence"))
            {
                //getSequenceNode(element, prefix);
                getElementNode(sequenceElement, complexElement, prefix);
            }
        }

        private void getElementNode(XElement sequenceElement, XElement parentElement, string prefix)
        {
            var xs = XNamespace.Get("http://www.w3.org/2001/XMLSchema");
            foreach (var element in sequenceElement.Descendants(xs + "element"))
            {
                //try
               // {
                string name = element.Attribute("name") != null ? element.Attribute("name").Value : "";
                string maxOccurs = element.Attribute("maxOccurs") != null ? element.Attribute("maxOccurs").Value : "";
                string minOccurs = element.Attribute("minOccurs") != null ? element.Attribute("minOccurs").Value : "";
                string type = element.Attribute("type") != null ? element.Attribute("type").Value : "";
                //Console.WriteLine("Name: " + element.Attribute("name").Value + " / Type: " + type + " / Parent: " + parentElement.Attribute("name").Value);
                
                if (!name.Trim().Equals(""))
                {
                    xmlNodeList.Add(new XMLNode()
                    {
                        name = prefix + ":" + (element.Attribute("name") != null ? element.Attribute("name").Value : ""),
                        type = element.Attribute("type") != null ? element.Attribute("type").Value : "",
                        maxOccur = element.Attribute("maxOccurs") != null ? element.Attribute("maxOccurs").Value : "",
                        minOccur = element.Attribute("minOccurs") != null ? element.Attribute("minOccurs").Value : "",
                        parentName = prefix + ":" + parentElement.Attribute("name").Value
                    });
                }
                
                //}
                //catch (NullReferenceException ex)
                //{
                   // continue;
                //}
                
            }
        }

        /**
	    * @return the xmlNodeList
	    */
        public List<XMLNode> getXmlNodeList()
        {
            return xmlNodeList;
        }

        public void readXSDFile()
        {
            string xml = File.ReadAllText(@"C:\Users\Windows-10\Desktop\ScoreReport_V1_20201228_1505\schema\etda\standard\ScoreReport_ReuseableAggregateCoreComponent_1p0.xsd");
            var xs = XNamespace.Get("http://www.w3.org/2001/XMLSchema");
            var doc = XDocument.Parse(xml);
            var documentElement = doc.Root;
            Console.WriteLine("Root is " + documentElement.Name);
            // if you have a file: var doc = XDocument.Load(<path to xml file>)
            foreach (var attribute in documentElement.Attributes())
            {
                Console.WriteLine("Name: " + attribute.Name + " / Value: " + attribute.Value);
            }
        }
    }
}

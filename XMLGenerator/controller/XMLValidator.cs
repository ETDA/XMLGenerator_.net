using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XMLGenerator.controller
{
    class XMLValidator
    {
        private String xmlFilePath, schemaFolderPath;
        private List<string> schemaFileList;
        private Dictionary<string, string> nameSpaceList;
        private List<Exception> exceptionList;

        public XMLValidator(string xmlFilePath, string schemaFolderPath)
        {
            schemaFileList = new List<string>();
            nameSpaceList = new Dictionary<string, string>();

            SetXmlFilePath(xmlFilePath);
            SetSchemaFolderPath(schemaFolderPath);
            
            GetNameSpaceList(xmlFilePath);

            
            ListSchemaFile(schemaFolderPath);


        }

        public List<Exception> Validate()
        {
            XmlSchemaSet schema = new XmlSchemaSet();;
            exceptionList = new List<Exception>();

            foreach (string schemaFile in schemaFileList)
            {
                string xsdFile = File.ReadAllText(schemaFile);
                XNamespace xmlns = XNamespace.Get("http://www.w3.org/2000/xmlns");
                XDocument document = XDocument.Parse(xsdFile);
                XElement documentElement = document.Root;
                string targetNameSpace = documentElement.Attribute("targetNamespace") != null ? documentElement.Attribute("targetNamespace").Value : "";
                string prefix = "";
                //Console.WriteLine(targetNameSpace + ":" + schemaFile);
                //schema.Add(targetNameSpace, schemaFile);

                
                if (nameSpaceList.TryGetValue(targetNameSpace, out prefix))
                {
                    if (!prefix.Equals(""))
                    {
                        //Console.WriteLine(targetNameSpace + ", " +  schemaFile);
                        schema.Add(targetNameSpace, schemaFile);
                        //Console.WriteLine(schema.GlobalElements.Names);
                    }
                }

            }

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.ValidationType = ValidationType.Schema;
            xrs.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            xrs.Schemas = schema;
            xrs.ValidationEventHandler += ValidationEventHandler;

            XDocument doc = XDocument.Load(xmlFilePath);

            using (XmlReader xr = XmlReader.Create(doc.CreateReader(), xrs))
            {
                while (xr.Read()) { }
            }

            return exceptionList;
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
            
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            
            //XmlSeverityType type;
            XmlSeverityType type = (XmlSeverityType)Enum.Parse(typeof(XmlSeverityType), "Error");
            if (type == XmlSeverityType.Error && !e.Message.Contains("The 'schemaLocation' attribute is not declared"))
            {
                exceptionList.Add(new Exception(e.Message));
            }
            
          
        }

        private void ListSchemaFile(string messagePackage)
        {
            List<string> schemaFileList = new List<string>();

            foreach (string file in Directory.GetFiles(messagePackage, "*.xsd", SearchOption.AllDirectories))
            {
                //Console.WriteLine(file);
                string filepath = file;
                this.schemaFileList.Add(filepath);
            }
        }

        public string GetXmlFilePath()
        {
            return xmlFilePath;
        }

        public void SetXmlFilePath(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
        }

        public string GetSchemaFolderPath()
        {
            return schemaFolderPath;
        }

        public void SetSchemaFolderPath(string schemaFolderPath)
        {
            this.schemaFolderPath = schemaFolderPath;

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using XMLGenerator.controller;
using XMLGenerator.model;

namespace XMLGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                runEithExternalInput(args);
            } catch(Exception ex)
            {
                Console.Error.Write(ex);
                Console.Read();
            }
        }

        static void runEithExternalInput(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - Start Library");
            //SchemaController schemaController = new SchemaController(@"C:\Users\Windows-10\Desktop\ScoreReport_V1_20201228_1505", @"C:\Users\Windows-10\Desktop\ScoreReport_V1_20201228_1505\example\ScoreReport.xml");

            bool waitIndicator = false;
            bool deleteIncomplete = false;

            // TODO 1.Validate input parameter
            Console.WriteLine(DateTime.Now.ToString() + " - Validate parameter");
            ParameterController parameterController = new ParameterController(args);
            Dictionary<string, string> libraryparamList = parameterController.GetLibraryParameter();
            List<UserParameter> userParameterList = parameterController.getUserParameterList();
            if (libraryparamList["-wait"] != null)
            {
                
                waitIndicator = libraryparamList["-wait"].Equals("true", StringComparison.InvariantCultureIgnoreCase) == true ? true : false;
            }

            if (libraryparamList["-deleteIncomplete"] != null)
            {
                deleteIncomplete = libraryparamList["-deleteIncomplete"].Equals("true", StringComparison.InvariantCultureIgnoreCase) == true ? true : false;

            }

            // TODO 2.Get database connection
            Console.WriteLine(DateTime.Now.ToString() + " - Get database connection.");
            DatabaseController databaseController = new DatabaseController(libraryparamList["-database"]);

            // TODO 3.Load schema control
            Console.WriteLine(DateTime.Now.ToString() + " - Load schema.");
            SchemaController schemaController = new SchemaController(libraryparamList["-schemas"], libraryparamList["-template"]);
            List<XMLNode> schemaControllerList = schemaController.getXmlNodeList();

            foreach (UserParameter userParameter in userParameterList)
            {
                // TODO 4.Read template, mapping with database and generate output
                Console.WriteLine(DateTime.Now.ToString() + " - Generate XML File: " + userParameter.getOutputFileName());
                XMLController xmlController = new XMLController(userParameter, libraryparamList["-template"], databaseController, schemaControllerList);
                xmlController.GenerateXMLFile();

                // TODO 5.Validate XML
                Console.WriteLine(DateTime.Now.ToString() + " - Validate XML File.");
                XMLValidator xmlValidator = new XMLValidator(userParameter.getOutputFileName(), libraryparamList["-schemas"]);
                List<Exception> exceptions = xmlValidator.Validate();

                if (exceptions.Count() > 0)
                {
                    foreach (Exception exception in exceptions)
                    {
                        //logController.writeErrorLog(exception.getMessage() + " at line " + exception.getLineNumber());
                        Console.WriteLine(DateTime.Now.ToString() + " - Error " + exception.Message);
                    }

                    //logController.writeErrorLog(userParameter.getOutputFileName() + " is invalid!");
                    Console.WriteLine(DateTime.Now.ToString() + " - " + userParameter.getOutputFileName() + " is invalid!");

                    if (deleteIncomplete)
                    {
                        if (File.Exists(userParameter.getOutputFileName()))
                        {
                            Console.WriteLine(DateTime.Now.ToString() + " - Clean incomplete output file.");
                            File.Delete(userParameter.getOutputFileName());
                        }
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now.ToString() + " - Incomplete output still remain.");
                    }

                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - " + userParameter.getOutputFileName() + " is valid!");
                }

                if (libraryparamList.ContainsKey("-enableFHIRValidation"))
                {
                    if (libraryparamList["-enableFHIRValidation"].Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Start validate with FHIR");
                        bool validateResult = FHIRController.Validate(userParameter.getOutputFileName());
                        if (validateResult == true)
                        {
                            Console.WriteLine("Validate Pass");
                        }
                        else
                        {
                            Console.WriteLine("Validate Fail");
                        }
                    }
                }
               

            }

            // TODO 6. Close database connection
            Console.WriteLine(DateTime.Now.ToString() + " - Close database connection.");
            databaseController.CloseConnection();

            Console.WriteLine("\n\nPress enter to continue:");
            Console.ReadLine();

        }

        static void ValidateXMLFieldValues(string xmlFilename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilename);
            int depth = 0;
            TraverseXMLNode(xmlDoc.DocumentElement, depth, 2);
        }


        static void TraverseXMLNode(XmlNode xmlNode, int depth, int loop)
        {
            //Loop through results

            bool isRoot = false;
            bool isMandatory = false;
            bool isOptional = false;
            bool isRepeatable = false;
            bool isComplexType = false;
            bool isArray = false;
            depth = depth + 1;
            string indent = new String('\t', depth);
            if (xmlNode.NodeType == XmlNodeType.Element)
            {
                XmlElement element = (XmlElement)xmlNode;
                Console.WriteLine(indent + " elementName=" + element.Name + " Value=" + getElementTextValue(element));

                isRoot = (!element.GetAttribute("isRoot").Equals("") == true) ? true : false;
                isMandatory = (!element.GetAttribute("isMandatory").Equals("") == true) ? true : false;
                isOptional = (!element.GetAttribute("isOptional").Equals("") == true) ? true : false;
                isRepeatable = (!element.GetAttribute("isRepeatable").Equals("") == true) ? true : false;
                isComplexType = (!element.GetAttribute("isComplexType").Equals("") == true) ? true : false;
                isArray = (!element.GetAttribute("isArray").Equals("") == true) ? true : false;

                if (xmlNode.ParentNode == null)
                {
                    Console.WriteLine("Root node found");
                }

            }

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                TraverseXMLNode(childNode, depth, 2);
            }

            if (isArray == true && loop > 0)
            {
                TraverseXMLNode(xmlNode, depth - 1, loop - 1);

            }
        }

        static string getElementTextValue(XmlElement element)
        {
            XmlNode firstChildNode = element.FirstChild;
            if (firstChildNode != null)
            {
                return firstChildNode.Value;
            }
            else
            {
                return "";
            }
        }
    
    }
}

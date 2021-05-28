using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Linq;
using XMLGenerator.model;

namespace XMLGenerator.controller
{
    class XMLController
    {
		//private Document templateDocument, outputDocument;
		private DatabaseController databaseController;
		private int loop = 0;
		private string outputFile, templateFile;
		private XmlDocument templateDocument, outputDocument;
		private UserParameter userParameter;
		private List<XMLNode> nodeConfigList;

		public XMLController(UserParameter userParameter, string templateFile, DatabaseController databaseController, List<XMLNode> nodeConfigList)
        {
			setUserParameter(userParameter);
            setTemplateFile(templateFile);
            setDatabaseController(databaseController);
			setNodeConfigList(nodeConfigList);
		}

		public void GenerateXMLFile()
        {
			databaseController.setDatabaseParamList(userParameter.getDatabaseParameterList());

			templateDocument = new XmlDocument();
			templateDocument.Load(templateFile);

			outputDocument = new XmlDocument();

			TreverseLevel(templateDocument.DocumentElement, null, null, userParameter);

			outputDocument.Save(userParameter.getOutputFileName());
		}

		private void TreverseLevel(XmlNode xmlNode, XmlNode parentNode, XmlElement outputElement, UserParameter userParameter)
        {
			//Loop through results

			bool isRoot = false;
			bool isMandatory = false;
			bool isOptional = false;
			bool isRepeatable = false;
			bool isComplexType = false;
			bool isArray = false;

			XmlElement insertingElement = null;

			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement element = (XmlElement)xmlNode;

				//Console.WriteLine(element.Name);
				/*
				if (element.Name == "ram:IssueDateTime")
					Console.WriteLine();
				*/

				string elementValue = element.FirstChild != null ? element.FirstChild.Value : "";

				XMLNode nodeConfig = nodeConfigList.Find(node => xmlNode.Name.Equals(node.name, StringComparison.InvariantCultureIgnoreCase));
				if (nodeConfig != null)
                {
					isMandatory = nodeConfig.isMandatory;
					isOptional = nodeConfig.isOptional;
					isRepeatable = nodeConfig.isRepeatable;
					isComplexType = nodeConfig.isComplexType;
					isArray = nodeConfig.isArray;
				}

				//Split operation for root and non-root node
				if (xmlNode != templateDocument.DocumentElement)
                {
					//Non-root node
					insertingElement = outputDocument.CreateElement(xmlNode.Name, xmlNode.NamespaceURI);


					if (xmlNode.ChildNodes.Count > 1)
                    {
						//Console.WriteLine(xmlNode.Name + " is complex ");
						isComplexType = true;
					}
					
				
					if (xmlNode.FirstChild != null)
					{
						if (xmlNode.FirstChild.NodeType != XmlNodeType.Text)
                        {
							isComplexType = true;
						}
					}
					/*
					try
                    {
						var testNode = xmlNode.FirstChild.NextSibling;
						if (xmlNode.FirstChild != null)
                        {
							if (xmlNode.FirstChild.NextSibling != null)
                            {
								isComplexType = true;
							}
                        }
						isComplexType = true;
					}
					catch (NullReferenceException)
					{

					}
					*/


					if (!isComplexType)
                    {
						if (xmlNode.InnerText.Trim().StartsWith("@"))
                        {
							/*
							try
                            {
								insertingElement.InnerText = getDatabaseController().ReadDatabase(xmlNode.InnerText, loop);
							} 
							catch (Exception ex)
                            {
								Console.Out.WriteLine(ex.Message);
								Console.Out.WriteLine(ex.StackTrace);
								insertingElement.InnerText = "";
							}
							*/
							object obj = getDatabaseController().ReadDatabase(xmlNode.InnerText, loop);

							if (obj != null)
                            {
								if (!obj.ToString().Trim().Equals(""))
                                {
									if (obj.GetType() == typeof(DateTime))
                                    {
										DateTime dateTime = (DateTime)obj;
										insertingElement.InnerText = dateTime.ToString("o");
									} 
									else
                                    {
										insertingElement.InnerText = obj.ToString();
									}
                                }
								else
                                {
									if (isMandatory == true)
                                    {
										insertingElement.InnerText = "";
									}
									else
                                    {
										insertingElement = null;
                                    }
                                }
                            }
							else
                            {
								insertingElement = null;
							}


						}
						else if (!xmlNode.InnerText.Trim().Equals(""))
                        {
							insertingElement.InnerText = xmlNode.InnerText;
						}
                    }
					//outputElement.AppendChild(insertingElement);

					if (insertingElement != null)
                    {
						XmlAttributeCollection attributeList = xmlNode.Attributes;
						if (attributeList != null)
						{
							foreach (XmlAttribute attribute in attributeList)
							{
								if (attribute.Value.StartsWith("@"))
                                {
									object obj = getDatabaseController().ReadDatabase(attribute.Value, loop);
									if (obj != null)
									{
										if (!obj.ToString().Trim().Equals(""))
										{
											if (obj.GetType() == typeof(DateTime))
											{
												DateTime dateTime = (DateTime)obj;
												//insertingElement.InnerText = dateTime.ToString("o");
												insertingElement.SetAttribute(attribute.Name, dateTime.ToString("o"));
											}
											else
											{
												//insertingElement.InnerText = obj.ToString();
												insertingElement.SetAttribute(attribute.Name, obj.ToString());
											}
										}
										else
										{
											insertingElement.SetAttribute(attribute.Name, "");
										}
									}
									else
									{
										insertingElement.SetAttribute(attribute.Name, "");
									}
								} 
								else
                                {
									insertingElement.SetAttribute(attribute.Name, attribute.Value);
								}
								
							}
						}

						outputElement.AppendChild(insertingElement);
					}
				}
				else
                {
					//root node
					insertingElement = outputDocument.CreateElement(xmlNode.Name, xmlNode.NamespaceURI);
					var namespaceAttributeList = xmlNode.Attributes;
					if (namespaceAttributeList != null)
                    {
						foreach (XmlAttribute namespaceAttribute in namespaceAttributeList)
						{
							insertingElement.SetAttribute(namespaceAttribute.Name, namespaceAttribute.Value);
						}
                    }
					outputDocument.AppendChild(insertingElement);

				}

			}

			foreach (XmlNode childNode in xmlNode.ChildNodes)
			{
				TreverseLevel(childNode, xmlNode, insertingElement, userParameter);
			}

			int total;
			if (xmlNode.ChildNodes.Count > 1)
            {
				XmlNode firstChildElement = xmlNode.FirstChild;
				try
                {
					total = getDatabaseController().checkAll(firstChildElement.InnerText);
				} catch (Exception)
                {
					total = 0;
				}
				
			} 
			else
            {
				total = 0;
			}
			//try
			//{
			//	XmlNode firstChildElement = xmlNode.FirstChild.NextSibling;
			//	//total = getDatabaseController().checkAll(node.getTextContent());
			//	total = getDatabaseController().checkAll(firstChildElement.InnerText);
			//}
			//catch (Exception e)
			//{
			//	total = 0;
			//}

			if (isArray == true && loop < total-1)
			{
				loop += 1;
				TreverseLevel(xmlNode, parentNode, outputElement, userParameter);

			}
		}
		/**
		 * @return the templateFile
		 */
		public string getTemplateFile()
		{
			return templateFile;
		}

		/**
		 * @param templateFile the templateFile to set
		 */
		public void setTemplateFile(String templateFile)
		{
			this.templateFile = templateFile;
		}

		/**
		* @return the outputFile
		*/
		public string getOutputFile()
		{
			return outputFile;
		}

		/**
		 * @param outputFile the outputFile to set
		 */
		public void setOutputFile(String outputFile)
		{
			this.outputFile = outputFile;
		}

		/**
		 * @return the databaseController
		 */
		public DatabaseController getDatabaseController()
		{
			return databaseController;
		}

		/**
		 * @param databaseController the databaseController to set
		 */
		public void setDatabaseController(DatabaseController databaseController)
		{
			this.databaseController = databaseController;
		}

		/**
		 * @param return the templateDocument
		 */
		public XmlDocument getTemplateDocument()
		{
			return templateDocument;
		}

		/**
		 * @param templateDocument the templateDocument to set
		 */
		public void setTemplateDocument(XmlDocument templateDocument)
		{
			this.templateDocument = templateDocument;
		}

		/**
		* @return the nodeConfigList
		*/
		public List<XMLNode> getNodeConfigList()
		{
			return nodeConfigList;
		}

		/**
		 * @param nodeConfigList the nodeConfigList to set
		 */
		public void setNodeConfigList(List<XMLNode> nodeConfigList)
		{
			this.nodeConfigList = nodeConfigList;
		}

		/**
		* @return the userParameterList
		*/
		public UserParameter getUserParameter()
		{
			return userParameter;
		}

		/**
		 * @param userParameterList the userParameterList to set
		 */
		public void setUserParameter(UserParameter userParameter)
		{
			this.userParameter = userParameter;
		}
	}
}

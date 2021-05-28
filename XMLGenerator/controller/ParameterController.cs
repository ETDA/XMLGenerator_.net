using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using XMLGenerator.model;

namespace XMLGenerator.controller
{
    class ParameterController
    {
        private List<string> parameterList;
        private Dictionary<string, string> libraryParameter, databaseParameter;
        private List<UserParameter> userParameterList;

        public ParameterController(string[] args)
        {
            libraryParameter = new Dictionary<string, string>();
            userParameterList = new List<UserParameter>();

            parameterList = new List<string> { "-parameter", "-database", "-schemas", "-template" };
            GenerateParameter(args);
            PassUserParameter(libraryParameter["-parameter"]);
        }

        public void GenerateParameter(string[] args)
        {
            if (!ValidateRequireParameter(args))
            {
                throw new Exception("Required parameter is missing");
            }

            if (args.Length == 0)
            {
                throw new Exception("Parameter cannot be blank");
            }

            for (int i = 0; i < args.Length; i += 2)
            {
                if (args[i].StartsWith("-"))
                {
                    libraryParameter.Add(args[i], args[i + 1]);
                }
                else
                {
                    throw new Exception("Unrecognized paramater type");
                }
            }
        }

        private bool ValidateRequireParameter(string[] args)
        {
            List<string> argsList = args.ToList<string>();
            if (argsList.Intersect(parameterList).Any())
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        private void PassUserParameter(string userParameterFilePath)
        {
            JArray jsonArray = JArray.Parse(File.ReadAllText(userParameterFilePath));
            //JObject jObject = JObject.Parse(File.ReadAllText(userParameterFilePath));

            foreach (Object parameterList in jsonArray)
            {
                UserParameter userParameter = new UserParameter();
                foreach (var keyList in (JObject)parameterList)
                {
                    if (keyList.Key.Equals("output", StringComparison.InvariantCultureIgnoreCase))
                    {
                        userParameter.setOutputFileName(keyList.Value.ToString());
                    } 
                    else
                    {
                        userParameter.addDatabaseParameter(keyList.Key.ToString(), keyList.Value.ToString());
                    }
                }
                getUserParameterList().Add(userParameter);
            }
            /*
            var jsonData = jObject.Properties();

            foreach(var element in jsonData)
            {
                Console.WriteLine(element.Name);

            }
            */

        }


        public Dictionary<string, string> GetLibraryParameter()
        {
            return libraryParameter;
        }

        public Dictionary<string, string> GetDatabaseParameter()
        {
            return databaseParameter;
        }

        /**
	    * @return the userParameterList
	    */
        public List<UserParameter> getUserParameterList()
        {
            return userParameterList;
        }

        /**
         * @param userParameterList the userParameterList to set
         */
        public void setUserParameterList(List<UserParameter> userParameterList)
        {
            this.userParameterList = userParameterList;
        }
    }
}

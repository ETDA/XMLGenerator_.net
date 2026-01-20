using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace XMLGenerator.model
{
    class UserParameter
    {
        private string outputFileName;
        private Dictionary<string, string> databaseParameter;

		public UserParameter()
		{
			databaseParameter = new Dictionary<string, string>();
		}
		/**
		 * @return the outputFileName
		 */
		public string getOutputFileName()
		{
			return outputFileName;
		}
		/**
		 * @param outputFileName the outputFileName to set
		 */
		public void setOutputFileName(string outputFileName)
		{
			this.outputFileName = outputFileName;
		}

		/**
		 * @param key
		 * @param value
		 */
		public void addDatabaseParameter(string key, string value)
		{
			databaseParameter.Add(key, value);
		}

		/**
		 * @param key
		 */
		public void removeDatabaseParameter(string key)
		{
			databaseParameter.Remove(key);
		}

		/**
		 * 
		 */
		public void clearDatabaseParameter()
		{
			databaseParameter.Clear();
		}

		/**
		 * @param key
		 * @return
		 */
		public string getDatabaseParameter(string key)
		{
			return databaseParameter["key"];
		}

		/**
		 * @return the databaseParameter
		 */
		public Dictionary<string, string> getDatabaseParameterList()
		{
			return databaseParameter;
		}
	}
}

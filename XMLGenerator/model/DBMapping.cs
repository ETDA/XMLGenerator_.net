using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace XMLGenerator.model
{
    class DBMapping
    {
        public string tableName { get; set; }
        //public string fieldList { get; set; }
        public string orderBy { get; set; }
        public string where { get; set; }
    }
}

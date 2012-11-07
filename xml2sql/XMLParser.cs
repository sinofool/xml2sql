using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace xml2sql
{
    class XMLParser
    {
        private string filePath;

        public XMLParser(string filePath)
        {
            this.filePath = filePath;
        }

        public List<List<string> > output(string loopFor, Dictionary<string, string> columns)
        {
            XPathDocument doc = new XPathDocument(this.filePath);
            XPathNavigator nav = doc.CreateNavigator();
            XPathNodeIterator iter = nav.Select(loopFor);
            List<List<string>> rows = new List<List<string>>();
            while (iter.MoveNext())
            {
                List<string> row = new List<string>();
                foreach (KeyValuePair<string, string> pair in columns)
                {
                    XPathNodeIterator valueIter = iter.Current.Select(pair.Value);
                    if (valueIter.MoveNext())
                    {
                        string value = valueIter.Current.Value;
                        // Console.WriteLine("{0} = {1}", pair.Key, value);
                        row.Add(value);
                    }
                    else
                    {
                        row.Add("");
                    }
                }
                rows.Add(row);
            }
            return rows;
        }
    }
}

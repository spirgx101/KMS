using System;
using System.Xml;

namespace FT_Function.Xml
{
    public class XMLProcess
    {
        public XmlDocument xmlSetup;
        public string sXMLPath;

        public XMLProcess(string sPath)
        {
            sXMLPath = sPath;
            xmlSetup = new XmlDocument();
            xmlSetup.Load(sPath);
        }

        public string[] NodeItemsList(string sNodePath, string Item)
        {
            string[] sNodeItem = new string[100];
            int i = 0;
            XmlNodeList NodeLists = xmlSetup.SelectNodes(sNodePath);

            foreach (XmlNode OneNode in NodeLists)
            {
                sNodeItem[i] = OneNode.Attributes[Item].Value;
                i++;
            }
            return sNodeItem;
        }
        /// <summary>
        /// 路徑格式 ex: /Root/Sample/SamplePath
        /// </summary>
        /// <param name="sPath">資料路徑</param>
        /// <returns>string</returns>
        public string SelectInnerText(string sPath)
        {
            XmlNode xmlNode = xmlSetup.SelectSingleNode(sPath);
            return xmlNode.InnerText;
        }
        /// <summary>
        /// 路徑格式 ex: /Root/Sample/SamplePath
        /// </summary>
        /// <param name="sPath">資料路徑</param>
        /// <param name="sValue">屬性</param>
        public void EditInnerText(string sPath, string sValue)
        {
            XmlNode xmlNode = xmlSetup.SelectSingleNode(sPath);
            xmlNode.InnerText = sValue;
            xmlSetup.Save(sXMLPath);
        }
    }
}

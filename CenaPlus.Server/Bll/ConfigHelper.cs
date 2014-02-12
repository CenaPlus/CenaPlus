using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CenaPlus.Server.Bll
{
    public static class ConfigHelper
    {
        public static string WorkingDirectory
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("WorkingDirectory");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("WorkingDirectory");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }
        public static string UserName
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("UserName");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("UserName");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }
        public static string Password
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Password");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Password");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }
        public static bool JudgeEnabled
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Enabled");
                return Convert.ToBoolean(node[0].InnerText);
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Enabled");
                node[0].InnerText = value ? "True" : "False";
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }
    }
}

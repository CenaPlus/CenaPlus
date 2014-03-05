using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CenaPlus.Server.Bll
{
    public static class ConfigHelper
    {
        public static void Refresh()
        {
            workingdirectory = null;
            username = null;
            password = null;
            c = null;
            cxx = null;
            cxx11 = null;
            java = null;
            javac = null;
            pascal = null;
            python27 = null;
            python33 = null;
            ruby = null;
            servername = null;
            serverport = null;
            
        }
        private static string workingdirectory;
        public static string WorkingDirectory
        {
            get
            {
                if (workingdirectory == null || workingdirectory == String.Empty) 
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("WorkingDirectory");
                    workingdirectory = node[0].InnerText;
                }
                return workingdirectory;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("WorkingDirectory");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                workingdirectory = value;
            }
        }

        private static string servername;
        public static string ServerName
        {
            get
            {
                if (servername == null || servername == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("ServerName");
                    servername = node[0].InnerText;
                }
                return servername;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("ServerName");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                servername = value;
            }
        }

        private static int? serverport;
        public static int? ServerPort
        {
            get
            {
                if (serverport == null)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("ServerPort");
                    serverport = Convert.ToInt32(node[0].InnerText);
                }
                return serverport;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("ServerPort");
                node[0].InnerText = value.ToString();
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                serverport = Convert.ToInt32(value);
            }
        }

        private static string username;
        public static string UserName
        {
            get
            {
                if (username == null || username == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("UserName");
                    username = node[0].InnerText;
                }
                return username;
            }
            set
            {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("UserName");
                    node[0].InnerText = value;
                    xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                    username = value;
            }
        }
        public static string password;
        public static string Password
        {
            get
            {
                if (password == null || password == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("Password");
                    password = node[0].InnerText;
                }
                return password;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Password");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                password = value;
            }
        }

        private static string c;
        public static string C
        {
            get
            {
                if (c == null || c == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("C");
                    c = node[0].InnerText;
                }
                return c;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("C");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                c = value;
            }
        }
        private static string cxx;
        public static string CXX
        {
            get
            {
                if (cxx == null || cxx == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("CXX");
                    cxx = node[0].InnerText;
                }
                return cxx;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("CXX");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                cxx = value;
            }
        }
        private static string cxx11;
        public static string CXX11
        {
            get
            {
                if (cxx11 == null || cxx11 == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("CXX11");
                    cxx11 = node[0].InnerText;
                }
                return cxx11;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("CXX11");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                cxx11 = value;
            }
        }
        private static string javac;
        public static string Javac
        {
            get
            {
                if (javac == null || javac == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("Javac");
                    javac = node[0].InnerText;
                }
                return javac;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Javac");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                javac = value;
            }
        }
        private static string java;
        public static string Java
        {
            get
            {
                if (java == null || java == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("Java");
                    java = node[0].InnerText;
                }
                return java;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Java");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                java = value;
            }
        }
        private static string pascal;
        public static string Pascal
        {
            get
            {
                if (pascal == null || pascal == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("Pascal");
                    pascal = node[0].InnerText;
                }
                return pascal;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Pascal");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                pascal = value;
            }
        }
        private static string python33;
        public static string Python33
        {
            get
            {
                if (python33 == null || python33 == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("Python33");
                    python33 = node[0].InnerText;
                }
                return python33;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Python33");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                python33 = value;
            }
        }
        private static string python27;
        public static string Python27
        {
            get
            {
                if (python27 == null || python27 == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("Python27");
                    python27 = node[0].InnerText;
                }
                return python27;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Python27");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                python27 = value;
            }
        }
        private static string ruby;
        public static string Ruby
        {
            get
            {
                if (ruby == null || ruby == String.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("Ruby");
                    ruby = node[0].InnerText;
                }
                return ruby;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("Ruby");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
                ruby = value;
            }
        }

        public static string Dir_gcc
        {
            get
            {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                    XmlNodeList node;
                    node = xml.GetElementsByTagName("gcc");
                    return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("gcc");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }

        public static string Dir_gccinc
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("gccinc");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("gccinc");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }

        public static string Dir_gcclib
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("gcclib");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("gcclib");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }
        public static string Dir_fpc
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("fpc");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("fpc");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }

        public static string Dir_jdk
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("jdk");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("jdk");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }

        public static string Dir_py27
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("py27");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("py27");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }
        public static string Dir_py33
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("py33");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("py33");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }

        public static string Dir_rb
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("rb");
                return node[0].InnerText;
            }
            set
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Environment.CurrentDirectory + "\\Config.xml");
                XmlNodeList node;
                node = xml.GetElementsByTagName("rb");
                node[0].InnerText = value;
                xml.Save(Environment.CurrentDirectory + "\\Config.xml");
            }
        }
    }
}

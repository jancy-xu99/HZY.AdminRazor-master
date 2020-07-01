/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit
{
    using System.IO;
    using System.Reflection;
    using System.Xml;

    /// <summary>
    /// 读取 xml 摘要
    /// </summary>
    public static class ReadXmlSummary
    {
        static Dictionary<Assembly, XmlDocument> XmlCache { get; set; } = new Dictionary<Assembly, XmlDocument>();

        public static XmlElement XMLForMember(MemberInfo memberInfo)
        {
            var _XmlElement = XMLFromName(memberInfo.DeclaringType, memberInfo.MemberType.ToString()[0], memberInfo.Name);
            return _XmlElement;
        }

        private static XmlElement XMLFromName(Type type, char prefix, string name)
        {
            string fullName;

            if (String.IsNullOrEmpty(name))
            {
                fullName = prefix + ":" + type.FullName;
            }
            else
            {
                fullName = prefix + ":" + type.FullName + "." + name;
            }

            XmlDocument xmlDocument = XMLFromAssembly(type.Assembly);

            XmlElement matchedElement = null;

            foreach (XmlElement xmlElement in xmlDocument["doc"]["members"])
            {
                if (xmlElement.Attributes["name"].Value.Equals(fullName))
                {
                    matchedElement = xmlElement;
                }
            }

            return matchedElement;
        }

        public static XmlDocument XMLFromAssembly(Assembly assembly)
        {
            if (!XmlCache.ContainsKey(assembly))
            {
                // load the docuemnt into the cache
                XmlCache[assembly] = XMLFromAssemblyNonCached(assembly);
            }

            return XmlCache[assembly];
        }

        private static XmlDocument XMLFromAssemblyNonCached(Assembly assembly)
        {
            string location = assembly.Location;

            StreamReader streamReader;

            streamReader = new StreamReader(Path.ChangeExtension(location, ".xml"));

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(streamReader);
            return xmlDocument;
        }


    }
}

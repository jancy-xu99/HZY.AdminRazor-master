using GYWx.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Xml;

namespace WechatMall.WX.Model.Wx
{
    public class XmlHelper
    {
        public static string CreateXml(object obj)
        {
            try
            {
                using (StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.CreateXmlDeclaration("1.0", sw.Encoding.ToString(), null);
                    XmlNode root = doc.CreateElement("xml");
                    doc.AppendChild(root);
                    var props = obj.GetType().GetProperties();
                    foreach (var p in props)
                    {
                        _CraeteChildNode(doc, root, p, obj);
                    }
                    doc.Save(sw);
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static void LoadFromXml(string xml, object obj)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                var props = obj.GetType().GetProperties();
                foreach (var pi in props)
                {
                    string xpath = "/xml/" + pi.Name;

                    _SetClsValue(doc, pi, obj, xpath);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static void _SetClsValue(XmlNode doc, PropertyInfo pi, object obj, string xpath)
        {
            if (pi.PropertyType.IsClass && pi.PropertyType.Name != "String" && !pi.PropertyType.IsArray)
            {
                object obj1 = pi.GetValue(obj, null);
                var pprops = obj1.GetType().GetProperties();
                foreach (var p in pprops)
                {
                    _SetClsValue(doc, p, obj1, xpath + "/" + p.Name);
                }
            }
            else if (pi.PropertyType.IsArray)
            {
                XmlNodeList nodes = doc.SelectNodes(xpath);
                if (nodes.Count > 0)
                {
                    List<object> lst = new List<object>();
                    string typeName = pi.ToString().Substring(0, pi.ToString().IndexOf("[]"));
                    foreach (XmlNode nd in nodes)
                    {
                        ObjectHandle oNd = Activator.CreateInstance(null, typeName);
                        foreach (var s in oNd.Unwrap().GetType().GetProperties())
                        {
                            _SetClsValue(nd, s, oNd.Unwrap(), s.Name);
                        }
                        lst.Add(oNd.Unwrap());
                    }
                    string strClassName = typeName.Substring(typeName.LastIndexOf(".") + 1);
                    obj.GetType().GetMethod("Set" + strClassName + "Array").Invoke(obj, new[] { lst.ToArray() });
                }
            }
            else
            {
                XmlNode node = doc.SelectSingleNode(xpath);
                if (node != null)
                {
                    pi.SetValue(obj, node.InnerText, null);
                }
            }


        }

        private static void _CraeteChildNode(XmlDocument doc, XmlNode baseRoot, PropertyInfo pi, object obj)
        {
            if (pi.PropertyType.IsClass && pi.PropertyType.Name != "String" && !pi.PropertyType.IsArray)
            {
                XmlElement classNode = doc.CreateElement(pi.Name);
                object obj1 = pi.GetValue(obj, null);
                var pprops = obj1.GetType().GetProperties();
                foreach (var pp in pprops)
                {
                    _CraeteChildNode(doc, classNode, pp, obj1);
                }
                baseRoot.AppendChild(classNode);
            }
            else if (pi.PropertyType.IsArray)
            {

                object obj1 = pi.GetValue(obj, null);
                object[] objArry = new object[0];
                if (obj1 is Array)
                {
                    objArry = obj1 as object[];
                }
                foreach (var o in objArry)
                {
                    XmlElement classNode = doc.CreateElement(pi.Name);
                    var props = o.GetType().GetProperties();
                    foreach (var op in props)
                    {
                        _CraeteChildNode(doc, classNode, op, o);
                    }
                    baseRoot.AppendChild(classNode);
                }

            }
            else
            {
                XmlElement field = doc.CreateElement(pi.Name);
                string s = "";
                if (pi.GetValue(obj, null) != null)
                {
                    s = pi.GetValue(obj, null).ToString();
                }
                field.InnerXml = s;
                baseRoot.AppendChild(field);
            }
        }
    }
}

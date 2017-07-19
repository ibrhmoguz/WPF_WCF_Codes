#region Namespaces

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

#endregion

namespace WcfPoc.WcfAnnotation
{
    /// <summary>
    /// Thread safe storage of the XElement
    /// </summary>
    public class XmlDocumentationCache : Dictionary<string, XElement>
    {
        public ReaderWriterLock rwl = new ReaderWriterLock();
    }

    /// <summary>
    /// Helper class for handling C# Xml Documentation
    /// </summary>
    public class XmlDocumentation
    {
        static XmlDocumentationCache _storage = new XmlDocumentationCache();

        #region Load
        public static object Load(Type clrType, Type dataContractType, AnnotationAttribute annotation) 
        {
            if (annotation != null && annotation.ExportXmlDoc)
            {
                XElement element = Load(string.Concat("T:", clrType.FullName), dataContractType);
                if (element != null)
                {
                    element.AddFirst(annotation.Annotation);
                }
                return annotation.ExportAsText ? (object)element.ToString() : new Annotation(element);
            }
            else if (annotation != null && annotation.ExportXmlDoc == false)
            {
                return annotation.Annotation;
            }
            else
            {
                return new Annotation(Load(string.Concat("T:", clrType.FullName), dataContractType));
            }
        }
        public static object Load(MemberInfo memberInfo, Type dataContractType, AnnotationAttribute annotation)
        {
            if (annotation != null && annotation.ExportXmlDoc)
            {
                XElement element = Load(CreateMemberName(memberInfo), memberInfo.DeclaringType);
                if (element != null)
                {
                    element.AddFirst(annotation.Annotation);
                }
                return annotation.ExportAsText ? (object)element.ToString() : new Annotation(element);
            }
            else if (annotation != null && annotation.ExportXmlDoc == false)
            {
                return annotation.Annotation;
            }
            else
            {
                return new Annotation(Load(CreateMemberName(memberInfo), memberInfo.DeclaringType));
            }
        }
        public static string Load(XmlElement wsdlDocumentElement, Type clrType, DocumentationAttribute wsdlDocumentation)
        {
            if (wsdlDocumentation != null)
            {
                if (wsdlDocumentElement != null && wsdlDocumentation.ExportXmlDoc)
                {
                    XElement element = Load(string.Concat("T:", clrType.FullName), clrType);
                    if (element != null)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(element.ToString());
                        foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                        {
                            wsdlDocumentElement.AppendChild(wsdlDocumentElement.OwnerDocument.ImportNode(node, true));
                        }
                    }
                }
                return wsdlDocumentation.Documentation; 
            }
            return null;
        }
        public static string Load(XmlElement wsdlDocumentElement, MemberInfo memberInfo, DocumentationAttribute wsdlDocumentation)
        {
            if (wsdlDocumentation != null)
            {
                if (wsdlDocumentElement != null && wsdlDocumentation.ExportXmlDoc)
                {
                    XElement element = Load(CreateMemberName(memberInfo), memberInfo.DeclaringType);
                    if (element != null)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(element.ToString());
                        foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                        {
                            wsdlDocumentElement.AppendChild(wsdlDocumentElement.OwnerDocument.ImportNode(node, true));
                        }
                    }
                }
                return wsdlDocumentation.Documentation;
            }
            return null;
        }
     
        public static XElement Load(string memberName, Type type)
        {
            XElement member = null;
            try
            {
                if (!string.IsNullOrEmpty(memberName) && type != null)
                {
                    XElement doc = GetXmlDocument(type);
                    if (doc != null)
                    {
                        member = doc.Descendants("member").FirstOrDefault(e => e.Attribute("name").Value == memberName);
                        Trace.WriteLine($"memberName={memberName}, type={type.FullName}, member=\r\n{member}");
                    }
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine($"Failed for memberName={memberName}, type={type.FullName}; error={ex.Message}");
            }
            if (member != null)
            {
                // copy and remove attribute 'name'
                XElement memberclone = new XElement(member);
                memberclone.Attribute("name").Remove();
                return memberclone;
            }
            return member;
        }
        #endregion

        #region GetXmlDocument
        private static XElement GetXmlDocument(Type type)
        {
            XElement doc = null;
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;

            string fileName = Path.GetFileNameWithoutExtension(type.Assembly.Location);

            string directoryName = Path.GetDirectoryName(codeBase);

            string xmldocpath = directoryName + "\\" + fileName + ".xml";

            xmldocpath = xmldocpath.Remove(0, 6);

            //string xmldocpath = Path.ChangeExtension(type.Assembly.Location, "xml");

            try
            {
                _storage.rwl.AcquireReaderLock(TimeSpan.FromSeconds(10));
                doc = _storage.FirstOrDefault(e => e.Key == xmldocpath).Value;
                if (doc == null)
                {
                    if (File.Exists(xmldocpath))
                    {
                        _storage.rwl.UpgradeToWriterLock(TimeSpan.FromSeconds(10));
                        doc = XElement.Load(xmldocpath);
                        _storage.Add(xmldocpath, doc);
                        Trace.WriteLine(xmldocpath);
                    }
                }
            }
            finally
            {
                _storage.rwl.ReleaseLock();
            }
            return doc;
        }
        #endregion

        #region CreateMemberName
        public static string CreateMemberName(MemberInfo mi)
        {
            string memberName = string.Empty;
            switch (mi.MemberType)
            {
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    memberName = $"T:{((Type) mi).FullName}";
                    break;

                case MemberTypes.Property:
                    memberName = $"P:{mi.ReflectedType.FullName}.{mi.Name}";
                    break;

                case MemberTypes.Field:
                    memberName = $"F:{mi.ReflectedType.FullName}.{mi.Name}";
                    break;

                case MemberTypes.Method:
                    MethodInfo methodInfo = mi as MethodInfo;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    List<string> parms = new List<string>();
                    foreach (ParameterInfo pi in parameters)
                    {
                        parms.Add(pi.ParameterType.ToString().Replace('&', '@'));
                    }
                    memberName = $"M:{mi.DeclaringType.FullName}.{mi.Name}({string.Join(",", parms.ToArray())})";
                    break;
            }
            return memberName;
        }
        #endregion
    }
}

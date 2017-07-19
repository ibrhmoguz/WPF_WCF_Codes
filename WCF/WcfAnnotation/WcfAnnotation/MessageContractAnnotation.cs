#region Namespaces

using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Description;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

#endregion

namespace WcfPoc.WcfAnnotation
{
    /// <summary>
    /// Helper class for handling annotation in the MessageContract type 
    /// </summary>
    class MessageContractAnnotation
    {
        #region Export
        public static void Export(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            // MessageContractExporter
            Type type = Type.GetType("System.ServiceModel.Description.MessageContractExporter+MessageExportContext, System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            object messageContractExporterObj = null;
            if (exporter.State.TryGetValue(type, out messageContractExporterObj))
            {
                // walk through the Elements
                IDictionary o = (IDictionary)messageContractExporterObj.GetType().GetField("ElementTypes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(messageContractExporterObj);
                foreach (var item in o.Values)
                {
                    XmlSchemaElement elem = (XmlSchemaElement)item.GetType().GetProperty("Element", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item, null);
                    OperationDescription operation = (OperationDescription)item.GetType().GetProperty("Operation", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item, null);

                    var operAnnotation = operation.SyncMethod.GetCustomAttributes(typeof(DocumentationAttribute), false).FirstOrDefault() as DocumentationAttribute;
                    if (operAnnotation != null)
                    {
                        elem.Annotation = CreateXmlSchemaAnnotation(operAnnotation.Documentation, operAnnotation.ExportXmlDoc, operation.SyncMethod);
                    }

                    Trace.WriteLine($"ElementName = {elem.Name}");

                    // [MessageBody]
                    if (ExportAnnotationForMessageBody(exporter, operation, elem)) continue;

                    // [MessageHeader]
                    if (ExportAnnotationForMessageHeaders(exporter, operation, elem)) continue;

                    //  multi-parts
                    if (ExportAnnotationForMessageParameters(exporter, operation, elem)) continue;
                }
            }
        }
        #endregion

        #region Helpers
        internal static bool ExportAnnotationForMessageBody(WsdlExporter exporter, OperationDescription operation, XmlSchemaElement elem)
        {
            bool retVal = false;

            MessageDescription md = operation.Messages.Cast<MessageDescription>().FirstOrDefault(e => e.MessageType != null && e.MessageType.Name == elem.Name);
            if (md != null)
            {
                // type level
                var attribute = md.MessageType.GetCustomAttributes(typeof(AnnotationAttribute), false).FirstOrDefault() as AnnotationAttribute;
                if (attribute != null)
                {
                    elem.Annotation = CreateXmlSchemaAnnotation(attribute.Annotation, attribute.ExportXmlDoc, md.MessageType);
                }

                if (elem.ElementSchemaType != null && elem.ElementSchemaType is XmlSchemaComplexType)
                {
                    // [MessageBodyMember]
                    XmlSchemaComplexType complexType = elem.ElementSchemaType as XmlSchemaComplexType;
                    foreach (XmlSchemaElement body in (complexType.ContentTypeParticle as XmlSchemaSequence).Items)
                    {
                        // parts level
                        var clrBody = md.Body.Parts.Cast<MessagePartDescription>().FirstOrDefault(e => e.Name == body.Name);
                        if (clrBody != null)
                        {
                            var attributeBody = clrBody.MemberInfo.GetCustomAttributes(typeof(AnnotationAttribute), false).FirstOrDefault() as AnnotationAttribute;
                            if (attributeBody != null)
                            {
                                body.Annotation = CreateXmlSchemaAnnotation(attributeBody.Annotation, attributeBody.ExportXmlDoc, clrBody.MemberInfo);
                            }
                        }
                    }
                }
                retVal = true;
            }
            return retVal;
        }

        internal static bool ExportAnnotationForMessageHeaders(WsdlExporter exporter, OperationDescription operation, XmlSchemaElement elem)
        {
            bool retVal = false;
            MessageDescription md = operation.Messages.Cast<MessageDescription>().FirstOrDefault(e => e.Headers.Count > 0 && e.Headers.FirstOrDefault(h => h.Name == elem.Name) != null);
            if (md != null)
            {
                MemberInfo mi = md.Headers.FirstOrDefault(h => h.Name == elem.Name).MemberInfo;
                var attribute = mi.GetCustomAttributes(typeof(AnnotationAttribute), false).FirstOrDefault() as AnnotationAttribute;
                if (attribute != null)
                {
                    elem.Annotation = CreateXmlSchemaAnnotation(attribute.Annotation, attribute.ExportXmlDoc, mi);
                }
                retVal = true;
            }
            return retVal;
        }

        internal static bool ExportAnnotationForMessageParameters(WsdlExporter exporter, OperationDescription operation, XmlSchemaElement elem)
        {
            bool retVal = false;

            MessageDescription md = operation.Messages.Cast<MessageDescription>().FirstOrDefault(e => e.Headers.Count == 0 && e.MessageType == null && e.Body != null && e.Body.WrapperName == elem.Name);
            if (md != null)
            {
                if (elem.ElementSchemaType != null && elem.ElementSchemaType is XmlSchemaComplexType)
                {
                    // parts
                    XmlSchemaComplexType complexType = elem.ElementSchemaType as XmlSchemaComplexType;
                    if (complexType != null && complexType.ContentTypeParticle is XmlSchemaSequence)
                    {
                        foreach (XmlSchemaElement body in (complexType.ContentTypeParticle as XmlSchemaSequence).Items)
                        {
                            DocumentationAttribute attributeBody = null;
                            if (md.Body.ReturnValue != null)
                            {
                                // ReturnValue
                                attributeBody = operation.SyncMethod.ReturnTypeCustomAttributes.GetCustomAttributes(typeof(DocumentationAttribute), false).FirstOrDefault() as DocumentationAttribute;
                                retVal = true;
                            }
                            else
                            {
                                // part level
                                var clrPart = operation.SyncMethod.GetParameters().FirstOrDefault(e => e.Name == body.Name);
                                if (clrPart != null)
                                {
                                    attributeBody = clrPart.GetCustomAttributes(typeof(DocumentationAttribute), false).FirstOrDefault() as DocumentationAttribute;
                                }
                            }
                            if (attributeBody != null)
                            {
                                body.Annotation = CreateXmlSchemaAnnotation(attributeBody.Documentation);
                                retVal = true;
                            }
                        }
                    }
                }
            }
            return retVal;
        }

        internal static XmlSchemaAnnotation CreateXmlSchemaAnnotation(string text, bool bExportXmlDoc, MemberInfo memberInfo)
        {
            XElement element = null;
            XmlDocument doc = new XmlDocument();

            if (memberInfo != null && bExportXmlDoc)
            {
                Type type = memberInfo.DeclaringType == null ? (Type)memberInfo : memberInfo.DeclaringType;
                string memberName = XmlDocumentation.CreateMemberName(memberInfo);
                element = XmlDocumentation.Load(memberName, type);
            }
            if (element == null)
                element = new XElement("member", new XText(text));
            else
                element.AddFirst(new XText(text));
                           
            doc.LoadXml(element.ToString());
      
            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            XmlSchemaDocumentation documentation = new XmlSchemaDocumentation();
            documentation.Markup = doc.DocumentElement.ChildNodes.Cast<XmlNode>().ToArray();
            annotation.Items.Add(documentation);

            return annotation;
        }
        internal static XmlSchemaAnnotation CreateXmlSchemaAnnotation(string text)
        {
            return CreateXmlSchemaAnnotation(text, false, null);
        }
        #endregion
    }
}

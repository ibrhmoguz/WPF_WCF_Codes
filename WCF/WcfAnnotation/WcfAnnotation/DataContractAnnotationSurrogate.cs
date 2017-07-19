#region Namespaces

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

#endregion

namespace WcfPoc.WcfAnnotation
{
    /// <summary>
    /// Surrogate class for annotation
    /// </summary>
    public class DataContractAnnotationSurrogate : IDataContractSurrogate
    {
        public bool ExportAsText { get; set; }
        public bool IsBehaviorExtension { get; set; }

        #region IDataContractSurrogate
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            var annotationAttribute = clrType.GetCustomAttributes(typeof(AnnotationAttribute), false).FirstOrDefault() as AnnotationAttribute;

            // the config section has the highest priority 
            if (annotationAttribute != null && this.IsBehaviorExtension)
                annotationAttribute.ExportAsText = this.ExportAsText;

            return XmlDocumentation.Load(clrType, dataContractType, annotationAttribute);
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            var annotationAttribute = memberInfo.GetCustomAttributes(typeof(AnnotationAttribute), false).FirstOrDefault() as AnnotationAttribute;
            
            // the config section has the highest priority 
            if (annotationAttribute != null && this.IsBehaviorExtension)
                annotationAttribute.ExportAsText = this.ExportAsText;

            return XmlDocumentation.Load(memberInfo, dataContractType, annotationAttribute);
        }

        public Type GetDataContractType(Type type)
        {
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public System.CodeDom.CodeTypeDeclaration ProcessImportedType(System.CodeDom.CodeTypeDeclaration typeDeclaration, System.CodeDom.CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }

        public void GetKnownCustomDataTypes(System.Collections.ObjectModel.Collection<Type> customDataTypes)
        {
            customDataTypes.Add(typeof(Annotation));
            customDataTypes.Add(typeof(XElement));
        }
        #endregion
    }


    /// <summary>
    /// Annotation wrapper class with xml flat serialization
    /// </summary>
    [XmlSchemaProvider("GetSchemaAnnotation", IsAny = false)]
    [Serializable]
    class Annotation : IXmlSerializable
    {
        string _tns = string.Empty;
        XElement _annotation;

        public string Text { get; set; }

        public Annotation(XElement annotation)
        {
            _annotation = annotation;
        }
      
        #region IXmlSerializable Members
        public XmlSchema GetSchema()
        {
            // read schema
            StringReader reader = new StringReader(schemaXmlDocumentation);
            XmlSchema schema = XmlSchema.Read(reader, null);
            _tns = schema.TargetNamespace;
            return schema;
        }
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }
        public void WriteXml(XmlWriter writer)
        {
            if (_annotation != null)
            {
               _annotation.WriteTo(writer);
            }
        }
        #endregion

        public static XmlQualifiedName GetSchemaAnnotation(XmlSchemaSet xs)
        {
            StringReader reader = new StringReader(schemaXmlDocumentation);
            XmlSchema schema = XmlSchema.Read(reader, null);
            if (schema == null)
            {
                throw new Exception("Could not read schema");
            }
            xs.Add(schema);
            xs.Compile();

            XmlQualifiedName qname = new XmlQualifiedName("Annotation", "http://schemas.datacontract.org/2004/07/RKiss.WcfAnnotation");
            return qname;

        }

        private static string schemaXmlDocumentation = @"
            <xs:schema id='Annotation' 
                xmlns:tns='http://schemas.datacontract.org/2004/07/RKiss.WcfAnnotation' 
                targetNamespace='http://schemas.datacontract.org/2004/07/RKiss.WcfAnnotation' 
                attributeFormDefault='unqualified' 
                elementFormDefault='qualified' 
                xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element name='Annotation'>
                        <xs:complexType>
                          <xs:sequence>
                            <xs:choice maxOccurs='unbounded'>
                              <xs:element name='summary' type='xs:string' />
                              <xs:element maxOccurs='unbounded' name='param'>
                                <xs:complexType>
                                  <xs:simpleContent>
                                    <xs:extension base='xs:string'>
                                      <xs:attribute name='name' type='xs:string' use='required' />
                                    </xs:extension>
                                  </xs:simpleContent>
                                </xs:complexType>
                              </xs:element>
                              <xs:element name='returns' type='xs:string' />
                              <xs:element name='version' type='xs:string' />
                              <xs:element name='copyright' type='xs:string' />
                              <xs:element maxOccurs='unbounded' name='remarks' type='xs:string' />
                              <xs:element name='value' type='xs:string' />
                            </xs:choice>
                          </xs:sequence>
                          <xs:attribute name='name' type='xs:string' use='required' />
                        </xs:complexType>
                </xs:element>
            </xs:schema>";

    }  
}


#region Namespaces

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml.Schema;

#endregion

namespace WcfPoc.WcfAnnotation
{

    /// <summary>
    /// Plumbing class
    /// </summary>
    public class DocumentationAttribute : Attribute, 
        IServiceBehavior, 
        IContractBehavior, IOperationBehavior,
        IWsdlExportExtension
    {
        #region Private Fields
        ContractDescription _contractDescription;
        OperationDescription _operationDescription;
        #endregion

        #region Properties
        /// <summary>
        /// enable annotation and documentation process
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Documentation text
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// valid if the extension is applied
        /// </summary>
        public bool IsBehaviorExtensions { get; set; }

        /// <summary>
        /// Enable xml documentation from the source code (///)
        /// </summary>
        public bool ExportXmlDoc { get; set; }

        /// <summary>
        /// Export annotation in the DataContract as xml formatted text, otherwise IXmlSerializable object XElement
        /// </summary>
        public bool ExportAsText { get; set; }
        #endregion

        #region Constructors
        public DocumentationAttribute() : this(string.Empty, true) 
        { 
        }
        public DocumentationAttribute(string documentation) : this(documentation, true) 
        {
        }
        public DocumentationAttribute(string documentation, bool bExportXmlDoc)
        {
            this.Documentation = documentation;
            this.ExportXmlDoc = bExportXmlDoc;
            this.Enable = true;
            this.ExportAsText = false;
            this.IsBehaviorExtensions = false;
        }
        #endregion

        #region IServiceBehavior Members
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
            {
                var doc = endpoint.Contract.Behaviors.Find<DocumentationAttribute>();
                if (doc != null)
                {
                    doc.Enable = this.Enable;
                    doc.ExportAsText = this.ExportAsText;
                    doc.IsBehaviorExtensions = true;
                }

                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    doc = operation.Behaviors.Find<DocumentationAttribute>();
                    if (doc != null)
                    {
                        doc.Enable = this.Enable;
                        doc.ExportAsText = this.ExportAsText;
                        doc.IsBehaviorExtensions = true;
                    }
                }
            }
        }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
        #endregion

        #region IOperationBehavior Members
        public void AddBindingParameters(OperationDescription description, System.ServiceModel.Channels.BindingParameterCollection parameters)
        {
        }
        public void ApplyClientBehavior(OperationDescription description, System.ServiceModel.Dispatcher.ClientOperation proxy)
        {
        }
        public void ApplyDispatchBehavior(OperationDescription description, System.ServiceModel.Dispatcher.DispatchOperation dispatch)
        {
            if (this.Enable)
            {
                _operationDescription = description;
                ApplyDataContractSurrogate(description, this.IsBehaviorExtensions, this.ExportAsText);
            }
        }
        public void Validate(OperationDescription description)
        {
        }
        #endregion

        #region IContractBehavior Members
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {
            if (this.Enable)
            {
                _contractDescription = contractDescription;
                foreach (OperationDescription description in contractDescription.Operations)
                {
                    ApplyDataContractSurrogate(description, this.IsBehaviorExtensions, this.ExportAsText);
                }
            }
        }
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
        #endregion

        #region IWsdlExportExtension Members
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            if (this.Enable)
            {
                XsdDataContractExporter dataContractExporter;
                object dataContractExporterObj = null;
                if (exporter.State.TryGetValue(typeof(XsdDataContractExporter), out dataContractExporterObj))
                {
                    dataContractExporter = dataContractExporterObj as XsdDataContractExporter;
                }
                else
                {
                    XmlSchemaSet set = exporter.GeneratedXmlSchemas;
                    //Annotation.GetSchemaAnnotation(set);
                    //set.Compile();
                    dataContractExporter = new XsdDataContractExporter(set);
                    exporter.State.Add(typeof(XsdDataContractExporter), dataContractExporter);
                }
                if (dataContractExporter.Options == null)
                    dataContractExporter.Options = new ExportOptions();
                if (dataContractExporter.Options.DataContractSurrogate == null)
                    dataContractExporter.Options.DataContractSurrogate = new DataContractAnnotationSurrogate() { ExportAsText = this.ExportAsText, IsBehaviorExtension=this.IsBehaviorExtensions };
            }
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            if (this.Enable)
            {
                #region Wsdl:Documentation
                // [ServiceContract]
                var serviceAttribute = context.Endpoint.Contract.ContractType.GetCustomAttributes(typeof(DocumentationAttribute), false).FirstOrDefault() as DocumentationAttribute;
                if (serviceAttribute != null)
                {
                    context.WsdlPort.Service.Documentation = serviceAttribute.Documentation;
                    XmlDocumentation.Load(context.WsdlPort.Service.DocumentationElement, context.Endpoint.Contract.ContractType, serviceAttribute);
                }

                // [OperationContract]
                if (_operationDescription != null)
                {
                    var op = context.GetOperationBinding(_operationDescription);
                    var opAttribute = _operationDescription.SyncMethod.GetCustomAttributes(typeof(DocumentationAttribute), false).FirstOrDefault() as DocumentationAttribute;
                    if (opAttribute != null)
                    {
                        op.Documentation = opAttribute.Documentation;
                        XmlDocumentation.Load(op.DocumentationElement, _operationDescription.SyncMethod, opAttribute);
                    }

                    if (op.Output != null)
                    {
                        var retAttribute = _operationDescription.SyncMethod.ReturnTypeCustomAttributes.GetCustomAttributes(typeof(DocumentationAttribute), false).FirstOrDefault() as DocumentationAttribute;
                        if (retAttribute != null)
                            op.Output.Documentation = retAttribute.Documentation;
                    }
                    if (op.Input != null)
                    {
                        foreach (var item in _operationDescription.SyncMethod.GetParameters())
                        {
                            var attr = item.GetCustomAttributes(typeof(DocumentationAttribute), false).FirstOrDefault() as DocumentationAttribute; ;
                            if (attr != null)
                            {
                                // only one parameter can be used in this binding
                                op.Input.Documentation = attr.Documentation;
                                break;
                            }
                        }
                    }
                }
                #endregion

                MessageContractAnnotation.Export(exporter, context);
            }
        }
        #endregion

        #region Helpers
        private static void ApplyDataContractSurrogate(OperationDescription description, bool IsBehaviorExtensions, bool bExportAsText)
        {
            DataContractSerializerOperationBehavior dcsOperationBehavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            if (dcsOperationBehavior != null)
            {
                if (dcsOperationBehavior.DataContractSurrogate == null)
                    dcsOperationBehavior.DataContractSurrogate = new DataContractAnnotationSurrogate(){ ExportAsText = bExportAsText, IsBehaviorExtension = IsBehaviorExtensions };
            }
        }
        #endregion
    }



    /// <summary>
    /// Annotation class for XSD
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class AnnotationAttribute : Attribute
    {
        #region Properties
        public string Annotation { get; set; }
        public bool ExportXmlDoc { get; set; }
        public bool ExportAsText { get; set; }
        public bool IsBehaviorExtension { get; set; }
        #endregion

        #region Constructors
        public AnnotationAttribute() 
            : this(string.Empty, true, false) 
        { 
        }
        public AnnotationAttribute(string annotation)
            : this(annotation, true, false) 
        {
        }
        public AnnotationAttribute(string annotation, bool bExportXmlDoc)
            : this(annotation, bExportXmlDoc, false) 
        {
        }
        public AnnotationAttribute(string annotation, bool bExportXmlDoc, bool bExportAsText)
        {
            Annotation = annotation;
            ExportXmlDoc = bExportXmlDoc;
            ExportAsText = bExportAsText;
            IsBehaviorExtension = false;
        }
        #endregion
    }
}

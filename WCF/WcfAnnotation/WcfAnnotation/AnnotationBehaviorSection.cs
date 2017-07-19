#region Namespaces

using System;
using System.Configuration;
using System.ServiceModel.Configuration;

#endregion

namespace WcfPoc.WcfAnnotation
{
    
    #region AnnotationBehaviorSection
    /// <summary>
    /// Class for controling annotation from config file
    /// </summary>
    public class AnnotationBehaviorSection : BehaviorExtensionElement
    {
        #region Constructor(s)
        public AnnotationBehaviorSection()
        {
        }
        #endregion

        #region ConfigurationProperties
        [ConfigurationProperty("enable", DefaultValue = true)]
        public bool Enable
        {
            get { return (bool)base["enable"]; }
            set { base["enable"] = value; }
        }
        [ConfigurationProperty("exportAsText", DefaultValue = false)]
        public bool ExportAsText
        {
            get { return (bool)base["exportAsText"]; }
            set { base["exportAsText"] = value; }
        }
        #endregion

        #region BehaviorExtensionSection
        protected override object CreateBehavior()
        {
            DocumentationAttribute documentation = new DocumentationAttribute();
            documentation.Enable = (bool)this["enable"];
            documentation.ExportAsText = (bool)this["exportAsText"];
            return documentation;
        }

        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);
            AnnotationBehaviorSection section = (AnnotationBehaviorSection)from;
            this.Enable = section.Enable;
            this.ExportAsText = section.ExportAsText;
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                ConfigurationPropertyCollection collection = new ConfigurationPropertyCollection();
                collection.Add(new ConfigurationProperty("enable", typeof(bool), true));
                collection.Add(new ConfigurationProperty("exportAsText", typeof(bool), true));  
                return collection;
            }
        }
        #endregion

        #region BehaviorType
        public override Type BehaviorType
        {
            get { return typeof(DocumentationAttribute); }
        }
        #endregion
    }
    #endregion
}

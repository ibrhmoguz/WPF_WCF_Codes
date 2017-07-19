//*****************************************************************************
//    Description.....Contract Model - LocalRepositoryConcole
//                                
//    Author..........Roman Kiss, rkiss@pathcom.com
//    Copyright © 2008 ATZ Consulting Inc. (see included license.rtf file)    
//                        
//    Date Created:    07/07/08
//
//    Date        Modified By     Description
//-----------------------------------------------------------------------------
//    07/07/08    Roman Kiss     Initial Revision
//*****************************************************************************
//  
#region Namespaces
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;
using System.ServiceModel.Web;
using System.Web.Services;
using System.Windows.Forms;
using System.CodeDom;
using System.CodeDom.Compiler;
#endregion

namespace MetadataViewer
{
    public class MetadataHelper
    {
        #region GetMetadataSet
        public static MetadataSet GetMetadataSet(string url)
        {
            MetadataExchangeClientMode mode = MetadataExchangeClientMode.MetadataExchange;
            int maxReceivedMessageSize = 3000000;
            Uri address = new Uri(url);

            System.ServiceModel.Channels.Binding mexBinding = null;
            if (string.Compare(address.Scheme, "http", StringComparison.OrdinalIgnoreCase) == 0)
                mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
            else if (string.Compare(address.Scheme, "https", StringComparison.OrdinalIgnoreCase) == 0)
                mexBinding = MetadataExchangeBindings.CreateMexHttpsBinding();
            else if (string.Compare(address.Scheme, "net.tcp", StringComparison.OrdinalIgnoreCase) == 0)
                mexBinding = MetadataExchangeBindings.CreateMexTcpBinding();
            else if (string.Compare(address.Scheme, "net.pipe", StringComparison.OrdinalIgnoreCase) == 0)
                mexBinding = MetadataExchangeBindings.CreateMexNamedPipeBinding();
            else
                throw new Exception(string.Format("Not supported schema '{0}' for metadata exchange"));

            if (mexBinding is WSHttpBinding)
            {
                (mexBinding as WSHttpBinding).MaxReceivedMessageSize = maxReceivedMessageSize;
                mode = MetadataExchangeClientMode.HttpGet;
            }
            else if (mexBinding is CustomBinding)
                (mexBinding as CustomBinding).Elements.Find<TransportBindingElement>().MaxReceivedMessageSize = maxReceivedMessageSize;
            else
                throw new Exception(string.Format("Not supported binding for metadata exchange"));

            MetadataExchangeClient proxy = new MetadataExchangeClient(mexBinding);
            proxy.ResolveMetadataReferences = true;
            MetadataSet mds = proxy.GetMetadata(address, mode);
            return mds; 
        }
        #endregion      

        #region Misc
        public static string ErrorMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder(ex.Message + "\r\n");
            Exception inEx = ex.InnerException;
            while (inEx != null)
            {
                sb.AppendFormat("\r\n- {0}", inEx.Message);
                inEx = inEx.InnerException;
            }
            return sb.ToString();
        }

        //public string GetXmlMetadataSet(MetadataSet set)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    using (set.WriteTo
        //}
        #endregion

    }


}


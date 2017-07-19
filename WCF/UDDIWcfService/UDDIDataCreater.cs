using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Uddi3.Businesses;
using Microsoft.Uddi3;
using Microsoft.Uddi3.Services;
using Microsoft.Uddi3.TModels;
using Microsoft.Uddi3.Extensions;

namespace UDDIService
{
    class UDDIDataCreater
    {
        /// <summary>
        /// Creates the UDDI connection with windows authentication.
        /// </summary>
        /// <param name="UDDIURL">The UDDIURL.</param>
        /// <returns></returns>
        public static UddiConnection CreateUDDIConnection(string UDDIURL)
        {
            UddiSiteLocation usc = new UddiSiteLocation(
                    UDDIURL + "/inquire.asmx",
                    UDDIURL + "/publish.asmx",
                    UDDIURL + "/extension.asmx",
                    "UDDI",
                    AuthenticationMode.WindowsAuthentication);
            UddiConnection UDDIConnection = new UddiConnection(usc);

            return UDDIConnection;
        }

        /// <summary>
        /// Creates the UDDI connection .
        /// </summary>
        /// <param name="UDDIURL">The UDDIURL with uddi authentication.</param>
        /// <param name="UDDICUsername">The UDDIC username.</param>
        /// <param name="UDDICPassword">The UDDIC password.</param>
        /// <returns></returns>
        public static UddiConnection CreateUDDIConnection(string UDDIURL, string UDDICUsername, string UDDICPassword)
        {
            UddiSiteLocation usc = new UddiSiteLocation(
                    UDDIURL + "/inquire.asmx",
                    UDDIURL + "/publish.asmx",
                    UDDIURL + "/extension.asmx",
                    "UDDI",
                    AuthenticationMode.UddiAuthentication);
            UddiConnection UDDIConnection = new UddiConnection(usc, UDDICUsername, UDDICPassword);
            return UDDIConnection;
        }

        /// <summary>
        /// Creates the business entity.
        /// </summary>
        /// <param name="bName">Name of the business.</param>
        /// <returns></returns>
        public static BusinessEntity CreateBusinessEntity(string bName)
        {
            BusinessEntity bEntity = new BusinessEntity();
            bEntity.Names.Add(bName);
            return bEntity;
        }

        /// <summary>
        /// Creates the business entity.
        /// </summary>
        /// <param name="bName">Name of the business.</param>
        /// <param name="bDescription">The business description.</param>
        /// <returns></returns>
        public static BusinessEntity CreateBusinessEntity(string bName, string bDescription)
        {
            BusinessEntity bEntity = new BusinessEntity();
            bEntity.BusinessKey = null;
            bEntity.Names.Add(new Name(bName));
            bEntity.Descriptions.Add(new Description(bDescription));
            return bEntity;
        }

        /// <summary>
        /// Creates the business entity.
        /// </summary>
        /// <param name="bName">Name of the business.</param>
        /// <param name="bDescription">The business description.</param>
        /// <param name="bContact">The business contact.</param>
        /// <param name="bURL">The business URL.</param>
        /// <returns></returns>
        public static BusinessEntity CreateBusinessEntity(string bName, string bDescription, Contact bContact, string bURL)
        {
            BusinessEntity bEntity = new BusinessEntity();
            bEntity.Names.Add(bName);
            bEntity.Descriptions.Add(bDescription);
            bEntity.Contacts.Add(bContact);
            bEntity.DiscoveryUrls.Add(CreateDiscoveryURL(bURL));
            return bEntity;
        }

        /// <summary>
        /// Creates the business entity.
        /// </summary>
        /// <param name="bName">Name of the business.</param>
        /// <param name="bDescription">The business description.</param>
        /// <param name="bContact">The business contact.</param>
        /// <param name="bURL">The business URL.</param>
        /// <param name="bKeyedReference">The business keyed reference.</param>
        /// <returns></returns>
        public static BusinessEntity CreateBusinessEntity(string bName, string bDescription, Contact bContact, string bURL, KeyedReference bKeyedReference)
        {
            BusinessEntity bEntity = new BusinessEntity();
            bEntity.Names.Add(bName);
            bEntity.Descriptions.Add(bDescription);
            bEntity.Contacts.Add(bContact);
            bEntity.DiscoveryUrls.Add(CreateDiscoveryURL(bURL));
            bEntity.CategoryBag.KeyedReferences.Add(bKeyedReference);
            return bEntity;
        }

        /// <summary>
        /// Creates the T model.
        /// </summary>
        /// <param name="tMName">Name of the t Model.</param>
        /// <returns></returns>
        public static TModel CreateTModel(string tMName)
        {
            TModel tModel = new TModel(tMName);
            return tModel;
        }

        /// <summary>
        /// Creates the T model.
        /// </summary>
        /// <param name="tMName">Name of the t Model.</param>
        /// <param name="tMDescription">The t Model description.</param>
        /// <returns></returns>
        public static TModel CreateTModel(string tMName, string tMDescription)
        {
            TModel tModel = new TModel(tMName);
            tModel.Descriptions.Add(tMDescription);
            return tModel;
        }

        /// <summary>
        /// Creates the T model.
        /// </summary>
        /// <param name="tMName">Name of the t Model.</param>
        /// <param name="tMDescription">The t Model description.</param>
        /// <param name="tMKeyedRefence">The t Model keyed refence.</param>
        /// <returns></returns>
        public static TModel CreateTModel(string tMName, string tMDescription, KeyedReference tMKeyedRefence)
        {
            TModel tModel = new TModel(tMName);
            tModel.Descriptions.Add(tMDescription);
            tModel.CategoryBag.KeyedReferences.Add(tMKeyedRefence);
            return tModel;
        }

        /// <summary>
        /// Creates the business service.
        /// </summary>
        /// <param name="sName">Name of the service.</param>
        /// <returns></returns>
        public static BusinessService CreateBusinessService(string sName)
        {
            BusinessService bService = new BusinessService(sName);
            return bService;
        }

        /// <summary>
        /// Creates the business service.
        /// </summary>
        /// <param name="sName">Name of the service.</param>
        /// <param name="sDescription">The service description.</param>
        /// <returns></returns>
        public static BusinessService CreateBusinessService(string sName, string sDescription)
        {
            BusinessService bService = new BusinessService(sName);
            bService.Descriptions.Add(sDescription);
            return bService;
        }

        /// <summary>
        /// Creates the keyed reference.
        /// </summary>
        /// <param name="tModelKey">The t model key.</param>
        /// <param name="keyValue">The key value.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static KeyedReference CreateKeyedReference(string tModelKey, string keyValue, string keyName)
        {
            KeyedReference bKeyedReference = new KeyedReference(tModelKey,keyValue,keyName);
            return bKeyedReference;
        }

        /// <summary>
        /// Creates the discovery URL.
        /// </summary>
        /// <param name="tUrl">The t URL.</param>
        /// <returns></returns>
        public static DiscoveryUrl CreateDiscoveryURL(string tUrl)
        {
            DiscoveryUrl dUrl = new DiscoveryUrl();
            dUrl.Text = tUrl;
            return dUrl;
        }

        /// <summary>
        /// Creates the contact.
        /// </summary>
        /// <param name="cName">Name of the contact.</param>
        /// <param name="cDescription">The contact description.</param>
        /// <returns></returns>
        public static Contact CreateContact(string cName, string cDescription)
        {
            Contact bContact = new Contact();
            bContact.PersonNames.Add(new PersonName(cName));
            bContact.Descriptions.Add(cDescription);
            return bContact;
        }

        /// <summary>
        /// Creates the contact.
        /// </summary>
        /// <param name="cName">Name of the contact.</param>
        /// <param name="cDescription">The contact description.</param>
        /// <param name="cEMail">The contact E mail.</param>
        /// <param name="cPhone">The contact phone.</param>
        /// <param name="cAddress">The contact address.</param>
        /// <returns></returns>
        public static Contact CreateContact(string cName, string cDescription, string cEMail, string cPhone, string cAddress)
        {
            Contact bContact = new Contact();
            bContact.PersonNames.Add(new PersonName(cName));
            bContact.Descriptions.Add(cDescription);
            bContact.Emails.Add(cEMail);
            bContact.Phones.Add(cPhone);
            bContact.Addresses.Add(new Address(new AddressLine(cAddress)));
            return bContact;
        }

        /// <summary>
        /// Creates the binding template.
        /// </summary>
        /// <returns></returns>
        public static BindingTemplate CreateBindingTemplate()
        {
            BindingTemplate bTemplate = new BindingTemplate();
            return bTemplate;
        }

        /// <summary>
        /// Creates the binding template.
        /// </summary>
        /// <param name="bAccessPoint">The access point.</param>
        /// <returns></returns>
        public static BindingTemplate CreateBindingTemplate(AccessPoint bAccessPoint)
        {
            BindingTemplate bTemplate = new BindingTemplate();
            bTemplate.AccessPoint = bAccessPoint;
            return bTemplate;
        }

        /// <summary>
        /// Creates the access point.
        /// </summary>
        /// <param name="tAccessPoint">The access point.</param>
        /// <returns></returns>
        public static AccessPoint CreateAccessPoint(string tAccessPoint)
        {
            AccessPoint accPoint = new AccessPoint();
            accPoint.Text = tAccessPoint;
            return accPoint;
        }
    }
}

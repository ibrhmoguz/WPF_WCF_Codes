using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Uddi3;
using Microsoft.Uddi3.Businesses;
using Microsoft.Uddi3.Services;
using Microsoft.Uddi3.TModels;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Resources;
using System.Globalization;
using System.Collections;

namespace UDDIService
{
    class UDDIManagement
    {
        
        private UddiConnection UDDIConnection;
        public bool isConnected = false;
        private Dictionary<string, string> serviceResponseTypes;
        private Dictionary<string, string> serviceSecurityTypes;
        private Dictionary<string, string> serviceNamesList;

        /// <summary>
        /// Initializes a new instance of the <see cref="UDDIManagement"/> class.
        /// </summary>
        /// <param name="UDDIURL">The UDDIURL.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public UDDIManagement(string UDDIURL, string username, string password)
        {
            try
            {
                UDDIConnection = UDDIDataCreater.CreateUDDIConnection(UDDIURL, username, password);
                
                if (null != UDDIConnection)
                {
                    isConnected = true;
                    GenerateServiceResponseTypesDictionary();
                    GenerateServiceSecurityTypesDictionary();
                    GenerateServiceNameListDictionary();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to connect to UDDI Server " + ex.Message);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UDDIManagement"/> class.
        /// </summary>
        /// <param name="UDDIURL">The UDDIURL.</param>
        public UDDIManagement(string UDDIURL)
        {
            try
            {
                UDDIConnection = UDDIDataCreater.CreateUDDIConnection(UDDIURL);
                if (null != UDDIConnection)
                {
                    isConnected = true;
                    GenerateServiceResponseTypesDictionary();
                    GenerateServiceSecurityTypesDictionary();
                    GenerateServiceNameListDictionary();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to connect to UDDI Server " + ex.Message);
            }   
        }


        /// <summary>
        /// Generates the service response types.
        /// read all response types from resource file and put them into dictionary 
        /// </summary>
        public void GenerateServiceResponseTypesDictionary()
        {
            serviceResponseTypes = new Dictionary<string, string>();
            ResourceSet resourceSet = UDDIWcfService.Properties.ResponseTypesResources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                serviceResponseTypes.Add(entry.Key.ToString(), entry.Value.ToString());
        }

        /// <summary>
        /// Generates the service security types.
        /// read all security types from resource file and put them into dictionary 
        /// </summary>
        public void GenerateServiceSecurityTypesDictionary()
        {
            serviceSecurityTypes = new Dictionary<string, string>();
            ResourceSet resourceSet = UDDIWcfService.Properties.SecurityTypeResources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                serviceSecurityTypes.Add(entry.Key.ToString(), entry.Value.ToString());
        }

        /// <summary>
        /// Generates the service name list.
        /// read all services from resource file and put them into dictionary 
        /// </summary>
        public void GenerateServiceNameListDictionary()
        {
            serviceNamesList = new Dictionary<string, string>();
            ResourceSet resourceSet = UDDIWcfService.Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                serviceNamesList.Add(entry.Key.ToString(), entry.Value.ToString());
        }

        /// <summary>
        /// Adds the T model.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="keyValue">The key value.</param>
        /// <param name="keyName">Name of the key.</param>
        public void AddTModel(string name, string description, string parent, string keyValue, string keyName)
        {
            TModel tm = UDDIDataCreater.CreateTModel(name, description);

            //search if tmodel has parent tmodel
            string parentKey = UDDISearcher.GetTModelKey(UDDIConnection, parent);

            if (parentKey != string.Empty)
            {
                //set the parent tmodel key of child tmodel 
                KeyedReference kf = UDDIDataCreater.CreateKeyedReference(parentKey, keyValue, keyName);
                tm.CategoryBag.KeyedReferences.Add(kf);
            }
            try
            {
                UDDIPublisher.SaveTModel(UDDIConnection, tm);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to save the TModel " + exception.Message);
            }
        }

        /// <summary>
        /// Adds the provider.
        /// </summary>
        /// <param name="pName">Name of the provider.</param>
        /// <param name="pDescription">The provider description.</param>
        public void AddProvider(string pName, string pDescription)
        {
            try
            {
                UDDIPublisher.SaveBusinessEntity(UDDIConnection, UDDIDataCreater.CreateBusinessEntity(pName, pDescription));
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to add the provider " + ex.Message);
            }
        }

        /// <summary>
        /// Adds the contact.
        /// </summary>
        /// <param name="pName">Name of the provider.</param>
        /// <param name="cName">Name of the contact.</param>
        /// <param name="cDescription">The contact description.</param>
        /// <param name="cEMail">The contact E mail.</param>
        /// <param name="cPhone">The contact phone.</param>
        /// <param name="cAddress">The contact address.</param>
        public void AddContact(string pName, string cName, string cDescription, string cEMail, string cPhone, string cAddress)
        {
            string pKey = UDDISearcher.GetBusinessKey(UDDIConnection, pName);
            if(!String.IsNullOrEmpty(pKey))
            {
                BusinessEntity bEntity = UDDISearcher.GetBusinessEntity(UDDIConnection, pKey);
                bEntity.Contacts.Add(UDDIDataCreater.CreateContact(cName, cDescription, cEMail, cPhone, cAddress));
                try
                {
                    UDDIPublisher.SaveBusinessEntity(UDDIConnection, bEntity);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to add the contact " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Updates the service name and descriotion.
        /// </summary>
        /// <param name="sName">Name of the service.</param>
        /// <param name="newSName">New name of the service.</param>
        /// <param name="newSDescription">The new service description.</param>
        public void UpdateService(string sName, string newSName,string newSDescription)
        {
            BusinessService bService = UDDISearcher.GetService(UDDIConnection, sName);
            if (null != bService)
            {
                bService.Names.Clear();
                bService.Descriptions.Clear();
                bService.Names.Add(new Name(newSName));
                bService.Descriptions.Add(new Description(newSDescription));
                UDDIPublisher.SaveBusinessService(UDDIConnection,bService);
            }
        }

        /// <summary>
        /// Adds the service. 
        /// </summary>
        /// <param name="pName">Name of the provider.</param>
        /// <param name="sName">Name of the service.</param>
        /// <param name="sDescription">The service description.</param>
        /// <param name="aPoint">Access point of the service.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="keyValue">The key value.</param>
        public void AddService(string pName, string sName,string sDescription ,string aPoint, string categoryName, string keyName, string keyValue)
        {
            if (UDDISearcher.GetServiceKey(UDDIConnection, sName).Equals(String.Empty))
            {
                BusinessService bService = UDDIDataCreater.CreateBusinessService(sName, sDescription);
                string bKey = UDDISearcher.GetBusinessKey(UDDIConnection, pName);
                bService.BusinessKey = bKey;
                bService.BindingTemplates.Add(UDDIDataCreater.CreateBindingTemplate(UDDIDataCreater.CreateAccessPoint(aPoint)));
                bService.CategoryBag.KeyedReferences.Add(UDDIDataCreater.CreateKeyedReference(UDDISearcher.GetTModelKey(UDDIConnection, categoryName), keyValue, keyName));
                try
                {
                    UDDIPublisher.SaveBusinessService(UDDIConnection, bService);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to add the service " + ex.Message);
                }
            }
            else
            {
                BusinessService bService = UDDISearcher.GetService(UDDIConnection,sName);
                bService.BindingTemplates.Add(UDDIDataCreater.CreateBindingTemplate(UDDIDataCreater.CreateAccessPoint(aPoint)));
                bService.CategoryBag.KeyedReferences.Add(UDDIDataCreater.CreateKeyedReference(UDDISearcher.GetTModelKey(UDDIConnection, categoryName), keyValue, keyName));
                try
                {
                    UDDIPublisher.SaveBusinessService(UDDIConnection, bService);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to add the service " + ex.Message);
                }
            }
        }


        public class BindigTemplateComparer : IEqualityComparer<BindingTemplate>
        {
            public bool Equals(BindingTemplate x, BindingTemplate y)
            {
                return x.AccessPoint.Text == y.AccessPoint.Text;
            }

            public int GetHashCode(BindingTemplate obj)
            {
                return obj.BindingKey.GetHashCode();
            }
        }
        /// <summary>
        /// Adds the service to the provider.
        /// </summary>
        /// <param name="pName">Name of the provider.</param>
        /// <param name="aPoint">Access point.</param>
        /// <param name="categoryNameList">The category list for the service.</param>
        public void AddService(string pName, string aPoint, List<CategoryObject> categoryNameList)
        {
            //if service not exists
            if (UDDISearcher.GetServiceKey(UDDIConnection, categoryNameList[0].cName + " Service").Equals(String.Empty))
            {
                //create service
                BusinessService bService = UDDIDataCreater.CreateBusinessService(categoryNameList[0].cName + " Service", "Service description for " + categoryNameList[0].cName + " Service");
                string bKey = UDDISearcher.GetBusinessKey(UDDIConnection, pName);
                bService.BusinessKey = bKey;
                //create binding template of the service
                bService.BindingTemplates.Add(UDDIDataCreater.CreateBindingTemplate(UDDIDataCreater.CreateAccessPoint(aPoint)));

                KeyedReference kReference;
                for (int i = 0; i < categoryNameList.Count; i++)
                {
                    kReference = UDDIDataCreater.CreateKeyedReference(UDDISearcher.GetTModelKey(UDDIConnection, categoryNameList[i].tModelName), UDDISearcher.GetCategory(UDDIConnection, categoryNameList[i].tModelName, categoryNameList[i].cName), categoryNameList[i].cName);
                    //add category to the service if not exits
                    if (!bService.CategoryBag.KeyedReferences.Contains(kReference))
                        bService.CategoryBag.KeyedReferences.Add(kReference);
                    //add category to the binding template if not exits
                    if (/*i > 0 && */!bService.BindingTemplates[bService.BindingTemplates.Count - 1].CategoryBag.KeyedReferences.Contains(kReference))
                        bService.BindingTemplates[bService.BindingTemplates.Count - 1].CategoryBag.KeyedReferences.Add(kReference);
                }
                try
                {
                    UDDIPublisher.SaveBusinessService(UDDIConnection, bService);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to add the service " + ex.Message);
                }
            }
            else
            {
                //find the service
                BusinessService bService = UDDISearcher.GetService(UDDIConnection, categoryNameList[0].cName + " Service");
                
                BindingTemplate bTemplate = UDDIDataCreater.CreateBindingTemplate(UDDIDataCreater.CreateAccessPoint(aPoint));
                //check if the binding already exists
                if (!bService.BindingTemplates.Contains(bTemplate, new BindigTemplateComparer()))
                {
                    //add binding template to the service
                    bService.BindingTemplates.Add(bTemplate);
                    KeyedReference kReference;
                    for (int i = 0; i < categoryNameList.Count; i++)
                    {
                        kReference = UDDIDataCreater.CreateKeyedReference(UDDISearcher.GetTModelKey(UDDIConnection, categoryNameList[i].tModelName), UDDISearcher.GetCategory(UDDIConnection, categoryNameList[i].tModelName, categoryNameList[i].cName), categoryNameList[i].cName);
                        //add category to the service if not exits
                        if (!bService.CategoryBag.KeyedReferences.Contains(kReference))
                            bService.CategoryBag.KeyedReferences.Add(kReference);
                        //add category to the binding template if not exits
                        if (/*i > 0 &&*/ !bService.BindingTemplates[bService.BindingTemplates.Count - 1].CategoryBag.KeyedReferences.Contains(kReference))
                            bService.BindingTemplates[bService.BindingTemplates.Count - 1].CategoryBag.KeyedReferences.Add(kReference);
                    }
                    try
                    {
                        UDDIPublisher.SaveBusinessService(UDDIConnection, bService);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to update the service " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all businesses.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllBusinesses()
        {
            try
            {
                return UDDISearcher.GetAllBusinesses(UDDIConnection);
            }
            catch (Exception ex)
            {
                isConnected = false;
                throw new Exception("Connection problem!");
            }
        }

        /// <summary>
        /// Gets all T models.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllTModels()
        {
            return UDDISearcher.GetAllTModels(UDDIConnection);
        }

        /*public string[] GetMSEServices()
        {
            return UDDISearcher.GetAllMSEServices();
        }*/

        public struct CategoryObject
        {
            public string tModelName;
            public string cName;
        }

        /// <summary>
        /// Gets necessary information from complete names of the services which comes from MSE web service.
        /// Create 3 categories for a service
        /// formats of the service as the parameter https://<application-address>:<application-port>/<MissionName>/WS-I Profile/example.svc 
        /// or https://<application-address>:<application-port>/WS-I Profile/ATOService.svc
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public List<CategoryObject> ServiceNameParser(string serviceName)
        {
            List<CategoryObject> resultList = new List<CategoryObject>();
            
            //Category types for the services and binding templates
            CategoryObject securityType, serviceType, responseType;
            //tmodel for the security type is Descriptions
            securityType.tModelName = UDDIWcfService.Properties.Resources.TModelDescriptions;
            //tmodel for the security type is Services
            serviceType.tModelName = UDDIWcfService.Properties.Resources.TModelService;
            //tmodel for the security type is Descriptions
            responseType.tModelName = UDDIWcfService.Properties.Resources.TModelDescriptions;
            
            try
            {
                string[] parsedServiceAccessPoint = Regex.Split(serviceName, "/");
                int sIndex = parsedServiceAccessPoint.Length - 2;
                if (!serviceSecurityTypes.TryGetValue(parsedServiceAccessPoint[sIndex], out securityType.cName))
                    securityType.cName = parsedServiceAccessPoint[sIndex];
                string[] parsedServiceName = Regex.Split(parsedServiceAccessPoint[sIndex + 1], "Service");
                if (!serviceNamesList.TryGetValue(parsedServiceName[0], out serviceType.cName))
                    serviceType.cName = parsedServiceName[0];
                responseType.cName = parsedServiceName[1];
                string sRType = Regex.Split(responseType.cName, ".svc")[0];
                if (sRType.Equals(String.Empty))
                    responseType.cName = UDDIWcfService.Properties.ResponseTypesResources.AirC2DataModel;
                else
                    if (!serviceResponseTypes.TryGetValue(sRType, out responseType.cName))
                        responseType.cName = sRType;
            }
            catch (Exception ex)
            {
                throw new Exception("Parsing error " + ex.Message);
            }
            resultList.Add(serviceType);
            resultList.Add(securityType);
            resultList.Add(responseType);

            return resultList;
        }

        /// <summary>
        /// Deletes the business all services.
        /// </summary>
        /// <param name="pName">Name of the provider</param>
        public void DeleteBusinessAllServices(string pName)
        {
            try
            {
                if (UDDISearcher.IsServiceExists(UDDIConnection, pName))
                    UDDIPublisher.DeleteBusinessAllServices(UDDIConnection, pName);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to delete the services " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes the business service.
        /// </summary>
        /// <param name="sName">Name of the service.</param>
        public void DeleteBusinessService(string sName)
        {
            try
            {
                string sKey = UDDISearcher.GetServiceKey(UDDIConnection, sName);
                if(! String.IsNullOrEmpty(sKey))
                    UDDIPublisher.DeleteBusinessService(UDDIConnection, sKey);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to delete the service " + ex.Message);
            }
        }

    }
}

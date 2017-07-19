using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Uddi3;
using Microsoft.Uddi3.Businesses;
using Microsoft.Uddi3.Services;
using Microsoft.Uddi3.TModels;
using Microsoft.Uddi3.Extensions;

namespace UDDIService
{
    public class UDDISearcher
    {

        /// <summary>
        /// Gets the business entity object.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="bKey">The business key.</param>
        /// <returns></returns>
        public static BusinessEntity GetBusinessEntity(UddiConnection UDDIConnection, string bKey)
        {
            if (!String.IsNullOrEmpty(bKey))
            {
                GetBusinessDetail bDetailObject = new GetBusinessDetail();

                bDetailObject.BusinessKeys.Add(bKey);
                BusinessDetail businessDetail = bDetailObject.Send(UDDIConnection);
                if (businessDetail.BusinessEntities.Count > 0)
                    return businessDetail.BusinessEntities[0];
            }
            return null;
            
        }

        /// <summary>
        /// Gets the business key.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="pName">Name of the provider.</param>
        /// <returns></returns>
        public static string GetBusinessKey(UddiConnection UDDIConnection, string pName)
        {
            FindBusiness findBusiness = new FindBusiness(pName);
            BusinessList bList = findBusiness.Send(UDDIConnection);
            if(bList.BusinessInfos.Count > 0)
                return bList.BusinessInfos[0].BusinessKey;
            return String.Empty;
        }

        /// <summary>
        /// Gets all MSE full path of the service from MSE web service.
        /// </summary>
        /// <returns></returns>
        /*public static string[] GetAllMSEServices()
        {
            MSEServiceCatalogue.ServiceModelClient serviceCatalogue = new MSEServiceCatalogue.ServiceModelClient("NetTcpBinding_ServiceModel");
            return serviceCatalogue.GetVirtualizedURLs("localRS");
        }*/

        /// <summary>
        /// Gets all businesses.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <returns></returns>
        public static List<string> GetAllBusinesses(UddiConnection UDDIConnection)
        {
            List<string> retList = new List<string>();
            FindBusiness findBusiness = new FindBusiness("%");
            findBusiness.FindQualifiers.Add(FindQualifier.ApproximateMatch);
            BusinessList bList = findBusiness.Send(UDDIConnection);
            
            foreach (BusinessInfo bInfo in bList.BusinessInfos)
                retList.Add(bInfo.Names[0].Text);
            return retList;
        }

        /// <summary>
        /// Gets all T models.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <returns></returns>
        public static List<string> GetAllTModels(UddiConnection UDDIConnection)
        {
            List<string> retList = new List<string>();
            FindTModel findTModel = new FindTModel("%");
            findTModel.FindQualifiers.Add(FindQualifier.ApproximateMatch);
            findTModel.FindQualifiers.Add(FindQualifier.SortByNameAscending);
            TModelList tMList = findTModel.Send(UDDIConnection);

            foreach (TModelInfo tMInfo in tMList.TModelInfos)
                retList.Add(tMInfo.Name.Text);
            return retList;
        }

        /// <summary>
        /// Gets the service key from service name.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="sName">Name of the service.</param>
        /// <returns></returns>
        public static string GetServiceKey(UddiConnection UDDIConnection, string sName)
        {
            FindService findService = new FindService(sName);
            ServiceList bList = findService.Send(UDDIConnection);
            if (bList.ServiceInfos.Count > 0)
                return bList.ServiceInfos[0].ServiceKey;
            return String.Empty;
        }

        /// <summary>
        /// Gets the service object.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="sName">Name of the service.</param>
        /// <returns></returns>
        public static BusinessService GetService(UddiConnection UDDIConnection, string sName)
        {
            GetServiceDetail sDetailObjList = new GetServiceDetail();
            sDetailObjList.ServiceKeys.Add(GetServiceKey(UDDIConnection, sName));
            ServiceDetail sDetailObj = sDetailObjList.Send(UDDIConnection);

            if (sDetailObj.BusinessServices.Count > 0)
                return sDetailObj.BusinessServices[0];
            return null;
        }

        /// <summary>
        /// Gets the Tmodel key.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="tMName">Name of the tModel.</param>
        /// <returns></returns>
        public static string GetTModelKey(UddiConnection UDDIConnection, string tMName)
        {
            FindTModel findTModel = new FindTModel(tMName);
            TModelList tMList = findTModel.Send(UDDIConnection);
            if (tMList.TModelInfos.Count > 0)
                return tMList.TModelInfos[0].TModelKey;
            return String.Empty;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="tModelName">Name of the t model.</param>
        /// <param name="cKeyName">Name of the category key.</param>
        /// <returns></returns>
        public static string GetCategory(UddiConnection UDDIConnection, string tModelName, string cKeyName)
        {
            Category cat = new Category(GetTModelKey(UDDIConnection, tModelName));
            cat.RelationshipQualifiers.Add(RelationshipQualifier.root);

            GetRelatedCategories grc = new GetRelatedCategories();

            grc.Categories.Add(cat);

            CategoryList list = grc.Send(UDDIConnection);
            string categoryKeyVal = "";
            foreach (CategoryInfo info in list.CategoryInfos)
            {
                foreach (CategoryValue cv in info.Roots)
                {
                    categoryKeyVal = GetCategoryItems(UDDIConnection, info.TModelKey, cv.KeyValue, cKeyName);
                    if (!categoryKeyVal.Equals(String.Empty))
                        return categoryKeyVal;
                }
            }

            return "";
        }

        /// <summary>
        /// Gets the category items.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="tModelKey">The t model key.</param>
        /// <param name="cKeyValue">The category key value.</param>
        /// <param name="cKeyName">Name of the category.</param>
        /// <returns></returns>
        public static string GetCategoryItems(UddiConnection UDDIConnection, string tModelKey, string cKeyValue, string cKeyName)
        {
            
            Category cat = new Category(tModelKey, cKeyValue);
            cat.RelationshipQualifiers.Add(RelationshipQualifier.child);

            GetRelatedCategories grc = new GetRelatedCategories();

            grc.Categories.Add(cat);

            CategoryList list = grc.Send(UDDIConnection);

            foreach (CategoryInfo info in list.CategoryInfos)
            {
                foreach (CategoryValue cv in info.Children)
                {
                    if (cv.KeyName.Equals(cKeyName))
                        return cv.KeyValue;
                    GetCategoryItems(UDDIConnection, tModelKey, cv.KeyValue, cKeyName);
                }
            }

            return "";
        }

        /// <summary>
        /// Determines whether there is any service on the business or not.
        /// </summary>
        /// <param name="UDDIConnection">The UDDI connection.</param>
        /// <param name="pName">Name of the provider</param>
        /// <returns>
        ///   <c>true</c> if [is service exists] [the specified UDDI connection]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsServiceExists(UddiConnection UDDIConnection, string pName)
        {
            FindBusiness findBusiness = new FindBusiness(pName);
            findBusiness.FindQualifiers.Add(FindQualifier.ApproximateMatch);
            BusinessList bList = findBusiness.Send(UDDIConnection);
            if (bList.BusinessInfos[0].ServiceInfos.Count > 0)
                return true;
            return false;
        }
    }
}

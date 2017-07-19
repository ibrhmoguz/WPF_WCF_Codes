using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Uddi3.Businesses;
using Microsoft.Uddi3;
using Microsoft.Uddi3.Services;
using Microsoft.Uddi3.TModels;

namespace UDDIService
{
    class UDDIPublisher
    {
        /// <summary>
        /// Saves the business entity.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="bEntity">The business entity.</param>
        public static void SaveBusinessEntity(UddiConnection uConn,BusinessEntity bEntity)
        {
            SaveBusiness sBusiness = new SaveBusiness(bEntity);
            sBusiness.Send(uConn);
        }

        /// <summary>
        /// Saves the business service.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="bService">The business service.</param>
        public static void SaveBusinessService(UddiConnection uConn, BusinessService bService)
        {
            SaveService sService = new SaveService(bService);
            sService.Send(uConn);
        }

        /// <summary>
        /// Saves the binding template.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="bTemplate">The business template.</param>
        public static void SaveBindingTemplate(UddiConnection uConn, BindingTemplate bTemplate)
        {
            SaveBinding sBinding = new SaveBinding(bTemplate);
            sBinding.Send(uConn);
        }

        /// <summary>
        /// Saves the T model.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="tModel">The t model.</param>
        public static void SaveTModel(UddiConnection uConn, TModel tModel)
        {
            SaveTModel sTModel = new SaveTModel(tModel);
            TModelDetail tDetail = sTModel.Send(uConn);
        }

        /// <summary>
        /// Deletes the business entity.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="bEntity">The business entity.</param>
        public static void DeleteBusinessEntity(UddiConnection uConn, BusinessEntity bEntity)
        {
            DeleteBusiness dBusiness = new DeleteBusiness(bEntity.BusinessKey);
            dBusiness.Send(uConn);
        }

        /// <summary>
        /// Deletes the business's all services.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="pName">Name of the provider</param>
        public static void DeleteBusinessAllServices(UddiConnection uConn, string pName)
        {
            BusinessEntity businessEntity = UDDISearcher.GetBusinessEntity(uConn, UDDISearcher.GetBusinessKey(uConn,pName));
            foreach(BusinessService businessService in businessEntity.BusinessServices)
            {
                DeleteService dService = new DeleteService(businessService.ServiceKey);
                dService.Send(uConn);
            }
        }

        /// <summary>
        /// Deletes the business service.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="bServiceKey">The business service key.</param>
        public static void DeleteBusinessService(UddiConnection uConn, string bServiceKey)
        {
            DeleteService dService = new DeleteService(bServiceKey);
            dService.Send(uConn);
        }

        /// <summary>
        /// Deletes the binding template.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="bTemplate">The binding template.</param>
        public static void DeleteBindingTemplate(UddiConnection uConn, BindingTemplate bTemplate)
        {
            DeleteBinding dBinding = new DeleteBinding(bTemplate.BindingKey);
            dBinding.Send(uConn);
        }

        /// <summary>
        /// Deletes the T model.
        /// </summary>
        /// <param name="uConn">The UDDI connection.</param>
        /// <param name="tModel">The t model.</param>
        public static void DeleteTModel(UddiConnection uConn, TModel tModel)
        {
            DeleteTModel dTModel = new DeleteTModel(tModel.TModelKey);
            dTModel.Send(uConn);
        }
    }
}

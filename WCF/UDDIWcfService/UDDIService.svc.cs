using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace UDDIService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IUDDIService
    {
        public string CreateUDDIServices(string[] servicesList, string providerName, string UDDIServerURL)
        {
            try
            {
                UDDIManagement mng = new UDDIManagement(UDDIServerURL);
                
                mng.DeleteBusinessAllServices(providerName);
                foreach (string service in servicesList)
                    mng.AddService(providerName, service, mng.ServiceNameParser(service));
                return "Success";
            }   
            catch (Exception ex)
            {
                return "Failed " + ex.Message;
                
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NorthwindOps.Types;

namespace NorthwindOps
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface INorthwindOpsService
    {
        [OperationContract]
        List<Suppliers> SuppliersByCountry(string pCountry);

        [OperationContract]
        List<Orders> OrdersByCustomer(string pCountry);

        [OperationContract]
        List<Categories> CategoryList();

        [OperationContract]
        List<Products> ProductsByCategory(int pCategoryID);

    }
}

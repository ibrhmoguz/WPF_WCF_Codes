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
    public class NorthwindOpsService : INorthwindOpsService
    {

        public List<Suppliers> SuppliersByCountry(string pCountry)
        {
            NorthwindEntities dbContext = new NorthwindEntities();

            var result = from s in dbContext.Suppliers
                         where s.Country == pCountry
                         select s;

            return result.ToList<Suppliers>();
        }

        public List<Orders> OrdersByCustomer(string pCustomer)
        {
            NorthwindEntities dbContext = new NorthwindEntities();

            var result = from s in dbContext.Orders
                         where s.CustomerID == pCustomer
                         select s;

            return result.ToList<Orders>();
        }

        public List<Categories> CategoryList()
        {
            NorthwindEntities dbContext = new NorthwindEntities();

            var result = from s in dbContext.Categories
                         select s;

            return result.ToList<Categories>();
        }

        public List<Products> ProductsByCategory(int pCategoryID)
        {
            NorthwindEntities dbContext = new NorthwindEntities();

            var result = from s in dbContext.Products
                         where s.CategoryID == pCategoryID
                         select s;

            return result.ToList<Products>();
        }



    }
}

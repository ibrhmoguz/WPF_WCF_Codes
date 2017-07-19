using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthwindOps.Types;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //DataContext.KurumKodu = "985123657";

            NorthwindWS.NorthwindOpsServiceClient client = new NorthwindWS.NorthwindOpsServiceClient();

            Orders[] orderList = client.OrdersByCustomer("TOMSP");
            
        }
    }
}

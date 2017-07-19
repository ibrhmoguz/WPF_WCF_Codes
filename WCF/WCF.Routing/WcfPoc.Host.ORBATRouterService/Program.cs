using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Routing;
using WcfPoc.Host.Common;

namespace WcfPoc.Host.TestRouterService
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(RoutingService));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(System.ServiceModel.Routing.IRequestReplyRouter),
                                                               new BasicHttpBinding(),
                                                               "http://localhost:8090/ORBATService.svc");
            endpoint.Behaviors.Add(new ServiceMessagLogger());

            try
            {
                host.Open();
                Console.WriteLine("TestRoutingService have started.");
                Console.WriteLine("Address: http://localhost/ORBATService.svc");
                Console.ReadLine();
                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                host.Abort();
                Console.ReadLine();
            }
        }
    }
}

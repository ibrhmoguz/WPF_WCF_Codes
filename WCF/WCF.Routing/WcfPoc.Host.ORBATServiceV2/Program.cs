using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfPoc.Host.Common;
using WcfPoc.Services.ORBATServiceV2;

namespace WcfPoc.Host.ORBATServiceV2
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(ORBATService));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IORBATServiceV2),
                                                               new BasicHttpBinding("ServiceBinding"),
                                                               "http://localhost:8021/ORBATServiceV2/ORBATService.svc");
            endpoint.Behaviors.Add(new ServiceMessagLogger());

            try
            {
                host.Open();
                Console.WriteLine("ORBATServiceV2 have started.");
                Console.WriteLine("Address: http://localhost:8082/ORBATServiceV2/ORBATService.svc");
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
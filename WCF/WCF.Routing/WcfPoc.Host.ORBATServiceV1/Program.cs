using System;
using System.ServiceModel;
using WcfPoc.Services.ORBATServiceV1;
using WcfPoc.Host.Common;
using System.ServiceModel.Description;

namespace WcfPoc.Host.ORBATServiceV1
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(TestService));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(WcfPoc.Services.ORBATServiceV1.IORBATServiceV1),
                                                              new BasicHttpBinding("ServiceBinding"),
                                                              "http://localhost:8020/ORBATServiceV1/ORBATService.svc");
            endpoint.Behaviors.Add(new ServiceMessagLogger());

            try
            {
                host.Open();
                Console.WriteLine("ORBATServiceV1 have started.");
                Console.WriteLine("Address: http://localhost:8081/ORBATServiceV1/ORBATService.svc");
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

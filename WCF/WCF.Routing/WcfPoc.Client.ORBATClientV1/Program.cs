
using WcfPoc.Client.Common;
using WcfPoc.Services.ORBATServiceV1;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace WcfPoc.Client.TestClientV1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ClientV1 have started.");

                var factory = new ChannelFactory<IORBATServiceV1>(new BasicHttpBinding(), "http://localhost:8090/ORBATService/router");
                factory.Endpoint.Behaviors.Add(new ClientAddHeaderBehavior("v1.0"));
                var channel = factory.CreateChannel();

                Console.WriteLine("Press ENTER to send request to service TestV1");
                Console.ReadLine();
                const string missionCode = "testCode";
                var queryList = channel.GetSavedQueries(missionCode);

                Console.WriteLine("Query Count: " + queryList.Count.ToString());
                Console.ReadLine();

                var clientChannel = channel as IClientChannel;
                if (clientChannel != null)
                    clientChannel.Close();

                factory.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine("Service fault: " + exc.Message.ToString());
                Console.ReadLine();
            }
        }
    }
}

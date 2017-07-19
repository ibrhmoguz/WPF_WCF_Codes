
using WcfPoc.Client.Common;
using WcfPoc.Services.ORBATServiceV2;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WcfPoc.Client.TestClientV2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ClientV2 have started.");

                var factory = new ChannelFactory<IORBATServiceV2>(new BasicHttpBinding(), "http://localhost:6392/ORBATService.svc");
                //factory.Endpoint.Behaviors.Add(new ClientAddHeaderBehavior("v2.0"));
                var channel = factory.CreateChannel();

                TestEntity TestEntity = new TestEntity()
                {
                    AirRefuelingType = "ASD",
                    Capacity = "10",
                    TestType = "WSE",
                    TestTypeIdentifier = "OTI"
                };

                Console.WriteLine("Press ENTER to send request to service TestV2");
                Console.ReadLine();

                bool result = false;
                string missionCode = "testCode";
                result = channel.WriteTestEntities(missionCode, TestEntity);

                Console.WriteLine("WriteTestEntities result: " + result.ToString());

                Console.ReadLine();

                ((IClientChannel)channel).Close();
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

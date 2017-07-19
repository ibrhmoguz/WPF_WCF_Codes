using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WcfPoc.Host.Common
{
    public class ServiceMessagLogger : IDispatchMessageInspector, IEndpointBehavior
    {
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            bool isVersionExist = false;

            for (int i = 0; i < request.Headers.Count; i++)
            {
                var headerInfo = request.Headers[i];
                if (headerInfo.Name == "Version" && headerInfo.Namespace == "http://WcfPoc.wcfRouting.int/Increment1")
                {
                    isVersionExist = true;
                    break;
                }
            }

            //if (!isVersionExist)
            //{
            //    throw new Exception("SOAP Message don't have VERSION header!");
            //}

            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();
            Console.WriteLine("Message Received:\n{0}", buffer.CreateMessage().ToString());

            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();
            Console.WriteLine("Message Sending:\n{0}", buffer.CreateMessage().ToString());
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WcfPoc.Client.Common
{
    public class ClientAddHeaderBehavior : IEndpointBehavior
    {
        private string Version { get; set; }

        public ClientAddHeaderBehavior(string version)
        {
            this.Version = version;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new CustomHeaderMessageInspector(this.Version));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        { }

        public void Validate(ServiceEndpoint endpoint)
        { }
    }

    public class CustomHeaderMessageInspector : IClientMessageInspector
    {
        private string Version { get; set; }

        public CustomHeaderMessageInspector(string version)
        {
            this.Version = version;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        { }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request.Headers.Add(new CustomMessageHeader(this.Version));
            return request;
        }
    }


}

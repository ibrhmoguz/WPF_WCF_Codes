using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace Test
{
    public class AddCustomHeaderBehavior : IEndpointBehavior
    {

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            CustomHeaderMessageInspector headerInspector = new CustomHeaderMessageInspector();
            clientRuntime.MessageInspectors.Add(headerInspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }

    public class CustomHeaderMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            CustomHeader ch = new CustomHeader();
            request.Headers.Add(ch);
            return request;
        }
    }

    public class CustomHeader : MessageHeader
    {
        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteValue(DataContext.KurumKodu);
        }

        public override string Name
        {
            get { return "KurumKodu"; }
        }

        public override string Namespace
        {
            get { return String.Empty; }
        }
    }
}

namespace Infra.Service.Core.Behaviors
{
    #region

    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Configuration;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    #endregion

    /// <summary>
    ///     Silverlight endpoint fault behavior which converts all http codes into http 200
    /// </summary>
    public class SilverlightEndpointFaultBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        #region Public Properties

        /// <summary>
        ///     Gets the type of behavior.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" />.</returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(SilverlightEndpointFaultBehavior);
            }
        }

        #endregion

        // The following methods are stubs and not relevant.
        #region Public Methods and Operators

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint to modify.
        /// </param>
        /// <param name="bindingParameters">
        /// The objects that binding elements require to support the behavior.
        /// </param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint that is to be customized.
        /// </param>
        /// <param name="clientRuntime">
        /// The client runtime to be customized.
        /// </param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint that exposes the contract.
        /// </param>
        /// <param name="endpointDispatcher">
        /// The endpoint dispatcher to be modified or extended.
        /// </param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            if (endpointDispatcher != null && endpointDispatcher.DispatchRuntime != null)
            {
                var inspector = new SilverlightFaultMessageInspector();
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
            }
        }

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">
        /// The endpoint to validate.
        /// </param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        ///     The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new SilverlightEndpointFaultBehavior();
        }

        #endregion

        /// <summary>
        ///     Converts http messages to http 200 before replying
        /// </summary>
        internal class SilverlightFaultMessageInspector : IDispatchMessageInspector
        {
            #region Public Methods and Operators

            /// <summary>
            /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
            /// </summary>
            /// <param name="request">
            /// The request message.
            /// </param>
            /// <param name="channel">
            /// The incoming channel.
            /// </param>
            /// <param name="instanceContext">
            /// The current service instance.
            /// </param>
            /// <returns>
            /// The object used to correlate state. This object is passed back in the
            ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/>
            ///     method.
            /// </returns>
            public object AfterReceiveRequest(
                ref Message request, 
                IClientChannel channel, 
                InstanceContext instanceContext)
            {
                // Do nothing to the incoming message.
                return null;
            }

            /// <summary>
            /// Called after the operation has returned but before the reply message is sent.
            /// </summary>
            /// <param name="reply">
            /// The reply message. This value is null if the operation is one way.
            /// </param>
            /// <param name="correlationState">
            /// The correlation object returned from the
            ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/>
            ///     method.
            /// </param>
            public void BeforeSendReply(ref Message reply, object correlationState)
            {
                if (reply != null && reply.IsFault)
                {
                    var property = new HttpResponseMessageProperty();

                    //// Here the response code is changed to 200.
                    property.StatusCode = HttpStatusCode.OK;
                    reply.Properties[HttpResponseMessageProperty.Name] = property;
                }
            }

            #endregion
        }
    }
}
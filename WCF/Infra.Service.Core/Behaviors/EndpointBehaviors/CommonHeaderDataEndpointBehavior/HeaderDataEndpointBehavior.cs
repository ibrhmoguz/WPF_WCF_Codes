    
namespace Infra.Service.Core.Behaviors.EndpointBehaviors.CommonHeaderDataEndpointBehavior
{
    #region

    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    #endregion

    /// <summary>
    ///     Header Data Endpoint behavior
    /// </summary>
    public class HeaderDataEndpointBehavior : IEndpointBehavior
    {
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
            throw new NotSupportedException();
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
            if (endpointDispatcher != null)
            {
                var headerDataDispatchMessageInspector = new HeaderDataDispatchMessageInspector();
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(headerDataDispatchMessageInspector);
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
    }
}
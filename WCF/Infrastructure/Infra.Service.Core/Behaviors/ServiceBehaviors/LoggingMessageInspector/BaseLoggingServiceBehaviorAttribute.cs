namespace Infra.Service.Core.Behaviors.ServiceBehaviors.LoggingMessageInspector
{
    #region

    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    #endregion

    /// <summary>
    ///     Logging Dispatch Message Inspector Service Behavior
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class BaseLoggingServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        #region Public Methods and Operators

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description of the service.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host of the service.
        /// </param>
        /// <param name="endpoints">
        /// The service endpoints.
        /// </param>
        /// <param name="bindingParameters">
        /// Custom objects to which binding elements have access.
        /// </param>
        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers,
        ///     message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host that is currently being built.
        /// </param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (serviceHostBase == null)
            {
                throw new ArgumentNullException("serviceHostBase");
            }

            for (int i = 0; i < serviceHostBase.ChannelDispatchers.Count; i++)
            {
                var channelDispatcher = serviceHostBase.ChannelDispatchers[i] as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        var inspector = new LoggingDispatchMessageInspector();
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
                    }
                }
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run
        ///     successfully.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The service host that is currently being constructed.
        /// </param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
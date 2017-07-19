namespace Infra.Service.Core.Behaviors.ServiceBehaviors
{
    #region

    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    #endregion

    /// <summary>
    ///     Service behavior for adding Unity instance provider in each
    ///     endpoint dispatcher
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class UnityInstanceProviderServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        /// <summary>
        /// Gets or sets the name of the session factory.
        /// </summary>
        /// <value>
        /// The name of the session factory which will be used to open session
        /// </value>
        public string SessionFactoryName { get; set; }

        /// <summary>
        /// Gets or sets the cache connection string.
        /// </summary>
        /// <value>
        /// The cache connection string.
        /// </value>
        public string CacheConnectionString { get; set; }

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
            this.CacheConnectionString = ConfigurationManager.ConnectionStrings["CacheConnectionString"].ConnectionString;
            if (serviceHostBase != null && serviceHostBase.ChannelDispatchers.Count > 0)
            {
                foreach (var item in serviceHostBase.ChannelDispatchers)
                {
                    var dispatcher = item as ChannelDispatcher;
                    if (dispatcher != null)
                    {
                        //// add new instance provider for each end point dispatcher
                        dispatcher.Endpoints.ToList()
                            .ForEach(
                                endpoint =>
                                    {
                                        endpoint.DispatchRuntime.InstanceProvider =
                                            new UnityInstanceProvider(serviceDescription.ServiceType, this.CacheConnectionString, this.SessionFactoryName);
                                    });
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
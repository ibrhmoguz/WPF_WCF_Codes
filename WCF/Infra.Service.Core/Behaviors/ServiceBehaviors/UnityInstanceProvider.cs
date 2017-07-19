namespace Infra.Service.Core.Behaviors.ServiceBehaviors
{
    #region
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Infra.CrossCutting.Common.CommonOperationContext;
    using Infra.Infrastructure.Utils.Unity;
    using Microsoft.Practices.Unity;
    using NHibernate;
    using StackExchange.Redis;
    using ISession = NHibernate.ISession;

    #endregion

    /// <summary>
    ///     The unity instance provider. This class provides
    ///     an extensibility point for creating instances of WCF
    ///     service.
    ///     <remarks>
    ///         The goal is to inject dependencies from the inception point
    ///     </remarks>
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider
    {
        #region Fields

        /// <summary>
        /// The cache session.
        /// </summary>
        private static object cacheSession;

        /// <summary>
        ///     Unity Container
        /// </summary>
        private IUnityContainer container;

        /// <summary>
        ///     The locker
        /// </summary>
        private volatile object locker = new object();

        /// <summary>
        ///     Service Type
        /// </summary>
        private Type serviceType;

        /// <summary>
        /// The session factory name which will be used to resolve the session factory
        /// </summary>
        private string sessionFactoryName;

        /// <summary>
        /// The cache connection string.
        /// </summary>
        private string cacheConnectionString;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityInstanceProvider" /> class.
        /// </summary>
        /// <param name="serviceType">The service type where instance provider is to be applied</param>
        /// <param name="cacheConnectionString">The cache connection string.</param>
        /// <param name="sessionFactoryName">Name of the session factory which will be used to resolve and open session</param>
        /// <exception cref="System.ArgumentNullException">Service type cannot be null</exception>
        public UnityInstanceProvider(Type serviceType,  string cacheConnectionString, string sessionFactoryName = "")
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.serviceType = serviceType;
            this.container = Container.Current;
            this.sessionFactoryName = sessionFactoryName;
            this.cacheConnectionString = cacheConnectionString;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of the specified service type
        /// </summary>
        /// <param name="instanceContext">
        /// instance context
        /// </param>
        /// <param name="message">
        /// message from channel
        /// </param>
        /// <returns>
        /// an instance of the specified service type
        /// </returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            lock (this.locker)
            {
                ISessionFactory sessionFactory = null;
                if (string.IsNullOrEmpty(this.sessionFactoryName))
                {
                    sessionFactory = this.container.Resolve<ISessionFactory>();
                }
                else
                {
                    sessionFactory = this.container.Resolve<ISessionFactory>(this.sessionFactoryName);
                }

                try
                {
                    CommonData.Session = sessionFactory.OpenSession();
                    System.Data.ConnectionState connectionState = ((ISession)CommonData.Session).Connection.State;

                    if (cacheSession == null)
                    {
                        cacheSession = ConnectionMultiplexer.Connect(this.cacheConnectionString).GetDatabase();
                    }

                    CommonData.CacheSession = cacheSession;
                }
                catch (Exception)
                {
                    throw new Infra.Common.Exception.DataSourceNotFoundException("An error occurred while connecting to database. Check the availability, configuration parameters and access permissions for the database."); 
                }
                ////     (CommonData.Session as ISession).BeginTransaction();
            }

            //// This is the only call to UNITY container in the whole solution
            return this.container.Resolve(this.serviceType);
        }

        /// <summary>
        /// Returns a service instance
        /// </summary>
        /// <param name="instanceContext">
        /// instance context
        /// </param>
        /// <returns>
        /// The service instance
        /// </returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Releases the specified instance
        /// </summary>
        /// <param name="instanceContext">
        /// instance context
        /// </param>
        /// <param name="instance">
        /// instance to be released
        /// </param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (CommonData.Session != null)
            {
                (CommonData.Session as ISession).Close();
                (CommonData.Session as ISession).Dispose();
                (CommonData.CacheSession as IDatabase).Multiplexer.Close();
            }

            IDisposable instanceToBeDisposed = instance as IDisposable;

            if (instanceToBeDisposed != null)
            {
                instanceToBeDisposed.Dispose();
            }
        }

        #endregion
    }
}

namespace Infra.Service.Core.SessionFactory
{
    #region

    using System;
    using System.Collections.Generic;

    using NHibernate;
    using NHibernate.Cfg;

    using Environment = NHibernate.Cfg.Environment;

    #endregion

    /// <summary>
    ///     Speeds up loading of ISessionFactory by loading Configuration only once per assembly.
    /// </summary>
    public abstract class CacheableSessionFactoryProvider
    {
        #region Static Fields

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static object syncRoot = new object();

        /// <summary>
        ///     The ISessionFactory cache,
        ///     only one session factory is created per Assembly/ConnectionString
        /// </summary>
        private static IDictionary<string, ISessionFactory> sessionFactoryCache;

        #endregion

        #region Fields

        /// <summary>
        ///     the Configuration cache,
        ///     only one Configuration is created per Assembly
        /// </summary>
        private IDictionary<string, Configuration> configurationCache;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration cache.
        /// </summary>
        protected IDictionary<string, Configuration> ConfigurationCache
        {
            get
            {
                if (this.configurationCache == null)
                {
                    lock (syncRoot)
                    {
                        this.configurationCache = new Dictionary<string, Configuration>();
                    }
                }

                return this.configurationCache;
            }
        }

        /// <summary>
        /// Gets the session factory cache.
        /// </summary>
        private static IDictionary<string, ISessionFactory> SesssionFactoryCache
        {
            get
            {
                if (sessionFactoryCache == null)
                {
                    lock (syncRoot)
                    {
                        sessionFactoryCache = new Dictionary<string, ISessionFactory>();
                    }
                }

                return sessionFactoryCache;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Main entry point, caches the ISessionFactory based on the assembly full name
        /// </summary>
        /// <param name="connectionString">the connection string to use</param>
        /// <param name="configurationKey">The configuration key.</param>
        /// <returns>
        /// The <see cref="ISessionFactory" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The connectionString can not be null, empty or white space
        /// or
        /// Specified session factory configuration type not found!
        /// </exception>
        public ISessionFactory CreateSessionFactory(string connectionString, string configurationKey)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("The connectionString can not be null, empty or whitespace");
            }

            Configuration config = null;

            // cache the configuration
            if (!this.ConfigurationCache.TryGetValue(configurationKey, out config))
            {
                throw new ArgumentException("Specified sessionfactory configuration type not found!", configurationKey);
            }

            // differentiate ISessionFactory by connection string 
            string factoryKey = string.Format("{0}|{1}", configurationKey, connectionString);

            // cache the session factory
            if (!SesssionFactoryCache.ContainsKey(factoryKey))
            {
                config.SetProperty(Environment.ConnectionString, connectionString);
                var sessionFactory = config.BuildSessionFactory();

                // Add to the session factory cache
                SesssionFactoryCache.Add(factoryKey, sessionFactory);
            }

            return SesssionFactoryCache[factoryKey];
        }

        #endregion
    }
}
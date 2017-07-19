using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infra.Service.Core.Behaviors.ServiceBehaviors
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using System.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;



    /// <summary>
    /// DI container accesor
    /// </summary>
    public static class Container
    {
        #region Properties

        static IUnityContainer _currentContainer;

        /// <summary>
        /// Get the current configured container
        /// </summary>
        /// <returns>Configured container</returns>
        public static IUnityContainer Current
        {
            get
            {
                return _currentContainer;
            }
        }

        #endregion

        #region Constructor

        static Container()
        {
            ConfigureContainer();

            ConfigureFactories();
        }

        #endregion

        #region Methods

        static void ConfigureContainer()
        {
            /*
             * Add here the code configuration or the call to configure the container 
             * using the application configuration file
             */

            _currentContainer = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_currentContainer, "UnityContainer");
        }


        static void ConfigureFactories()
        {
            //LoggerFactory.SetCurrent(new TraceSourceLogFactory());
            //EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }

        #endregion
    }
}

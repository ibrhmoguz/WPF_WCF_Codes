namespace Infra.Service.Core.ConfigurationServices
{
    #region

    using System.Collections.Generic;

    using Infra.Infrastructure.Utils.Config;

    #endregion

    /// <summary>
    ///         Configuration service base class used to expose app.CONFIG data to clients
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fetches all configurations from configuration file and returns as a dictionary
        /// </summary>
        /// <returns>configurations as a string and string array dictionary</returns>
        public Dictionary<string, string[]> GetAllConfiguration()
        {
            var configurations = new Dictionary<string, string[]>();
            configurations = ConfigurationManager.GetAllConfigurationsFromWebConfig();
            return configurations;
        }

        /// <summary>
        /// Fetches a specific configuration by key
        /// </summary>
        /// <param name="key">
        /// key of the configuration to be fetched
        /// </param>
        /// <returns>
        /// string array of configuration values
        /// </returns>
        public string[] GetConfigurationByKey(string key)
        {
            string[] configuration;
            configuration = ConfigurationManager.GetConfigurationFromWebConfigByKey(key);
            return configuration;
        }

        #endregion
    }
}
namespace Infra.Service.Core.ConfigurationServices
{
    #region

    using System.Collections.Generic;
    using System.ServiceModel;

    #endregion

    /// <summary>
    ///         Configuration service interface used to expose APP.CONFIG data to clients
    /// </summary>
    [ServiceContract]
    public interface IConfigurationService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fetches all configurations from configuration file and returns as a dictionary
        /// </summary>
        /// <returns>configurations as a string and string array dictionary</returns>
        [OperationContract]
        Dictionary<string, string[]> GetAllConfiguration();

        /// <summary>
        /// Fetches a specific configuration by key
        /// </summary>
        /// <param name="key">
        /// key of the configuration to be fetched
        /// </param>
        /// <returns>
        /// string array of configuration values
        /// </returns>
        [OperationContract]
        string[] GetConfigurationByKey(string key);

        #endregion
    }
}
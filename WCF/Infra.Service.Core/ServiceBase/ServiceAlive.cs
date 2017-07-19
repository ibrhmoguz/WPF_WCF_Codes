namespace Infra.Service.Core.ServiceBase
{
    using Infra.Common;

    /// <summary>
    ///     Service Alive manages where the web service is working or not.
    /// </summary>
    public class ServiceAlive : IServiceAlive
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Determines whether this instance is alive.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is alive; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAlive()
        {
            return true;
        }

        #endregion
    }
}
namespace Infra.Service.Core.ServiceBase
{
    #region

    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Service;

    #endregion

    /// <summary>
    ///     BaseLoggingService : Allows clients to submit log entries into the server log.
    /// </summary>
    public class BaseLoggingService : LoggingService
    {
        #region Methods

        /// <summary>
        /// Get the user IP address and add them as an extended property.
        ///     NOTE. This information can easily be spoofed by the caller. Use this information only
        ///     for diagnostic purposes, not for authentication or auditing purposes.
        /// </summary>
        /// <param name="entry">
        /// The log entry coming from the client.
        /// </param>
        protected override void CollectInformation(LogEntry entry)
        {
            ////var messageProperties = OperationContext.Current.IncomingMessageProperties;
            ////var endpoint = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            ////string clientAddress = endpoint != null ? endpoint.Address : string.Empty;

            ////entry.ExtendedProperties.Add("ClientIPAddress", clientAddress);

            ////******Infra.CrossCutting.Logging *******
            ////Log.Write("Service Logging", BaseLoggingCategory.General, BaseLogging.LogType.Information);

            ////// Also add the client host address as a category. This allows you to turn on server side logging for a single client
            ////if (!string.IsNullOrEmpty(clientAddress))
            ////{
            ////    entry.Categories.Add(clientAddress);
            ////}            
            base.CollectInformation(entry);
        }

        /// <summary>
        /// Translates the incoming <see cref="LogEntryMessage"/> into a <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="entry">
        /// The log entry coming from the client.
        /// </param>
        /// <returns>
        /// A <see cref="LogEntry"/> instance that can be stored in the log.
        /// </returns>
        protected override LogEntry Translate(LogEntryMessage entry)
        {
            var logEntry = entry.ToLogEntry();

            if (logEntry.MachineName == null)
            {
                logEntry.MachineName = string.Empty;
            }

            if (logEntry.ProcessId == null)
            {
                logEntry.ProcessId = string.Empty;
            }

            if (logEntry.ProcessName == null)
            {
                logEntry.ProcessName = string.Empty;
            }

            return logEntry;
        }

        #endregion
    }
}
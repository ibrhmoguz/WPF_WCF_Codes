namespace Infra.Service.Core.Behaviors.EndpointBehaviors.CommonHeaderDataEndpointBehavior
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Infra.Common.Header;
    using Infra.CrossCutting.Common.CommonOperationContext;
    using Infra.Infrastructure.Utils;
    using Infra.Infrastructure.Utils.Unity;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    ///     Header Data is investigated and related items are added into common data class
    /// </summary>
    public class HeaderDataDispatchMessageInspector : IDispatchMessageInspector
    {
        #region Public Methods and Operators

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">
        /// The request message.
        /// </param>
        /// <param name="channel">
        /// The incoming channel.
        /// </param>
        /// <param name="instanceContext">
        /// The current service instance.
        /// </param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the
        ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/>
        ///     method.
        /// </returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            OperationContext.Current.Extensions.Add(new OperationContextExtension());
            OperationContext.Current.Extensions.Add(new SessionOperationContextExtension());
            CommonData.ClearDictionary();
            if (request != null && request.Headers != null)
            {
                var messageHeaders = request.Headers;

                IEnumerable<MessageHeaderInfo> messageHeaderInformation =
                    messageHeaders.Where(p => p.Namespace == "CustomMessageHeaders");
                string userName = "Unauthorized User";
                if (ServiceSecurityContext.Current != null)
                {
                    userName = this.ParseUserName(ServiceSecurityContext.Current.WindowsIdentity.Name);
                }

                CommonData.Current.Add(MessageHeaderCodes.UserName, userName);
                if (!messageHeaderInformation.IsNullOrEmpty())
                {
                    foreach (MessageHeaderInfo messageHeaderInfo in messageHeaderInformation)
                    {
                        string headerName = messageHeaderInfo.Name;
                        var headerValue = messageHeaders.GetHeader<string>(headerName, messageHeaderInfo.Namespace);
                        CommonData.Current.Add(headerName, headerValue);
                    }
                }

                if (string.IsNullOrEmpty(CommonData.GetHeader(MessageHeaderCodes.MissionCode)))
                {
                    CommonData.Current.Add(MessageHeaderCodes.MissionCode, MessageHeaderCodes.EmptyMissionCode);
                }

                if (string.IsNullOrEmpty(CommonData.GetHeader(MessageHeaderCodes.DataSource)))
                {
                    CommonData.Current.Add(MessageHeaderCodes.DataSource, MessageHeaderCodes.EmptyDataSource);
                }

                if (string.IsNullOrEmpty(CommonData.GetHeader(MessageHeaderCodes.InitialCatalog)))
                {
                    CommonData.Current.Add(MessageHeaderCodes.InitialCatalog, MessageHeaderCodes.EmptyCatalog);
                }

                CommonData.Current.Add(MessageHeaderCodes.CorrelationId, Guid.NewGuid().ToString());
                var unityConfigurationProvider = Container.Current.Resolve<IUnityConfigurationProvider>();
                unityConfigurationProvider.RegisterMissionBasedTypesToContainer();
            }

            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">
        /// The reply message. This value is null if the operation is one way.
        /// </param>
        /// <param name="correlationState">
        /// The correlation object returned from the
        ///     <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/>
        ///     method.
        /// </param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            OperationContext.Current.Extensions.Remove(
                OperationContext.Current.Extensions.Find<OperationContextExtension>());
            OperationContext.Current.Extensions.Remove(
                OperationContext.Current.Extensions.Find<SessionOperationContextExtension>());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses the user name to split the domain name and user name
        /// </summary>
        /// <param name="userName">
        /// user name to be parsed
        /// </param>
        /// <returns>
        /// User name part; Domain name removed
        /// </returns>
        private string ParseUserName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                string[] userNameArray = userName.Split('\\');

                if (userNameArray.Length > 1)
                {
                    userName = userNameArray[1];
                }
            }

            return userName;
        }

        #endregion
    }
}
namespace Infra.Service.Core.Behaviors.EndpointBehaviors.CommonHeaderDataEndpointBehavior
{
    #region

    using System;
    using System.ServiceModel.Configuration;

    #endregion

    /// <summary>
    ///     Header data endpoint behavior
    /// </summary>
    public class HeaderDataEndpointBehaviorExtensionElement : BehaviorExtensionElement
    {
        #region Public Properties

        /// <summary>
        ///     Gets the type of behavior.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" />.</returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(HeaderDataEndpointBehavior);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        ///     The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new HeaderDataEndpointBehavior();
        }

        #endregion
    }
}
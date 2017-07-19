namespace Infra.Presentation.Core.Shell
{
    #region

    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Infra.Presentation.Common.ViewManagement;
    using Infra.Presentation.Core.ModuleManagement;

    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    ///     abstract base class for shell view models
    /// </summary>
    public abstract class ShellViewModel : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="coreModuleManager">
        /// The core module manager.
        /// </param>
        /// <param name="eventManager">
        /// The event manager.
        /// </param>
        /// <param name="viewManager">
        /// The view manager.
        /// </param>
        protected ShellViewModel(
            IUnityContainer unityContainer,
            ICoreModuleManager coreModuleManager,
            IEventAggregator eventManager,
            IViewManager viewManager)
        {
            this.IUnityContainer = unityContainer;
            this.ICoreModuleManager = coreModuleManager;
            this.IEventManager = eventManager;
            this.IViewManager = viewManager;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the ICoreModuleManager.
        /// </summary>
        /// <value>
        ///     The ICoreModuleManager.
        /// </value>
        public ICoreModuleManager ICoreModuleManager { get; set; }

        /// <summary>
        ///     Gets or sets the event manager instance.
        /// </summary>
        /// <value>
        ///     The event manager instance.
        /// </value>
        public IEventAggregator IEventManager { get; set; }

        /// <summary>
        ///     Gets or sets the I unity container.
        /// </summary>
        /// <value>
        ///     The I unity container.
        /// </value>
        public IUnityContainer IUnityContainer { get; set; }

        /// <summary>
        ///     Gets or sets the I view manager.
        /// </summary>
        /// <value>
        ///     The I view manager.
        /// </value>
        public IViewManager IViewManager { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Fires the property changed.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        public void FirePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
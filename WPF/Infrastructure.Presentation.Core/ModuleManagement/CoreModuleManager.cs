namespace Infra.Presentation.Core.ModuleManagement
{
    #region

    using System;
    using System.Linq;

    using Microsoft.Practices.Prism.Modularity;

    #endregion

    /// <summary>
    ///     The core module manager which manager modules
    /// </summary>
    public class CoreModuleManager : ICoreModuleManager
    {
        #region Fields

        /// <summary>
        ///     the module catalog interface
        /// </summary>
        private IModuleCatalog moduleCatalog;

        /// <summary>
        ///     the module manager interface
        /// </summary>
        private IModuleManager moduleManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreModuleManager"/> class.
        /// </summary>
        /// <param name="moduleManager">
        /// The module manager interface.
        /// </param>
        /// <param name="moduleCatalog">
        /// The module catalog interface.
        /// </param>
        public CoreModuleManager(IModuleManager moduleManager, IModuleCatalog moduleCatalog)
        {
            if (moduleManager != null && moduleCatalog != null)
            {
                this.moduleManager = moduleManager;
                this.moduleCatalog = moduleCatalog;
                this.moduleManager.LoadModuleCompleted += this.IModuleManager_LoadModuleCompleted;
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when [on module loaded].
        /// </summary>
        public event EventHandler<LoadModuleCompletedEventArgs> OnModuleLoaded;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Loads the module if not already loaded.
        /// </summary>
        /// <param name="moduleName">
        /// Name of the module to be loaded.
        /// </param>
        public void LoadModuleIfNotLoaded(string moduleName)
        {
            ModuleInfo moduleToBeLoaded =
                this.moduleCatalog.Modules.Where(p => p.ModuleName == moduleName).SingleOrDefault();
            if (moduleToBeLoaded.State == ModuleState.Initialized)
            {
                this.ModuleLoaded(new LoadModuleCompletedEventArgs(moduleToBeLoaded, null));
            }

            if (this.moduleCatalog.Modules.Where(p => p.ModuleName == moduleName).SingleOrDefault().State
                != ModuleState.Initialized)
            {
                this.moduleManager.LoadModule(moduleName);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the LoadModuleCompleted event of the iModuleManager control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="Microsoft.Practices.Prism.Modularity.LoadModuleCompletedEventArgs"/> instance
        ///     containing the event data.
        /// </param>
        private void IModuleManager_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            this.ModuleLoaded(e);
        }

        /// <summary>
        /// Calls all registered delegates when the module is loaded.
        /// </summary>
        /// <param name="loadModuleCompletedEventArgs">
        /// The
        ///     <see cref="Microsoft.Practices.Prism.Modularity.LoadModuleCompletedEventArgs"/> instance containing the event
        ///     data.
        /// </param>
        private void ModuleLoaded(LoadModuleCompletedEventArgs loadModuleCompletedEventArgs)
        {
            if (this.OnModuleLoaded != null)
            {
                this.OnModuleLoaded(this, loadModuleCompletedEventArgs);
            }
        }

        #endregion
    }
}
namespace Infra.Presentation.Core.ModuleManagement
{
    #region

    using System;

    using Microsoft.Practices.Prism.Modularity;

    #endregion

    /// <summary>
    ///     Module Manager interface
    /// </summary>
    public interface ICoreModuleManager
    {
        #region Public Events

        /// <summary>
        ///     Occurs when [on module loaded].
        /// </summary>
        event EventHandler<LoadModuleCompletedEventArgs> OnModuleLoaded;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Loads the module if not loaded.
        /// </summary>
        /// <param name="moduleName">
        /// Name of the module.
        /// </param>
        void LoadModuleIfNotLoaded(string moduleName);

        #endregion
    }
}
namespace Infra.Presentation.Core.Modularization
{
    #region

    using System.Collections.Generic;

    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;

    #endregion

    /// <summary>
    ///         This class is used to fix Prism bug that manages Library Caching
    /// </summary>
    public class ModuleManagerWithLibraryCachingFix : ModuleManager
    {
        #region Fields

        /// <summary>
        ///     type loader collection instance
        /// </summary>
        private IEnumerable<IModuleTypeLoader> typeLoaders;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerWithLibraryCachingFix"/> class.
        /// </summary>
        /// <param name="moduleInitializer">
        /// Service used for initialization of modules.
        /// </param>
        /// <param name="moduleCatalog">
        /// Catalog that enumerates the modules to be loaded and initialized.
        /// </param>
        /// <param name="loggerFacade">
        /// Logger used during the load and initialization of modules.
        /// </param>
        public ModuleManagerWithLibraryCachingFix(
            IModuleInitializer moduleInitializer, 
            IModuleCatalog moduleCatalog, 
            ILoggerFacade loggerFacade)
            : base(moduleInitializer, moduleCatalog, loggerFacade)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Returns the list of registered <see cref="T:Microsoft.Practices.Prism.Modularity.IModuleTypeLoader" /> instances
        ///     that will be
        ///     used to load the types of modules.
        /// </summary>
        /// <value>
        ///     The module type loaders.
        /// </value>
        public override IEnumerable<IModuleTypeLoader> ModuleTypeLoaders
        {
            get
            {
                if (this.typeLoaders == null)
                {
                    this.typeLoaders = new List<IModuleTypeLoader> { new XapModuleTypeLoaderWithLibraryCachingFix() };
                }

                return this.typeLoaders;
            }

            set
            {
                this.typeLoaders = value;
            }
        }

        #endregion
    }
}
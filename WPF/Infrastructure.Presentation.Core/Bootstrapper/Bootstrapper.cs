namespace Infra.Presentation.Core.Bootstrapper
{
    #region

    using System;
    using System.Linq;
    using System.Windows;
    
    using Infra.Presentation.Caching;
    using Infra.Presentation.Common.Cache;
    using Infra.Presentation.Common.Exception;
    using Infra.Presentation.Common.HyperlinkServices;
    using Infra.Presentation.Common.Log;
    using Infra.Presentation.Common.Proxy;
    using Infra.Presentation.Common.UserContext;
    using Infra.Presentation.Common.ViewManagement;
    using Infra.Presentation.Core.AAS.Context;
    using Infra.Presentation.Core.Entities.UserContext;
    using Infra.Presentation.Core.Modularization;
    using Infra.Presentation.Core.ModuleManagement;
    using Infra.Presentation.Core.ProxyBase;
    using Infra.Presentation.Core.Shell;
    using Infra.Presentation.Core.ViewManagement;
    using Infra.Presentation.ExceptionHandling;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.UnityExtensions;
    using Microsoft.Practices.Unity;    

    #endregion

    /// <summary>
    /// A class that provides a basic bootstrapping sequence that registers most of the Composite Application Library assets 
    /// </summary>
    public abstract class Bootstrapper : UnityBootstrapper
    {
        #region Methods

        /// <summary>
        ///     Configures the <see cref="T:Microsoft.Practices.Unity.IUnityContainer" />. May be overwritten in a derived class to
        ///     add specific
        ///     type mappings required by the application.
        /// </summary>
        protected override void ConfigureContainer()
        {
            IUnityContainer unityContainer = this.Container;

            unityContainer.RegisterType<IExternalPartsLoader, ExternalPartsLoader>();
            unityContainer.RegisterType<IModuleManager, ModuleManagerWithLibraryCachingFix>(
                new ContainerControlledLifetimeManager());

            base.ConfigureContainer();

            this.RegisterDefaultTypesToUnity();

            this.RegisterUtilities();
            this.RegisterServiceProxies();
            this.RegisterViewsAndViewModels();
            
            this.InitializeInstances();
        }

        /// <summary>
        ///     Creates the <see cref="T:Microsoft.Practices.Unity.IUnityContainer" /> that will be used as the default container.
        /// </summary>
        /// <returns>
        ///     A new instance of <see cref="T:Microsoft.Practices.Unity.IUnityContainer" />.
        /// </returns>
        protected override IUnityContainer CreateContainer()
        {
            IUnityContainer createContainer = base.CreateContainer();

            CreateUnityContainerExtensions(createContainer);

            UnityContainerExtension[] createCustomUnityContainerExtensions = this.CreateCustomUnityContainerExtensions();
            if (createCustomUnityContainerExtensions != null)
            {
                foreach (UnityContainerExtension extension in createCustomUnityContainerExtensions)
                {
                    createContainer.AddExtension(extension);
                }
            }

            return createContainer;
        }

        /// <summary>
        ///     Creates custom unity container extensions.
        /// </summary>
        /// <returns>new UnityContainerExtensions</returns>
        protected virtual UnityContainerExtension[] CreateCustomUnityContainerExtensions()
        {
            return new UnityContainerExtension[] { };
        }

        /// <summary>
        ///     Creates the <see cref="T:Microsoft.Practices.Prism.Modularity.IModuleCatalog" /> used by Prism.
        /// </summary>
        /// <returns>new IModuleCatalog</returns>
        /// <remarks>
        ///     The base implementation returns a new ModuleCatalog.
        /// </remarks>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(this.GetModuleCatalogURI());
        }

        /// <summary>
        ///     Creates the shell or main window of the application.
        /// </summary>
        /// <returns>
        ///     The shell of the application.
        /// </returns>
        /// <remarks>
        ///     If the returned instance is a <see cref="T:System.Windows.DependencyObject" />, the
        ///     <see cref="T:Microsoft.Practices.Prism.Bootstrapper" /> will attach the default
        ///     <seealso cref="T:Microsoft.Practices.Prism.Regions.IRegionManager" /> of
        ///     the application in its <see cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionManagerProperty" />
        ///     attached property
        ///     in order to be able to add regions by using the
        ///     <seealso cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionNameProperty" />
        ///     attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            ShellView shellView = this.ResolveShell();
            Application.Current.RootVisual = shellView;

            return shellView;
        }

        /// <summary>
        ///     Return the URI of the module catalog XAML file
        /// </summary>
        /// <returns>URI of the module catalog XAML</returns>
        protected abstract Uri GetModuleCatalogURI();

        /// <summary>
        ///     Initializes instances.
        /// </summary>
        protected abstract void InitializeInstances();

        /// <summary>
        ///     Register Service Proxies
        /// </summary>
        protected abstract void RegisterServiceProxies();

        /// <summary>
        ///     Registers the utilities to Unity Container.
        /// </summary>
        protected abstract void RegisterUtilities();

        /// <summary>
        ///     Registers views and view models.
        /// </summary>
        protected abstract void RegisterViewsAndViewModels();

        /// <summary>
        ///     Resolve shell view using unity
        /// </summary>
        /// <returns>Shell view object</returns>
        protected abstract ShellView ResolveShell();

        /// <summary>
        /// Creates unity container extensions.
        /// </summary>
        /// <param name="createContainer">
        /// The create container.
        /// </param>
        private static void CreateUnityContainerExtensions(IUnityContainer createContainer)
        {
            createContainer.AddNewExtension<EnterpriseLibraryCoreExtension>();
        }

        /// <summary>
        ///     Registers the types to unity.
        /// </summary>
        private void RegisterDefaultTypesToUnity()
        {
            IUnityContainer unityContainer = this.Container;

            unityContainer.RegisterType<ICoreModuleManager, CoreModuleManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IViewManager, ViewManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<ISecurityContext, SecurityContext>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IUserContext, UserContext>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<IExceptionDisplayService, ExceptionDisplayService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ILogger, Logging.Logger>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IExceptionManager, ExceptionManager>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<ICacheHubProxy, CacheHubProxy>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ICache, Cache>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<IHyperlinkService, HyperlinkService>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType(typeof(IProxy<>), typeof(Proxy<>));

            ObjectCache objectCache = new InMemoryCache("InMemoryCache", 10000, 5000, TimeSpan.FromDays(1.0));
            unityContainer.RegisterInstance(objectCache, new ContainerControlledLifetimeManager());
        }

        #endregion
    }
}
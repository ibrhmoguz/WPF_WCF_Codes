using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using Infra.Presentation.Core.LayoutManagement.ViewModel;

namespace Infra.Presentation.Core
{
    public class ViewManager : IViewManager
    {
        private IUnityContainer iUnityContainer;
        private IRegionManager iRegionManager;

        public ViewManager(IRegionManager iRegionManager, IUnityContainer iUnityContainer)
        {
            this.iRegionManager = iRegionManager;
            this.iUnityContainer = iUnityContainer;
        }

        public void RegisterView<TType>(string viewName) where TType : Control
        {
            this.iUnityContainer.RegisterType<object, TType>(viewName);
        }

        public void ActivateViewAndDeActiveAllViewsOnTheSameRegion<TType>(string viewName, string region) where TType : Control
        {
            Control view = this.iUnityContainer.Resolve<TType>(viewName);
            var activeViews = this.iRegionManager.Regions[region].ActiveViews;
            foreach (var activeView in activeViews)
            {
                this.iRegionManager.Regions[region].Deactivate(activeView);
            }
            this.iRegionManager.Regions[region].Add(view);
        }

        public void ActivateViewAndDeActiveAllViewsOnTheSameRegion(Type viewType, string region)
        {
            Control view = this.iUnityContainer.Resolve(viewType) as Control;
            var activeViews = this.iRegionManager.Regions[region].ActiveViews;
            foreach (var activeView in activeViews)
            {
                this.iRegionManager.Regions[region].Deactivate(activeView);
            }
            this.iRegionManager.Regions[region].Add(view);
        }

        [Obsolete]
        public void ActivateView(Type viewType, string region)
        {
            Control view = this.iUnityContainer.Resolve(viewType) as Control;
            IRegion iRegion = this.iRegionManager.Regions[region];
            iRegion.Add(view);
        }

        public void ActivateView<TType>(string viewName, string region) where TType : Control
        {
            Control view = this.iUnityContainer.Resolve<TType>(viewName);
            this.iRegionManager.Regions[region].Add(view);
        }

        #region LayoutManagement
        public void RegisterLayoutView(Type viewType, string region)
        {
            Control view = this.iUnityContainer.Resolve(viewType) as Control;
            ILayoutControllerViewModel layoutControllerViewModel = this.iUnityContainer.Resolve<ILayoutControllerViewModel>();
            layoutControllerViewModel.RegisterPane(region, view);
            layoutControllerViewModel.ApplyLayout();
        }
        #endregion
    }
}

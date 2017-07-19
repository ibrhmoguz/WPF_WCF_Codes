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

namespace Infra.Presentation.Core
{
    public interface IViewManager
    {
        void RegisterView<TType>(string viewName) where TType : Control;
        void ActivateViewAndDeActiveAllViewsOnTheSameRegion<TType>(string viewName, string region) where TType : Control;
        void ActivateViewAndDeActiveAllViewsOnTheSameRegion(Type viewType, string region);
        void ActivateView(Type viewType, string region);
        void ActivateView<TType>(string viewName, string region) where TType : Control;

        #region LayoutManagement
        void RegisterLayoutView(Type viewType, string region);
        #endregion
    }
}

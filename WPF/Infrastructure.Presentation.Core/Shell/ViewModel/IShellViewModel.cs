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
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using System.ComponentModel;

namespace Infra.Presentation.Core.Shell
{
    public interface IShellViewModel : INotifyPropertyChanged
    {
        void DisplayView(string moduleName, string viewName, string region);        
        void SubscribeToEvent<TEvent, TMessage>(Action<TMessage> callBack) where TEvent : CompositePresentationEvent<TMessage>, new();

        #region LayoutManagement
        void RegisterLayoutView(string moduleName, string viewName, string paneIdentifier);
        #endregion
    }
}

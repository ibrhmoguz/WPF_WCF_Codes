namespace Infra.Presentation.Core.Shell
{
    #region

    using System.Windows;
    using System.Windows.Controls;

    #endregion

    /// <summary>
    ///     Shell view base class
    /// </summary>
    public class ShellView : UserControl
    {
        #region Fields

        /// <summary>
        ///     shell view model instance which is bound to the view
        /// </summary>
        private ShellViewModel shellViewModel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShellView" /> class.
        ///     DO NOT USE, Only for designer support
        /// </summary>
        public ShellView()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellView"/> class.
        /// </summary>
        /// <param name="shellViewModel">
        /// The shell view model.
        /// </param>
        public ShellView(ShellViewModel shellViewModel)
        {
            this.shellViewModel = shellViewModel;
            this.Loaded += this.ShellLoaded;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Loaded event of the Shell control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        protected void ShellLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.shellViewModel;
        }

        #endregion
    }
}
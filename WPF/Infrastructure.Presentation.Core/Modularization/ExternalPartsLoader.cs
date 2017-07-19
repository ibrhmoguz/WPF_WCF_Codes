namespace Infra.Presentation.Core.Modularization
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Windows;
    using System.Windows.Resources;
    using System.Xml;

    #endregion

    /// <summary>
    ///         This class is used to fix Prism bug that downloads external parts of the module
    /// </summary>
    public class ExternalPartsLoader : IExternalPartsLoader
    {
        #region Fields

        /// <summary>
        ///     client call back
        /// </summary>
        private Action callback;

        /// <summary>
        ///     external part downloader client instance
        /// </summary>
        private WebClient externalPartDownloader = new WebClient();

        /// <summary>
        ///     pending extension parts queue
        /// </summary>
        private Queue<ExtensionPart> pendingExtensionParts = new Queue<ExtensionPart>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExternalPartsLoader" /> class.
        /// </summary>
        public ExternalPartsLoader()
        {
            this.externalPartDownloader.OpenReadCompleted += this.ExternalPartDownloader_OpenReadCompleted;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Loads the external parts.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="callbackMethod">
        /// The callback.
        /// </param>
        public void LoadExternalParts(Stream stream, Action callbackMethod)
        {
            this.callback = callbackMethod;
            var streamReader =
                new StreamReader(
                    Application.GetResourceStream(
                        new StreamResourceInfo(stream, null), 
                        new Uri("AppManifest.xaml", UriKind.Relative)).Stream);
            using (XmlReader xmlReader = XmlReader.Create(streamReader))
            {
                xmlReader.MoveToContent();
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Deployment.ExternalParts")
                    {
                        using (XmlReader xmlReaderExtensionParts = xmlReader.ReadSubtree())
                        {
                            while (xmlReaderExtensionParts.Read())
                            {
                                if (xmlReaderExtensionParts.NodeType == XmlNodeType.Element
                                    && xmlReaderExtensionParts.Name == "ExtensionPart")
                                {
                                    var extensionPart = new ExtensionPart();
                                    extensionPart.Source = new Uri(
                                        xmlReaderExtensionParts.GetAttribute("Source"), 
                                        UriKind.Relative);
                                    this.pendingExtensionParts.Enqueue(extensionPart);
                                }
                            }
                        }

                        break;
                    }
                }
            }

            var source = this.pendingExtensionParts.Dequeue().Source;
            this.externalPartDownloader.OpenReadAsync(source, source);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the assembly from stream.
        /// </summary>
        /// <param name="sourceStream">
        /// The source stream.
        /// </param>
        /// <param name="assemblyPart">
        /// The assembly part.
        /// </param>
        private static void LoadAssemblyFromStream(Stream sourceStream, AssemblyPart assemblyPart)
        {
            Stream assemblyStream =
                Application.GetResourceStream(
                    new StreamResourceInfo(sourceStream, null), 
                    new Uri(assemblyPart.Source, UriKind.Relative)).Stream;

            assemblyPart.Load(assemblyStream);
        }

        /// <summary>
        /// Handles the OpenReadCompleted event of the ExternalPartDownloader control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Net.OpenReadCompletedEventArgs"/> instance containing the event data.
        /// </param>
        private void ExternalPartDownloader_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            var uri = e.UserState as Uri;
            if (uri != null)
            {
                var dllPath = uri.ToString().Replace(".zip", ".dll");

                LoadAssemblyFromStream(e.Result, new AssemblyPart { Source = dllPath });
            }

            if (this.pendingExtensionParts.Count == 0)
            {
                this.callback.Invoke();
            }
            else
            {
                var source = this.pendingExtensionParts.Dequeue().Source;
                this.externalPartDownloader.OpenReadAsync(source, source);
            }
        }

        #endregion
    }
}
namespace Microsoft.Practices.Prism.Modularity
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Windows;
    using System.Windows.Resources;
    using System.Xml;

    using Infra.Presentation.Core.Modularization;

    using Microsoft.Practices.ServiceLocation;

    #endregion

    /// <summary>
    ///     Component responsible for downloading remote modules
    ///     and load their <see cref="Type" /> into the current application domain.
    /// </summary>
    public class XapModuleTypeLoaderWithLibraryCachingFix : IModuleTypeLoader
    {
        #region Fields

        /// <summary>
        ///     downloaded uris that blocks reloading
        /// </summary>
        private HashSet<Uri> downloadedUris = new HashSet<Uri>();

        /// <summary>
        ///     module download dictionary
        /// </summary>
        private Dictionary<Uri, List<ModuleInfo>> downloadingModules = new Dictionary<Uri, List<ModuleInfo>>();

        #endregion

        #region Public Events

        /// <summary>
        ///     Raised when a module is loaded or fails to load.
        /// </summary>
        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        /// <summary>
        ///     Raised repeatedly to provide progress as modules are loaded in the background.
        /// </summary>
        public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Evaluates the <see cref="ModuleInfo.Ref"/> property to see if the current type loader will be able to retrieve the
        ///     <paramref name="moduleInfo"/>.
        /// </summary>
        /// <param name="moduleInfo">
        /// Module that should have it's type loaded.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current type loader is able to retrieve the module, otherwise
        ///     <see langword="false"/>.
        /// </returns>
        public bool CanLoadModuleType(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException("moduleInfo");
            }

            if (!string.IsNullOrEmpty(moduleInfo.Ref))
            {
                Uri uri;
                return Uri.TryCreate(moduleInfo.Ref, UriKind.RelativeOrAbsolute, out uri);
            }

            return false;
        }

        /// <summary>
        /// Retrieves the <paramref name="moduleInfo"/>.
        /// </summary>
        /// <param name="moduleInfo">
        /// Module that should have it's type loaded.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification = "Error is sent to completion event")]
        public void LoadModuleType(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException("moduleInfo");
            }

            try
            {
                var uri = new Uri(moduleInfo.Ref, UriKind.RelativeOrAbsolute);

                // If this module has already been downloaded, I fire the completed event.
                if (this.IsSuccessfullyDownloaded(uri))
                {
                    this.RaiseLoadModuleCompleted(moduleInfo, null);
                }
                else
                {
                    bool needToStartDownload = !this.IsDownloading(uri);

                    // I record downloading for the moduleInfo even if I don't need to start a new download
                    this.RecordDownloading(uri, moduleInfo);

                    if (needToStartDownload)
                    {
                        IFileDownloader downloader = this.CreateDownloader();
                        downloader.DownloadProgressChanged += this.IFileDownloader_DownloadProgressChanged;
                        downloader.DownloadCompleted += this.IFileDownloader_DownloadCompleted;
                        downloader.DownloadAsync(uri, uri);
                    }
                }
            }
            catch (Exception ex)
            {
                this.RaiseLoadModuleCompleted(moduleInfo, ex);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the <see cref="IFileDownloader" /> used to retrieve the remote modules.
        /// </summary>
        /// <returns>The <see cref="IFileDownloader" /> used to retrieve the remote modules.</returns>
        protected virtual IFileDownloader CreateDownloader()
        {
            return new FileDownloader();
        }

        /// <summary>
        /// Gets the parts.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// external assembly parts
        /// </returns>
        private static IEnumerable<AssemblyPart> GetParts(Stream stream)
        {
            var assemblyParts = new List<AssemblyPart>();

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
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Deployment.Parts")
                    {
                        using (XmlReader xmlReaderAssemblyParts = xmlReader.ReadSubtree())
                        {
                            while (xmlReaderAssemblyParts.Read())
                            {
                                if (xmlReaderAssemblyParts.NodeType == XmlNodeType.Element
                                    && xmlReaderAssemblyParts.Name == "AssemblyPart")
                                {
                                    var assemblyPart = new AssemblyPart();
                                    assemblyPart.Source = xmlReaderAssemblyParts.GetAttribute("Source");
                                    assemblyParts.Add(assemblyPart);
                                }
                            }
                        }

                        break;
                    }
                }
            }

            return assemblyParts;
        }

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
        /// Gets the downloading modules.
        /// </summary>
        /// <param name="uri">
        /// The URI parameter.
        /// </param>
        /// <returns>
        /// module info list
        /// </returns>
        private List<ModuleInfo> GetDownloadingModules(Uri uri)
        {
            lock (this.downloadingModules)
            {
                return new List<ModuleInfo>(this.downloadingModules[uri]);
            }
        }

        /// <summary>
        /// Handles the module download progress changed.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Net.DownloadProgressChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void HandleModuleDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            var uri = (Uri)e.UserState;
            List<ModuleInfo> moduleInfos = this.GetDownloadingModules(uri);

            foreach (ModuleInfo moduleInfo in moduleInfos)
            {
                this.RaiseModuleDownloadProgressChanged(
                    new ModuleDownloadProgressChangedEventArgs(moduleInfo, e.BytesReceived, e.TotalBytesToReceive));
            }
        }

        /// <summary>
        /// Handles the module downloaded.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Microsoft.Practices.Prism.Modularity.DownloadCompletedEventArgs"/> instance containing
        ///     the event data.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification = "Exception sent to completion event")]
        private void HandleModuleDownloaded(DownloadCompletedEventArgs e)
        {
            var uri = (Uri)e.UserState;
            List<ModuleInfo> moduleInfos = this.GetDownloadingModules(uri);

            Exception error = e.Error;
            if (error == null)
            {
                try
                {
                    this.RecordDownloadComplete(uri);

                    Debug.Assert(!e.Cancelled, "Download should not be cancelled");
                    Stream stream = e.Result;

                    foreach (AssemblyPart part in GetParts(stream))
                    {
                        LoadAssemblyFromStream(stream, part);
                    }

                    this.RecordDownloadSuccess(uri);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                finally
                {
                    e.Result.Close();
                }
            }

            foreach (ModuleInfo moduleInfo in moduleInfos)
            {
                this.RaiseLoadModuleCompleted(moduleInfo, error);
            }
        }

        /// <summary>
        /// Handles the DownloadCompleted event of the IFileDownloader control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="Microsoft.Practices.Prism.Modularity.DownloadCompletedEventArgs"/> instance containing
        ///     the event data.
        /// </param>
        private void IFileDownloader_DownloadCompleted(object sender, DownloadCompletedEventArgs e)
        {
            // A new IFileDownloader instance is created for each download.
            // I unregister the event to allow for garbage collection.
            var fileDownloader = (IFileDownloader)sender;
            fileDownloader.DownloadProgressChanged -= this.IFileDownloader_DownloadProgressChanged;
            fileDownloader.DownloadCompleted -= this.IFileDownloader_DownloadCompleted;

            // I ensure the download completed is on the UI thread so that types can be loaded into the application domain.
            if (!Deployment.Current.Dispatcher.CheckAccess())
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    new Action<DownloadCompletedEventArgs>(
                        param =>
                        ServiceLocator.Current.GetInstance<IExternalPartsLoader>()
                            .LoadExternalParts(param.Result, () => this.HandleModuleDownloaded(param))), 
                    e);
            }
            else
            {
                ServiceLocator.Current.GetInstance<IExternalPartsLoader>()
                    .LoadExternalParts(e.Result, () => this.HandleModuleDownloaded(e));
            }
        }

        /// <summary>
        /// Handles the DownloadProgressChanged event of the IFileDownloader control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Net.DownloadProgressChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void IFileDownloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // I ensure the download completed is on the UI thread so that types can be loaded into the application domain.
            if (!Deployment.Current.Dispatcher.CheckAccess())
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    new Action<DownloadProgressChangedEventArgs>(this.HandleModuleDownloadProgressChanged), 
                    e);
            }
            else
            {
                this.HandleModuleDownloadProgressChanged(e);
            }
        }

        /// <summary>
        /// Determines whether the specified URI is downloading.
        /// </summary>
        /// <param name="uri">
        /// The URI parameter.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified URI is downloading; otherwise, <c>false</c>.
        /// </returns>
        private bool IsDownloading(Uri uri)
        {
            lock (this.downloadingModules)
            {
                return this.downloadingModules.ContainsKey(uri);
            }
        }

        /// <summary>
        /// Determines whether [is successfully downloaded] [the specified URI].
        /// </summary>
        /// <param name="uri">
        /// The URI parameter.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is successfully downloaded] [the specified URI]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsSuccessfullyDownloaded(Uri uri)
        {
            lock (this.downloadedUris)
            {
                return this.downloadedUris.Contains(uri);
            }
        }

        /// <summary>
        /// Raises the load module completed.
        /// </summary>
        /// <param name="moduleInfo">
        /// The module info.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        private void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
        {
            this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
        }

        /// <summary>
        /// Raises the load module completed.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Microsoft.Practices.Prism.Modularity.LoadModuleCompletedEventArgs"/> instance
        ///     containing the event data.
        /// </param>
        private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
        {
            if (this.LoadModuleCompleted != null)
            {
                this.LoadModuleCompleted(this, e);
            }
        }

        /// <summary>
        /// Raises the module download progress changed.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Microsoft.Practices.Prism.Modularity.ModuleDownloadProgressChangedEventArgs"/> instance
        ///     containing the event data.
        /// </param>
        private void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs e)
        {
            if (this.ModuleDownloadProgressChanged != null)
            {
                this.ModuleDownloadProgressChanged(this, e);
            }
        }

        /// <summary>
        /// Records the download complete.
        /// </summary>
        /// <param name="uri">
        /// The URI parameter.
        /// </param>
        private void RecordDownloadComplete(Uri uri)
        {
            lock (this.downloadingModules)
            {
                if (!this.downloadingModules.ContainsKey(uri))
                {
                    this.downloadingModules.Remove(uri);
                }
            }
        }

        /// <summary>
        /// Records the download success.
        /// </summary>
        /// <param name="uri">
        /// The URI parameter.
        /// </param>
        private void RecordDownloadSuccess(Uri uri)
        {
            lock (this.downloadedUris)
            {
                this.downloadedUris.Add(uri);
            }
        }

        /// <summary>
        /// Records the downloading.
        /// </summary>
        /// <param name="uri">
        /// The URI parameter.
        /// </param>
        /// <param name="moduleInfo">
        /// The module info.
        /// </param>
        private void RecordDownloading(Uri uri, ModuleInfo moduleInfo)
        {
            lock (this.downloadingModules)
            {
                List<ModuleInfo> moduleInfos;
                if (!this.downloadingModules.TryGetValue(uri, out moduleInfos))
                {
                    moduleInfos = new List<ModuleInfo>();
                    this.downloadingModules.Add(uri, moduleInfos);
                }

                if (!moduleInfos.Contains(moduleInfo))
                {
                    moduleInfos.Add(moduleInfo);
                }
            }
        }

        #endregion
    }
}
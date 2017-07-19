namespace Infra.Presentation.Core.Modularization
{
    #region

    using System;
    using System.IO;

    #endregion

    /// <summary>
    ///         This class is used to fix Prism bug that downloads external parts of the module
    /// </summary>
    public interface IExternalPartsLoader
    {
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
        void LoadExternalParts(Stream stream, Action callbackMethod);

        #endregion
    }
}
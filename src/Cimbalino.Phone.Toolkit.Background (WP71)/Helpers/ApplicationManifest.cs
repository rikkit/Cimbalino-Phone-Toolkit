﻿// ****************************************************************************
// <copyright file="ApplicationManifest.cs" company="Pedro Lamas">
// Copyright © Pedro Lamas 2012
// </copyright>
// ****************************************************************************
// <author>Pedro Lamas</author>
// <email>pedrolamas@gmail.com</email>
// <date>25-12-2012</date>
// <project>Cimbalino.Phone.Toolkit</project>
// <web>http://www.pedrolamas.com</web>
// <license>
// See license.txt in this solution or http://www.pedrolamas.com/license_MIT.txt
// </license>
// ****************************************************************************

using System;
using System.Windows;
using System.Xml;
using Cimbalino.Phone.Toolkit.Extensions;

namespace Cimbalino.Phone.Toolkit.Helpers
{
    /// <summary>
    /// Represents the contents of the application manifest.
    /// </summary>
    public class ApplicationManifest
    {
        private const string AppManifestName = "WMAppManifest.xml";

        private static ApplicationManifest _current;

        #region Properties

        /// <summary>
        /// Gets the current <see cref="ApplicationManifest"/> instance.
        /// </summary>
        /// <value>The current <see cref="ApplicationManifest"/> instance.</value>
        public static ApplicationManifest Current
        {
            get
            {
                if (_current == null)
                {
                    var appManifestResourceInfo = Application.GetResourceStream(new Uri(AppManifestName, UriKind.Relative));

                    using (var appManifestStream = appManifestResourceInfo.Stream)
                    {
                        using (var reader = XmlReader.Create(appManifestStream, new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
                        {
                            _current = ParseXml(reader);
                        }
                    }
                }

                return _current;
            }
        }

        /// <summary>
        /// Gets or sets the version of the Windows Phone SDK or the runtime binaries of the platform. The default value is 8.0 for Windows Phone 8 and 7.1 for Windows Phone OS 7.1.
        /// </summary>
        /// <value>The version of the Windows Phone SDK or the runtime binaries of the platform.</value>
        public string AppPlatformVersion { get; set; }

        /// <summary>
        /// Gets or sets the application default language.
        /// </summary>
        /// <value>The application default language.</value>
        public ApplicationManifestLanguageNode DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or sets the application extra elements.
        /// </summary>
        /// <value>The application extra elements.</value>
        public ApplicationManifestNamedNode[] AppExtras { get; set; }

        /// <summary>
        /// Gets or sets the application supported languages.
        /// </summary>
        /// <value>The application supported languages.</value>
        public ApplicationManifestLanguageNode[] Languages { get; set; }

        /// <summary>
        /// Gets or sets the app detailed information.
        /// </summary>
        /// <value>The app detailed information.</value>
        public ApplicationManifestAppNode App { get; set; }

        #endregion

        internal static ApplicationManifest ParseXml(XmlReader reader)
        {
            reader.MoveToContent();

            var node = new ApplicationManifest
            {
                AppPlatformVersion = reader.GetAttribute("AppPlatformVersion")
            };

            reader.ReadStartElement();

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                switch (reader.Name)
                {
                    case "DefaultLanguage":
                        node.DefaultLanguage = ApplicationManifestLanguageNode.ParseXml(reader);

                        break;

                    case "AppExtra":
                        node.AppExtras = reader.ReadElementContentAsArray(ApplicationManifestNamedNode.ParseXml);

                        break;

                    case "Languages":
                        node.Languages = reader.ReadElementContentAsArray(ApplicationManifestLanguageNode.ParseXml);

                        break;

                    case "App":
                        node.App = ApplicationManifestAppNode.ParseXml(reader);

                        break;

                    default:
                        reader.Skip();

                        break;
                }
            }

            reader.ReadEndElement();

            return node;
        }
    }
}
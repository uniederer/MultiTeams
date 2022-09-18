//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;

namespace MultiTeams
{
    public interface IContextMenuApp : IDisposable
    {
        /// <summary>
        /// The application name displayed in the context menu
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// The product name used as installer path
        /// </summary>
        /// <remarks>
        /// This string is supposed to be path safe. Avoid non printable characters
        /// </remarks>
        public string ProductName { get; }

        /// <summary>
        /// The files that have to be copied along with the application upon installation
        /// </summary>
        public string[] Files { get; }

        /// <summary>
        /// Builds the menu item covering the application's commands
        /// </summary>
        /// <returns>The main menu item of the application</returns>
        IEnumerable<ToolStripItem> MenuBuild();

        /// <summary>
        /// This method is called as soon as the application's construction is finished. 
        /// It can be used to trigger last jobs.
        /// </summary>
        void Start();
    }
}

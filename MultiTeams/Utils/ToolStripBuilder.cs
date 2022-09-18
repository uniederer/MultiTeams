//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;

namespace MultiTeams.Utils
{
    internal class ToolStripBuilder
    {
        /// <summary>
        /// Create a ToolStripMenuItem and set it up as a drop-down
        /// </summary>
        /// <param name="caption">Caption to be displayed</param>
        /// <returns></returns>
        public static ToolStripMenuItem DropDown(string caption, ToolStripMenuItem[] children)
        {
            var result = new ToolStripMenuItem(caption);
            foreach (var child in children)
            {
                result.DropDownItems.Add(child);
            }
            return result;
        }

        /// <summary>
        /// Create a ToolStripMenuItem and set it up as a checkbox
        /// </summary>
        /// <param name="caption">Caption to be displayed</param>
        /// <param name="initalState">Initial checkbox state</param>
        /// <param name="onChanged">Action to be invoked upon state change</param>
        /// <returns></returns>
        public static ToolStripMenuItem Checkbox(string caption, bool initalState, EventHandler onChanged)
        {
            var result = new ToolStripMenuItem(caption);
            result.Checked = initalState;
            result.CheckOnClick = true;
            result.CheckedChanged += onChanged;
            return result;
        }

        /// <summary>
        /// Create a ToolStripMenuItem and set it up as a button to click on
        /// </summary>
        /// <param name="caption">Caption to be displayed</param>
        /// <param name="onClick">Click-Action to be executed</param>
        /// <returns></returns>
        public static ToolStripMenuItem Button(string caption, EventHandler onClick)
        {
            var result = new ToolStripMenuItem(caption);
            result.Click += (sender, e) =>
            {
                result.InvokeIfNeeded(() =>
                {
                    onClick(sender, e);
                });
            };
            return result;
        }
    }
}

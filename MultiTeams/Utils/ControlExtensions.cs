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
    public static class ControlExtensions
    {
        public static void InvokeIfNeeded(this ToolStripItem item, Action action)
        {
            item.GetCurrentParent()
                .InvokeIfNeeded(action);
        }

        public static void InvokeIfNeeded(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}

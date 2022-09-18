//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;

namespace MultiTeams.Instances
{
    public class InstanceSettings
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();

        public bool Autostart { get; set; } = true;
    }
}

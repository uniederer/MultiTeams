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
    internal class MultiTeamsSetup
    {
        public string LauncherLocation { get; set; } = @"%LOCALAPPDATA%\Microsoft\Teams\Update.exe";

        public string LauncherArguments { get; set; } = @"--processStart ""Teams.exe""";

        public string TeamsProfileBasePath { get; set; } = @"%LOCALAPPDATA%\Microsoft\Teams\CustomProfiles\";

        public string LauncherWorkingDirectory { get; set; } = @"%LOCALAPPDATA%\Microsoft\Teams\";

        public IEnumerable<InstanceSettings> Instances { get; set; } = new List<InstanceSettings>();

        public void AddInstance(InstanceSettings data)
        {
            if (Instances.Any(o => o.Name.Equals(data.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new InvalidDataException($"There is already a configuration named '{data.Name}'.");
            }
            Instances = Instances.Append(data);
        }

        public void RemoveInstance(string name)
        {
            Instances = Instances.Where(i => i.Name != name);
        }
    }
}

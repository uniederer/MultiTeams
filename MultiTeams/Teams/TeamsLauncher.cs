//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using MultiTeams.Utils;
using System;
using System.Diagnostics;
using System.Linq;

namespace MultiTeams.Teams
{
    internal class TeamsLauncher
    {
        public string LauncherLocation { get; init; } = @"%LOCALAPPDATA%\Microsoft\Teams\Update.exe";

        public string LauncherArguments { get; init; } = @"--processStart ""Teams.exe""";

        public string TeamsProfileBasePath { get; init; } = @"%LOCALAPPDATA%\Microsoft\Teams\CustomProfiles\";

        public string LauncherWorkingDirectory { get; init; } = string.Empty;

        /// <summary>
        /// Launch a teams instance named according to <paramref name="name"/>
        /// </summary>
        /// <param name="name">Name of the instance</param>
        public void Launch(string name)
        {
            var Launcher = Environment.ExpandEnvironmentVariables(LauncherLocation);
            var WorkingDirectory = !string.IsNullOrEmpty(LauncherWorkingDirectory) 
                                 ? Environment.ExpandEnvironmentVariables(LauncherWorkingDirectory) 
                                 : Path.GetDirectoryName(Launcher);
            var ProfileBase = Environment.ExpandEnvironmentVariables(TeamsProfileBasePath);

            Process proc = new Process();
            proc.StartInfo.FileName = Launcher;
            proc.StartInfo.Arguments = Environment.ExpandEnvironmentVariables(LauncherArguments);
            proc.StartInfo.WorkingDirectory = WorkingDirectory;

            proc.StartInfo.LoadUserProfile = true;
            proc.StartInfo.EnvironmentVariables["USERPROFILE"] = Path.Combine(ProfileBase, name.MakePathSafe());

            proc.Start();
        }
    }
}

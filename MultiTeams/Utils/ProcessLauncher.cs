using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTeams.Utils
{
    internal static class ProcessLauncher
    {
        /// <inheritdoc cref="Process.Start"/>
        public static bool Start(string exe, string args)
        {
            using Process myProcess = new Process();
            myProcess.StartInfo.FileName = exe;
            myProcess.StartInfo.Arguments = args;
            return myProcess.Start();
        }
    }
}

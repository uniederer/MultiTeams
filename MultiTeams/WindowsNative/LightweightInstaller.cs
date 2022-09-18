//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Win32;
using System;
using System.Linq;

namespace MultiTeams.WindowsNative
{
    public class LightweightInstaller
    {
        private readonly string _productName;
        private readonly string _exeSrc;
        private readonly IEnumerable<string>? _deps;

        private string ExeDstPath => Path.Combine(InstallDir, Path.GetFileName(Application.ExecutablePath));

        private string UninstallKeyName => $"{_productName}.uninstall";

        public string AppDataDir => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public string InstallDir => Path.Combine(AppDataDir, _productName);

        public bool Installed => Directory.Exists(InstallDir);

        public bool IsInstalledExe => Application.ExecutablePath.StartsWith(InstallDir);

        public LightweightInstaller(string productName, string exeSrc, IEnumerable<string>? depsSrc = null)
        {
            if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentOutOfRangeException(nameof(productName));
            if (string.IsNullOrWhiteSpace(exeSrc)) throw new ArgumentOutOfRangeException(nameof(exeSrc));

            _productName = productName;
            _exeSrc = exeSrc;
            _deps = depsSrc;
        }

        public void Install()
        {
            if (IsInstalledExe) return;

            string srcDir = Path.GetDirectoryName(_exeSrc) ?? string.Empty;
            string dstDir = InstallDir;
            if (!_exeSrc.StartsWith(dstDir))
            {
                Directory.CreateDirectory(dstDir);

                ApplicationCopy(srcDir, dstDir);
            }

            RemovalCancel();
        }

        private void ApplicationCopy(string srcDir, string dstDir)
        {
            CopyFileToDir(dstDir, _exeSrc, true);
            if (_deps != null)
            {
                foreach (var dep in _deps)
                {
                    var srcPath = Path.Combine(srcDir, dep);
                    CopyFileToDir(dstDir, srcPath, true);
                }
            }
        }

        private static void CopyFileToDir(string dstPath, string srcPath, bool overwrite = false)
        {
            File.Copy(srcPath, Path.Combine(dstPath, Path.GetFileName(srcPath)), overwrite);
        }

        public void Uninstall()
        {
            if (!Installed) return;

            AutostartDisable();
            RemovalSchedule();

            MessageBox.Show("The application will be removed upon next reboot.", "Uninstall prepared.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RemovalSchedule()
        {
            var registry = RunOnceKeyGet();
            if (registry == null) throw new NullReferenceException($"Unable to access RunOnce key");

            registry.SetValue(UninstallKeyName, $"rmdir /S /Q \"{InstallDir}\"");
        }

        private void RemovalCancel()
        {
            var registry = RunOnceKeyGet();
            if (registry == null) throw new NullReferenceException($"Unable to access RunOnce key");

            registry?.DeleteValue(UninstallKeyName, false);
        }

        private RegistryKey? RunOnceKeyGet()
        {
            return Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);
        }

        public void AutostartEnable()
        {
            var registry = AutoStartKeyGet();
            if (registry == null) throw new NullReferenceException($"Unable to access autostart key");

            registry.SetValue(_productName, $"\"{ExeDstPath}\"");
        }

        public bool AutostartEnabled()
        {
            var autostart = AutoStartKeyGet();
            if (autostart == null) return false;

            return autostart.GetValueNames()
                            .Any(x => x.Equals(_productName));
        }

        public void AutostartDisable()
        {
            var registry = AutoStartKeyGet();
            if (registry == null) throw new NullReferenceException($"Unable to access autostart key");

            registry.DeleteValue(_productName, false);
        }

        private RegistryKey? AutoStartKeyGet()
        {
            return Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        }
    }
}

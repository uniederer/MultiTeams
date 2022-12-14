//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using MultiTeams.Utils;
using MultiTeams.WindowsNative;
using System;
using System.Linq;

namespace MultiTeams
{
    internal class ContextMenu : ApplicationContext
    {
        private NotifyIcon _icon;
        private IContextMenuApp _app;

        private LightweightInstaller _installer;

        public ContextMenu(IContextMenuApp app)
        {
            _app = app;
            _installer = new LightweightInstaller(app.ProductName.MakePathSafe()
                                                , Application.ExecutablePath
                                                , app.Files);



            var contextMenu = TrayMenuBuild();

            _icon = new NotifyIcon()
            {
                Icon = MultiTeams.AppIcon,
                ContextMenuStrip = contextMenu,
                Visible = true,
            };

            _app.Start();
        }

        private ContextMenuStrip TrayMenuBuild()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            var heading = ToolStripBuilder.Label($"{_app.ApplicationName}");
            heading.Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
            menu.Items.Add(heading);
            var version = ToolStripBuilder.Label($"v{Application.ProductVersion}",
                                                 new Font(FontFamily.GenericSansSerif, 7, FontStyle.Regular))
                                          .MarginLeftSet(20);
            version.Alignment = ToolStripItemAlignment.Right;
            menu.Items.Add(version);

            {
                menu.Items.Add(new ToolStripSeparator());
                foreach (var item in _app.MenuBuild())
                {
                    menu.Items.Add(item);
                }
                menu.Items.Add(new ToolStripSeparator());
                menu.Items.Add(SettingsMenuBuild());
                menu.Items.Add(new ToolStripSeparator());
            }

            var ExitItem = ToolStripBuilder.Button("Exit", (sender, e) =>
            {
                ApplicationExit();
            });
            menu.Items.Add(ExitItem);

            return menu;
        }

        private void ApplicationExit()
        {
            _icon.Visible = false;
            _app?.Dispose();
            Application.Exit();
        }

        private ToolStripMenuItem SettingsMenuBuild()
        {
            ToolStripMenuItem Autostart = AppAutostartSettingBuild();
            ToolStripMenuItem Uninstall = AppUninstallBuild();

            var SettingsMenu = ToolStripBuilder.DropDown("Settings", new[]
            {
                Autostart,
                Uninstall,
            });

            SettingsMenu.DropDownOpening += (sender, e) =>
            {
                Uninstall.Enabled = _installer.Installed;
            };

            return SettingsMenu;
        }

        private ToolStripMenuItem AppAutostartSettingBuild()
        {
            return ToolStripBuilder.Checkbox("Autostart", _installer.AutostartEnabled(), (sender, e) =>
            {
                var source = sender as ToolStripMenuItem;
                if (source == null) return;

                if (source.Checked)
                {
                    _installer.Install();
                    _installer.AutostartEnable();
                    if (!_installer.IsInstalledExe)
                    {
                        var exe = Path.Combine(_installer.InstallDir, Path.GetFileName(Application.ExecutablePath));
                        ProcessLauncher.Start(exe, $"/new");
                        ApplicationExit();
                    }
                }
                else
                {
                    _installer.AutostartDisable();
                }
            });
        }

        private ToolStripMenuItem AppUninstallBuild()
        {
            return ToolStripBuilder.Button("Uninstall", (sender, e) =>
            {
                _installer.Uninstall();
            });
        }
    }
}

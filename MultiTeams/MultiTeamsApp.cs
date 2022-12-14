//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using MultiTeams.Instances;
using MultiTeams.Utils;
using System;
using System.Diagnostics;
using System.Linq;

namespace MultiTeams
{
    internal class MultiTeamsApp : IContextMenuApp
    {
        public const string PRODUCT_NAME = "MultiTeams";
        public const string CONFIG_NAME = "MultiTeamsSetup.json";

        private bool _disposed = false;
        private MultiTeamsService _service;
        private ToolStripMenuItem _menu;

        public string ApplicationName => PRODUCT_NAME;

        public string ProductName => PRODUCT_NAME;

        public string[] Files => new string[]
        {
            "MultiTeams.dll"
            , "MultiTeams.runtimeconfig.json"
            , CONFIG_NAME
        };

        public MultiTeamsApp()
        {
            var appDir = Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty;

            _service = new MultiTeamsService(Path.Combine(appDir, CONFIG_NAME));
            _menu = new ToolStripMenuItem("Instances");

            _service.OnSetupChanged += OnSetupChanged;
        }

        private void OnSetupChanged(object? sender, EventArgs args)
        {
            _menu.InvokeIfNeeded(() =>
            {
                InstanceMenuPopulate(_service.TeamsInstances);
            });
        }

        public IEnumerable<ToolStripItem> MenuBuild()
        {
            InstanceMenuPopulate(_service.TeamsInstances);

            return new[] {
                _menu
            };
        }

        private void InstanceMenuPopulate(IEnumerable<InstanceSettings> instances)
        {
            _menu.DropDownItems.Clear();

            if (instances != null)
            {
                foreach (var instance in instances.OrderBy(i => i.Name))
                {
                    _menu.DropDownItems.Add(InstanceMenuDetailMenu(instance));
                }
            }

            _menu.DropDownItems.Add(new ToolStripSeparator());
            _menu.DropDownItems.Add(ToolStripBuilder.Button("Add...", OnAddInstance_Click));
            _menu.DropDownItems.Add(ToolStripBuilder.Button("Reveal config file in explorer...", (sender, args) =>
            {
                ProcessLauncher.Start("explorer.exe", $"/select, \"{_service.ConfigPath}\"");
            }));
        }

        private void OnAddInstance_Click(object? sender, EventArgs e)
        {
            var addDialog = new AddInstance();
            var result = addDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    _service.Setup.AddInstance(addDialog.Data);
                    SetupStore();

                    var doStart = MessageBox.Show("Instance added. Do you want to start it now?", "Instance added", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (doStart == DialogResult.Yes)
                    {
                        _service.LaunchInstance(addDialog.Data);
                    }
                }
                catch (InvalidDataException)
                {
                    ReportError($"A configuration named '{addDialog.Data.Name}' already exists. Nothing added.");
                }
                catch (Exception ex)
                {
                    ReportError("Error while storing setup:", ex);
                }
            }
        }

        private ToolStripMenuItem InstanceMenuDetailMenu(InstanceSettings instance)
        {
            var DetailMenu = new ToolStripMenuItem(instance.Name);

            var Autostart = ToolStripBuilder.Checkbox("Autostart", instance.Autostart, (sender, e) =>
            {
                instance.Autostart = !instance.Autostart;
                SetupStore();
            });

            var Launch = ToolStripBuilder.Button("Launch", (sender, e) =>
            {
                _service.LaunchInstance(instance);
            });

            var RemoveInstance = ToolStripBuilder.Button("Remove", (object? sender, EventArgs e) =>
            {
                _service.Setup.RemoveInstance(instance.Name);
                SetupStore();
            });

            return ToolStripBuilder.DropDown(instance.Name, new[]
            {
                Launch, Autostart, RemoveInstance,
            });
        }

        public void Start()
        {
            _service.LaunchAutostartFlagged();
        }

        public void SetupStore()
        {
            try
            {
                _service.SetupStore();
            }
            catch (Exception ex)
            {
                ReportError("Error while storing setup:", ex);
            }
        }

        private static void ReportError(string error, Exception ex)
        {
            ReportError($"{error}\n\n{ex}");
        }

        private static void ReportError(string error)
        {
            MessageBox.Show(error, "An error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _service.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

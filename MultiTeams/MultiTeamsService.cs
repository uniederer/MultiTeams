//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using MultiTeams.Instances;
using MultiTeams.Teams;
using System;
using System.Linq;

namespace MultiTeams
{
    internal class MultiTeamsService : IDisposable
    {
        private bool _disposed;
        private TeamsLauncher _launcher;
        private MultiTeamsSetupFactory _setupSource;

        public EventHandler? OnSetupChanged;

        public MultiTeamsSetup Setup => _setupSource.Setup;

        public IEnumerable<InstanceSettings> TeamsInstances => Setup.Instances;

        public MultiTeamsService(string configPath)
        {
            _setupSource = new MultiTeamsSetupFactory(configPath);
            _launcher = LauncherCreate();

            _setupSource.OnSetupChanged += OnSetupFileChanged;
        }

        private void OnSetupFileChanged(object? sender, EventArgs args)
        {
            _launcher = LauncherCreate();
            OnSetupChanged?.Invoke(this, args);
        }

        private TeamsLauncher LauncherCreate()
        {
            return new TeamsLauncher()
            {
                LauncherLocation = Setup.LauncherLocation,
                LauncherArguments = Setup.LauncherArguments,
                LauncherWorkingDirectory = Setup.LauncherWorkingDirectory,
                TeamsProfileBasePath = Setup.TeamsProfileBasePath,
            };
        }

        public void LaunchAutostartFlagged()
        {
            foreach (var instance in _setupSource.Setup.Instances)
            {
                if (instance.Autostart)
                {
                    LaunchInstance(instance);
                }
            }
        }

        public void LaunchInstance(InstanceSettings instance)
        {
            Task.Run(() =>
            {
                _launcher.Launch(instance.Name);
            });
        }

        public void SetupStore()
        {
            _setupSource.ConfigStore();
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _setupSource.Dispose();
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

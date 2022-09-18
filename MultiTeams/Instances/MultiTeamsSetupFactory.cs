//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MultiTeams.Instances
{
    internal class MultiTeamsSetupFactory : IDisposable
    {
        private bool _isDisposed = false;
        private string _configPath = string.Empty;
        private readonly FileSystemWatcher _configWatcher;

        public event EventHandler? OnSetupChanged;

        public MultiTeamsSetup Setup { get; private set; } = new MultiTeamsSetup();

        public MultiTeamsSetupFactory(string configPath)
        {
            var setupDir = Path.GetDirectoryName(configPath) ?? string.Empty;

            _configPath = configPath;

            ConfigCreateIfNotExisting();
            ConfigLoad();

            _configWatcher = ConfigWatcherCreate(setupDir);
        }

        private void ConfigCreateIfNotExisting()
        {
            if (!File.Exists(_configPath))
            {
                ConfigStore();
            }
        }

        private FileSystemWatcher ConfigWatcherCreate(string dir)
        {
            var result = new FileSystemWatcher(dir);
            result.NotifyFilter = NotifyFilters.LastWrite;
            result.Filter = "*.json";
            result.EnableRaisingEvents = true;

            result.Changed += OnConfigChanged;

            return result;
        }

        private void OnConfigChanged(object sender, FileSystemEventArgs e)
        {
            ConfigLoad();
            OnSetupChanged?.Invoke(this, new EventArgs());
        }

        private void ConfigLoad()
        {
            MultiTeamsSetup? result;
            try
            {
                var json = File.ReadAllText(_configPath, Encoding.UTF8);
                result = JsonSerializer.Deserialize<MultiTeamsSetup>(json);
            }
            catch (IOException)
            {
                return;
            }

            if (result == null)
            {
                result = new MultiTeamsSetup();
            }

            Setup = result;
        }

        public void ConfigStore()
        {
            string setupText = JsonSerializer.Serialize(Setup, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            File.WriteAllText(_configPath, setupText);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _configWatcher.Dispose();
                }
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

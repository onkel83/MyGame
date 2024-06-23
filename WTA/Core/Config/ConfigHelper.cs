using System;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;


namespace Core.Helper
{
    public static class ConfigHelper
    {
        private static readonly string ConfigDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs");
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _Configs = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
        private static readonly FileSystemWatcher FileWatcher = new FileSystemWatcher(ConfigDirectory);

        public static ConcurrentDictionary<string, ConcurrentDictionary<string, string>> Configs { get => _Configs;}

        static ConfigHelper()
        {
            FileHelper.EnsureDirectoryExists(ConfigDirectory);
            LoadAllConfigs();
            InitializeFileWatcher();
        }

        private static void LoadAllConfigs()
        {
            try
            {
                string[] configFiles = Directory.GetFiles(ConfigDirectory, "*.json");
                foreach (var configFile in configFiles)
                {
                    LoadConfig(Path.GetFileNameWithoutExtension(configFile));
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error loading all configs: {ex.Message}", LogLevel.Crit);
            }
        }

        public static ConcurrentDictionary<string, string> LoadConfig(string configName)
        {
            string configFilePath = Path.Combine(ConfigDirectory, $"{configName}.json");

            if (!File.Exists(configFilePath))
            {
                LoggerHelper.Log($"Config file not found: {configFilePath}. Creating default config.", LogLevel.Dbug, nameof(ConfigHelper.LoadConfig), 38);
                SaveConfig(configName, new ConcurrentDictionary<string, string>());
                return new ConcurrentDictionary<string, string>();
            }

            try
            {
                string configJson = FileHelper.ReadFile(configFilePath);
                var config = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(configJson);
                if (config != null)
                {
                    _Configs[configName] = config;
                    return config;
                }
                else
                {
                    LoggerHelper.Log($"No Config Key,Value-Pair found in {configFilePath}", LogLevel.Dbug, nameof(ConfigHelper.LoadConfig), 38);
                    return new ConcurrentDictionary<string, string>();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error loading config {configName}: {ex.Message}", LogLevel.Crit);
                return new ConcurrentDictionary<string, string>();
            }
        }

        public static void SaveConfig(string configName, ConcurrentDictionary<string, string> config)
        {
            try
            {
                string configFilePath = Path.Combine(ConfigDirectory, $"{configName}.json");
                string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                FileHelper.WriteFile(configFilePath, configJson);
                _Configs[configName] = config;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error saving config {configName}: {ex.Message}", LogLevel.Crit);
            }
        }

        public static string GetConfigValue(string configName, string key, string? defaultValue = null)
        {
            try
            {
                if (_Configs.TryGetValue(configName, out var config) && config.TryGetValue(key, out var value))
                {
                    return value;
                }
                else
                {
                    SetConfigValue(configName, key, defaultValue??string.Empty);
                    return defaultValue ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error getting config value {key} from {configName}: {ex.Message}", LogLevel.Crit);
            }
            return string.Empty;
        }

        public static void SetConfigValue(string configName, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));
            }

            try
            {
                var config = LoadConfig(configName);
                config[key] = value;
                SaveConfig(configName, config);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error setting config value {key} in {configName}: {ex.Message}", LogLevel.Crit);
            }
        }

        private static void InitializeFileWatcher()
        {
            try
            {
                FileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                FileWatcher.Changed += OnConfigFileChanged;
                FileWatcher.Created += OnConfigFileChanged;
                FileWatcher.Deleted += OnConfigFileChanged;
                FileWatcher.Renamed += OnConfigFileChanged;
                FileWatcher.Filter = "*.json";
                FileWatcher.IncludeSubdirectories = false;
                FileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error initializing file watcher: {ex.Message}", LogLevel.Crit);
            }
        }

        private static void OnConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                string configName = Path.GetFileNameWithoutExtension(e.FullPath);
                LoggerHelper.Log($"Config file changed: {configName}. Reloading config.", LogLevel.Dbug);
                LoadConfig(configName);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error reloading config on file change: {ex.Message}", LogLevel.Crit);
            }
        }
    }
}

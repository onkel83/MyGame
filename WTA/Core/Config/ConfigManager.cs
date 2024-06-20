using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Core.Log;
using Core.Helper;
using Core.Enum;

namespace Core.Config
{
    public class ConfigManager
    {
        private static readonly string ConfigDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs");
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> Configs = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
        private static readonly FileSystemWatcher FileWatcher = new FileSystemWatcher(ConfigDirectory);

        static ConfigManager()
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
                Logger.Instance.Log($"Error loading all configs: {ex.Message}", LogLevel.Crit);
            }
        }

        public static ConcurrentDictionary<string, string> LoadConfig(string configName)
        {
            string configFilePath = Path.Combine(ConfigDirectory, $"{configName}.json");

            if (!File.Exists(configFilePath))
            {
                Logger.Instance.Log($"Config file not found: {configFilePath}. Creating default config.", LogLevel.Info);
                SaveConfig(configName, new ConcurrentDictionary<string, string>());
                return new ConcurrentDictionary<string, string>();
            }

            try
            {
                string configJson = File.ReadAllText(configFilePath);
                var config = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(configJson);
                if (config != null)
                {
                    Configs[configName] = config;
                    return config;
                }
                else { return new ConcurrentDictionary<string, string>(); }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error loading config {configName}: {ex.Message}", LogLevel.Crit);
                return new ConcurrentDictionary<string, string>();
            }
        }

        public static void SaveConfig(string configName, ConcurrentDictionary<string, string> config)
        {
            try
            {
                string configFilePath = Path.Combine(ConfigDirectory, $"{configName}.json");
                string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configFilePath, configJson);
                Configs[configName] = config;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error saving config {configName}: {ex.Message}", LogLevel.Crit);
            }
        }

        public static string GetConfigValue(string configName, string key)
        {
            try
            {
                if (Configs.TryGetValue(configName, out var config) && config.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error getting config value {key} from {configName}: {ex.Message}", LogLevel.Crit);
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
                Logger.Instance.Log($"Error setting config value {key} in {configName}: {ex.Message}", LogLevel.Crit);
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
                Logger.Instance.Log($"Error initializing file watcher: {ex.Message}", LogLevel.Crit);
            }
        }

        private static void OnConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                string configName = Path.GetFileNameWithoutExtension(e.FullPath);
                Logger.Instance.Log($"Config file changed: {configName}. Reloading config.", LogLevel.Info);
                LoadConfig(configName);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error reloading config on file change: {ex.Message}", LogLevel.Crit);
            }
        }
    }
}

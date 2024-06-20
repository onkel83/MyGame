using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
            string[] configFiles = Directory.GetFiles(ConfigDirectory, "*.json");
            foreach (var configFile in configFiles)
            {
                LoadConfig(Path.GetFileNameWithoutExtension(configFile));
            }
        }

        public static ConcurrentDictionary<string, string> LoadConfig(string configName)
        {
            string configFilePath = Path.Combine(ConfigDirectory, $"{configName}.json");

            if (!File.Exists(configFilePath))
            {
                Logger.Instance.Log($"Config file not found: {configFilePath}. Creating default config.", Core.Enum.LogLevel.Info);
                SaveConfig(configName, new ConcurrentDictionary<string, string>());
                return new ConcurrentDictionary<string, string>();
            }

            string configJson = File.ReadAllText(configFilePath);
            var config = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(configJson);
            Configs[configName] = config?? new ConcurrentDictionary<string, string>();
            return config??new ConcurrentDictionary<string, string>();
        }

        public static void SaveConfig(string configName, ConcurrentDictionary<string, string> config)
        {
            string configFilePath = Path.Combine(ConfigDirectory, $"{configName}.json");
            string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFilePath, configJson);
            Configs[configName] = config;
        }

        public static string GetConfigValue(string configName, string key)
        {
            if (Configs.TryGetValue(configName, out var config) && config.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                Logger.Instance.Log($"Config : {configName}, enthält keinen Key : {key}", Enum.LogLevel.Info);
                return string.Empty;
            }
        }

        public static void SetConfigValue(string configName, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));
            }

            var config = LoadConfig(configName);
            config[key] = value;
            SaveConfig(configName, config);
        }

        private static void InitializeFileWatcher()
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

        private static void OnConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            string configName = Path.GetFileNameWithoutExtension(e.FullPath);
            Logger.Instance.Log($"Config file changed: {configName}. Reloading config.", LogLevel.Info);
            LoadConfig(configName);
        }
    }
}

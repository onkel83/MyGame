using Core.Helper;
using Core.Log;
using Core.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WTA_Test
{
    public class ConfigTest
    {
        public ConfigTest() { Run(); }
        private static void Run()
        {
            Console.WriteLine("Setting up configuration values...");
            //CreateTest();
            
            // Get and display configuration values
            Console.WriteLine("Reading configuration values...");
            //Display();
            ShowTable();
            // Modify a configuration value
            //Console.WriteLine("Modifying a configuration value...");
            //ConfigHelper.SetConfigValue("TestConfig", "TestKey", "new TestValue");
            //DisplayConfigValue("AppConfig", "DataPfad");

            // Delete a configuration value
            //Console.WriteLine("Deleting a configuration value...");
            //DeleteTest("TestConfig", "TestKey");
        }
        private static void ShowTable()
        {
            string[] headers = ["ID","ConfigFile","ConfigName","Wert"];
            int i = 1;
            List<ConfigModel> Values = [];
            foreach (var item in ConfigHelper.Configs)
            {
                foreach (var item1 in item.Value)
                {
                    Values.Add(new ConfigModel {ID = i.ToString("D6"), Config = item.Key, Key = item1.Key, Value = item1.Value });
                    i++;
                }
            }
            ConsoleHelper.WriteTable(Values, headers);
        }

        private static void Display()
        {
            DisplayConfigValue("AppConfig", "DataPfad");
            DisplayConfigValue("TableConfig", "BorderChars");
            DisplayConfigValue("MenuConfig", "Delimiter");
            DisplayConfigValue("LogConfig", "LogToFile");
            DisplayConfigValue("LogConfig", "LogToConsole");
            DisplayConfigValue("LogConfig", "LogDirectory");
            DisplayConfigValue("LogConfig", "MinimumLogLevel");
            DisplayConfigValue("TestConfig", "TestKey");
        }
        private static void CreateTest()
        {
            ConfigHelper.SetConfigValue("AppConfig", "DataPfad", "./data");
            ConfigHelper.SetConfigValue("TableConfig", "BorderChars", "═║╔╗╚╝╬╦╩╬╟╢");
            ConfigHelper.SetConfigValue("MenuConfig", "Delimiter", "=");
            ConfigHelper.SetConfigValue("LogConfig", "LogToFile", "true");
            ConfigHelper.SetConfigValue("LogConfig", "LogToConsole", "true");
            ConfigHelper.SetConfigValue("LogConfig", "LogDirectory", "./logs");
            ConfigHelper.SetConfigValue("LogConfig", "MinimumLogLevel", "Info");
            ConfigHelper.SetConfigValue("TestConfig", "TestKey", "TestValue");

        }
        private static void DeleteTest(string config, string key)
        {
            DeleteConfigValue($"{config}", $"{key}");
        }
        private static void DisplayConfigValue(string configName, string key)
        {
            string value = ConfigHelper.GetConfigValue(configName, key);
            Console.WriteLine($"{configName} - {key}: {value}");
        }
        private static void DeleteConfigValue(string configName, string key)
        {
            var config = ConfigHelper.LoadConfig(configName);
            if (config.TryRemove(key, out string _))
            {
                ConfigHelper.SaveConfig(configName, config);
                Logger.Instance.Log($"Deleted configuration key: {configName} - {key}", LogLevel.Info);
                Console.WriteLine($"Deleted configuration key: {configName} - {key}");
            }
            else
            {
                Logger.Instance.Log($"Failed to delete configuration key: {configName} - {key}", LogLevel.Warn);
                Console.WriteLine($"Failed to delete configuration key: {configName} - {key}");
            }
        }
    }
}

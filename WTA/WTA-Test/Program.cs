using Core.Helper;
using Core.Log;
using Core.Manager;
using WTA_Test;

//ConfigTest _ = new();
//LogTest _ = new();

// Set up configuration values
//ConfigHelper.SetConfigValue("AppConfig", "DataPfad", "./data");
//ConfigHelper.SetConfigValue("TableConfig", "BorderChars", "═║╔╗╚╝╬╦╩╬╟╢");
//ConfigHelper.SetConfigValue("MenuConfig", "Delimiter", "=");
//ConfigHelper.SetConfigValue("LogConfig", "LogToFile", "true");
//ConfigHelper.SetConfigValue("LogConfig", "LogToConsole", "true");
//ConfigHelper.SetConfigValue("LogConfig", "LogDirectory", "./logs");
//ConfigHelper.SetConfigValue("LogConfig", "MinimumLogLevel", "Info");

// Initialize Logger
Logger.Instance.Log("Logger initialized.", LogLevel.Info);

// Display menu to manage TestModel objects
MenuManager.DisplayMenu();



Console.WriteLine();
Console.WriteLine("Please press [Enter]");
Console.ReadLine();
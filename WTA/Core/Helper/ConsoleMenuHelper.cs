using System;
using System.Collections.Generic;
using Core.Config;
using Core.Enum;
using Core.Log;

namespace Core.Helper
{
    public static class ConsoleMenuHelper
    {
        public static void DisplayMenu(string title, Dictionary<char, (string description, Action action)> menuItems)
        {
            // Load delimiter configuration
            var delimiter = ConfigManager.GetConfigValue("MenuConfig", "Delimiter") ?? "=";

            // Calculate console width
            int consoleWidth = Console.WindowWidth;

            // Print top delimiter line
            PrintDelimiterLine(delimiter, consoleWidth);

            // Print title line
            PrintTitleLine(title, delimiter, consoleWidth);

            // Print second delimiter line
            PrintDelimiterLine(delimiter, consoleWidth);

            // Print menu items
            foreach (var menuItem in menuItems)
            {
                PrintMenuItemLine(menuItem.Key, menuItem.Value.description, delimiter, consoleWidth);
            }

            // Print bottom delimiter line
            PrintDelimiterLine(delimiter, consoleWidth);

            // Read user input and execute corresponding action
            char choice = Console.ReadKey(true).KeyChar;
            if (menuItems.ContainsKey(choice))
            {
                try
                {
                    menuItems[choice].action();
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log($"Error executing action: {ex.Message}", LogLevel.Crit);
                }
            }
            else
            {
                Logger.Instance.Log($"Invalid menu choice: {choice}", LogLevel.Warn);
            }
        }

        private static void PrintDelimiterLine(string delimiter, int width)
        {
            Console.WriteLine(new string(delimiter[0], width));
        }

        private static void PrintTitleLine(string title, string delimiter, int width)
        {
            int padding = (width - title.Length - 4) / 2;
            Console.WriteLine($"{delimiter[0]} {new string(' ', padding)}{title}{new string(' ', padding)} {delimiter[0]}");
        }

        private static void PrintMenuItemLine(char choice, string description, string delimiter, int width)
        {
            string line = $"{delimiter[0]} {choice} - {description} {delimiter[0]}";
            Console.WriteLine(line.PadRight(width - 1) + delimiter[0]);
        }
    }
}

using System;
using System.Collections.Generic;
using Core.Enums;

namespace Core.Helper
{
    public static class ConsoleMenuHelper
    {
        public static void DisplayMenu(string title, Dictionary<char, (string description, Action action)> menuItems)
        {
            var delimiter = ConfigHelper.GetConfigValue("MenuConfig", "Delimiter") ?? "=";
            int consoleWidth = Console.WindowWidth;

            PrintDelimiterLine(delimiter, consoleWidth);
            PrintTitleLine(title, delimiter, consoleWidth);
            PrintDelimiterLine(delimiter, consoleWidth);

            foreach (var menuItem in menuItems)
            {
                PrintMenuItemLine(menuItem.Key, menuItem.Value.description, delimiter, consoleWidth);
            }

            PrintDelimiterLine(delimiter, consoleWidth);

            char choice = Console.ReadKey(true).KeyChar;
            if (menuItems.ContainsKey(choice))
            {
                try
                {
                    menuItems[choice].action();
                }
                catch (Exception ex)
                {
                    LoggerHelper.Log($"Error executing action: {ex.Message}", LogLevel.Crit);
                }
            }
            else
            {
                LoggerHelper.Log($"Invalid menu choice: {choice}", LogLevel.Warn);
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

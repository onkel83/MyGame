using Core.Model;
using System;
using System.Collections.Generic;

namespace Core.Helper
{
    public static class ConsoleHelper
    {
        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        public static void Write(string message, ConsoleColor color = ConsoleColor.White)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = originalColor;
        }

        public static void WriteTable<T>(List<T> items, string[]? headers = null) where T : BaseModel
        {
            // Logic to print table with optional headers
            ConsoleTableHelper<T>.PrintTable(items, headers);
        }
    }
}
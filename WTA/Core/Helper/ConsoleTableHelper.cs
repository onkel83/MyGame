using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Config;
using Core.Enum;
using Core.Log;
using Core.Model;

namespace Core.Helper
{
    public static class ConsoleTableHelper<T> where T : BaseModel
    {
        public static void PrintTable(List<T> items, string[]? headers = null)
        {
            // Configurations for borders
            var borderTop = '═';
            var borderBottom = '═';
            var borderLeft = '║';
            var borderRight = '║';
            var borderCornerTopLeft = '╔';
            var borderCornerTopRight = '╗';
            var borderCornerBottomLeft = '╚';
            var borderCornerBottomRight = '╝';
            var borderJoint = '╬';
            var borderHorizontal = '╦';
            var borderVertical = '╩';
            var borderCross = '╬';
            var borderRowSeparator = '╟';
            var borderColumnSeparator = '╢';

            // Load border configurations from ConfigManager
            LoadBorderConfig(ref borderTop, ref borderBottom, ref borderLeft, ref borderRight,
                ref borderCornerTopLeft, ref borderCornerTopRight, ref borderCornerBottomLeft,
                ref borderCornerBottomRight, ref borderJoint, ref borderHorizontal, ref borderVertical,
                ref borderCross, ref borderRowSeparator, ref borderColumnSeparator);

            // Determine column widths
            var columnWidths = new Dictionary<string, int>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var header = headers?.FirstOrDefault(h => h.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase)) ?? prop.Name;
                var maxLength = items.Max(i => prop.GetValue(i)?.ToString()?.Length ?? 0);
                var headerLength = header.Length;
                columnWidths[prop.Name] = Math.Max(maxLength, headerLength);
            }

            // Print the header if available
            if (headers != null && headers.Length > 0)
            {
                PrintLine(borderCornerTopLeft, borderTop, borderHorizontal, borderCornerTopRight, columnWidths);
                PrintRow(headers, columnWidths, borderLeft, borderRight, borderJoint);
                PrintLine(borderRowSeparator, borderBottom, borderCross, borderRowSeparator, columnWidths);
            }
            else
            {
                PrintLine(borderCornerTopLeft, borderTop, borderHorizontal, borderCornerTopRight, columnWidths);
            }

            // Print each item
            foreach (var item in items)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty).ToArray();
                PrintRow(values, columnWidths, borderLeft, borderRight, borderJoint);
            }

            PrintLine(borderCornerBottomLeft, borderBottom, borderVertical, borderCornerBottomRight, columnWidths);
        }

        private static void PrintLine(char cornerLeft, char horizontal, char joint, char cornerRight, Dictionary<string, int> columnWidths)
        {
            Console.Write(cornerLeft);
            foreach (var width in columnWidths.Values)
            {
                Console.Write(new string(horizontal, width + 2));
                Console.Write(joint);
            }
            Console.WriteLine(cornerRight);
        }

        private static void PrintRow(string[] columns, Dictionary<string, int> columnWidths, char borderLeft, char borderRight, char borderJoint)
        {
            Console.Write(borderLeft);
            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];
                var width = columnWidths[columnWidths.Keys.ElementAt(i)];
                PrintCell(column, width);
                Console.Write(borderJoint);
            }
            Console.WriteLine(borderRight);
        }

        private static void PrintCell(string text, int width)
        {
            var wrappedLines = WrapText(text, width).ToArray();
            foreach (var line in wrappedLines)
            {
                Console.Write($" {line.PadRight(width)} ");
            }
        }

        private static IEnumerable<string> WrapText(string text, int width)
        {
            for (int i = 0; i < text.Length; i += width)
            {
                yield return text.Substring(i, Math.Min(width, text.Length - i));
            }
        }

        private static void LoadBorderConfig(ref char borderTop, ref char borderBottom, ref char borderLeft, ref char borderRight,
            ref char borderCornerTopLeft, ref char borderCornerTopRight, ref char borderCornerBottomLeft, ref char borderCornerBottomRight,
            ref char borderJoint, ref char borderHorizontal, ref char borderVertical, ref char borderCross,
            ref char borderRowSeparator, ref char borderColumnSeparator)
        {
            try
            {
                var borderChars = ConfigManager.GetConfigValue("TableConfig", "BorderChars");
                if (!string.IsNullOrEmpty(borderChars))
                {
                    var chars = borderChars.ToCharArray();
                    if (chars.Length >= 14)
                    {
                        borderTop = chars[0];
                        borderBottom = chars[1];
                        borderLeft = chars[2];
                        borderRight = chars[3];
                        borderCornerTopLeft = chars[4];
                        borderCornerTopRight = chars[5];
                        borderCornerBottomLeft = chars[6];
                        borderCornerBottomRight = chars[7];
                        borderJoint = chars[8];
                        borderHorizontal = chars[9];
                        borderVertical = chars[10];
                        borderCross = chars[11];
                        borderRowSeparator = chars[12];
                        borderColumnSeparator = chars[13];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error loading border config: {ex.Message}", LogLevel.Crit);
            }
        }
    }
}

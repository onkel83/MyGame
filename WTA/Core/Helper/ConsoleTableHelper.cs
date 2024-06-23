using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Model;

namespace Core.Helper
{
    public static class ConsoleTableHelper<T> where T : BaseModel
    {
        public static void PrintTable(List<T> items, string[]? headers = null)
        {
            var borderChars = ConfigHelper.GetConfigValue("TableConfig", "BorderChars", "═║╔╗╚╝╬╦╩╬╟╢");
            var borders = borderChars.ToCharArray();

            var borderTop = borders[0];
            var borderBottom = borders[0];
            var borderLeft = borders[1];
            var borderRight = borders[1];
            var borderCornerTopLeft = borders[2];
            var borderCornerTopRight = borders[3];
            var borderCornerBottomLeft = borders[4];
            var borderCornerBottomRight = borders[5];
            var borderJoint = borders[6];
            var borderHorizontal = borders[7];
            var borderVertical = borders[8];
            var borderCross = borders[9];
            var borderRowSeparator = borders[10];
            var borderColumnSeparator = borders[11];

            var columnWidths = new Dictionary<string, int>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var header = headers?.FirstOrDefault(h => h.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase)) ?? prop.Name;
                var maxLength = items.Max(i => prop.GetValue(i)?.ToString()?.Length ?? 0);
                var headerLength = header.Length;
                columnWidths[prop.Name] = Math.Max(maxLength, headerLength);
            }

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
    }
}

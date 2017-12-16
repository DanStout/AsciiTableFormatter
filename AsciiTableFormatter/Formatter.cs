using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsciiTableFormatter
{
    public class Formatter
    {
        /// <summary>
        /// Returns an ASCII table showing the data contained in the public properties of <paramref name="rows"/>
        /// </summary>
        public static string Format<T>(IEnumerable<T> rows)
        {
            var type = typeof(T);
            var isSimple = type.IsSimpleType();
            var props = type
                .GetProperties()
                // Skip the indexed properties (e.g. exampleObj[0])
                .Where(x => x.GetIndexParameters().Length == 0)
                .ToArray();

            var headerStrs = new List<object>();
            if (isSimple)
            {
                // For simple types there is no header label
                headerStrs.Add(null);
            }
            else
            {
                foreach (var prop in props)
                {
                    var name = prop.Name;
                    headerStrs.Add(name);
                }
            }

            var rowsStrs = new List<List<object>>();
            rowsStrs.Add(headerStrs);

            foreach (var row in rows)
            {
                var rowStrs = new List<object>();
                if (isSimple)
                {
                    rowStrs.Add(row);
                }
                else
                {
                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(row);
                        rowStrs.Add(value);
                    }
                }

                rowsStrs.Add(rowStrs);
            }

            return Format(rowsStrs);
        }

        public static string Format(List<List<object>> rows)
        {
            var bldr = new StringBuilder();

            var sizes = MaxLengthInEachColumn(rows);
            var types = GetColumnTypes(rows);

            for (int rowNum = 0; rowNum < rows.Count; rowNum++)
            {
                if (rowNum == 0)
                {
                    // Top border
                    AppendLine(bldr, sizes);
                    if (rows[0][0] == null)
                    {
                        continue;
                    }
                }

                var row = rows[rowNum];
                for (int i = 0; i < row.Count; i++)
                {
                    var item = row[i];
                    var size = sizes[i];
                    bldr.Append("| ");
                    if (item == null)
                    {
                        bldr.Append("".PadLeft(size));
                    }
                    else if (types[i] == ColumnType.Numeric)
                    {
                        bldr.Append(item.ToString().PadLeft(size));
                    }
                    else if (types[i] == ColumnType.Text)
                    {
                        bldr.Append(item.ToString().PadRight(size));
                    }
                    else
                    {
                        throw new InvalidOperationException("Unexpected state");
                    }

                    bldr.Append(" ");

                    if (i == row.Count - 1)
                    {
                        // Add right border for last column
                        bldr.Append("|");
                    }
                }
                bldr.Append('\n');
                if (rowNum == 0)
                {
                    AppendLine(bldr, sizes);
                }
            }

            AppendLine(bldr, sizes);

            return bldr.ToString();
        }

        private static void AppendLine(StringBuilder bldr, List<int> sizes)
        {
            bldr.Append('o');

            for (int i = 0; i < sizes.Count; i++)
            {
                bldr.Append(new string('-', sizes[i] + 2));
                bldr.Append('o');
            }
            bldr.Append('\n');
        }


        private static List<int> MaxLengthInEachColumn(List<List<object>> rows)
        {
            var sizes = new List<int>();
            //Start from second row to skip the header
            for (int i = 0; i < rows[1].Count; i++)
            {
                var max = rows.Max(row => row[i]?.ToString()?.Length ?? 0);
                sizes.Insert(i, max);
            }
            return sizes;
        }

        private static List<ColumnType> GetColumnTypes(List<List<object>> rows)
        {
            var types = new List<ColumnType>();
            for(int i = 0; i < rows[1].Count; i++)
            {
                var isNumeric = rows.Skip(1).All(row => row[i]?.GetType()?.IsNumericType() ?? false);
                var coltype = isNumeric ? ColumnType.Numeric : ColumnType.Text;
                types.Insert(i, coltype);
            }
            return types;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsciiTableFormatter
{
    public class Formatter
    {
        public static string Format<T>(IEnumerable<T> rows)
        {
            var type = typeof(T);
            var isSimple = type.IsSimpleType();
            var props = type
                .GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0)
                .ToArray();

            var headerStrs = new List<object>();
            if (isSimple)
            {
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

            for (int rowNum = 0; rowNum < rows.Count; rowNum++)
            {
                if (rowNum == 0)
                {
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
                    else if (item.GetType().IsNumericType())
                    {
                        bldr.Append(item.ToString().PadLeft(size));
                    }
                    else
                    {
                        bldr.Append(item.ToString().PadRight(size));
                    }
                    bldr.Append(" ");

                    if (i == row.Count - 1)
                    {
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
            for (int i = 0; i < rows[1].Count; i++)
            {
                var max = rows.Max(row => row[i]?.ToString()?.Length ?? 0);
                sizes.Insert(i, max);
            }
            return sizes;
        }
    }
}

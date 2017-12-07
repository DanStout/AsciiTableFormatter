using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsciiTableFormatter
{
    public class Formatter
    {
        public static string Format<T>(IEnumerable<T> rows)
        {
            var props = typeof(T).GetProperties();

            var headerStrs = new List<string>();
            foreach (var prop in props)
            {
                var name = prop.Name;
                headerStrs.Add(name);
            }

            var rowsStrs = new List<List<string>>();
            rowsStrs.Add(headerStrs);

            foreach (var row in rows)
            {
                var rowStrs = new List<string>();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(row);
                    rowStrs.Add(value.ToString());
                }
                rowsStrs.Add(rowStrs);
            }

            return Format(rowsStrs);
        }

        public static string Format(List<List<string>> rows)
        {
            var bldr = new StringBuilder();

            var sizes = MaxLengthInEachColumn(rows);

            foreach (var row in rows)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    var size = sizes[i];
                    var item = row[i];
                    bldr.Append(item);
                }
                bldr.Append('\n');
            }

            return bldr.ToString();
        }


        private static List<int> MaxLengthInEachColumn(List<List<string>> rows)
        {

            var sizes = new List<int>();
            for (int i = 0; i < rows[0].Count; i++)
            {
                var max = rows.Max(row => row[i].Length);
                sizes.Insert(i, max);
            }
            return sizes;
        }
    }
}

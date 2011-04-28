using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.Models
{
    public class Table : Item
    {
        public List<string> HeaderCells { get; set; }
        public List<DataRow> DataRows { get; set; }

        public Table()
        {
            HeaderCells = new List<string>();
            DataRows = new List<DataRow>();
        }

        public Table(string id)
        {
            Name = id;
            HeaderCells = new List<string>();
            DataRows = new List<DataRow>();
        }

        public Table Parse(List<string> lines, ref int currentLine)
        {
            var table = new Table();

            if (lines[currentLine].StartsWith("|"))
            {
                var split = lines[currentLine].Split('|').ToList();

                foreach (string h in split)
                    if (!string.IsNullOrEmpty(h))
                        table.HeaderCells.Add(h.Trim());

                currentLine++;
            }

            while (lines[currentLine].StartsWith("|"))
            {
                var split = lines[currentLine].Split('|').ToList();

                split.RemoveAll(c => string.IsNullOrEmpty(c));

                table.AddRowData(split.ToArray());

                if (currentLine == lines.Count - 1)
                    lines[currentLine] = "<EOF>";
                else
                    currentLine++;
            }

            return table;
        }

        public void AddColumn(string key)
        {
            HeaderCells.Add(key);
            foreach (DataRow row in DataRows)
                row.DataCells.Add("");
        }

        public void AddRowData(params string[] values)
        {
            var dataRow = new DataRow();

            foreach (string v in values)
                dataRow.DataCells.Add(v.Trim());

            DataRows.Add(dataRow);
        }

        public string GetValue(string key, int row)
        {
            if (row < 0)
                return "";

            var column = HeaderCells.IndexOf(key);

            return DataRows[row].DataCells[column];
        }
    }
}

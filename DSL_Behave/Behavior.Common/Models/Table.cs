using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using Behavior.Common.LookUps;

namespace Behavior.Common.Models
{
    public class Table
    {
        public string Name { get; set; }
        public List<string> HeaderRow { get; set; }
        public List<DataRow> DataRows { get; set; }

        public Table()
        {
            Name = "";
            HeaderRow = new List<string>();
            DataRows = new List<DataRow>();
        }

        public Table(string id)
        {
            Name = id;
            HeaderRow = new List<string>();
            DataRows = new List<DataRow>();
        }

        public Table(XmlRpcStruct rpcStruct)
        {
            HeaderRow = new List<string>();
            DataRows = new List<DataRow>();
            
            foreach (string key in rpcStruct.Keys)
            {
                if (key.StartsWith("Name"))
                    Name = rpcStruct[key] as string;
                
                else if(!key.StartsWith("Type"))
                {
                    if (HeaderRow.Count == 0)
                        foreach (string s in (rpcStruct[key] as XmlRpcStruct).Keys)
                            AddColumn(s);

                    DataRows.Add(new DataRow(HeaderRow.Count));

                    var dataStruct = rpcStruct[key] as XmlRpcStruct;

                    foreach (string s in dataStruct.Keys)
                    {
                        var row = 0;

                        Int32.TryParse(key, out row);

                        SetCellValue(s, row, dataStruct[s] as string);
                    }
                }
            }
        }

        public XmlRpcStruct ToRpcTable()
        {
            var rpcStruct = new XmlRpcStruct();

            rpcStruct.Add("Type", "Table");

            rpcStruct.Add("Name", this.Name);

            int dataRowCount = 0;

            foreach (DataRow dr in this.DataRows)
            {
                int cellCount = 0;

                var dataStruct = new XmlRpcStruct();

                foreach (string cell in dr.DataCells)
                {
                    dataStruct.Add(this.HeaderRow[cellCount], cell);

                    cellCount++;
                }

                rpcStruct.Add(dataRowCount.ToString(), dataStruct);

                dataRowCount++;
            }

            return rpcStruct;
        }

        public Table Parse(List<string> lines, ref int currentLine)
        {
            var table = new Table();

            if (lines[currentLine].StartsWith(LanguageElements.TableDelimiter))
            {
                var split = lines[currentLine].Split(LanguageElements.TableDelimiter.ToCharArray()).ToList();

                foreach (string h in split)
                    if (!string.IsNullOrEmpty(h))
                        table.HeaderRow.Add(h.Trim());

                currentLine++;
            }

            while (lines[currentLine].StartsWith(LanguageElements.TableDelimiter))
            {
                var split = lines[currentLine].Split(LanguageElements.TableDelimiter.ToCharArray()).ToList();

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
            HeaderRow.Add(key);
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

        public void SetCellValue(string key, int row, string value)
        {
            if (row < 0)
                return;

            var column = HeaderRow.IndexOf(key);

            DataRows[row].DataCells[column] = value;
        }

        public string GetCellValue(string key, int row)
        {
            if (row < 0)
                return "";

            var column = HeaderRow.IndexOf(key);

            return DataRows[row].DataCells[column];
        }
    }
}

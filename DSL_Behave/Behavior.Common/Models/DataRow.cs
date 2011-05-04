using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Behavior.Common.Models
{
    public class DataRow
    {
        public List<string> DataCells { get; set; }

        public DataRow()
        {
            DataCells = new List<string>();
        }

        public DataRow(int headerCount)
        {
            DataCells = new List<string>();

            for (int i = 0; i < headerCount; i++)
                DataCells.Add("");
        }
    }
}

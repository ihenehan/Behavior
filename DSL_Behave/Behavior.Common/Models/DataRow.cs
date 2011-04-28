using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.Models
{
    public class DataRow
    {
        public List<string> DataCells { get; set; }

        public DataRow()
        {
            DataCells = new List<string>();
        }
    }
}

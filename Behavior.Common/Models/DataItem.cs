using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.Models
{
    public class DataItem : Item
    {
        public string Data { get; set; }

        public DataItem()
        {
            Name = "";
            Description = "";
            Data = "";
            Id = Guid.NewGuid();
            Tags = new List<string>();
            Type = "DataItem";
            IsLast = true;
        }
    }
}

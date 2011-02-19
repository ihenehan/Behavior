using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
    public class Interaction : Item
    {
        public string DataSelection { get; set; }
        public bool ExpectFailure { get; set; }
        public string ReturnName { get; set; }
        public List<DataItem> DataItems { get; set; }

        public Interaction()
        {
            Name = "";
            Description = "";
            ReturnName = "";
            Id = Guid.NewGuid();
            Tags = new List<string>();
            Type = "Interaction";
            IsLast = true;
            DataItems = new List<DataItem>();
            ChildrenIds = new List<Guid>();
            ChildrenType = null;
        }
    }
}

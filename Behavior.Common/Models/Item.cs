using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
    public class Item
    {
        public virtual string Name { get; set; }
        public  Guid Id { get; set; }
        public  Guid ParentId { get; set; }
        public  string Description { get; set; }
        public  string Type { get; set; }
        public  List<string> Tags { get; set; }
        public  int Index { get; set; }
        public  bool IsLast { get; set; }
        public  List<Guid> ChildrenIds { get; set; }
        public  Type ChildrenType { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public  List<Item> Children { get; set; }
    }
}

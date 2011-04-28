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
        public  string Description { get; set; }
        public  List<Tag> Tags { get; set; }
        public Table Table { get; set; }
        
    }
}

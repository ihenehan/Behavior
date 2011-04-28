using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
	public class Project : Item
	{
        [JsonIgnore]
        public List<Story> Stories { get; set; }

        public Project()
        {
            Name = "";
            Description = "";
            Id = Guid.NewGuid();
            Tags = new List<string>();
            Type = "Project";
            IsLast = true;
            ChildrenType = typeof(Story);
            ChildrenIds = new List<Guid>();
            Stories = new List<Story>();
        }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Extensions
{
    public static class CriterionListExtention
    {
        public static List<Criterion> Clone(this List<Criterion> criteria)
        {
            var newList = new List<Criterion>();

            criteria.ForEach(s => newList.Add(s.Clone()));

            return newList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Builders
{
    public interface ICriterionBuilder
    {
        List<Criterion> BuildCriterionFromOutline(Criterion criterion, List<Criterion> commonCriteria);
    }
}

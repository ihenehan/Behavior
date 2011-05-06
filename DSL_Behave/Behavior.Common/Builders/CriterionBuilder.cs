using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Common.Repository;
using Behavior.Common.Extensions;

namespace Behavior.Common.Builders
{
    public class CriterionBuilder : ICriterionBuilder
    {
        private List<Criterion> builtCriteria;

        public List<Criterion> BuildCriterionFromOutline(Criterion criterion, List<Criterion> commonCriteria)
        {
            builtCriteria = new List<Criterion>();

            if (criterion.Table != null)
            {
                var count = 0;

                foreach (DataRow dr in criterion.Table.DataRows)
                {
                    builtCriteria.AddRange(criterion.BeforeCriteria.Clone());

                    builtCriteria.AddRange(commonCriteria.Clone());

                    builtCriteria.Add(InsertParametricData(criterion, count));

                    builtCriteria.AddRange(criterion.AfterCriterion.Clone());

                    count++;
                }
            }
            else
                builtCriteria.Add(criterion);

            return builtCriteria;
        }

        public List<Criterion> BuildCriterion(Criterion criterion, List<Criterion> criterionCommon)
        {
            var sequence = new List<Criterion>();

            sequence.AddRange(criterion.BeforeCriteria);

            sequence.AddRange(criterionCommon.Clone());

            sequence.Add(criterion);

            sequence.AddRange(criterion.AfterCriterion);

            return sequence;
        }

        public Criterion InsertParametricData(Criterion criterion, int count)
        {
            var newCriterion = criterion.Clone();

            newCriterion.Name = newCriterion.Name + "_" + count;

            foreach (CriterionStep i in newCriterion.Steps)
                for (int x = 0; x < i.Parameters.Count; x++)
                    if (i.Parameters[x].GetType().Equals(typeof(string)))
                    {
                        if ((i.Parameters[x] as string).StartsWith("<"))
                            i.Parameters[x] = InsertValue(criterion.Table, i.Parameters[x] as string, count);
                    }
                    else
                        i.Parameters[x] = i.Table;

            return newCriterion;
        }

        public string InsertValue(Table table, string parameter, int row)
        {
            var key = parameter.Replace("<", "").Replace(">", "");

            return table.GetCellValue(key, row);
        }
    }
}

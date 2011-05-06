using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Common.Models;
using Behavior.Remote.Results;
using Behavior.Remote.Client;

namespace Behavior.ModelExtensions
{
    public static class CriterionExtensions
    {
        public static CriterionResult Run(this Criterion criterion, IRemoteClient proxy)
        {
            var criterionResult = new CriterionResult();

            criterionResult.Criterion = criterion;

            criterionResult.StepResults.AddRange(RunCriterion(criterion, -1, proxy).StepResults);

            criterionResult.SetResult();

            return criterionResult;
        }

        public static CriterionResult RunCriterion(Criterion criterion, int dataRow, IRemoteClient proxy)
        {
            var criterionResult = new CriterionResult() { Criterion = criterion };

            foreach (CriterionStep s in criterion.Steps)
            {
                if(criterion.Table != null && criterion.Table.DataRows.Count > 0)
                    s.SetTestData(criterion.Table, dataRow);

                if (s.Table != null && s.Table.DataRows.Count > 0)
                    s.SetTestData(s.Table);


                criterionResult.StepResults.Add(s.Run(proxy));

                criterionResult.SetResult();

                if (criterionResult.Result.status.Equals("FAIL"))
                    break;
            }

            return criterionResult;
        }
    }
}

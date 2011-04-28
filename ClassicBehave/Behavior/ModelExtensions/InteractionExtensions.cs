using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using CookComputing.XmlRpc;
using Behavior.Common.Models;
using Behavior.Remote;
using Behavior.Remote.Results;
using Behavior.Remote.Client;

namespace Behavior.ModelExtensions
{
    public static class InteractionExtensions
    {
        public static InteractionResult Run(this Interaction interaction, IRemoteClient proxy)
        {
            var keyword = new Keyword(interaction, proxy, Behavior.Config) { ReturnName = interaction.ReturnName };

            var result = new InteractionResult();

            if (keyword.KeywordExists && keyword.ParametersAreCorrect)
            {
                result = keyword.Run();

                if (interaction.ExpectFailure)
                {
                    if (result.Result.status.ToLower().Equals("pass"))
                        return new InteractionResult(Result.CreateFail("Expected failure, but interaction incorrectly passed."));
                    else
                        return new InteractionResult(Result.CreatePass("Interaction failed correctly with: " + result.Result.error));
                }

                return result;
            }

            return new InteractionResult(keyword);
        }
    }
}

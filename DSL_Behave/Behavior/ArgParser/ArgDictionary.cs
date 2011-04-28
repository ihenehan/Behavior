using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.ArgParser
{
    public class ArgDictionary<T> : Dictionary<string, Action<T, string>>
    {
        private readonly T target;

        public ArgDictionary(T target)
        {
            this.target = target;
        }

        public void ParseAndExecute(string[] args)
        {
            args.ToList().ForEach(a =>
                                          {
                                              var aName = a.Split('=')[0].ToLower();
                                              if (ContainsKey(aName))
                                                  this[aName](target, a);
                                          });
        }
    }
}

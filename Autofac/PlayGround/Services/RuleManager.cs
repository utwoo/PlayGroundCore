using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGround.Services
{
    public class RuleManager
    {
        public RuleManager(IList<IRule> rules)
        {
            this.Rules = rules;
        }

        public IList<IRule> Rules { get; private set; }
    }

    public interface IRule { }

    public class SingletonRule : IRule { }

    public class InstancePerDependencyRule : IRule { }
    
}

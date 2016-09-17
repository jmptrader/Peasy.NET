using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peasy.Core
{
    public interface IRule
    {
        IEnumerable<string> Errors { get; }
        IRule IfInvalidThenExecute(Action<IRule> method);
        IRule IfValidThenExecute(Action<IRule> method);
        IRule IfValidThenValidate(params IRule[] rule);
        bool IsValid { get; }
        IRule Validate(bool shortCircuitOnFirstFailedSuccessor = true);
        Task<IRule> ValidateAsync();
    }
}

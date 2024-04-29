using System;

namespace Kitbag.Builder.Core.Domain.Exceptions;

public class BrokenBusinessRuleException : Exception
{
    public IBusinessRule BrokenRule { get; }

    public string? Details => BrokenRule.BrokenRuleMessage;
    public string Code => BrokenRule.Code;

    public BrokenBusinessRuleException(IBusinessRule businessRule)
    {
        BrokenRule = businessRule ?? throw new ArgumentNullException(nameof(businessRule));
    }
}